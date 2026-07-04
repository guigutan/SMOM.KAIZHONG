using SIE.Warehouses.Configs;

namespace SIE.Web.Warehouses.Configs
{
    /// <summary>
    /// 库区编码 视图配置
    /// </summary>
    public class StorageAreaCodeConfigValueViewConfig : WebViewConfig<StorageAreaCodeConfigValue>
    {
        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.StorageAreaCodeRule).Show(ShowInWhere.All);
            }
        }
    }
}
