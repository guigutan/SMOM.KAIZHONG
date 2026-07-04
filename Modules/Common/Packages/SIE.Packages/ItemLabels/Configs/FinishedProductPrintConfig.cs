using SIE.Common.Configs;

namespace SIE.Packages.ItemLabels.Configs
{
    /// <summary>
    /// 成品条码规则配置
    /// </summary>
    [System.ComponentModel.DisplayName("成品条码规则")]
    [System.ComponentModel.Description("成品条码规则")]
    public class FinishedProductPrintConfig : ModuleConfig<FinishedProductPrintConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        private readonly FinishedProductPrintConfigValue defaultValue = new FinishedProductPrintConfigValue { BarcodeRule = null };

        /// <summary>
        /// 默认值
        /// </summary>
        public override FinishedProductPrintConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }
}
