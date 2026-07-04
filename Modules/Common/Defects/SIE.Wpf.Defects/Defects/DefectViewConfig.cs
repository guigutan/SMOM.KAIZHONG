using SIE.Defects;
using SIE.Wpf.Common;
using SIE.Wpf.Defects.Commands;

namespace SIE.Wpf.Defects
{
    /// <summary>
    /// 缺陷代码视图配置
    /// </summary>
    internal class DefectViewConfig : WPFViewConfig<Defect>
    {
        private const string TYPE_CODE = "分类编码";

        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.InlineEdit()
                .UseDefaultBehaviors()
                .UseDefaultCommands()
                .UseCommands(typeof(DefectCategoryCommand))
                .RemoveCommands(WPFCommandNames.Undo, WPFCommandNames.Redo);
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.Property(p => p.Code).ShowInList(150);
            View.Property(p => p.Description);
            View.Property(p => p.DefectGrade);
            View.Property(p => p.QualityType);
            View.Property(p => p.DefectCategoryId).UseEditor(WPFEditorNames.PagingLookUp).HasLabel(TYPE_CODE);
            View.Property(p => p.CategoryDescription).HasLabel("分类描述");
        }

        /// <summary>
        /// 查询面板视图配置
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code).Show();
            View.Property(p => p.Description);
            View.Property(p => p.DefectCategory).HasLabel(TYPE_CODE);
            View.Property(p => p.QualityType);
            View.Property(p => p.DefectGrade);
        }

        /// <summary>
        /// 选择视图配置
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Description);
            View.Property(p => p.DefectGrade);
            View.Property(p => p.QualityType);
            View.Property(p => p.CategoryCode).HasLabel(TYPE_CODE);
            View.Property(p => p.CategoryDescription).HasLabel("分类描述");
        }
    }
}