using SIE.Barcodes;

namespace SIE.WPF.Barcodes
{
    /// <summary>
	/// 打印日志 查询视图配置
	/// </summary>
	internal class BarcodeLogCriteriaViewConfig : WPFViewConfig<BarcodeLogCriteria>
    {
        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.WorkOrderNo).ShowInDetail();
            View.Property(p => p.Barcode).ShowInDetail();
            View.Property(p => p.Type).ShowInDetail();
            View.Property(p => p.OperatDate).ShowInDetail();
            View.Property(p => p.Operator).ShowInDetail();
        }
    }
}