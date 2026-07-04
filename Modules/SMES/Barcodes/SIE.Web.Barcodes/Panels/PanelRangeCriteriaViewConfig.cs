using SIE.Barcodes;
using SIE.Barcodes.Panels;

namespace SIE.Web.Barcodes.Panels
{
    /// <summary>
    /// 条码范围查询视图配置
    /// </summary>
    internal class PanelRangeCriteriaViewConfig : WebViewConfig<PanelRangeCriteria>
    {
        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.WorkOrder).ShowInDetail();
            View.Property(p => p.PanelCode).ShowInDetail();
            View.Property(p => p.ReceiveBy).ShowInDetail();
            View.Property(p => p.ReceiveDate).ShowInDetail();
        }
    }
}