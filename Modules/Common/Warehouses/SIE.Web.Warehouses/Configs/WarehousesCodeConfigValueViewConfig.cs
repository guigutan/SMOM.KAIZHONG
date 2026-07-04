using SIE.Warehouses.Configs;

namespace SIE.Web.Warehouses.Configs
{
    /// <summary>
    /// 仓库编码 视图配置
    /// </summary>
    public class WarehousesCodeConfigValueViewConfig : WebViewConfig<WarehousesCodeConfigValue>
    {
        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.WarehousesCodeRule).Show(ShowInWhere.All);
            }
        }
    }
}
