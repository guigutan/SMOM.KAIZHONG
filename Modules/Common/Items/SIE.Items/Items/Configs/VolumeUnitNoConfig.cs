using SIE.Common.Configs;

namespace SIE.Items.Items.Configs
{
    /// <summary>
    /// 体积单位配置
    /// </summary>
    [System.ComponentModel.DisplayName("体积单位类型")]
    [System.ComponentModel.Description("用于选择体积单位类型,具体类型请参考快码中UNIT_TYPE")]
    public class VolumeUnitNoConfig : ModuleConfig<VolumeUnitNoConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly VolumeUnitNoConfigValue defaultValue = new VolumeUnitNoConfigValue { };

        /// <summary>
        /// 默认值
        /// </summary>
        public override VolumeUnitNoConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }
}
