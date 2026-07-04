using SIE.Kit.UrgentOrder.ItemUrgentOrders;
using SIE.ObjectModel;

namespace SIE.Web.Kit.UrgentOrder.ItemUrgentOrders
{
    /// <summary>
    /// 物料加急单查询视图
    /// </summary>
    public class ItemUrgentOrderCriteriaViewConfig : WebViewConfig<ItemUrgentOrderCriteria>
    {
        /// <summary>
        /// 列表逻辑视图配置
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.No).Show(ShowInWhere.All);
                View.Property(p => p.OrderState).Show(ShowInWhere.All);
                View.Property(p => p.CreateDate).UseDateRangeEditor(p => p.DateRangeType = DateRangeType.Today).Show(ShowInWhere.All);
            }
        }
    }
}
