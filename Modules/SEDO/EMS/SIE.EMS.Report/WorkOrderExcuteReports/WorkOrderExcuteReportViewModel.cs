using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Report.WorkOrderExcuteReports
{
    /// <summary>
    /// 工单执行统计报表
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(WorkOrderExcuteReportViewModelCriteria))]
    [Label("工单执行统计报表")]
    public class WorkOrderExcuteReportViewModel : ViewModel
    {
        #region 统计时间 SummaryTime
        /// <summary>
        /// 统计时间
        /// </summary>
        [Label("统计时间")]
        public static readonly Property<DateTime> SummaryTimeProperty = P<WorkOrderExcuteReportViewModel>.Register(e => e.SummaryTime);

        /// <summary>
        /// 统计时间
        /// </summary>
        public DateTime SummaryTime
        {
            get { return this.GetProperty(SummaryTimeProperty); }
            set { this.SetProperty(SummaryTimeProperty, value); }
        }
        #endregion

        #region 年份 Year
        /// <summary>
        /// 年份
        /// </summary>
        [Label("年份")]
        public static readonly Property<int> YearProperty = P<WorkOrderExcuteReportViewModel>.Register(e => e.Year);

        /// <summary>
        /// 年份
        /// </summary>
        public int Year
        {
            get { return this.GetProperty(YearProperty); }
            set { this.SetProperty(YearProperty, value); }
        }
        #endregion

        #region 月份 Month
        /// <summary>
        /// 月份
        /// </summary>
        [Label("月份")]
        public static readonly Property<int> MonthProperty = P<WorkOrderExcuteReportViewModel>.Register(e => e.Month);

        /// <summary>
        /// 月份
        /// </summary>
        public int Month
        {
            get { return this.GetProperty(MonthProperty); }
            set { this.SetProperty(MonthProperty, value); }
        }
        #endregion

        #region 工单总数 WorkOrderQty
        /// <summary>
        /// 工单总数
        /// </summary>
        [Label("工单总数")]
        public static readonly Property<int> WorkOrderQtyProperty = P<WorkOrderExcuteReportViewModel>.Register(e => e.WorkOrderQty);

        /// <summary>
        /// 入库数量
        /// </summary>
        public int WorkOrderQty
        {
            get { return this.GetProperty(WorkOrderQtyProperty); }
            set { this.SetProperty(WorkOrderQtyProperty, value); }
        }
        #endregion

        #region 工单完成数 CompleteQty
        /// <summary>
        /// 工单完成数
        /// </summary>
        [Label("工单完成数")]
        public static readonly Property<int> CompleteQtyProperty = P<WorkOrderExcuteReportViewModel>.Register(e => e.CompleteQty);

        /// <summary>
        /// 工单完成数
        /// </summary>
        public int CompleteQty
        {
            get { return this.GetProperty(CompleteQtyProperty); }
            set { this.SetProperty(CompleteQtyProperty, value); }
        }
        #endregion

        #region 工单完成率 CompleteRate 
        /// <summary>
        /// 工单完成率
        /// </summary>
        [Label("工单完成率")]
        public static readonly Property<decimal> CompleteRateProperty = P<WorkOrderExcuteReportViewModel>.Register(e => e.CompleteRate);

        /// <summary>
        /// 工单完成率
        /// </summary>
        public decimal CompleteRate
        {
            get { return this.GetProperty(CompleteRateProperty); }
            set { this.SetProperty(CompleteRateProperty, value); }
        }
        #endregion
    }
}

