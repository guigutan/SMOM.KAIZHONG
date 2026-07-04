using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.Report.SparePartMitReports
{

    /// <summary>
    /// 备件库综合统计表
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(SparePartMixReportViewModelCriteria))]
    [Label("备件库综合统计表")]
    public class SparePartMixtReportViewModel : ViewModel
    {
        #region 统计时间 SummaryTime
        /// <summary>
        /// 统计时间
        /// </summary>
        [Label("统计时间")]
        public static readonly Property<DateTime> SummaryTimeProperty = P<SparePartMixtReportViewModel>.Register(e => e.SummaryTime);

        /// <summary>
        /// 统计时间
        /// </summary>
        public DateTime SummaryTime
        {
            get { return this.GetProperty(SummaryTimeProperty); }
            set { this.SetProperty(SummaryTimeProperty, value); }
        }
        #endregion

        #region 月份 Month
        /// <summary>
        /// 月份
        /// </summary>
        [Label("月份")]
        public static readonly Property<int> MonthProperty = P<SparePartMixtReportViewModel>.Register(e => e.Month);

        /// <summary>
        /// 月份
        /// </summary>
        public int Month
        {
            get { return this.GetProperty(MonthProperty); }
            set { this.SetProperty(MonthProperty, value); }
        }
        #endregion

        #region 年份 Year
        /// <summary>
        /// 年份
        /// </summary>
        [Label("年份")]
        public static readonly Property<int> YearProperty = P<SparePartMixtReportViewModel>.Register(e => e.Year);

        /// <summary>
        /// 年份
        /// </summary>
        public int Year
        {
            get { return this.GetProperty(YearProperty); }
            set { this.SetProperty(YearProperty, value); }
        }
        #endregion

        #region 显示时间 TimeDispaly
        /// <summary>
        /// 月份
        /// </summary>
        [Label("月份")]
        public static readonly Property<string> TimeDispalyProperty = P<SparePartMixtReportViewModel>.RegisterReadOnly(
            e => e.TimeDispaly, e => e.GetTimeDispaly(), MonthProperty, YearProperty);
        /// <summary>
        /// 显示时间
        /// </summary>

        public string TimeDispaly
        {
            get { return this.GetProperty(TimeDispalyProperty); }
        }
        private string GetTimeDispaly()
        {
            return "{0}年{1}月".L10nFormat(Year,Month);
        }
        #endregion

        #region 周转率值 TurnoverRate
        /// <summary>
        /// 周转率值
        /// </summary>
        [Label("周转率值")]
        public static readonly Property<decimal> TurnoverRateProperty = P<SparePartMixtReportViewModel>.Register(e => e.TurnoverRate);

        /// <summary>
        /// 周转率
        /// </summary>
        public decimal TurnoverRate
        {
            get { return this.GetProperty(TurnoverRateProperty); }
            set { this.SetProperty(TurnoverRateProperty, value); }
        }
        #endregion


        #region 入库数量 ReceiptQty
        /// <summary>
        /// 入库数量
        /// </summary>
        [Label("入库数量")]
        public static readonly Property<int> ReceiptQtyProperty = P<SparePartMixtReportViewModel>.Register(e => e.ReceiptQty);

        /// <summary>
        /// 入库数量
        /// </summary>
        public int ReceiptQty
        {
            get { return this.GetProperty(ReceiptQtyProperty); }
            set { this.SetProperty(ReceiptQtyProperty, value); }
        }
        #endregion

        #region 入库金额 ReceiptAmount
        /// <summary>
        /// 入库金额
        /// </summary>
        [Label("入库金额")]
        public static readonly Property<decimal> ReceiptAmountProperty = P<SparePartMixtReportViewModel>.Register(e => e.ReceiptAmount);

        /// <summary>
        /// 入库金额
        /// </summary>
        public decimal ReceiptAmount
        {
            get { return this.GetProperty(ReceiptAmountProperty); }
            set { this.SetProperty(ReceiptAmountProperty, value); }
        }
        #endregion

        #region 出库金额 ExWarehouseAmount
        /// <summary>
        /// 出库金额
        /// </summary>
        [Label("出库金额")]
        public static readonly Property<decimal> ExWarehouseAmountProperty = P<SparePartMixtReportViewModel>.Register(e => e.ExWarehouseAmount);

        /// <summary>
        /// 出库金额
        /// </summary>
        public decimal ExWarehouseAmount
        {
            get { return this.GetProperty(ExWarehouseAmountProperty); }
            set { this.SetProperty(ExWarehouseAmountProperty, value); }
        }
        #endregion

        #region 出库数量 ExWarehouseQty
        /// <summary>
        /// 出库数量
        /// </summary>
        [Label("出库数量")]
        public static readonly Property<int> ExWarehouseQtyProperty = P<SparePartMixtReportViewModel>.Register(e => e.ExWarehouseQty);

        /// <summary>
        /// 出库数量
        /// </summary>
        public int ExWarehouseQty
        {
            get { return this.GetProperty(ExWarehouseQtyProperty); }
            set { this.SetProperty(ExWarehouseQtyProperty, value); }
        }
        #endregion

        #region 月末结余数 MonthSurplusQty
        /// <summary>
        /// 月末结余数
        /// </summary>
        [Label("月末结余数")]
        public static readonly Property<int> MonthSurplusQtyProperty = P<SparePartMixtReportViewModel>.Register(e => e.MonthSurplusQty);

        /// <summary>
        /// 月末结余数
        /// </summary>
        public int MonthSurplusQty
        {
            get { return this.GetProperty(MonthSurplusQtyProperty); }
            set { this.SetProperty(MonthSurplusQtyProperty, value); }
        }
        #endregion

        #region 月末结余金额 MonthSurplusAmount
        /// <summary>
        /// 月末结余金额
        /// </summary>
        [Label("月末结余金额")]
        public static readonly Property<decimal> MonthSurplusAmountProperty = P<SparePartMixtReportViewModel>.Register(e => e.MonthSurplusAmount);

        /// <summary>
        /// 月末结余金额
        /// </summary>
        public decimal MonthSurplusAmount
        {
            get { return this.GetProperty(MonthSurplusAmountProperty); }
            set { this.SetProperty(MonthSurplusAmountProperty, value); }
        }
        #endregion

    }
}
