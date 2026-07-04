using SIE.Common.Prints;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.PackingPrints;
using SIE.Wpf.Common.Prints;
using System;

namespace SIE.Wpf.MES.PackingPrints
{
    /// <summary>
    /// 条码打印帮助类
    /// </summary>
    public class PackingPrintHelper 
    {
        /// <summary>
        /// 打印条码
        /// </summary>
        /// <param name="barcodes">待条码列表</param>
        /// <param name="template">打印模板</param>
        /// <param name="printer">打印机</param>
        /// <param name="times">打印次数</param>
        /// <param name="action">回调</param>
        public void PrintBarcodes(EntityList<PackingBarcode> barcodes, PrintTemplate template, string printer, short times, Action action)
        {
            if (template == null)
                throw new ValidationException("打印模板不能为空".L10N());
            action();
            var filePath = RT.Service.Resolve<PrintsController>().DownloadPrintTemplate(template.Id);
            var printable = new PackingPrintable();
            var report = ReportFactory.Current.GetReportByExtension(template.Type);
            report.Print(printable, filePath, printer, () =>
            {
                return barcodes;
            }, () =>
            { }, times);
        }
    }
}
