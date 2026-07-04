using SIE.Kit.UrgentOrder.ItemUrgentOrders.Configs;

namespace SIE.Web.Kit.UrgentOrder.ItemUrgentOrders.Configs
{
    /// <summary>
    /// 物料加急单时间项视图配置
    /// </summary>
    internal class ItemUrgentOrderDateConfigValueViewConfig : WebViewConfig<ItemUrgentOrderDateConfigValue>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.Property(p => p.Time).UseSpinEditor(e => {
                e.MinValue = 0;
                e.DecimalPrecision =2;
                e.AllowDecimals = true;
            }).Show(ShowInWhere.All);
        }
    }
}
