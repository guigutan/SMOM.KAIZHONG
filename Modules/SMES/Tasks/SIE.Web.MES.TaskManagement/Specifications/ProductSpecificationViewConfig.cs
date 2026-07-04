using SIE.MES.TaskManagement.Specifications;
using SIE.MetaModel.View;
using SIE.Web.MES.TaskManagement.Specifications.Commands;

namespace SIE.Web.MES.TaskManagement.Specifications
{
    /// <summary>
	/// 产品规格件对照表视图配置
	/// </summary>
	internal class ProductSpecificationViewConfig : WebViewConfig<ProductSpecification>
    {
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.ReplaceCommands(WebCommandNames.Add, typeof(SelectItemCommand).FullName);
            View.ReplaceCommands(WebCommandNames.Delete, typeof(DeletedProdSpecificationCommand).FullName);
            View.UseImportCommands();
            View.Property(p => p.ProductCode).HasLabel("物料编码").Readonly();
            View.Property(p => p.ProductName).HasLabel("物料名称").Readonly();
            View.Property(p => p.ProductSpecificationModel).Readonly();
            View.Property(p => p.ProductUnitName).Readonly();
            View.Property(p => p.ProductType).Readonly();
            View.Property(p => p.ProductItemSourceType).Readonly();
            View.Property(p => p.ProductState).Readonly();
            View.Property(p => p.ProductSourceType).Readonly();
            View.ChildrenProperty(p => p.Details).HasLabel("规格件清单").Show(ChildShowInWhere.All);
        }

        /// <summary>
        /// 配置导入模板
        /// </summary>
        protected override void ConfigImportView()
        {
            View.PropertyRef(p => p.Product.Code).HasLabel("物料编码").ImportIndexer();
        }
    }
}
