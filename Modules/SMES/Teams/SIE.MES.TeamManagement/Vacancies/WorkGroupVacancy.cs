using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using SIE.Resources.ShiftTypes;
using SIE.Resources.WipResources;
using System;
using System.Collections.Generic;

namespace SIE.MES.TeamManagement.Vacancies
{
    /// <summary>
    /// 班组缺编统计表
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(WorkGroupVacancyCriteria))]
    [Label("班组缺编统计表")]
    public partial class WorkGroupVacancy : DataEntity
    {
        #region 日期 VacancyDate
        /// <summary>
        /// 日期
        /// </summary>
        [Label("日期")]
        public static readonly Property<DateTime> VacancyDateProperty = P<WorkGroupVacancy>.Register(e => e.VacancyDate);

        /// <summary>
        /// 日期
        /// </summary>
        public DateTime VacancyDate
        {
            get { return GetProperty(VacancyDateProperty); }
            set { SetProperty(VacancyDateProperty, value); }
        }
        #endregion

        #region 在编人数（人） ActualQty
        /// <summary>
        /// 在编人数（人）
        /// </summary>
        [Label("在编人数（人）")]
        public static readonly Property<decimal> ActualQtyProperty = P<WorkGroupVacancy>.Register(e => e.ActualQty);

        /// <summary>
        /// 在编人数（人）
        /// </summary>
        public decimal ActualQty
        {
            get { return GetProperty(ActualQtyProperty); }
            set { SetProperty(ActualQtyProperty, value); }
        }
        #endregion

        #region 当日出勤（人） ClockingInQty
        /// <summary>
        /// 当日出勤（人）
        /// </summary>
        [Label("当日出勤（人）")]
        public static readonly Property<decimal> ClockingInQtyProperty = P<WorkGroupVacancy>.Register(e => e.ClockingInQty);

        /// <summary>
        /// 当日出勤（人）
        /// </summary>
        public decimal ClockingInQty
        {
            get { return GetProperty(ClockingInQtyProperty); }
            set { SetProperty(ClockingInQtyProperty, value); }
        }
        #endregion

        #region 出勤异常（人） AbnormalQty
        /// <summary>
        /// 出勤异常（人）
        /// </summary>
        [Label("出勤异常（人）")]
        public static readonly Property<decimal> AbnormalQtyProperty = P<WorkGroupVacancy>.Register(e => e.AbnormalQty);

        /// <summary>
        /// 出勤异常（人）
        /// </summary>
        public decimal AbnormalQty
        {
            get { return GetProperty(AbnormalQtyProperty); }
            set { SetProperty(AbnormalQtyProperty, value); }
        }
        #endregion

        #region 班次 Shift
        /// <summary>
        /// 班次Id
        /// </summary>
        [Label("班次")]
        public static readonly IRefIdProperty ShiftIdProperty = P<WorkGroupVacancy>.RegisterRefId(e => e.ShiftId, ReferenceType.Normal);

        /// <summary>
        /// 班次Id
        /// </summary>
        public double ShiftId
        {
            get { return (double)GetRefId(ShiftIdProperty); }
            set { SetRefId(ShiftIdProperty, value); }
        }

        /// <summary>
        /// 班次
        /// </summary>
        public static readonly RefEntityProperty<Shift> ShiftProperty = P<WorkGroupVacancy>.RegisterRef(e => e.Shift, ShiftIdProperty);

        /// <summary>
        /// 班次
        /// </summary>
        public Shift Shift
        {
            get { return GetRefEntity(ShiftProperty); }
            set { SetRefEntity(ShiftProperty, value); }
        }
        #endregion

        #region 班组 WorkGroup
        /// <summary>
        /// 班组Id
        /// </summary>
        [Label("班组")]
        public static readonly IRefIdProperty WorkGroupIdProperty = P<WorkGroupVacancy>.RegisterRefId(e => e.WorkGroupId, ReferenceType.Normal);

        /// <summary>
        /// 班组Id
        /// </summary>
        public double WorkGroupId
        {
            get { return (double)GetRefId(WorkGroupIdProperty); }
            set { SetRefId(WorkGroupIdProperty, value); }
        }

        /// <summary>
        /// 班组
        /// </summary>
        public static readonly RefEntityProperty<WorkGroup> WorkGroupProperty = P<WorkGroupVacancy>.RegisterRef(e => e.WorkGroup, WorkGroupIdProperty);

        /// <summary>
        /// 班组
        /// </summary>
        public WorkGroup WorkGroup
        {
            get { return GetRefEntity(WorkGroupProperty); }
            set { SetRefEntity(WorkGroupProperty, value); }
        }
        #endregion

        #region 资源 WipResource
        /// <summary>
        /// 资源Id
        /// </summary>
        [Label("资源")]
        public static readonly IRefIdProperty WipResourceIdProperty = P<WorkGroupVacancy>.RegisterRefId(e => e.WipResourceId, ReferenceType.Normal);

        /// <summary>
        /// 资源Id
        /// </summary>
        public double WipResourceId
        {
            get { return (double)GetRefId(WipResourceIdProperty); }
            set { SetRefId(WipResourceIdProperty, value); }
        }

        /// <summary>
        /// 资源
        /// </summary>
        public static readonly RefEntityProperty<WipResource> WipResourceProperty = P<WorkGroupVacancy>.RegisterRef(e => e.WipResource, WipResourceIdProperty);

        /// <summary>
        /// 资源
        /// </summary>
        public WipResource WipResource
        {
            get { return GetRefEntity(WipResourceProperty); }
            set { SetRefEntity(WipResourceProperty, value); }
        }
        #endregion

        #region 车间 WorkShop
        /// <summary>
        /// 车间Id
        /// </summary>
        [Label("车间")]
        public static readonly IRefIdProperty WorkShopIdProperty = P<WorkGroupVacancy>.RegisterRefId(e => e.WorkShopId, ReferenceType.Normal);

        /// <summary>
        /// 车间Id
        /// </summary>
        public double WorkShopId
        {
            get { return (double)GetRefId(WorkShopIdProperty); }
            set { SetRefId(WorkShopIdProperty, value); }
        }

        /// <summary>
        /// 车间
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> WorkShopProperty = P<WorkGroupVacancy>.RegisterRef(e => e.WorkShop, WorkShopIdProperty);

        /// <summary>
        /// 车间
        /// </summary>
        public Enterprise WorkShop
        {
            get { return GetRefEntity(WorkShopProperty); }
            set { SetRefEntity(WorkShopProperty, value); }
        }
        #endregion

        #region 注册视图

        #region 班组名称 WorkGroupName
        /// <summary>
        /// 班组名称
        /// </summary>
        [Label("班组名称")]
        public static readonly Property<string> WorkGroupNameProperty = P<WorkGroupVacancy>.RegisterView(e => e.WorkGroupName, e => e.WorkGroup.Name);

        /// <summary>
        /// 班组名称
        /// </summary>
        public string WorkGroupName
        {
            get { return GetProperty(WorkGroupNameProperty); }
        }
        #endregion

        #region 班组编码 WorkGroupCode
        /// <summary>
        /// 班组编码
        /// </summary>
        [Label("班组编码")]
        public static readonly Property<string> WorkGroupCodeProperty = P<WorkGroupVacancy>.RegisterView(e => e.WorkGroupCode, e => e.WorkGroup.Code);

        /// <summary>
        /// 班组编码
        /// </summary>
        public string WorkGroupCode
        {
            get { return GetProperty(WorkGroupCodeProperty); }
        }
        #endregion

        #endregion

        #region 出勤员工集合 EmployeeIds
        /// <summary>
        /// 出勤员工集合
        /// </summary>
        [Label("出勤员工集合")]
        public static readonly Property<List<double>> EmployeeIdsProperty = P<WorkGroupVacancy>.Register(e => e.EmployeeIds);

        /// <summary>
        /// 出勤员工集合
        /// </summary>
        public List<double> EmployeeIds
        {
            get { return this.GetProperty(EmployeeIdsProperty); }
            set { this.SetProperty(EmployeeIdsProperty, value); }
        }
        #endregion

        /// <summary>
        /// 班次时间
        /// </summary>
        public DateRange ShiftDateRange
        {
            get; set;
        }
    }

    /// <summary>
    /// 班组缺编统计表 实体配置
    /// </summary>
    internal class WorkGroupVacancyConfig : EntityConfig<WorkGroupVacancy>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WG_VACANCY").MapAllProperties();
            Meta.Property(WorkGroupVacancy.EmployeeIdsProperty).DontMapColumn();
            Meta.Property(WorkGroupVacancy.VacancyDateProperty).ColumnMeta.HasIndex();
            Meta.EnablePhantoms();
        }
    }
}