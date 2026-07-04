using SIE.MES.WIP.Products;

namespace SIE.Wpf.MES.WIP.Products
{
    /// <summary>
    /// 视图配置
    /// </summary>
    internal class WipProductPropertyViewConfig : WPFViewConfig<WipProductProperty>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
		protected override void ConfigView()
        {
            View.UseDefaultCommands();

            using (View.OrderProperties())
            {
                View.Property(p => p.PropertyName).Show(ShowInWhere.All);
                View.Property(p => p.Value).Show(ShowInWhere.All);
            }
        }
    }
}