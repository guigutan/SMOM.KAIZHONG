using SIE.Items;

namespace SIE.Wpf.Items
{
    /// <summary>
    /// 产品BOM查询实体视图配置
    /// </summary>
    internal class ProductBomCriteriaViewConfig : WPFViewConfig<ProductBomCriteria>
    {
        /// <summary>
        /// 默认查询视图
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultCommands();

            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Show(ShowInWhere.All);
                View.Property(p => p.Name).Show(ShowInWhere.All);
                View.Property(p => p.ProductCode).Show(ShowInWhere.All);
                View.Property(p => p.ProductName).Show(ShowInWhere.All);
                View.Property(p => p.SpecificationModel).Show(ShowInWhere.All);
            }
        }
    }
}
