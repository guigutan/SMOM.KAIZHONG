using SIE.Common.Configs;

namespace SIE.AbnormalInfo.AbnormalMonitors.Configs
{
    /// <summary>
    /// 预警定义-红牌管理配置项
    /// </summary>
    [System.ComponentModel.DisplayName("预警定义-SPC配置项")]
    [System.ComponentModel.Description("配置SPC使用的预警定义")]
    public class WarnDefineForSpcConfig : ModuleConfig<WarnDefineForSpcConfigValue>
    {
        /// <summary>
        /// 预警定义
        /// </summary>
        readonly WarnDefineForSpcConfigValue defaultValue = new WarnDefineForSpcConfigValue {};

        /// <summary>
        /// 默认值
        /// </summary>
        public override WarnDefineForSpcConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }
}
