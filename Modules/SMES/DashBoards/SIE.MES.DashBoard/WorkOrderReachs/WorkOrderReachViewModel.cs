using SIE.Domain;
using SIE.MES.DashBoard.Reports.Commons;
using SIE.ObjectModel;
using System;

namespace SIE.MES.DashBoard.WorkOrderReachs
{
    /// <summary>
    /// 报表数据来源表ViewModel
    /// </summary>
    [RootEntity, Serializable]
    [Label("报表数据来源表")]
    public class WorkOrderReachViewModel : ViewModel
    {
        #region 年 Year
        /// <summary>
        /// 年
        /// </summary>
        [Label("年")]
        [FieldSettingAttribute("年", FieldArea.FilterArea, 0)]
        public static readonly Property<string> YearProperty = P<WorkOrderReachViewModel>.Register(e => e.Year);

        /// <summary>
        /// 年
        /// </summary>
        public string Year
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
        public static readonly Property<string> MonthProperty = P<WorkOrderReachViewModel>.Register(e => e.Month);

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
        public static readonly Property<string> WeekProperty = P<WorkOrderReachViewModel>.Register(e => e.Week);

        /// <summary>
        /// 周
        /// </summary>
        public string Week
        {
            get { return this.GetProperty(WeekProperty); }
            set { this.SetProperty(WeekProperty, value); }
        }
        #endregion       

        #region 工单计划时间 PlanDate
        /// <summary>
        /// 计划时间
        /// </summary>
        [Label("计划时间")]
        [FieldSettingAttribute("日", FieldArea.ColumnArea, 1)]
        public static readonly Property<DateTime> PlanDateProperty = P<WorkOrderReachViewModel>.Register(e => e.PlanDate);

        /// <summary>
        /// 计划时间
        /// </summary>
        public DateTime PlanDate
        {
            get { return this.GetProperty(PlanDateProperty); }
            set { this.SetProperty(PlanDateProperty, value); }
        }
        #endregion

        #region 行属性 RowName
        /// <summary>
        /// 行属性
        /// </summary>
        [Label("行属性")]
        [FieldSetting("", FieldArea.RowArea, 1, SummaryType = SummaryType.Custom)]
        public static readonly Property<string> RowNameProperty = P<WorkOrderReachViewModel>.Register(e => e.RowName);

        /// <summary>
        /// 行属性
        /// </summary>
        public string RowName
        {
            get { return this.GetProperty(RowNameProperty); }
            set { this.SetProperty(RowNameProperty, value); }
        }
        #endregion

        #region 数据 Data
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数据")]
        [FieldSetting("", FieldArea.DataArea, 0)]
        public static readonly Property<double> DataProperty = P<WorkOrderReachViewModel>.Register(e => e.Data);

        /// <summary>
        /// 数量
        /// </summary>
        public double Data
        {
            get { return GetProperty(DataProperty); }
            set { SetProperty(DataProperty, value); }
        }
        #endregion

        #region 工单总数 TotalQty
        /// <summary>
        /// 工单总数
        /// </summary>
        [Label("工单总数")]
        public static readonly Property<int> TotalQtyProperty = P<WorkOrderReachViewModel>.Register(e => e.TotalQty);

        /// <summary>
        /// 工单总数
        /// </summary>
        public int TotalQty
        {
            get { return GetProperty(TotalQtyProperty); }
            set { SetProperty(TotalQtyProperty, value); }
        }
        #endregion

        #region 准时完工数 CompleteQty
        /// <summary>
        /// 准时完工数
        /// </summary>
        [Label("准时完工数")]
        public static readonly Property<int> CompleteQtyProperty = P<WorkOrderReachViewModel>.Register(e => e.CompleteQty);

        /// <summary>
        /// 准时完工数
        /// </summary>
        public int CompleteQty
        {
            get { return GetProperty(CompleteQtyProperty); }
            set { SetProperty(CompleteQtyProperty, value); }
        }
        #endregion

        #region 工单关闭数 ClosedQty
        /// <summary>
        /// 工单关闭数
        /// </summary>
        [Label("工单关闭数")]
        public static readonly Property<int> ClosedQtyProperty = P<WorkOrderReachViewModel>.Register(e => e.ClosedQty);

        /// <summary>
        /// 工单关闭数
        /// </summary>
        public int ClosedQty
        {
            get { return GetProperty(ClosedQtyProperty); }
            set { SetProperty(ClosedQtyProperty, value); }
        }
        #endregion

        #region 准时达成率 ReachRate
        /// <summary>
        /// 准时达成率
        /// </summary>
        [Label("准时达成率")]
        public static readonly Property<double> ReachRateProperty = P<WorkOrderReachViewModel>.Register(e => e.ReachRate);

        /// <summary>
        /// 准时达成率
        /// </summary>
        public double ReachRate
        {
            get { return this.GetProperty(ReachRateProperty); }
            set { this.SetProperty(ReachRateProperty, value); }
        }
        #endregion

        #region 工单关闭率 ClosedRate
        /// <summary>
        /// 工单关闭率
        /// </summary>
        [Label("工单关闭率")]
        public static readonly Property<double> ClosedRateProperty = P<WorkOrderReachViewModel>.Register(e => e.ClosedRate);

        /// <summary>
        /// 工单关闭率
        /// </summary>
        public double ClosedRate
        {
            get { return this.GetProperty(ClosedRateProperty); }
            set { this.SetProperty(ClosedRateProperty, value); }
        }
        #endregion


        #region 行排序 RowOrder
        /// <summary>
        /// 行排序
        /// </summary>
        [Label("行排序")]
        public static readonly Property<int> RowOrderProperty = P<WorkOrderReachViewModel>.Register(e => e.RowOrder);

        /// <summary>
        /// 行排序
        /// </summary>
        public int RowOrder
        {
            get { return this.GetProperty(RowOrderProperty); }
            set { this.SetProperty(RowOrderProperty, value); }
        }
        #endregion

    }
}