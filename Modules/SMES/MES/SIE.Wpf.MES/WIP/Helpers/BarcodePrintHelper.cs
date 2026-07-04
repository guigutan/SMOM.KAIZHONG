using SIE.Barcodes;
using SIE.Barcodes.Printables;
using SIE.Common.Prints;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.WIP.Models;
using SIE.Threading;
using SIE.Wpf.Common.Prints;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SIE.Wpf.MES.WIP.Helpers
{
    /// <summary>
    /// 条码打印帮助类
    /// </summary>
    public static class BarcodePrintHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static EntityList<Barcode> GenerateAndPrintSn(BarcodePrintInfo info)
        {
            EntityList<Barcode> result = new EntityList<Barcode>();
            try
            {
                Check.NotNull(info, "条码打印信息不能为空".L10N());
                var barcodes = RT.Service.Resolve<BarcodeController>().Print(info);
                result.AddRange(barcodes);
                PrintBarcode(info, barcodes);
                return result;
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.Message);
                return result;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static EntityList<Barcode> GenerateSn(BarcodePrintInfo info)
        {
            EntityList<Barcode> result = new EntityList<Barcode>();
            try
            {
                if (info == null)
                {
                    throw new ArgumentNullException("条码打印信息不能为空".L10N());
                }
                var barcodes = RT.Service.Resolve<BarcodeController>().Print(info);
                result.AddRange(barcodes);
                return result;
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.Message);
                return result;
            }
        }

        /// <summary>
        /// 打印条码标签
        /// </summary>
        /// <param name="info"></param>
        /// <param name="barcodes"></param>
        public static void PrintBarcode(BarcodePrintInfo info, EntityList<Barcode> barcodes)
        {
            if (!barcodes.Any())
            {
                throw new ValidationException("条码不能为空".L10N());
            }
            if (info == null || info.Printer.IsNullOrEmpty())
            {
                throw new ValidationException("打印机不能为空".L10N());
            }
            if (info.TemplateType.IsNullOrEmpty())
            {
                throw new ValidationException("模板类型不能为空".L10N());
            }
            var filePath = RT.Service.Resolve<PrintsController>().DownloadPrintTemplate(info.PrintTemplateId);
            var printable = new BarcodePrintable();
            var report = ReportFactory.Current.GetReportByExtension(info.TemplateType);
#pragma warning disable CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法
            DoPrint(info.Printer, barcodes, filePath, printable, report);
#pragma warning restore CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法

        }

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="printer"></param>
        /// <param name="barcodes"></param>
        /// <param name="filePath"></param>
        /// <param name="printable"></param>
        /// <param name="report"></param>
        /// <returns></returns>
        private static async Task DoPrint(string printer, EntityList<Barcode> barcodes, string filePath, BarcodePrintable printable, IReport report)
        {
            await Task.Run(new Action(() =>
            {
                report.Print(printable, filePath, printer, () =>
                {
                    return barcodes;
                }, () =>
                {
                });
            }).WithCurrentThreadContext()).ConfigureAwait(true);
        }
    } 
}