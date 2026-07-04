using SIE.Common.Configs;

namespace SIE.AbnormalInfo.AbnormalMonitors
{
    /// <summary>
    /// 指标编码生成规则
    /// </summary>
    [System.ComponentModel.DisplayName("指标编码生成规则")]
    [System.ComponentModel.Description("指标编码生成规则")]
    public class IndicatorNoConfig : ModuleConfig<IndicatorNoConfigValue>
    {
        /// <summary>
        /// 指标编码配置默认值
        /// </summary>
        readonly IndicatorNoConfigValue defaultValue = new IndicatorNoConfigValue { NumberRule = null };

        /// <summary>
        /// 获取默认值
        /// </summary>
        public override IndicatorNoConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }
}
