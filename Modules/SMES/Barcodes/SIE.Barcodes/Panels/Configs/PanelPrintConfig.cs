using SIE.Common.Configs;

namespace SIE.Barcodes.Panels.Configs
{
    /// <summary>
    /// 拼板码打印规则配置
    /// </summary>
    [System.ComponentModel.DisplayName("拼板码打印规则配置")]
    [System.ComponentModel.Description("用于拼板码编码规则，打印模板配置")]
    public class PanelPrintConfig : ModuleConfig<PanelPrintConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly PanelPrintConfigValue defaultValue = new PanelPrintConfigValue { BacodeRule = null, LabelTemplate = null };

        /// <summary>
        /// 默认值 
        /// </summary>
        public override PanelPrintConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }
}
