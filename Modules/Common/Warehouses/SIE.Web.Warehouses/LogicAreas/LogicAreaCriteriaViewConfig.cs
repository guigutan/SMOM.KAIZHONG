using SIE.Domain;
using SIE.Warehouses;

namespace SIE.Web.Warehouses
{
    /// <summary>
    /// 逻辑分区查询视图配置
    /// </summary>
    internal class LogicAreaCriteriaViewConfig : WebViewConfig<LogicAreaCriteria>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DomainName("逻辑分区");
            View.UseDefaultCommands();

            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Show(ShowInWhere.All);
                View.Property(p => p.Name).Show(ShowInWhere.All);
                View.Property(p => p.WarehouseId).Show(ShowInWhere.All);
                View.Property(p => p.StorageLocation).UsePagingLookUpEditor().UseDataSource((o, e, r) =>
                {
                    var criteria = o as LogicAreaCriteria;
                    if (criteria == null) return new EntityList<StorageLocation>();
                    return RT.Service.Resolve<WarehouseController>().GetEnableStorageLocations(null, criteria.WarehouseId, r, e);
                }).Show(ShowInWhere.All);
            }
        }
    }
}
