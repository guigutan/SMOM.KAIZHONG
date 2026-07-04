using SIE.AbnormalInfo.AbnormalMonitors.Configs;

namespace SIE.Web.AbnormalInfo.AbnormalMonitors
{
	/// <summary>
	/// FMEA启动后通知推送方式值视图配置
	/// </summary>
	public class AbnormalMonitorInventoryConfigValueViewConfig : WebViewConfig<AbmMonitorInventoryConfigValue>
    {
        /// <summary>
        /// 表单视图配置
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.AutoTask);
        }
    }
}
