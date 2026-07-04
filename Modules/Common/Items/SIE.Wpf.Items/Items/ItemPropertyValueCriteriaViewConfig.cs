using SIE.Items.Items;

namespace SIE.Wpf.Items
{
    /// <summary>
    /// 物料属性值查询实体视图配置
    /// </summary>
    internal class ItemPropertyValueCriteriaViewConfig : WPFViewConfig<ItemPropertyValueCriteria>
    {
        /// <summary>
        /// 物料属性查询视图
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                //View.Property(p => p.Item).Show(ShowInWhere.All);
                //View.Property(p => p.Definition).Show(ShowInWhere.All);
                View.Property(p => p.Value).Show(ShowInWhere.All);
            }
        }
    }
}
