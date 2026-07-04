using SIE.Items.Items;

namespace SIE.Wpf.Items.Items
{
    /// <summary>
    /// 物料分类查询实体视图
    /// </summary>
    public class MultiItemCateCriteriaViewConfig : WPFViewConfig<MultiItemCategoryCriteria>
    {
        /// <summary>
        /// 多选查询实体配置
        /// </summary>
        public const string MultiQueryView = "MultiQueryView";

        /// <summary>
        /// 默认配置
        /// </summary>
        protected override void ConfigView()
        {
            if (ViewGroup == MultiQueryView)
                MultiQueryConfigView();
        }

        /// <summary>
        /// 多选查询实体配置
        /// </summary>
        protected void MultiQueryConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Show(ShowInWhere.Detail);
                View.Property(p => p.Name).Show(ShowInWhere.Detail);
                View.Property(p => p.Level).UseItemCateLevelEditor(p =>
                {
                    p.CategoryType = CategoryType.Item;
                }).Show(ShowInWhere.Detail);
                View.Property(p => p.ItemType).Show(ShowInWhere.Detail);
            }
        }
    }
}
