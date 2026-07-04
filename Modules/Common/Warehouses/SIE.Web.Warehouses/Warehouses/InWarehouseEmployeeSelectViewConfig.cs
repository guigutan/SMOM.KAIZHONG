using SIE.Warehouses;
using SIE.Warehouses.Warehouses;
using SIE.Web.Common;

namespace SIE.Web.Warehouses
{
    /// <summary>
    /// 配置仓库查询实体视图
    /// </summary>
    internal class InWarehouseEmployeeSelectViewConfig : WebViewConfig<InWarehouseEmployeeSelect>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DisableEditing();
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Show().Readonly();
                View.Property(p => p.Name).Show().Readonly();
                View.Property(p => p.InvOrgCode).Show().Readonly();
                View.Property(p => p.InvOrgName).Show().Readonly();
                View.Property(p => p.LibraryType).Show().Readonly();
                View.Property(p => p.Category).UseCatalogEditor(e => { e.CatalogType = Warehouse.CatalogCategory; e.CatalogReloadData = true; }).Show().Readonly();
                View.Property(p => p.IsFrozen).Show().Readonly();
            }
        }
    }
}
