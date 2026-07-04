using SIE.Barcodes;

namespace SIE.WPF.Barcodes
{
    /// <summary>
    /// 条码范围查询视图配置
    /// </summary>
    internal class BarcodeRangeCriteriaViewConfig : WPFViewConfig<BarcodeRangeCriteria>
    {
        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.WorkOrderNo).ShowInDetail();
            View.Property(p => p.ReceiveBy).ShowInDetail();
            View.Property(p => p.ReceiveDate).ShowInDetail();
        }
    }
}