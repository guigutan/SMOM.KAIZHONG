using SIE.Warehouses;

namespace SIE.Web.Warehouses
{
    /// <summary>
    /// 配置ERP子库查询实体视图
    /// </summary>
    internal class ErpWarehouseCriteriaViewConfig : WebViewConfig<ErpWarehouseCriteria>
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
                View.Property(p => p.WmsInvOrg).Show();
                View.Property(p => p.WarehouseCode).Show();
                View.Property(p => p.StorageAreaCode).Show();
                View.Property(p => p.StorageLocationCode).Show();
            }
        }
    }
}
