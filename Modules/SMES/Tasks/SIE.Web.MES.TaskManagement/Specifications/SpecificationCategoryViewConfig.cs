using SIE.MES.TaskManagement.Specifications;
using SIE.MetaModel.View;

namespace SIE.Web.MES.TaskManagement.Specifications
{
    /// <summary>
	/// 规格件分类视图配置
	/// </summary>
	internal class SpecificationCategoryViewConfig : WebViewConfig<SpecificationCategory>
    {
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(Specification));
        }
        protected override void ConfigListView()
        {
            View.UseDefaultCommands().UseImportCommands();
            View.ReplaceCommands(WebCommandNames.Copy, "SIE.Web.MES.TaskManagement.Specifications.Commands.SpecificationCategoryCopyCommand");
            View.Property(p => p.Code);
            View.Property(p => p.Name);
        }

        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
        }

        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
        }

        /// <summary>
        /// 配置导入视图
        /// </summary>
        protected override void ConfigImportView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
        }
    }
}
