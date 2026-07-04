using SIE.Common.Configs;

namespace SIE.AbnormalInfo.AbnormalMonitors.Configs
{
    /// <summary>
    /// 推送升级机制循环次数配置项
    /// </summary>
    [System.ComponentModel.DisplayName("推送升级机制循环次数")]
    [System.ComponentModel.Description("推送升级机制，循环推送的次数")]
    public class PushUpgradeRuleTimeConfig : ModuleConfig<PushUpgradeRuleTimeConfigValue>
    {
        /// <summary>
        /// 循环次数
        /// </summary>
        readonly PushUpgradeRuleTimeConfigValue defaultValue = new PushUpgradeRuleTimeConfigValue { CyclicTimes = 3};

        /// <summary>
        /// 默认值
        /// </summary>
        public override PushUpgradeRuleTimeConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }
}
