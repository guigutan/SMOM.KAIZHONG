using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Items.Items.Configs
{
    /// <summary>
    /// 长度单位配置项内容
    /// </summary>
    [RootEntity, Serializable]
    [Label("长度单位")]
    public class LengthUnitNoConfigValue : ConfigValue
    {
        #region 单位类型 LengthTypeCode
        /// <summary>
        /// 单位类型
        /// </summary>
        [Label("单位类型")]
        public static readonly Property<string> LengthTypeCodeProperty = P<LengthUnitNoConfigValue>.Register(e => e.LengthTypeCode);

        /// <summary>
        /// 单位类型
        /// </summary>
        public string LengthTypeCode
        {
            get { return GetProperty(LengthTypeCodeProperty); }
            set { SetProperty(LengthTypeCodeProperty, value); }
        }
        #endregion

        /// <summary>
        ///  显示值
        /// </summary>
        /// <returns>string</returns>
        public override string Display()
        {
            return LengthTypeCode;
        }
    }
}
