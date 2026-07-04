using SIE.Items.Items;

namespace SIE.Wpf.Items.Items
{
    /// <summary>
    /// 产品等级查询视图类
    /// </summary>
    internal class ProductGradeCriteriaViewConfig : WPFViewConfig<ProductGradeCriteria>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Show(ShowInWhere.All); //.UseProductGradeCatalogLookupEditor(p => { p.DisplayMember = nameof(ProductGrade.Code); })
                View.Property(p => p.Name).Show(ShowInWhere.All); //UseProductGradeCatalogLookupEditor(p => { p.DisplayMember = nameof(ProductGrade.Name); })
                View.Property(p => p.Describe).Show(ShowInWhere.All);
            }
        }
    }
}
