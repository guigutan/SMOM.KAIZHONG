using SIE.Defects;
using SIE.ManagedProperty;
using SIE.Wpf.Defects.Commands;
using System.Collections.Generic;

namespace SIE.Wpf.Defects
{
    /// <summary>
    /// 缺陷责任视图配置
    /// </summary>
    internal class DefectResponsibilityViewConfig : WPFViewConfig<DefectResponsibility>
    {
        /// <summary>
        /// 视图通用配置
        /// </summary>
        protected override void ConfigView()
        {
            View.InlineEdit()
                .UseDefaultBehaviors()
                .UseDefaultCommands()
                .UseCommands(typeof(DefectRespCategoryCommand))
                .RemoveCommands(WPFCommandNames.Undo, WPFCommandNames.Redo);
        }

        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Description);
            View.Property(p => p.CategoryId).UsePagingLookUpEditor(e =>
            {
                e.QueryMembers = new List<IManagedProperty> { DefectResponsibilityCategory.CodeProperty, DefectResponsibilityCategory.DescriptionProperty };
                e.ReloadDataOnPopping = true;
            }).HasLabel("分类编码");
            View.Property(p => p.CategoryDescription).HasLabel("分类描述").Readonly();
        }

        /// <summary>
        /// 查询视图配置
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Category).UsePagingLookUpEditor(e =>
            {
                e.QueryMembers = new List<IManagedProperty> { DefectResponsibilityCategory.CodeProperty, DefectResponsibilityCategory.DescriptionProperty };
                e.ReloadDataOnPopping = true;
            }).HasLabel("分类编码");
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
            View.Property(p => p.CategoryCode).HasLabel("分类编码");
            View.Property(p => p.CategoryDescription).HasLabel("分类描述");
        }
    }
}
