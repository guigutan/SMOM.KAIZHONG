using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Items.Items.Configs
{
    /// <summary>
    /// 体积单位配置项内容
    /// </summary>
    [RootEntity, Serializable]
    [Label("体积单位")]
    public class VolumeUnitNoConfigValue : ConfigValue
    {
        #region 单位类型 LengthTypeCode
        /// <summary>
        /// 单位类型
        /// </summary>
        [Label("单位类型")]
        public static readonly Property<string> VolumeTypeCodeProperty = P<VolumeUnitNoConfigValue>.Register(e => e.VolumeTypeCode);

        /// <summary>
        /// 单位类型
        /// </summary>
        public string VolumeTypeCode
        {
            get { return GetProperty(VolumeTypeCodeProperty); }
            set { SetProperty(VolumeTypeCodeProperty, value); }
        }
        #endregion

        /// <summary>
        ///  显示值
        /// </summary>
        /// <returns>string</returns>
        public override string Display()
        {
            return VolumeTypeCode;
        }
    }
}
