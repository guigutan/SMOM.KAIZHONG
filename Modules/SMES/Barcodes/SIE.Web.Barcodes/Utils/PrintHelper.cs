using SIE.Barcodes;
using SIE.Barcodes.Printables;
using SIE.Common.Prints;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Web.Common.Prints;
using System;

namespace SIE.Web.Barcodes.Utils
{
    /// <summary>
    /// 条码打印帮助类
    /// </summary>
    public class PrintHelper
    {
        /// <summary>
        /// 打印条码
        /// </summary>
        /// <param name="barcodes">待条码列表</param>
        /// <param name="template">打印模板</param>
        /// <param name="printer">打印机</param>
        /// <param name="times">打印次数</param>
        /// <param name="action">回调</param>
        public string PrintBarcodes(EntityList<Barcode> barcodes, PrintTemplate template, string printer, short times, Action action)
        {
            if (template == null)
                throw new ValidationException("打印模板不能为空".L10N());
            action();
            var printable = new BarcodePrintable();
            var report = ReportFactory.Current.GetReportByExtension(template.Type);
            return report.PrintProcess(printable, template.Id, template.Content, () =>
            {
                return barcodes;
            }, times);
        }
    }
}
