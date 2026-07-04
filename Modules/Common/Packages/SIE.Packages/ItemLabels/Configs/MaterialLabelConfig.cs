using SIE.Common.Configs;

namespace SIE.Packages.ItemLabels.Configs
{
    /// <summary>
    /// 原材料条码规则配置
    /// </summary>
    [System.ComponentModel.DisplayName("原材料条码规则")]
    [System.ComponentModel.Description("设置物料标签类型为原材料的条码规则")]
    public class MaterialLabelConfig : ModuleConfig<MaterialLabelConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        private readonly MaterialLabelConfigValue defaultValue = new MaterialLabelConfigValue { BarcodeRule = null };

        /// <summary>
        /// 默认值
        /// </summary>
        public override MaterialLabelConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }
}
