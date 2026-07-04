using SIE.MES.WorkOrders.Configs;

namespace SIE.Wpf.MES.WorkOrders.Configs
{
    /// <summary>
    /// 工单号配置值视图配置
    /// </summary>
    class WorkOrderNoConfigValueViewConfig : WPFViewConfig<WorkOrderNoConfigValue>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.BacodeRule).Show(ShowInWhere.All);
            }
        }
    }
}
