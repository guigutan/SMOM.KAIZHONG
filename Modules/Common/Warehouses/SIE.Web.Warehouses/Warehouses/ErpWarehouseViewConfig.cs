using SIE.MetaModel.View;
using SIE.Warehouses;
using SIE.Web.Warehouses.Warehouses.Commands;

namespace SIE.Web.Warehouses
{
    /// <summary>
    /// Erp子库 视图配置
    /// </summary>
    internal class ErpWarehouseViewConfig : WebViewConfig<ErpWarehouse>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.HasDelegate(ErpWarehouse.NameProperty);
        }

        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands();
            View.ReplaceCommands(WebCommandNames.Save, typeof(SaveErpWarehouseCommand).FullName);
            View.Property(p => p.ErpOrgId).Readonly(p => p.SourceType == ErpWarehouseSourceType.Erp).ShowInList(150);
            View.Property(p => p.ErpOrgCode).Readonly(p => p.SourceType == ErpWarehouseSourceType.Erp).ShowInList(150);
            View.Property(p => p.ErpOrgName).Readonly(p => p.SourceType == ErpWarehouseSourceType.Erp).ShowInList(150);
            View.Property(p => p.Code).Readonly(p => p.SourceType == ErpWarehouseSourceType.Erp).ShowInList(150);
            View.Property(p => p.Name).Readonly(p => p.SourceType == ErpWarehouseSourceType.Erp).ShowInList(150);
            View.Property(p => p.WmsInvOrg).Readonly(p => p.SourceType == ErpWarehouseSourceType.Erp).ShowInList(150);           
            View.Property(p => p.State).Readonly(p => p.SourceType == ErpWarehouseSourceType.Erp);
            View.ChildrenProperty(p => p.ErpWarehouseDetailList).OrderNo = 2;
        }

        /// <summary>
        /// 选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code).Show();
            View.Property(p => p.Name).Show();
            View.Property(p => p.WmsInvOrg).Show();
        }
    }
}
