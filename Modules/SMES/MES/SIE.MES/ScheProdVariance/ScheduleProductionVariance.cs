using SIE.Core.Items;
using SIE.Domain;
using SIE.KZ.Base.Interfaces;
using SIE.MES.ItemLine;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ScheProdVariance
{
    /// <summary>
    /// 当日排程生产数量差异
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(ScheduleProductionVarianceCriteria))]
    [Label("当日排程生产数量差异")]
    public class ScheduleProductionVariance : DataEntity
    {
        #region 开始时间 BeginDate
        /// <summary>
        /// 开始时间
        /// </summary>
        [Label("开始时间")]
        public static readonly Property<DateTime> BeginDateProperty = P<ScheduleProductionVariance>.Register(e => e.BeginDate);

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime BeginDate
        {
            get { return GetProperty(BeginDateProperty); }
            set { SetProperty(BeginDateProperty, value); }
        }
        #endregion

        #region 结束时间 EndDate
        /// <summary>
        /// 结束时间
        /// </summary>
        [Label("结束时间")]
        public static readonly Property<DateTime> EndDateProperty = P<ScheduleProductionVariance>.Register(e => e.EndDate);

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndDate
        {
            get { return GetProperty(EndDateProperty); }
            set { SetProperty(EndDateProperty, value); }
        }
        #endregion

        #region 计划开始时间 PlanBeginTime
        /// <summary>
        /// 计划开始时间
        /// </summary>
        [Label("计划开始时间")]
        public static readonly Property<DateTime> PlanBeginTimeProperty = P<ScheduleProductionVariance>.Register(e => e.PlanBeginTime);

        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateTime PlanBeginTime
        {
            get { return GetProperty(PlanBeginTimeProperty); }
            set { SetProperty(PlanBeginTimeProperty, value); }
        }
        #endregion

        #region 报工时间 Report_Time
        /// <summary>
        /// 报工时间
        /// </summary>
        [Label("报工时间")]
        public static readonly Property<DateTime> Report_TimeProperty = P<ScheduleProductionVariance>.Register(e => e.Report_Time);

        /// <summary>
        /// 报工时间
        /// </summary>
        public DateTime Report_Time
        {
            get { return GetProperty(Report_TimeProperty); }
            set { SetProperty(Report_TimeProperty, value); }
        }
        #endregion

        #region 排程日期 NowDay
        /// <summary>
        /// 排程日期
        /// </summary>
        [Label("排程日期")]
        public static readonly Property<String> NowDayProperty = P<ScheduleProductionVariance>.Register(e => e.NowDay);

        /// <summary>
        /// 排程日期
        /// </summary>
        public String NowDay
        {
            get { return GetProperty(NowDayProperty); }
            set { SetProperty(NowDayProperty, value); }
        }
        #endregion
        
        #region 工序 ProcessCode
        /// <summary>
        /// 工序
        /// </summary>
        [Label("工序")]
        public static readonly Property<string> ProcessCodeProperty = P<ScheduleProductionVariance>.Register(e => e.ProcessCode);

        /// <summary>
        /// 工序
        /// </summary>
        public string ProcessCode
        {
            get { return this.GetProperty(ProcessCodeProperty); }
            set { this.SetProperty(ProcessCodeProperty, value); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<ScheduleProductionVariance>.Register(e => e.ItemCode);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
            set { this.SetProperty(ItemCodeProperty, value); }
        }
        #endregion

        #region 物料描述 ItemName
        /// <summary>
        /// 物料描述
        /// </summary>
        [Label("物料描述")]
        public static readonly Property<string> ItemNameProperty = P<ScheduleProductionVariance>.Register(e => e.ItemName);

        /// <summary>
        /// 物料描述
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
            set { this.SetProperty(ItemNameProperty, value); }
        }
        #endregion

        #region 旧料号 OldItem
        /// <summary>
        /// 旧料号
        /// </summary>
        [Label("旧料号")]
        public static readonly Property<string> OldItemProperty = P<ScheduleProductionVariance>.Register(e => e.OldItem);

        /// <summary>
        /// 旧料号
        /// </summary>
        public string OldItem
        {
            get { return this.GetProperty(OldItemProperty); }
            set { this.SetProperty(OldItemProperty, value); }
        }
        #endregion

        #region 工单号 WoNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WoNoProperty = P<ScheduleProductionVariance>.Register(e => e.WoNo);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WoNo
        {
            get { return this.GetProperty(WoNoProperty); }
            set { this.SetProperty(WoNoProperty, value); }
        }
        #endregion

        #region 任务单号 No
        /// <summary>
        /// 任务单号
        /// </summary>
        [Label("任务单号")]
        public static readonly Property<string> NoProperty = P<ScheduleProductionVariance>.Register(e => e.No);

        /// <summary>
        /// 任务单号
        /// </summary>
        public string No
        {
            get { return this.GetProperty(NoProperty); }
            set { this.SetProperty(NoProperty, value); }
        }
        #endregion

        #region 班次 ShiftType
        /// <summary>
        /// 班次
        /// </summary>
        [Label("班次")]
        public static readonly Property<string> ShiftTypeProperty = P<ScheduleProductionVariance>.Register(e => e.ShiftType);

        /// <summary>
        /// 班次
        /// </summary>
        public string ShiftType
        {
            get { return this.GetProperty(ShiftTypeProperty); }
            set { this.SetProperty(ShiftTypeProperty, value); }
        }
        #endregion

        #region 排程数量 TotalShiftValue
        /// <summary>
        /// 排程数量
        /// </summary>
        [Label("排程数量")]
        public static readonly Property<decimal> TotalShiftValueProperty = P<ScheduleProductionVariance>.Register(e => e.TotalShiftValue);

        /// <summary>
        /// 排程数量
        /// </summary>
        public decimal TotalShiftValue
        {
            get { return this.GetProperty(TotalShiftValueProperty); }
            set { this.SetProperty(TotalShiftValueProperty, value); }
        }
        #endregion

        #region 报工数量 ReportQty
        /// <summary>
        /// 报工数量
        /// </summary>
        [Label("报工数量")]
        public static readonly Property<decimal> ReportQtyProperty = P<ScheduleProductionVariance>.Register(e => e.ReportQty);

        /// <summary>
        /// 报工数量
        /// </summary>
        public decimal ReportQty
        {
            get { return this.GetProperty(ReportQtyProperty); }
            set { this.SetProperty(ReportQtyProperty, value); }
        }
        #endregion
    }

    internal class ScheduleProductionVarianceConfig : EntityConfig<ScheduleProductionVariance>
    {
        protected override void ConfigMeta()
        {
            Meta.MapView("V_SCHE_PROD_VARIANCE").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}