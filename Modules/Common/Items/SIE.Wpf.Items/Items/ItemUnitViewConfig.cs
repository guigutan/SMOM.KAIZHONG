using SIE.Items;
using SIE.Wpf.Common;

namespace SIE.Wpf.Items
{
    /// <summary>
    /// 转换单位视图配置
    /// </summary>
    internal class ItemUnitViewConfig : WPFViewConfig<ItemUnit>
    {
        /// <summary>
        /// 默认视图配置
        /// </summary>
		protected override void ConfigView()
        {
            View.UseDefaultBehaviors();
            View.InlineEdit();
            View.UseCommands(WPFCommandNames.ListAdd, WPFCommandNames.ListEdit, WPFCommandNames.ListDelete);
            using (View.OrderProperties())
            {
                View.Property(p => p.Unit).Show(ShowInWhere.All);
                View.Property(p => p.Unit.Type).Show(ShowInWhere.All).UseCatalogEditor(p => p.CatalogType = Unit.CatalogType).HasLabel("类型");
                View.Property(p => p.Numerator).UseSpinEditor(e => { e.MinValue = 1; e.MaxValue = 100; e.Decimals = 0; }).Show(ShowInWhere.All);
                View.Property(p => p.Denominator).UseSpinEditor(e => { e.MinValue = 1; e.MaxValue = 100; e.Decimals = 0; }).Show(ShowInWhere.All);
            }
        }
    }
}
