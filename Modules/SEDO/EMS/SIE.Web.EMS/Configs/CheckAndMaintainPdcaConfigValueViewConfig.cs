using SIE.EMS.GlobalConfigs;

namespace SIE.Web.EMS.Configs
{
    /// <summary>
    /// 设备点检和保养异常PDCA管控配置的配置值的视图配置
    /// </summary>
    public class CheckAndMaintainPdcaConfigValueViewConfig : WebViewConfig<CheckAndMaintainPdcaConfigValue>
    {
        /// <summary>
        /// 配置表单视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.Pdca);
        }
    }
}
