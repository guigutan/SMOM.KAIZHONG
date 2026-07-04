using SIE.Items.Items;

namespace SIE.Web.Items.Items
{
    /// <summary>
    /// 产品等级查询视图类
    /// </summary>
    internal class ProductGradeCriteriaViewConfig : WebViewConfig<ProductGradeCriteria>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Show(ShowInWhere.All);
                View.Property(p => p.Name).Show(ShowInWhere.All);
                View.Property(p => p.Describe).Show(ShowInWhere.All);
                View.Property(p => p.ItemId).Show(ShowInWhere.Hide);
            }
        }
    }
}
