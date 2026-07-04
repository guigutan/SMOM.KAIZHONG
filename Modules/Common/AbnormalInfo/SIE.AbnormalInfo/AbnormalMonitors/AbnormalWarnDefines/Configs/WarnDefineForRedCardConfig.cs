using SIE.Common.Configs;

namespace SIE.AbnormalInfo.AbnormalMonitors.Configs
{
    /// <summary>
    /// 预警定义-红牌管理配置项
    /// </summary>
    [System.ComponentModel.DisplayName("预警定义-红牌管理配置项")]
    [System.ComponentModel.Description("配置红牌管理使用的预警定义")]
    public class WarnDefineForRedCardConfig : ModuleConfig<WarnDefineForRedCardConfigValue>
    {
        /// <summary>
        /// 预警定义
        /// </summary>
        readonly WarnDefineForRedCardConfigValue defaultValue = new WarnDefineForRedCardConfigValue {};

        /// <summary>
        /// 默认值
        /// </summary>
        public override WarnDefineForRedCardConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }
}
