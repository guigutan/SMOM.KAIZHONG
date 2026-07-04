using SIE.Common.Configs;

namespace SIE.Items.Items.Configs
{
    /// <summary>
    /// 长度单位配置
    /// </summary>
    [System.ComponentModel.DisplayName("长度单位类型")]
    [System.ComponentModel.Description("用于选择长度单位类型,具体类型请参考快码中UNIT_TYPE")]
    public class LengthUnitNoConfig : ModuleConfig<LengthUnitNoConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly LengthUnitNoConfigValue defaultValue = new LengthUnitNoConfigValue { };

        /// <summary>
        /// 默认值
        /// </summary>
        public override LengthUnitNoConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }
}
