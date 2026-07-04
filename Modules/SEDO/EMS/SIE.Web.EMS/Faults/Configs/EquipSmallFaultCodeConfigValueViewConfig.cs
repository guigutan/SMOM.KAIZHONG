using SIE.EMS.Faults.Configs;

namespace SIE.Web.EMS.Configs
{
    /// <summary>
    /// 配置设备故障小类视图
    /// </summary>
    public class EquipSmallFaultCodeConfigValueViewConfig : WebViewConfig<EquipSmallFaultCodeConfigValue>
    {
        /// <summary>
        /// 配置表单视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.CodeRule);
        }
    }
}
