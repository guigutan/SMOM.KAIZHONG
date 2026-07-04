using SIE.Common.Configs;

namespace SIE.LES.Distributions.Configs
{
    /// <summary>
    /// 配送单管理生成规则配置
    /// </summary>
    [System.ComponentModel.DisplayName("配送单管理单号生成规则配置")]
    [System.ComponentModel.Description("用于配送单管理单号生成规则配置,具体规则详细请在编码规则进行配置")]
    public class DistributionNoConfig : ModuleConfig<DistributionConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        private readonly DistributionConfigValue defaultValue = new DistributionConfigValue { DistributionNoRule = null };

        /// <summary>
        /// 默认值
        /// </summary>
        public override DistributionConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }
}
