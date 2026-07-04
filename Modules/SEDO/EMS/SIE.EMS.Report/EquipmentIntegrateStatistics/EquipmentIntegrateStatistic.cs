using SIE.Domain;
using SIE.EMS.Equipments.Accounts;
using SIE.Equipments.EquipAccounts;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Report.EquipmentIntegrateStatistics
{
    /// <summary>
    /// 设备综合统计
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(EquipmentIntegrateStatisticCriteria))]
    [Label("设备综合统计")]
    public class EquipmentIntegrateStatistic : DataEntity
    {
        #region 设备 EquipAccount
        /// <summary>
        /// 设备Id
        /// </summary>
        [Label("设备")]
        public static readonly IRefIdProperty EquipAccountIdProperty =
            P<EquipmentIntegrateStatistic>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Normal);

        /// <summary>
        /// 设备Id
        /// </summary>
        public double EquipAccountId
        {
            get { return (double)this.GetRefId(EquipAccountIdProperty); }
            set { this.SetRefId(EquipAccountIdProperty, value); }
        }

        /// <summary>
        /// 设备
        /// </summary>
        public static readonly RefEntityProperty<EquipAccountSelect> EquipAccountProperty =
            P<EquipmentIntegrateStatistic>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

        /// <summary>
        /// 设备
        /// </summary>
        public EquipAccountSelect EquipAccount
        {
            get { return this.GetRefEntity(EquipAccountProperty); }
            set { this.SetRefEntity(EquipAccountProperty, value); }
        }
        #endregion

        #region 日期 StatisticDate
        /// <summary>
        /// 日期
        /// </summary>
        [Label("日期")]
        public static readonly Property<DateTime> StatisticDateProperty
            = P<EquipmentIntegrateStatistic>.Register(e => e.StatisticDate);

        /// <summary>
        /// 日期
        /// </summary>
        public DateTime StatisticDate
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
            = P<EquipmentIntegrateStatistic>.Register(e => e.PlanningTime);

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
        [Label("运行时长（h）")]
        public static readonly Property<decimal> RunningTimeProperty
            = P<EquipmentIntegrateStatistic>.Register(e => e.RunningTime);

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
        [Label("停机时长（h）")]
        public static readonly Property<decimal> StandbyTimeProperty
            = P<EquipmentIntegrateStatistic>.Register(e => e.StandbyTime);

        /// <summary>
        /// 待机时长（h）
        /// </summary>
        public decimal StandbyTime
        {
            get { return this.GetProperty(StandbyTimeProperty); }
            set { this.SetProperty(StandbyTimeProperty, value); }
        }
        #endregion

        #region 关机时长（h） DownTime
        /// <summary>
        /// 停机时长（h）
        /// </summary>
        [Label("关机时长（h）")]
        public static readonly Property<decimal> DownTimeProperty
            = P<EquipmentIntegrateStatistic>.Register(e => e.DownTime);

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
            = P<EquipmentIntegrateStatistic>.Register(e => e.FailureTime);

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
            = P<EquipmentIntegrateStatistic>.Register(e => e.UnkownTime);

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
            = P<EquipmentIntegrateStatistic>.Register(e => e.OfflineTime);

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
            = P<EquipmentIntegrateStatistic>.Register(e => e.NumberOfFailures);

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
            = P<EquipmentIntegrateStatistic>.Register(e => e.RepairTime);

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
            = P<EquipmentIntegrateStatistic>.Register(e => e.UtilizationRate);

        /// <summary>
        /// 设备利用率
        /// </summary>
        public decimal UtilizationRate
        {
            get { return this.GetProperty(UtilizationRateProperty); }
            set { this.SetProperty(UtilizationRateProperty, value); }
        }
        #endregion


        #region 设备故障总时间 EquipMentFailureTime
        /// <summary>
        /// 设备故障总时间
        /// </summary>
        [Label("设备故障总时间")]
        public static readonly Property<decimal> EquipMentFailureTimeProperty = P<EquipmentIntegrateStatistic>.Register(e => e.EquipMentFailureTime);

        /// <summary>
        /// 设备故障总时间
        /// </summary>
        public decimal EquipMentFailureTime
        {
            get { return this.GetProperty(EquipMentFailureTimeProperty); }
            set { this.SetProperty(EquipMentFailureTimeProperty, value); }
        }
        #endregion

    }

    /// <summary>
    /// 设备综合统计 实体配置
    /// </summary>
    internal class EquipmentIntegrateStatisticConfig : EntityConfig<EquipmentIntegrateStatistic>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_EQUIP_INTG_STAT").MapAllProperties();
        }
    }
}
