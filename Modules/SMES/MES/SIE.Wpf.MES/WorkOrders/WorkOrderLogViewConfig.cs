using SIE.MES.WorkOrders;

namespace SIE.Wpf.MES.WorkOrders
{
    /// <summary>
    /// 工单日志视图配置
    /// </summary>
    internal class WorkOrderLogViewConfig : WPFViewConfig<WorkOrderLog>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
		protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Type).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.Operator).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.OperatDate).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.Reason).Show(ShowInWhere.All).Readonly();
            }
        }
    }
}
