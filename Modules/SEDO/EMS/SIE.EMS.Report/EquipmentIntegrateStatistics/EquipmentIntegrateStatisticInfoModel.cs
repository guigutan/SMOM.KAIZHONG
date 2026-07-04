using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Report.EquipmentIntegrateStatistics
{
    /// <summary>
    /// 设备综合统计
    /// </summary>
    [RootEntity, Serializable]
    public class EquipmentIntegrateStatisticInfoModel : ViewModel
    {
        #region 日期 StatisticDate
        /// <summary>
        /// 日期
        /// </summary>
        [Label("日期")]
        public static readonly Property<string> StatisticDateProperty
            = P<EquipmentIntegrateStatisticInfoModel>.Register(e => e.StatisticDate);

        /// <summary>
        /// 日期
        /// </summary>
        public string StatisticDate
        {
            get { return this.GetProperty(StatisticDateProperty); }
            set { this.SetProperty(StatisticDateProperty, value); }
        }
        #endregion

        #region 计划时间（h) PlanningTime
        /// <summary>
        /// 计划时间（h)
        /// </summary>
        [Label("计划时间（h)")]
        public static readonly Property<decimal> PlanningTimeProperty
            = P<EquipmentIntegrateStatisticInfoModel>.Register(e => e.PlanningTime);

        /// <summary>
        /// 计划时间（h)
        /// </summary>
        public decimal PlanningTime
        {
            get { return this.GetProperty(PlanningTimeProperty); }
            set { this.SetProperty(PlanningTimeProperty, value); }
        }
        #endregion

        #region 运行时长（h） RunningTime
        /// <summary>
        /// 运行时长（h）
        /// </summary>
        [Label("属性名")]
        public static readonly Property<decimal> RunningTimeProperty
            = P<EquipmentIntegrateStatisticInfoModel>.Register(e => e.RunningTime);

        /// <summary>
        /// 运行时长（h）
        /// </summary>
        public decimal RunningTime
        {
            get { return this.GetProperty(RunningTimeProperty); }
            set { this.SetProperty(RunningTimeProperty, value); }
        }
        #endregion

        #region 待机时长（h） StandbyTime
        /// <summary>
        /// 待机时长（h）
        /// </summary>
        [Label("待机时长（h）")]
        public static readonly Property<decimal> StandbyTimeProperty
            = P<EquipmentIntegrateStatisticInfoModel>.Register(e => e.StandbyTime);

        /// <summary>
        /// 待机时长（h）
        /// </summary>
        public decimal StandbyTime
        {
            get { return this.GetProperty(StandbyTimeProperty); }
            set { this.SetProperty(StandbyTimeProperty, value); }
        }
        #endregion

        #region 停机时长（h） DownTime
        /// <summary>
        /// 停机时长（h）
        /// </summary>
        [Label("停机时长（h）")]
        public static readonly Property<decimal> DownTimeProperty
            = P<EquipmentIntegrateStatisticInfoModel>.Register(e => e.DownTime);

        /// <summary>
        /// 停机时长（h）
        /// </summary>
        public decimal DownTime
        {
            get { return this.GetProperty(DownTimeProperty); }
            set { this.SetProperty(DownTimeProperty, value); }
        }
        #endregion

        #region 故障时长（h） FailureTime
        /// <summary>
        /// 故障时长（h）
        /// </summary>
        [Label("故障时长（h）")]
        public static readonly Property<decimal> FailureTimeProperty
            = P<EquipmentIntegrateStatisticInfoModel>.Register(e => e.FailureTime);

        /// <summary>
        /// 故障时长（h）
        /// </summary>
        public decimal FailureTime
        {
            get { return this.GetProperty(FailureTimeProperty); }
            set { this.SetProperty(FailureTimeProperty, value); }
        }
        #endregion

        #region 未知时长（h） UnkownTime
        /// <summary>
        /// 未知时长（h）
        /// </summary>
        [Label("未知时长（h）")]
        public static readonly Property<decimal> UnkownTimeProperty
            = P<EquipmentIntegrateStatisticInfoModel>.Register(e => e.UnkownTime);

        /// <summary>
        /// 未知时长（h）
        /// </summary>
        public decimal UnkownTime
        {
            get { return this.GetProperty(UnkownTimeProperty); }
            set { this.SetProperty(UnkownTimeProperty, value); }
        }
        #endregion

        #region 设备离线时长（h） OfflineTime
        /// <summary>
        /// 设备离线时长（h）
        /// </summary>
        [Label("设备离线时长（h）")]
        public static readonly Property<decimal> OfflineTimeProperty
            = P<EquipmentIntegrateStatisticInfoModel>.Register(e => e.OfflineTime);

        /// <summary>
        /// 设备离线时长（h）
        /// </summary>
        public decimal OfflineTime
        {
            get { return this.GetProperty(OfflineTimeProperty); }
            set { this.SetProperty(OfflineTimeProperty, value); }
        }
        #endregion

        #region 故障次数（次） NumberOfFailures
        /// <summary>
        /// 故障次数（次）
        /// </summary>
        [Label("故障次数（次）")]
        public static readonly Property<int> NumberOfFailuresProperty
            = P<EquipmentIntegrateStatisticInfoModel>.Register(e => e.NumberOfFailures);

        /// <summary>
        /// 故障次数（次）
        /// </summary>
        public int NumberOfFailures
        {
            get { return this.GetProperty(NumberOfFailuresProperty); }
            set { this.SetProperty(NumberOfFailuresProperty, value); }
        }
        #endregion

        #region 维修时长（h） RepairTime
        /// <summary>
        /// 维修时长（h）
        /// </summary>
        [Label("维修时长（h）")]
        public static readonly Property<decimal> RepairTimeProperty
            = P<EquipmentIntegrateStatisticInfoModel>.Register(e => e.RepairTime);

        /// <summary>
        /// 维修时长（h）
        /// </summary>
        public decimal RepairTime
        {
            get { return this.GetProperty(RepairTimeProperty); }
            set { this.SetProperty(RepairTimeProperty, value); }
        }
        #endregion

        #region 设备利用率 UtilizationRate
        /// <summary>
        /// 设备利用率
        /// </summary>
        [Label("设备利用率")]
        public static readonly Property<decimal> UtilizationRateProperty
            = P<EquipmentIntegrateStatisticInfoModel>.Register(e => e.UtilizationRate);

        /// <summary>
        /// 设备利用率
        /// </summary>
        public decimal UtilizationRate
        {
            get { return this.GetProperty(UtilizationRateProperty); }
            set { this.SetProperty(UtilizationRateProperty, value); }
        }
        #endregion

        #region 利用率标准 TargetRate
        /// <summary>
        /// 利用率标准
        /// </summary>
        [Label("利用率标准")]
        public static readonly Property<decimal> TargetRateProperty
            = P<EquipmentIntegrateStatisticInfoModel>.Register(e => e.TargetRate);

        /// <summary>
        /// 利用率标准
        /// </summary>
        public decimal TargetRate
        {
            get { return this.GetProperty(TargetRateProperty); }
            set { this.SetProperty(TargetRateProperty, value); }
        }
        #endregion


    }
}
