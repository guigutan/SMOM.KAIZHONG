using SIE.Kit.UrgentOrder.ItemUrgentOrders.Configs;

namespace SIE.Web.Kit.UrgentOrder.ItemUrgentOrders.Configs
{
    /// <summary>
    /// 物料加急单号视图配置
    /// </summary>
    internal class ItemUrgentOrderNoConfigValueViewConfig : WebViewConfig<ItemUrgentOrderNoConfigValue>
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
