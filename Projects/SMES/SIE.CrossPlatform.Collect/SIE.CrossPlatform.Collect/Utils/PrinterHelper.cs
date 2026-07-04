using SIE.CrossPlatform.Collect.PDFPrinter;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Versioning;
namespace SIE.CrossPlatform.Collect.Utils
{
    //命令执行帮助类
    public static class PrinterHelper
    {
        [SupportedOSPlatform("windows")]
        public static void WindowsPrint(string pdfFilePath, string printerName = "Microsoft Print to PDF")
        {
            using var pdfiumViewer = PdfDocument.Load(pdfFilePath);
            
            var printDoc = pdfiumViewer.CreatePrintDocument();

            if (!string.IsNullOrEmpty(printerName))
            {
                printDoc.PrinterSettings.PrinterName = printerName; 
            }
            printDoc.Print();
        }
        /// <summary>
        /// Linux打印文件
        /// </summary>
        /// <param name="pdfFile">文件名称</param>
        /// <param name="printerName">打印机名称 默认:"Kylin-f22"</param>
        [SupportedOSPlatform("linux")]
        public static void LinuxPrintFile(string pdfFile, string printerName = "Kylin-f22")
        {
            string lprCommand = $"lpr {pdfFile} -P {printerName}";
            Process lprProcess = new Process();
            lprProcess.StartInfo.FileName = "/bin/bash";
            lprProcess.StartInfo.Arguments = $"-c \"{lprCommand}\"";
            lprProcess.Start();
            lprProcess.WaitForExit();

        }

        /// <summary>
        ///执行shell 命令
        /// </summary>
        /// <param name="command"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string RunCommand(string? command, string? args)
        {
            if (string.IsNullOrWhiteSpace(command))
                return string.Empty;
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = command,
                    Arguments = args,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();
            if (string.IsNullOrEmpty(error)) { return output; }
            else { return error; }
        }

        /// <summary>
        /// 获取所有打印机名称
        /// </summary>
        /// <returns></returns>

        public static List<string> GetAllPrinterNames()
        {
            var result = new List<string>();
            if (OperatingSystem.IsLinux())
            {
                string cmdResult = PrinterHelper.RunCommand("lpstat", "-p");
                string[] strArray = cmdResult.Split('\n');
                List<string> strList = new List<string>();
                int index1, index2;
                for (int i = 0; i < strArray.Length; i++)
                {
                    index1 = strArray[i].IndexOf(' ');
                    if (index1 <= 0) continue;

                    index2 = strArray[i].IndexOf(' ', index1 + 1);
                    if (index2 <= 0) continue;

                    strList.Add(strArray[i].Substring(index1 + 1, index2 - index1 - 1));
                }
                //获取打印机信息
                result = strList;
            }
            else if (OperatingSystem.IsWindows())
            {
                System.Drawing.Printing.PrinterSettings.StringCollection collection = System.Drawing.Printing.PrinterSettings.InstalledPrinters;
                foreach (var item in collection)
                {
                    result.Add(item?.ToString());
                }
            }
            return result;
        }
    }
}
