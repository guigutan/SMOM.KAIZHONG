using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.MES.DashBoard.Reports.FpySettings
{
    /// <summary>
    /// 直通率设置
    /// </summary>
    [RootEntity, Serializable]
    [Label("直通率设置")]
    public partial class FpySetting : DataEntity
    {
        #region 直通率期望值 Desired
        /// <summary>
        /// 直通率期望值
        /// </summary>
        [Label("直通率期望值")]
        [MinValue(0)]
        [MaxValue(100)]
        public static readonly Property<decimal> DesiredProperty = P<FpySetting>.Register(e => e.Desired);

        /// <summary>
        /// 直通率期望值
        /// </summary>
        public decimal Desired
        {
            get { return GetProperty(DesiredProperty); }
            set { SetProperty(DesiredProperty, value); }
        }
        #endregion

        #region 直通率预警值 Alarm
        /// <summary>
        /// 直通率预警值
        /// </summary>
        [Label("直通率预警值")]
        [MinValue(0)]
        [MaxValue(100)]
        public static readonly Property<decimal> AlarmProperty = P<FpySetting>.Register(e => e.Alarm);

        /// <summary>
        /// 直通率预警值
        /// </summary>
        public decimal Alarm
        {
            get { return GetProperty(AlarmProperty); }
            set { SetProperty(AlarmProperty, value); }
        }
        #endregion
    }
}