using SIE.MES.WIP.Configs;

namespace SIE.Wpf.MES.WIP.Configs
{
    /// <summary>
    /// 端口类型值 视图配置
    /// </summary>
    public class DevicePortConfigValueViewConfig : WPFViewConfig<DevicePortConfigValue>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.Property(p => p.DevicePort).ShowInDetail();
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.ClearCommands();
            View.Property(p => p.DevicePort);
        }
    }
}
