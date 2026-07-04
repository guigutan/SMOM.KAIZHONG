using SIE.Common.Configs;
using System.ComponentModel;

namespace SIE.LES.MaterialReceptions.Configs
{
    /// <summary>
    /// 编码配置
    /// </summary>
    [DisplayName("生产退料配置项")]
    [Description("生产退料配置项规则")]
    public class ReturnMaterialConfig : ModuleConfig<ReturnMaterialConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly ReturnMaterialConfigValue defaultValue = new ReturnMaterialConfigValue();

        /// <summary>
        /// 默认值
        /// </summary>
        public override ReturnMaterialConfigValue DefaultValue
        {
            get
            {
                return defaultValue;
            }
        }
    }
}
