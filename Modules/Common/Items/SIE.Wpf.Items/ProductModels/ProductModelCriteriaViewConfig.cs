using SIE.Items;

namespace SIE.WPF.Sales.Products
{
    /// <summary>
    /// 产品机型查询实体 视图配置
    /// </summary>
    internal class ProductModelCriteriaViewConfig : WPFViewConfig<ProductModelCriteria>
    {
        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultCommands();

            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Show(ShowInWhere.All);
                View.Property(p => p.Name).Show(ShowInWhere.All);
            }
        }
    }
}
