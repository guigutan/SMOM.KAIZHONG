using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using SIE.Domain.Validation;
using SIE.Common.Configs;
using SIE.Common.Prints;
using SIE.Logging;
using SIE.Core.Prints.Configs;

namespace SIE.Core.Prints
{
    /// <summary>
    /// HostBarTender 报表
    /// </summary>
    public class BarTenderHostReport : HostReport
    {
        /// <summary>
        /// 报表文件扩展名
        /// </summary>
        public override string Extension
        {
            get { return ".btw"; }
        }
        /// <summary>
        /// 生成新的模板
        /// </summary>
        /// <param name="printable">打印提供者</param>
        /// <param name="fileName">打印模板文件名</param>
        /// <returns>模板文件路径</returns>
        public override string GenerateNewTemplate(IPrintable printable, string fileName)
        {
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SIE.Common.Prints.Reports.Template.btw");
            if (stream == null)
                throw new ValidationException("未找到空白模板".L10N());
            var path = GetPath();
            string filePath = Path.Combine(path, GetFileFullName(fileName));
            byte[] data = new byte[stream.Length];
            stream.Read(data, 0, data.Length);
            using (var fileStream = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                fileStream.Write(data, 0, data.Length);
                fileStream.Flush();
            }
            stream.Close();
            return filePath;
        }

        /// <summary>
        /// 获取 BarTender 路径
        /// </summary>
        /// <returns></returns>
        protected virtual string GetBarTenderPath()
        {
            //if (SIE.Common.Properties.Settings.Default.BarTender.IsNullOrEmpty())
            //{
            //    var dialog = new OpenFileDialog();
            //    dialog.Title = "选择BarTender打印程序".Translate();
            //    dialog.Filter = "BarTender程序文件|*.exe";
            //    if (dialog.ShowDialog() == true)
            //    {
            //        SIE.Common.Properties.Settings.Default.BarTender = dialog.FileName;
            //        SIE.Common.Properties.Settings.Default.Save();
            //    }
            //}
            //  @"D:\Program Files\Seagull\BarTender Suite\bartend.exe";
            var config = ConfigService.GetConfig(new HostReportConfig());
            //!hack is local debug
            //config.BarTenderPath = @"C:\\Program Files\\Seagull\\BarTender 2019\\BarTend.exe";

            return config?.BarTenderPath;
        }


        //private static readonly object Lock = new object();
        private readonly ILog hostPrintLog = Logging.LogManager.GetLogger("hostPrint_logger");


        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="printable">打印提供者</param>
        /// <param name="filePath">文件路径</param>
        /// <param name="printerName">打印名称</param>
        /// <param name="datas">打印的数据集合</param>
        /// <param name="completeCallBack">打印完成回调方法</param>
        /// <param name="copies">打印份数</param>
        public override void PrintProcess(IPrintable printable, string filePath, string printerName, Func<IEnumerable<object>> datas, Action completeCallBack, short copies)
        {
            using (Diagnostics.PerformenceWatcher.Start(hostPrintLog, $"调用Bartender命令打印耗时:"))
            {
                ValidateFilePath(filePath);
                ValidatePrinter(printerName);
                var barTenderPath = GetBarTenderPath();
                if (barTenderPath.IsNullOrEmpty())
                    throw new ValidationException("Host未配置：{0} 的路径".L10N().FormatArgs("bartend.exe"));
                if (!File.Exists(barTenderPath))
                    throw new ValidationException("路径：{0} 不存在".L10N().FormatArgs(barTenderPath));
                var dataFilePath = GenerateDataFile(printable, datas.Invoke());
                var processArgs = $"/AF=\"{filePath}\" /D=\"{dataFilePath}\" /CLOSE /PRN=\"{printerName}\" /P /C={copies} /NOSPLASH";
                /*
                 //!record tobe顶配，bartend预加载，全局队列，局部队列
                参考：https://help.seagullscientific.com/2022/en/Index.htm#file_command_params_inside_running.htm%3FTocPath%3DAutomating%2520BarTender%7CAutomation%2520with%2520the%2520Command%2520Line%2520Interface%7CRunning%2520BarTender%2520from%2520Inside%2520Other%2520Programs%7C_____1
                 */
                var args = $"\"{barTenderPath}\" {processArgs}";
                hostPrintLog.Debug(args);
                Process.Start("CMD.exe", $"/C \"{args}\"");
                completeCallBack?.Invoke();
            }
            ClearCsvFile();
        }
    }
}
