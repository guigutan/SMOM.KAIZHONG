using SIE.LES.StockOrders.Configs;

namespace SIE.Web.LES.StockOrders
{
    /// <summary>
    /// 调度触发的备料单状态
    /// </summary>
    public class SchedulingTriggeredStatusConfigValueViewConfig : WebViewConfig<SchedulingTriggeredStatusConfigValue>
    {
        /// <summary>
        /// 配置
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.TriggeredStatus).Show(ShowInWhere.All);
            }
        }
    }
}
