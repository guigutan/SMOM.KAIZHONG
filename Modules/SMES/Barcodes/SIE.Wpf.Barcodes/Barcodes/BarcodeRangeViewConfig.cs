using SIE.Barcodes;
using SIE.Domain;
using SIE.ManagedProperty;
using SIE.ObjectModel;
using SIE.Wpf.Barcodes.Commonds;

namespace SIE.Wpf.Barcodes
{
    /// <summary>
    /// 条码领用 视图配置
    /// </summary>
    [CompiledPropertyDeclarer]
    public class BarcodeRangeViewConfig : WPFViewConfig<BarcodeRange>
    {
        #region 条码范围
        /// <summary>
        /// 条码范围
        /// </summary>
        [Label("条码范围")]
        public static readonly Property<string> SnRangeProperty = P<BarcodeRange>.RegisterExtensionReadOnly("SnRange", typeof(BarcodeRangeViewConfig), GetSnRange, BarcodeRange.StartSnProperty, BarcodeRange.EndSnProperty);

        /// <summary>
        /// 格式化条码范围
        /// </summary>
        /// <param name="me">条码范围</param>
        /// <returns>格式化条码范围String</returns>
        public static string GetSnRange(BarcodeRange me)
        {
            return me.StartSn + "-" + me.EndSn;
        }
        #endregion 

        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(WPFCommandNames.Export, typeof(ReceiveBarcodeCommand));
            View.Property(p => p.WONo).HasLabel("工单号");
            View.Property(p => p.ProductName).HasLabel("产品名称");
            View.Property(SnRangeProperty).UseListSetting(c => c.ListGridWidth = 250);
            View.Property(p => p.PrintQty).UseSpinEditor(p => p.Decimals = 0);
            View.Property(p => p.ScrapedQty).UseSpinEditor(p => p.Decimals = 0);
            View.Property(p => p.State);
            View.Property(p => p.ReceiveBy);
            View.Property(p => p.ReceiveDate);
        }
    }
}