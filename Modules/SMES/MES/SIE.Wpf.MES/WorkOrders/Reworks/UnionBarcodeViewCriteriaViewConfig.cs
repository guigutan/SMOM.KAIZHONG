using SIE.MES.WorkOrders.Reworks;

namespace SIE.Wpf.MES.WorkOrders.Reworks
{
    /// <summary>
    /// 关联条码条件视图配置
    /// </summary>
    internal class UnionBarcodeViewCriteriaViewConfig : WPFViewConfig<UnionBarcodeViewCriteria>
    {
        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.WorkOrderNo).ShowInDetail();
            View.Property(p => p.Barcode).ShowInDetail();
            View.Property(p => p.InspetNo).ShowInDetail();
        }
    }
}