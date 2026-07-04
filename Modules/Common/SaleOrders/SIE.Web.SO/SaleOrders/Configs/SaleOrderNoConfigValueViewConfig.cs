using SIE.SO.SaleOrders.Configs;

namespace SIE.Web.SO.SaleOrders.Configs
{
    /// <summary>
    /// 销售订单配置视图
    /// </summary>
    public class SaleOrderNoConfigValueViewConfig : WebViewConfig<SaleOrderNoConfigValue>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.Property(p => p.NumberRule).Show(ShowInWhere.All);
        }
    }
}
