using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Items.Items.Configs
{
    /// <summary>
    /// 重量单位配置项内容
    /// </summary>
    [RootEntity, Serializable]
    [Label("重量单位")]
    public class WeightUnitNoConfigValue : ConfigValue
    {
        #region 单位类型 LengthTypeCode
        /// <summary>
        /// 单位类型
        /// </summary>
        [Label("单位类型")]
        public static readonly Property<string> WeightTypeCodeProperty = P<WeightUnitNoConfigValue>.Register(e => e.WeightTypeCode);

        /// <summary>
        /// 单位类型
        /// </summary>
        public string WeightTypeCode
        {
            get { return GetProperty(WeightTypeCodeProperty); }
            set { SetProperty(WeightTypeCodeProperty, value); }
        }
        #endregion

        /// <summary>
        ///  显示值
        /// </summary>
        /// <returns>string</returns>
        public override string Display()
        {
            return WeightTypeCode;
        }
    }
}
