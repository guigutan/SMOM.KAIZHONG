using SIE.Common.Configs;

namespace SIE.Items.Items.Configs
{
    /// <summary>
    /// 重量单位配置
    /// </summary>
    [System.ComponentModel.DisplayName("重量单位类型")]
    [System.ComponentModel.Description("用于选择重量单位类型,具体类型请参考快码中UNIT_TYPE")]
    public class WeightUnitNoConfig : ModuleConfig<WeightUnitNoConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly WeightUnitNoConfigValue defaultValue = new WeightUnitNoConfigValue { };

        /// <summary>
        /// 默认值
        /// </summary>
        public override WeightUnitNoConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }
}
