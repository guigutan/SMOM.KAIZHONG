using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.MES.Workbench.MonthFpys
{
    /// <summary>
    /// 直通率数据ViewModel
    /// </summary>
    [RootEntity, Serializable]
    public class FpyRateViewModel : ViewModel
    {
        #region 采集日期 Date
        /// <summary>
        /// 采集日期
        /// </summary>
        [Label("采集日期")]
        public static readonly Property<DateTime> DateProperty = P<FpyRateViewModel>.Register(e => e.Date);

        /// <summary>
        /// 采集日期
        /// </summary>
        public DateTime Date
        {
            get { return this.GetProperty(DateProperty); }
            set { this.SetProperty(DateProperty, value); }
        }
        #endregion        

        #region 直通率 DirectRate
        /// <summary>
        /// 直通率
        /// </summary>
        [Label("直通率")]
        public static readonly Property<decimal> DirectRateProperty = P<FpyRateViewModel>.Register(e => e.DirectRate);

        /// <summary>
        /// 直通率
        /// </summary>
        public decimal DirectRate
        {
            get { return this.GetProperty(DirectRateProperty); }
            set { this.SetProperty(DirectRateProperty, value); }
        }
        #endregion

    }
}
