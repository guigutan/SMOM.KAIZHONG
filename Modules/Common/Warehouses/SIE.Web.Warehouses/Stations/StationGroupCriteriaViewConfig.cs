using SIE.Warehouses.Stations;

namespace SIE.Web.Warehouses.Stations
{
    /// <summary>
    /// 查询视图配置
    /// </summary>
    public partial class StationGroupCriteriaViewConfig : WebViewConfig<StationGroupCriteria>
    {
        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Show();
                View.Property(p => p.Name).Show();               
                View.Property(p => p.Location).Show();
                View.Property(p => p.WarehouseCode).Show();
            }
        }
    }
}
