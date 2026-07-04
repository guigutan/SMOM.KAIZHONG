using SIE.Core.WorkOrders;
using SIE.DIST.Distribution;

namespace SIE.Wpf.DIST.Distribution
{
    /// <summary>
    /// 配送单查询实体视图配置
    /// </summary>
    internal class DistributionBillCriteriaViewConfig : WPFViewConfig<DistributionBillCriteria>
    {
        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.ContainerNo).ShowInDetail();
            View.Property(p => p.WorkOrder).UseDataSource((e, p, s) =>
            {
                return RT.Service.Resolve<WorkOrderController>().GetWorkOrders(p, s);
            }).ShowInDetail();
            View.Property(p => p.Item).ShowInDetail();
        }
    }
}