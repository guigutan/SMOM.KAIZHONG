using SIE.MES.WorkOrders.Reworks;
using SIE.Wpf.Common.ViewBehaviors;

namespace SIE.Wpf.MES.WorkOrders.Reworks
{
    /// <summary>
    /// 关联条码视图配置
    /// </summary>
    internal class UnionBarcodeCoreViewConfig : WPFViewConfig<UnionBarcodeCore>
    {
        /// <summary>
        /// 关联条码列表ViewGroup
        /// </summary>
        protected override void ConfigListView()
        {
            View.AddBehavior(typeof(MultipleRowViewBehavior));
            View.Property(p => p.WorkOrderNo);
            View.Property(p => p.ItemNo);
            View.Property(p => p.Barcode);
            View.Property(p => p.InspetNo);
            View.Property(p => p.Result);
        }
    }
}