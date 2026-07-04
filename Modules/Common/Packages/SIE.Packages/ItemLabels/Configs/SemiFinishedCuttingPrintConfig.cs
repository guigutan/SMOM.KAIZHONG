using SIE.Common.Configs;

namespace SIE.Packages.ItemLabels.Configs
{
    /// <summary>
    /// 半成品条码规则
    /// </summary>
    [System.ComponentModel.DisplayName("半成品条码规则")]
    [System.ComponentModel.Description("设置物料标签类型为开料的条码规则和模板")]
    public class SemiFinishedCuttingPrintConfig : ModuleConfig<SemiFinishedCuttingPrintConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        private readonly SemiFinishedCuttingPrintConfigValue defaultValue = new SemiFinishedCuttingPrintConfigValue { BarcodeRule = null };

        /// <summary>
        /// 默认值
        /// </summary>
        public override SemiFinishedCuttingPrintConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }
}
