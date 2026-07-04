using SIE.Barcodes;
using SIE.MES.PackingPrints;
using SIE.MES.PackingPrints.ViewModels;
using System;


namespace SIE.Web.MES.PackingPrints.ViewModels
{
    /// <summary>
    /// 条码补打 视图
    /// </summary>
    internal class ReprintInfoViewModelViewConfig : WebViewConfig<ReprintInfoViewModel>
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
            View.Property(p => p.Template).UseDataSource((e, p, r) =>
            {
                return RT.Service.Resolve<BarcodeController>().GetPrintTemplatesByType(typeof(PackingPrintable).GetQualifiedName(), p, r);
            });
            View.Property(p => p.Reason).UseMemoEditor().ShowInDetail(rowSpan: 3, columnSpan: 2);
        }
    }
}
