using SIE.Barcodes;

namespace SIE.Wpf.Barcodes
{
    /// <summary>
    /// 条码打印日志视图配置
    /// </summary>
    internal class BarcodeLogViewConfig : WPFViewConfig<BarcodeLog>
    {
        /// <summary>
        /// 列表配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(WPFCommandNames.Export);
            View.Property(p => p.BarcodeLogWONo).HasLabel("工单");
            View.Property(p => p.BarcodeLogBarcodeSn).HasLabel("条码");
            View.Property(p => p.Type);
            View.Property(p => p.Qty);
            View.Property(p => p.Reason);
            View.Property(p => p.BarcodeLogOperatorName).HasLabel("操作人");
            View.Property(p => p.OperatDate);
        }
    }
}