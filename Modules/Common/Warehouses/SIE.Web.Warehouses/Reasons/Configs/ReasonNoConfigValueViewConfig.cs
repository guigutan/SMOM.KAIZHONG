using SIE.Warehouses.Configs;

namespace SIE.Web.Warehouses.Configs
{
    /// <summary>
    /// 事务原因编码生成 视图配置
    /// </summary>
    public class ReasonNoConfigValueViewConfig : WebViewConfig<ReasonNoConfigValue>
    {
        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.Property(p => p.ReasonNoRule).Show();
        }
    }
}
