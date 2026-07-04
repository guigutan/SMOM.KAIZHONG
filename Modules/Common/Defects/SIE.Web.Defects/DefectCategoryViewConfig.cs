using SIE.Defects;
using SIE.MetaModel.View;
using SIE.Web.Defects.Commands;

namespace SIE.Web.Defects
{
    /// <summary>
    /// 缺陷分类视图配置类
    /// </summary>
    internal class DefectCategoryViewConfig : WebViewConfig<DefectCategory>
    {
        /// <summary>
        /// 默认配置
        /// </summary>
        protected override void ConfigView()
        {
            View.InlineEdit();
            View.AssignAuthorize(typeof(Defect));
        }

        /// <summary>
        /// 配置列表视图
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
            View.Property(p => p.Description).ShowInList(width: 300);

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
