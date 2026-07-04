using SIE.Warehouses.Stations;

namespace SIE.Web.Warehouses.Stations
{
    /// <summary>
    /// 查询视图配置
    /// </summary>
    public partial class StationCriteriaViewConfig : WebViewConfig<StationCriteria>
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
                View.Property(p => p.State).UseEnumEditor(p => p.AllowBlank = true).Show();
                View.Property(p => p.StationType).UseEnumEditor(p => p.AllowBlank = true).Show();
                View.Property(p => p.WarehouseId).UseWarehouseEditor().Show();
                View.Property(p => p.Routeway).Show();
                View.Property(p => p.Led).Show();
            }
        }
    }
}
