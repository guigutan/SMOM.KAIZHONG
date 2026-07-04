using Stimulsoft.Report;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using System.Text.RegularExpressions;

namespace SIE.CrossPlatform.Collect.Utils
{
    public static class SiedPrinter
    {
        private static readonly string TemplatesPath = "Templates";
        private static readonly string dataFileName = "data.csv";

        /// <summary>
        /// 调用打印
        /// </summary>
        /// <param name="templateFile"></param>
        /// <param name="datas"></param>
        /// <param name="printerName"></param>
        /// <param name="isShowPreView"></param>

        public static void Print(string templateFileName, List<object> datas, string printerName = "", bool isShowPreView = false)
        {
            StiOptions.Configuration.IsAvalonia = true;
            var executablePath = Directory.GetCurrentDirectory();
            var printerFileFullPath = Path.Combine(GetPath(), templateFileName);
            //处理数据
            ConvertDataToCSV(datas);
            UpdateTemplateCsvDataPath(printerFileFullPath);
            //创建报表并打印
            var stiReport = StiReport.CreateNewReport();
            stiReport.Culture = CultureInfo.CurrentCulture.Name;//"zh-CN";
            stiReport.Load(printerFileFullPath);
            stiReport.Render(true);
            var printPdfFileFullPath = Path.Combine(executablePath, TemplatesPath, GetNewPdfFileName());
            stiReport.ExportDocument(StiExportFormat.Pdf, printPdfFileFullPath);

            if (OperatingSystem.IsLinux())
            {
                PrinterHelper.LinuxPrintFile(printPdfFileFullPath, printerName);
            }
            if (OperatingSystem.IsWindows())
            {
                if (!string.IsNullOrEmpty(printerName))
                {
                    ValidatePrinter(printerName);
                }
                PrinterHelper.WindowsPrint(printPdfFileFullPath, printerName);
            }
            stiReport.Dispose();
        }

        /// <summary>
        /// 验证打印机 windows有效
        /// </summary>
        /// <param name="printerName"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        [SupportedOSPlatform("windows")]
        private static System.Drawing.Printing.PrinterSettings ValidatePrinter(string printerName)
        {
            var setting = new System.Drawing.Printing.PrinterSettings();
            setting.PrinterName = printerName;
            if (!setting.IsValid)
                throw new ValidationException("该打印机：{0} 无效");
            return setting;
        }


        /// <summary>
        /// 获取pdf文件名称 为保证唯一性 使用时间戳+随机数(1-10000).pdf方式
        /// </summary>
        /// <returns>时间戳+随机数(1-10000).pdf</returns>
        public static string GetNewPdfFileName()
        {
            long timeStamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            var random = Random.Shared.Next(1, 10000);
            return timeStamp.ToString() + random + ".pdf";
        }
        /// <summary>
        /// 更新模板对应的data.csv的路径
        /// </summary>
        /// <param name="filePath"></param>
        private static void UpdateTemplateCsvDataPath(string filePath)
        {
            var info = new FileInfo(filePath);
            string content = File.ReadAllText(filePath);
            Regex regex = new Regex("<PathData>(.*?)</PathData>", RegexOptions.IgnoreCase);
            content = regex.Replace(content, "<PathData>" + info.DirectoryName + "</PathData>");
            File.WriteAllText(filePath, content);
        }


        /// <summary>
        /// 打印数据转data.csv
        /// </summary>
        /// <param name="datas"></param>
        private static void ConvertDataToCSV(IEnumerable<object> datas)
        {
            ClearCsvAndPDF();//清除模板路径下的csv和pdf

            if (datas == null || !datas.Any())
            {
                return;
            }
            const char SEP = '|';

            Type type = datas.FirstOrDefault().GetType();
            var propertys = GetPropertys(type);

            StringBuilder content = new StringBuilder();

            StringBuilder header = new StringBuilder();
            foreach (string propertyName in propertys)
            {
                header.Append(propertyName);
                header.Append(SEP);
            }
            content.Append($"{header.ToString().TrimEnd(SEP)}\n");

            foreach (object data in datas)
            {
                StringBuilder row = new StringBuilder();
                foreach (string propertyName in propertys)
                {
                    var propertyInfo = type.GetProperty(propertyName);
                    if (propertyInfo != null)
                    {
                        string val = propertyInfo.GetValue(data, null)?.ToString();
                        if (propertyInfo.PropertyType == typeof(decimal) || propertyInfo.PropertyType == typeof(double) || propertyInfo.PropertyType == typeof(float))
                        {
                            if (val != null && val.Contains(".", StringComparison.CurrentCulture))
                            {
                                val = val.TrimEnd('0').TrimEnd('.');
                            }
                        }
                        row.Append(val);
                        row.Append(SEP);
                    }
                }
                content.Append($"{row.ToString().TrimEnd(SEP)}\n");
            }

            string contentStr = content+ "\r\n";
            string dataFilePath = Path.Combine(GetPath(), dataFileName);
            File.WriteAllText(dataFilePath, contentStr, Encoding.UTF8);
        }

        /// <summary>
        /// 获取对象的属性名称列表
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static IEnumerable<string> GetPropertys(Type type)
        {
            var propertyNames = new List<string>();
            foreach (var property in type.GetProperties())
            {
                if (property.PropertyType.IsInterface)
                    continue;
                if (property.PropertyType.IsClass && property.PropertyType.Name != "String")
                    continue;
                propertyNames.Add(property.Name);
            }
            return propertyNames.Distinct();
        }


        /// <summary>
        /// 清除csv和pdf文件
        /// </summary>
        private static void ClearCsvAndPDF()
        {
            string path = GetPath();
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            foreach (var item in Directory.GetFiles(path, "*.csv"))
            {
                File.Delete(item);
            }
            foreach (var item in Directory.GetFiles(path, "*.pdf"))
            {
                File.Delete(item);
            }
        }

        /// <summary>
        /// 获取模板路径
        /// </summary>
        /// <returns></returns>
        private static string GetPath()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), TemplatesPath);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return path;
        }
    }
}
