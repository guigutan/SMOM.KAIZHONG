using SIE.Common.Configs;

namespace SIE.Resources.ProcessTechs.Configs
{
    /// <summary>
    /// 制程工艺编码配置
    /// </summary>
    [System.ComponentModel.DisplayName("制程工艺编码生成规则")]
    [System.ComponentModel.Description("用于制程工艺编码生成的具体规则,具体规则详细请在编码规则进行配置")]
    public class ProcessTechNoConfig : ModuleConfig<ProcessTechNoConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly ProcessTechNoConfigValue defaultValue = new ProcessTechNoConfigValue();

        /// <summary>
        /// 默认值
        /// </summary>
        public override ProcessTechNoConfigValue DefaultValue
        {
            get
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 描述
        /// </summary>
        public override string Description
        {
            get
            {
                return "用于配置制程工艺编码生成规则";
            }
        }
    }
}
