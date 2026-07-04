using SIE.Common.Configs;

namespace SIE.Equipments.DeviceControls.Configs
{
    /// <summary>
    /// 设备WebApi地址配置
    /// </summary>
    [System.ComponentModel.DisplayName("设备WebApi地址生成规则")]
    [System.ComponentModel.Description("用于设置设备集成WebApi地址")]
    public class SmdcUrlConfig : ModuleConfig<SmdcUrlConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly SmdcUrlConfigValue defaultValue = new SmdcUrlConfigValue { Url = "" };

        /// <summary>
        /// 默认值 
        /// </summary>
        public override SmdcUrlConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }
}
