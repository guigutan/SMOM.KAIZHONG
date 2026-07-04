using SIE.Domain;
using SIE.Items;
using SIE.Wpf.Common;

namespace SIE.Wpf.Items
{
    /// <summary>
    /// 单位视图配置
    /// </summary>
    class UnitViewConfig : WPFViewConfig<Unit>
    {
        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            // 视图配置
        }

        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultBehaviors().InlineEdit().UseDefaultCommands();
            View.Property(p => p.Code).Readonly(DataEntityStatus.IsEditStatusProperty);
            View.Property(p => p.Name);
            View.Property(p => p.Type).UseCatalogEditor(e => e.CatalogType = Unit.CatalogType);
            View.Property(p => p.Precision).UseSpinEditor(e => { e.MinValue = 0; });
        }

        /// <summary>
        /// 查询视图配置
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.Type).UseCatalogEditor(e => e.CatalogType = Unit.CatalogType);
            View.Property(p => p.Precision);
        }

        /// <summary>
        /// 选择视图配置
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.Type).UseCatalogEditor(e => e.CatalogType = Unit.CatalogType);
            View.Property(p => p.Precision);
        }
    }
}
