using SIE.MES.WIP.Products;

namespace SIE.Wpf.MES.WIP.Products
{
    /// <summary>
    /// 生产产品视图配置
    /// </summary>
    internal class WipProductViewConfig : WPFViewConfig<WipProduct>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
		protected override void ConfigView()
        {
            View.UseDefaultCommands();

            using (View.OrderProperties())
            {
                View.Property(p => p.Puid).Show(ShowInWhere.All);
                View.Property(p => p.Model).Show(ShowInWhere.All);
                View.Property(p => p.IsHold).Show(ShowInWhere.All);
                View.Property(p => p.IsFixed).Show(ShowInWhere.All);
                View.Property(p => p.IsConcession).Show(ShowInWhere.All);
                View.Property(p => p.BatchQty).Show(ShowInWhere.All);
                View.Property(p => p.Grade).Show(ShowInWhere.All);
                View.Property(p => p.Result).Show(ShowInWhere.All);
                View.Property(p => p.State).Show(ShowInWhere.All);
                View.Property(p => p.ItemId).Show(ShowInWhere.All);
            }
        }
    }
}
