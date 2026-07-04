using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.MES.DashBoard.Reports.Commons
{
    /// <summary>
    /// 直通率数据ViewModel
    /// </summary>
    [RootEntity, Serializable]
    public class DirectRateBaseViewModel : ViewModel
    {
        #region 年 Year
        /// <summary>
        /// 年
        /// </summary>
        [Label("年")]
        [FieldSettingAttribute("年", FieldArea.FilterArea, 0)]
        public static readonly Property<int> YearProperty = P<DirectRateBaseViewModel>.Register(e => e.Year);

        /// <summary>
        /// 年
        /// </summary>
        public int Year
        {
            get { return this.GetProperty(YearProperty); }
            set { this.SetProperty(YearProperty, value); }
        }
        #endregion

        #region 月 Month
        /// <summary>
        /// 月
        /// </summary>
        [Label("月")]
        [FieldSetting("月", FieldArea.ColumnArea, 0)]
        public static readonly Property<string> MonthProperty = P<DirectRateBaseViewModel>.Register(e => e.Month);

        /// <summary>
        /// 月
        /// </summary>
        public string Month
        {
            get { return this.GetProperty(MonthProperty); }
            set { this.SetProperty(MonthProperty, value); }
        }
        #endregion

        #region 周 Week
        /// <summary>
        /// 周
        /// </summary>
        [Label("周")]
        [FieldSettingAttribute("周", FieldArea.FilterArea, 1)]
        public static readonly Property<string> WeekProperty = P<DirectRateBaseViewModel>.Register(e => e.Week);

        /// <summary>
        /// 周
        /// </summary>
        public string Week
        {
            get { return this.GetProperty(WeekProperty); }
            set { this.SetProperty(WeekProperty, value); }
        }
        #endregion

        #region 采集日期 Date
        /// <summary>
        /// 采集日期
        /// </summary>
        [Label("采集日期")]
        [FieldSettingAttribute("日", FieldArea.ColumnArea, 1)]
        public static readonly Property<DateTime> DateProperty = P<DirectRateBaseViewModel>.Register(e => e.Date);

        /// <summary>
        /// 采集日期
        /// </summary>
        public DateTime Date
        {
            get { return this.GetProperty(DateProperty); }
            set { this.SetProperty(DateProperty, value); }
        }
        #endregion

        #region 投入数 InputQty
        /// <summary>
        /// 投入数
        /// </summary>
        [Label("投入数")]
        public static readonly Property<decimal> InputQtyProperty = P<DirectRateBaseViewModel>.Register(e => e.InputQty);

        /// <summary>
        /// 投入数
        /// </summary>
        public decimal InputQty
        {
            get { return this.GetProperty(InputQtyProperty); }
            set { this.SetProperty(InputQtyProperty, value); }
        }
        #endregion

        #region 合格数 PassQty
        /// <summary>
        /// 合格数
        /// </summary>
        [Label("合格数")]
        public static readonly Property<decimal> PassQtyProperty = P<DirectRateBaseViewModel>.Register(e => e.PassQty);

        /// <summary>
        /// 合格数
        /// </summary>
        public decimal PassQty
        {
            get { return this.GetProperty(PassQtyProperty); }
            set { this.SetProperty(PassQtyProperty, value); }
        }
        #endregion

        #region 不合格数 FailedQty
        /// <summary>
        /// 不合格数
        /// </summary>
        [Label("不合格数")]
        public static readonly Property<decimal> FailedQtyProperty = P<DirectRateBaseViewModel>.Register(e => e.FailedQty);

        /// <summary>
        /// 不合格数
        /// </summary>
        public decimal FailedQty
        {
            get { return this.GetProperty(FailedQtyProperty); }
            set { this.SetProperty(FailedQtyProperty, value); }
        }
        #endregion

        #region 直通率 DirectRate
        /// <summary>
        /// 直通率
        /// </summary>
        [Label("直通率")]
        [FieldSettingAttribute("直通率", FieldArea.DataArea, 0)]
        public static readonly Property<decimal> DirectRateProperty = P<DirectRateBaseViewModel>.Register(e => e.DirectRate);

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
