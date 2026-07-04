using SIE.Core.WorkOrders;

namespace SIE.Web.Core.WorkOrders
{
    /// <summary>
    /// 工单查询实体视图配置
    /// </summary>
    internal class WorkOrderCriteriaViewConfig : WebViewConfig<WorkOrderCriteria>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.No).Show(ShowInWhere.Detail).HasOrderNo(10).HasLabel("工单号");
                View.Property(p => p.Item).Show(ShowInWhere.Detail).HasOrderNo(30).HasLabel("产品编码");
                View.Property(p => p.ItemName).Show(ShowInWhere.Detail).HasOrderNo(40).HasLabel("产品名称");
            }
        }
    }
}