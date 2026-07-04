using SIE.Common.Configs;

namespace SIE.Warehouses.Configs
{
    /// <summary>
    /// 事务原因编码生成规则
    /// </summary>
    [System.ComponentModel.DisplayName("事务原因编码生成规则")]
    [System.ComponentModel.Description("事务原因编码生成规则,具体规则详细请在编码规则进行配置")]
    internal class ReasonNoConfig : ModuleConfig<ReasonNoConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly ReasonNoConfigValue defaultValue = new ReasonNoConfigValue { ReasonNoRule = null };

        /// <summary>
        /// 默认值
        /// </summary>
        public override ReasonNoConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }
}
