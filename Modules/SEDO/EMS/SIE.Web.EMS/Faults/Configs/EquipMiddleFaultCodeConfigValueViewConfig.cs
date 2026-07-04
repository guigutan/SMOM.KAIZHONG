using SIE.EMS.Faults.Configs;

namespace SIE.Web.EMS.Configs
{
    /// <summary>
    /// 配置设备故障中类视图
    /// </summary>
    public class EquipMiddleFaultCodeConfigValueViewConfig : WebViewConfig<EquipMiddleFaultCodeConfigValue>
    {
        /// <summary>
        /// 配置表单视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.NumberRule);
        }
    }
}
