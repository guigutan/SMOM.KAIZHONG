using SIE.Common.Configs;
using System.ComponentModel;

namespace SIE.ProductIntfc.Configs
{
    /// <summary>
    /// 首检单号配置
    /// </summary>
    [DisplayName("首检单号配置")]
    [Description("用于配置首检单号的生成规则")]
    public class FirstInspNoConfig : ModuleConfig<FirstInspNoConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly FirstInspNoConfigValue defaultValue = new FirstInspNoConfigValue();

        /// <summary>
        /// 默认值
        /// </summary>
        public override FirstInspNoConfigValue DefaultValue
        {
            get
            {
                return defaultValue;
            }
        }
    }
}