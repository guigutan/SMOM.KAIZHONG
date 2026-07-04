using DevExpress.XtraReports.UI;
using SIE.Domain.Validation;

namespace SIE.Common.Prints
{
    /// <summary>
    /// 报表接口实现
    /// </summary>
    public abstract class HostReport : IHostReport
    {
        /// <summary>
        /// 报表文件扩展名
        /// </summary>
        public abstract string Extension { get; }
        /// <summary>
        /// 
        /// </summary>
        public virtual string Name
        {
            get { return this.GetType().Name; }
        }
        /// <summary>
        /// 获取文件全名
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string GetFileFullName(string fileName)
        {
            return "{0}{1}".FormatArgs(fileName, Extension);
        }

        /// <summary>
        /// 生成新的模板
        /// </summary>
        /// <param name="printable">打印提供者</param>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public abstract string GenerateNewTemplate(IPrintable printable, string fileName);
        /// <summary>
        /// 清理报表文件路径下的历史 csv 文件
        /// </summary>
        protected virtual void ClearCsvFile()
        {
            string path = GetPath();
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            var dataFiles = Directory.GetFiles(path, "*.csv")
                .Select(f => new { LastWriteTime = Directory.GetLastWriteTime(f), Url = f }).ToList();
            var tobeFiles = dataFiles.Where(d => d.LastWriteTime < DateTime.Now.AddDays(-2)).ToList(); //清理历史的数据文件
            tobeFiles.ForEach(p => File.Delete(p.Url));
        }
        /// <summary>
        /// 生成数据文件
        /// </summary>
        /// <param name="printable">打印提供者</param>
        /// <param name="datas">打印的数据集合</param>
        /// <returns>生成的数据文件路径</returns>
        public virtual string GenerateDataFile(IPrintable printable, IEnumerable<object> datas)
        {
            if (printable == null)
                throw new ArgumentNullException(nameof(printable));
            if (datas == null)
                throw new ArgumentNullException(nameof(datas));
            var dataFilePath = string.Empty;
            var csvFileName = $"{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.csv";
            try
            {
                string content = string.Empty;
                var propertys = printable.GetPropertys();
                foreach (var simpleData in datas)
                {
                    content += printable.ConverterData(simpleData);
                    content = content.TrimEnd(printable.Separator);
                    content += "\n";
                }
                string header = string.Empty;
                propertys.ForEach(p => header += p + printable.Separator);
                header = header.TrimEnd(printable.Separator);
                header += "\n";
                string templatePath = GetPath();
                dataFilePath = Path.Combine(templatePath, csvFileName); //为了支持并发
                if (!Directory.Exists(templatePath))
                    Directory.CreateDirectory(templatePath);
                File.WriteAllText(dataFilePath, header + content);
                var bindDataFile = Path.Combine(templatePath, "data.csv");
                if (!File.Exists(bindDataFile))
                {
                    File.Copy(dataFilePath, bindDataFile);
                }
            }
            catch (IOException ioEx)
            {
                throw new IOException(ioEx.Message, ioEx);
            }
            catch (Exception ex)
            {
                throw new PlatformException(ex.Message,ex);
            }
            
            return csvFileName;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="printable"></param>
        /// <param name="filePath"></param>
        /// <param name="printerName"></param>
        /// <param name="datas"></param>
        /// <param name="completeCallBack"></param>
        /// <param name="copies"></param>
        public abstract void PrintProcess(IPrintable printable, string filePath, string printerName, Func<IEnumerable<object>> datas, Action completeCallBack, short copies);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetPath()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return path;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        protected virtual void ValidateFilePath(string filePath)
        {
            if (filePath.IsNullOrWhiteSpace())
                throw new ArgumentNullException(nameof(filePath));
            if (!File.Exists(filePath))
                throw new Exception("文件：{0} 不存在".L10N().FormatArgs(filePath));
            if (!Path.GetExtension(filePath).EqualsIgnoreCase(Extension))
                throw new Exception("文件后缀必须是：{0}".L10N().FormatArgs(Extension));
        }
        protected virtual System.Drawing.Printing.PrinterSettings ValidatePrinter(string printerName)
        {
            var setting = new System.Drawing.Printing.PrinterSettings();
            setting.PrinterName = printerName;
            if (!setting.IsValid)
                throw new ValidationException("该打印机：{0} 无效".L10N().FormatArgs(printerName));
            return setting;
        }

        public virtual void UpdateCsvDataPath(string filePath)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="report"></param>
        public virtual void UpdateCsvDataPath(XtraReport report)
        {
        }
    }
}
