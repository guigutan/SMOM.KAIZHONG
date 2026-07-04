using SIE.CSM.Suppliers;
using SIE.Wpf.CSM.Suppliers.Commonds;

namespace SIE.Wpf.CSM.Suppliers
{
    /// <summary>
    /// 供应商与物料关系视图配置
    /// </summary>
    internal class SupplierItemViewConfig : WPFViewConfig<SupplierItem>
    {
        /// <summary>
        /// 默认配置
        /// </summary>
        protected override void ConfigView()
        {
            View.UseCommands(typeof(SelectItemCommand), WPFCommandNames.ListEdit, WPFCommandNames.ListDelete);
        }

        /// <summary>
        /// 视图列表配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.Property(p => p.Item.Code).HasLabel("编码");
            View.Property(p => p.Item.Name).HasLabel("名称");
            View.Property(p => p.Item.SpecificationModel).HasLabel("规格型号");
            View.Property(p => p.PurchaseSupplyType);
            View.Property(p => p.Remark);
        }
    }
}
