using SIE.Equipments.DeviceControls.Configs;

namespace SIE.Web.Equipments.DeviceControls.Configs
{
    /// <summary>
    /// 设备WebApi地址配置值视图配置
    /// </summary>
    class SmdcUrlConfigValueViewConfig : WebViewConfig<SmdcUrlConfigValue>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Url).Show(ShowInWhere.All);
            }
        }
    }
}
