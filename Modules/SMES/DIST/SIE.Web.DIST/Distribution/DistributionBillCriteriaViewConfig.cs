using SIE.Core.WorkOrders;
using SIE.DIST.Distribution;

namespace SIE.Web.DIST.Distribution
{
    /// <summary>
    /// 配送单查询实体视图配置
    /// </summary>
    internal class DistributionBillCriteriaViewConfig : WebViewConfig<DistributionBillCriteria>
    {
        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.ContainerNo);
            View.Property(p => p.WorkOrder).UseDataSource((e, p, s) =>
            {
                return RT.Service.Resolve<WorkOrderController>().GetWorkOrders(p, s);
            });
            View.Property(p => p.Item);
        }
    }
}