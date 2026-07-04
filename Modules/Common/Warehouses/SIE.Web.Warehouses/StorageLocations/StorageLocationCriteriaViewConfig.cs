using SIE.Warehouses;

namespace SIE.Web.Warehouses
{
    /// <summary>
    /// 库区查询视图配置
    /// </summary>
    internal class StorageLocationCriteriaViewConfig : WebViewConfig<StorageLocationCriteria>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DomainName("库区");
            View.UseDefaultCommands();

            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Show(ShowInWhere.All);
                View.Property(p => p.Name).Show(ShowInWhere.All);
                View.Property(p => p.LibraryType).Show(ShowInWhere.All);
                View.Property(p => p.WarehouseId).UseAllWarehouseEditor().Show(ShowInWhere.All);
                View.Property(p => p.Area).Show(ShowInWhere.All);
                View.Property(p => p.IsFrozen).Show(ShowInWhere.All).UseCheckDropDownEditor();
                View.Property(p => p.ErpInvOrg).Show(ShowInWhere.Hide);
                View.Property(p => p.ErpSubLibrary).Show(ShowInWhere.Hide);
                View.Property(p => p.ErpLocation).Show(ShowInWhere.Hide);
                View.Property(p => p.IsAutomatedArea).Show(ShowInWhere.All);
                View.Property(p => p.RowNo).UseSpinEditor(p => p.MinValue = 1).Show(ShowInWhere.All);
                View.Property(p => p.LayerNo).UseSpinEditor(p => p.MinValue = 1).Show(ShowInWhere.All);
                View.Property(p => p.ColumnNo).UseSpinEditor(p => p.MinValue = 1).Show(ShowInWhere.All);
                View.Property(p => p.RoutewayId).UseDataSource((source, pagingInfo, keyword) =>
                {
                    var storageLocation = source as StorageLocationCriteria;
                    return RT.Service.Resolve<WarehouseController>().GetRouteways(storageLocation.WarehouseId, null, keyword, pagingInfo);
                }).UsePagingLookUpEditor((m, r) =>
                {
                    m.ReloadDataOnPopping = true;
                }).Show(ShowInWhere.All);
                View.Property(p => p.LogicArea).Show(ShowInWhere.All);
            }
        }
    }
}
