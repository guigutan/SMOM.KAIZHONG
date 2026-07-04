using SIE.CSM.Suppliers;
using SIE.MetaModel.View;
using SIE.Web.CSM.Suppliers.Commands;

namespace SIE.Web.CSM.Suppliers
{
    /// <summary>
    /// 供应商物料视图
    /// </summary>
    internal class SupplierItemViewConfig : WebViewConfig<SupplierItem>
    {
        /// <summary>
        /// 视图
        /// </summary>
        protected override void ConfigView()
        {
            View.UseCommands(typeof(SelectItemCommand).FullName);
            View.UseCommands(WebCommandNames.Edit, typeof(SupplierItemDeleteCommand).FullName);
        }
        
        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.Property(p => p.ItemCode).Readonly();
            View.Property(p => p.ItemName).Readonly();
            View.Property(p => p.ItemSpecificationModel).Readonly();
            View.Property(p => p.PurchaseSupplyType).UseEnumEditor(p => p.AllowBlank = false);
            View.Property(p => p.Remark);
        }
    }
}
