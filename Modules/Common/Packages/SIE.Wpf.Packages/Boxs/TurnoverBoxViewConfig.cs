using SIE.Domain;
using SIE.Packages.Boxs;
using SIE.Wpf.Common;
using SIE.Wpf.Packages.Boxs.Behaviors;
using SIE.Wpf.Packages.Boxs.Commands;

namespace SIE.Wpf.Packages.Boxs
{
    /// <summary>
    /// 周转箱视图配置
    /// </summary>
    internal class TurnoverBoxViewConfig : WPFViewConfig<TurnoverBox>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.InlineEdit().UseDefaultBehaviors();
            View.AddBehavior(typeof(BoxViewBehavior));
            View.UseDefaultCommands();
            View.ReplaceCommands(WPFCommandNames.ListDelete, typeof(TurnoverBoxDeleteCommand));
            View.ReplaceCommands(WPFCommandNames.ListCopy, typeof(TurnoverBoxCopyCommand));
            View.UseCommands(typeof(TurnoverBoxScrapCommand));
        }

        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.Property(p => p.Code).Readonly(DataEntityStatus.IsEditStatusProperty);
            View.Property(p => p.Type).UseCatalogEditor(e => e.CatalogType = TurnoverBox.BoxTypeCatalog);
            View.Property(p => p.State).Readonly();
            View.Property(p => p.Capacity).UseSpinEditor(p => { p.MinValue = 1; p.MaxValue = 1000000; });
        }

        /// <summary>
        /// 查询视图配置
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Type).UseCatalogEditor(p => p.CatalogType = TurnoverBox.BoxTypeCatalog);
            View.Property(p => p.State);
        }

        /// <summary>
        /// 选择视图配置
        /// </summary>
        protected override void ConfigSelectionView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Code);
                View.Property(p => p.Type).UseCatalogEditor(p => p.CatalogType = TurnoverBox.BoxTypeCatalog);
                View.Property(p => p.State);
            }
        }
    }
}