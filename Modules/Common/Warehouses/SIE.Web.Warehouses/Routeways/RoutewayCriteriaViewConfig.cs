using SIE.Domain;
using SIE.Warehouses;

namespace SIE.Web.Warehouses
{
    /// <summary>
    /// 巷道查询视图配置
    /// </summary>
    internal class RoutewayCriteriaViewConfig : WebViewConfig<RoutewayCriteria>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DomainName("巷道");
            View.UseDefaultCommands();

            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Show(ShowInWhere.All);
                View.Property(p => p.Name).Show(ShowInWhere.All);
                View.Property(p => p.WarehouseId).UseWarehouseEditor(contianFrozen: true).Show(ShowInWhere.All).Cascade(f => f.AreaId, null);
                View.Property(p => p.AreaId).Readonly(f => f.WarehouseId == null || f.WarehouseId == 0).UseDataSource((source, pagingInfo, keyword) =>
                          {
                              var storageLocation = source as RoutewayCriteria;
                              if (storageLocation != null)
                              {
                                  return RT.Service.Resolve<WarehouseController>().GetStorageArea(null, storageLocation.WarehouseId.Value, keyword, pagingInfo);
                              }
                              return new EntityList<StorageArea>();
                          }).Show(ShowInWhere.All);
            }
        }
    }
}
