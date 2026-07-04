using SIE.LES.StockOrders.Configs;

namespace SIE.Web.LES.StockOrders.Configs
{
    /// <summary>
    /// 视图配置
    /// </summary>
    public class StockOrderIsAuditConfigValueViewConfig : WebViewConfig<StockOrderIsAuditConfigValue>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.ClearCommands();
            View.Property(p => p.IsAudit).Show();
        }
    }
}