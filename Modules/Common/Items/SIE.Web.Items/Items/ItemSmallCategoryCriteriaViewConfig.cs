using SIE.Items;

namespace SIE.Web.Items
{
    /// <summary>
    /// 物料分类小类查询实体视图配置
    /// </summary>
    internal class ItemSmallCategoryCriteriaViewConfig : WebViewConfig<ItemSmallCategoryCriteria>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Show(ShowInWhere.Detail);
                View.Property(p => p.Name).Show(ShowInWhere.Detail);
                View.Property(p => p.Level).UsePagingLookUpEditor().Show(ShowInWhere.Detail);
                View.Property(p => p.Type).Show(ShowInWhere.Detail);
                View.Property(p => p.ItemType).Show(ShowInWhere.Detail);
            }
        }
    }
}