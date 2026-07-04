using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.MES.Workbench.KeyPerformances
{
    /// <summary>
    /// 目标值设置
    /// </summary>
    [RootEntity, Serializable]
    [Label("目标值设置")]
    public partial class TargetSetting : DataEntity
    {
        #region 目标值类型 TargetSettingTypecs
        /// <summary>
        /// 目标值类型
        /// </summary>
        [Label("目标值类型")]
        public static readonly Property<TargetSettingType> TargetSettingTypeProperty = P<TargetSetting>.Register(e => e.TargetSettingType);

        /// <summary>
        /// 目标值类型
        /// </summary>
        public TargetSettingType TargetSettingType
        {
            get { return this.GetProperty(TargetSettingTypeProperty); }
            set { this.SetProperty(TargetSettingTypeProperty, value); }
        }
        #endregion

        #region 目标值 TargetValue
        /// <summary>
        /// 目标值
        /// </summary>
        [Label("目标值")]
        public static readonly Property<double> TargetValueProperty = P<TargetSetting>.Register(e => e.TargetValue);

        /// <summary>
        /// 目标值
        /// </summary>
        public double TargetValue
        {
            get { return this.GetProperty(TargetValueProperty); }
            set { this.SetProperty(TargetValueProperty, value); }
        }
        #endregion
    }
}
