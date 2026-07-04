using SIE.Common.Configs;

namespace SIE.EMS.GlobalConfigs
{
    /// <summary>
    /// EMS全局配置项控制器
    /// </summary>
    public class EmsGlobalConfigController : DomainController
    {
        /// <summary>
        /// 获取设备点检和保养异常PDCA管控的配置值
        /// </summary>
        /// <returns></returns>
        public virtual bool GetCheckAndMaintainPdcaConfigValue()
        {
            var configValue = ConfigService.GetConfig(new CheckAndMaintainPdcaConfig());
            if (configValue == null)
            {
                return false;
            }
            else
            {
                return configValue.Pdca == YesNo.Yes;
            }
        }
    }
}
