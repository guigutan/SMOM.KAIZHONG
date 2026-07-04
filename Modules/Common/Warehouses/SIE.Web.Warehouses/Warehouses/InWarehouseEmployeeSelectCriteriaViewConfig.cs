using SIE.Warehouses;
using SIE.Web.Common;

namespace SIE.Web.Warehouses
{
    /// <summary>
    /// 配置仓库查询实体视图
    /// </summary>
    internal class InWarehouseEmployeeSelectCriteriaViewConfig : WebViewConfig<InWarehouseEmployeeSelectCriteria>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Show();
                View.Property(p => p.Name).Show();
                View.Property(p => p.LibraryType).UseEnumEditor(p => p.IsEnumNull = true).Show();
                View.Property(p => p.Category).UseCatalogEditor(e => { e.CatalogType = Warehouse.CatalogCategory; e.CatalogReloadData = true; }).Show()
                    .UseListSetting(e => { e.HelpInfo = "仓库分类快码类型(WAREHOUSE_TYPE)"; });
                View.Property(p => p.IsFrozen).UseCheckDropDownEditor(p => p.AllowBlank = true).Show();
                View.Property(p => p.InvOrgId).Show();
            }
        }
    }
}
