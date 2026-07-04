using SIE.Defects;
using SIE.Wpf.Command;
using SIE.Wpf.Defects.Commands;

namespace SIE.Wpf.Defects
{
    /// <summary>
    /// 缺陷责任分类视图配置
    /// </summary>
    internal class DefectResponsibilityCategoryViewConfig : WPFViewConfig<DefectResponsibilityCategory>
    {
        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.InlineEdit()
                .UseDefaultBehaviors()
                .UseDefaultCommands()
                .UseCommands(typeof(InsertCommand), typeof(AddChildrenCommand))
                .RemoveCommands(typeof(UndoCommand), typeof(RedoCommand));
        }

        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Description);
        }

        /// <summary>
        /// 查询视图配置
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Description);
        }

        /// <summary>
        /// 选择视图配置
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Description);
        }
    }
}