using SIE.Defects;
using SIE.MetaModel.View;
using SIE.Web.Defects.Commands;

namespace SIE.Web.Defects
{
    /// <summary>
    /// 缺陷责任分类视图配置
    /// </summary>
    internal class DefectResponsibilityCategoryViewConfig : WebViewConfig<DefectResponsibilityCategory>
    {
        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.InlineEdit();
            View.AssignAuthorize(typeof(DefectResponsibility));
        }

        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(
                typeof(AddCategoryCommand).FullName,
                WebCommandNames.Edit,
                WebCommandNames.Delete,
                "SIE.Web.Portal.Receipt.Commands.AddCategoryCopyCommand",
                WebCommandNames.Save,
                typeof(InsertCommand).FullName,
                typeof(AddChildrenCommand).FullName,
                WebCommandNames.ExportXls);
            View.Property(p => p.Code).ShowInList(width: 150);
            View.Property(p => p.Description).ShowInList(width: 150);
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
