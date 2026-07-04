using SIE.EMS.DevicePurs.Configs;

namespace SIE.Web.EMS.DevicePurs.Configs
{
    /// <summary>
    /// 启用设备权限管理配置项的值的视图配置
    /// </summary>
    internal class EnableDevicePermissionsConfigValueViewConfig : WebViewConfig<EnableDevicePermissionsConfigValue>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.Property(p => p.EnableDevicePermissions).UseEnumEditor().Show(ShowInWhere.All);
        }
    }
}
