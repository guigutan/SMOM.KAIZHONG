using SIE.MES.TaskManagement.Specifications;
using SIE.MetaModel.View;
using SIE.Web.MES.TaskManagement.Specifications.Commands;

namespace SIE.Web.MES.TaskManagement.Specifications
{
    /// <summary>
	/// 产品规格件清单视图配置
	/// </summary>
	internal class ProductSpecificationDetailViewConfig : WebViewConfig<ProductSpecificationDetail>
    {
        protected override void ConfigListView()
        {
            View.UseDefaultCommands();
            View.RemoveCommands(WebCommandNames.Copy, WebCommandNames.ExportXls);
            View.ReplaceCommands(WebCommandNames.Add, typeof(SelectSpecificationCommand).FullName);
            View.ReplaceCommands(WebCommandNames.Save, typeof(SpecificationSaveCommand).FullName);
            View.UseCommands(typeof(ProductSpecificationDetailCommand).FullName, "SIE.Web.Common.Import.Commands.DownloadTemplateCommand");
            View.Property(p => p.SpecificationCode).Readonly();
            View.Property(p => p.SpecificationName).Readonly();
            View.Property(p => p.SpecificationDescription).Readonly();
            View.Property(p => p.SpecificationCategoryName).Readonly();
            View.Property(p => p.Qty).UseSpinEditor(e => { e.MinValue = 0; e.AllowDecimals = true; });
        }

        protected override void ConfigImportView()
        {
            View.PropertyRef(p => p.ProductSpecificationCode).HasLabel("物料编码");
            View.PropertyRef(p => p.Specification.Code).ImportIndexer().HasLabel("规格件编码");
            View.Property(p => p.Qty).HasLabel("单体定额");
        }
    }
}
