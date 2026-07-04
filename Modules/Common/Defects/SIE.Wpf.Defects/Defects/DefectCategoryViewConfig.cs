using SIE.Defects;
using SIE.Wpf.Command;
using SIE.Wpf.Defects.Commands;

namespace SIE.Wpf.Defects
{
    /// <summary>
    /// 缺陷分类视图配置类
    /// </summary>
    internal class DefectCategoryViewConfig : WPFViewConfig<DefectCategory>
    {
        /// <summary>
        /// 默认配置
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
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.Property(p => p.Code).ShowInList(150);
            View.Property(p => p.Description).ShowInList(300);
        }

        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Description);
        }

        /// <summary>
        /// 配置选择列表视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Description);
        }
    }
}