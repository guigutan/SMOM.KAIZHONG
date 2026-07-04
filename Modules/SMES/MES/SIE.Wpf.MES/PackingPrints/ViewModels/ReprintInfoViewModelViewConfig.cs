using SIE.Barcodes;
using SIE.MES.PackingPrints;
using SIE.MES.PackingPrints.ViewModels;
using SIE.Wpf.Common;
using System;

namespace SIE.Wpf.MES.PackingPrints.ViewModels
{
    /// <summary>
    /// 条码补打 视图
    /// </summary>
    internal class ReprintInfoViewModelViewConfig : WPFViewConfig<ReprintInfoViewModel>
    {
        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.AssignAuthorize(typeof(PackingBarcode));
            View.ClearCommands();
            View.HasDetailColumnsCount(2);
            View.Property(p => p.Times).UseSpinEditor(p => p.MinValue = 1);
            View.Property(p => p.Printer).UsePrinterEditor();
            View.Property(p => p.Template).UseDataSource((e, p, r) =>
            {
                return RT.Service.Resolve<BarcodeController>().GetPrintTemplatesByType(typeof(PackingPrintable).GetQualifiedName(), p, r);
            });
            View.Property(p => p.Reason).UseMemoEditor().ShowInDetail(rowSpan: 3, columnSpan: 2);
        }
    }
}
