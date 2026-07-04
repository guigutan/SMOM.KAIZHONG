using SIE.Common.Configs;

namespace SIE.Packages.ItemLabels.Configs
{
    /// <summary>
    /// 原材料条码规则和模板
    /// </summary>
    [System.ComponentModel.DisplayName("原材料条码规则和模板")]
    [System.ComponentModel.Description("用于选择供应商退货单单号生成规则,设置物料标签类型为原材料的条码规则和模板")]
    public class SemiFinishedStampingPrintConfig : ModuleConfig<SemiFinishedStampingPrintConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        private readonly SemiFinishedStampingPrintConfigValue defaultValue = new SemiFinishedStampingPrintConfigValue { BarcodeRule = null };

        /// <summary>
        /// 默认值
        /// </summary>
        public override SemiFinishedStampingPrintConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }
}
