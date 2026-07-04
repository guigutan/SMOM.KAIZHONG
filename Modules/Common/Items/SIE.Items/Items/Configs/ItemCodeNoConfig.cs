using SIE.Common.Configs;

namespace SIE.Items.Items.Configs
{
    /// <summary>
    /// 物料编码配置
    /// </summary>
    [System.ComponentModel.DisplayName("物料编码生成规则")]
    [System.ComponentModel.Description("用于选择物料编码生成的具体规则,具体规则详细请在条码规则进行配置")]
    public class ItemCodeNoConfig : ModuleConfig<ItemCodeNoConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly ItemCodeNoConfigValue defaultValue = new ItemCodeNoConfigValue { ItemCodeRule = null };

        /// <summary>
        /// 默认值
        /// </summary>
        public override ItemCodeNoConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }
}
