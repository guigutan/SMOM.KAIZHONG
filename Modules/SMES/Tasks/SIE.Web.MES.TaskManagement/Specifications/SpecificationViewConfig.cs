using SIE.MES.TaskManagement.Specifications;
using SIE.MetaModel.View;

namespace SIE.Web.MES.TaskManagement.Specifications
{
    /// <summary>
	/// 规格件清单视图配置
	/// </summary>
	internal class SpecificationViewConfig : WebViewConfig<Specification>
    {
        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands().UseImportCommands();
            View.ReplaceCommands(WebCommandNames.Copy, "SIE.Web.MES.TaskManagement.Specifications.Commands.SpecificationCopyCommand");
            View.UseCommands("SIE.Web.MES.TaskManagement.Specifications.Commands.SpecificationCategoryCommand");
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.Description).HasLabel("规格描述");
            View.Property(p => p.CategoryId).HasLabel("规格分类");
        }

        /// <summary>
        /// 查询视图配置
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.Category).HasLabel("规格分类");
        }

        /// <summary>
        /// 配置导入视图
        /// </summary>
        protected override void ConfigImportView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.Description).HasLabel("规格描述");
            View.PropertyRef(p => p.Category.Code).HasLabel("规格分类编码");
        }
    }
}
