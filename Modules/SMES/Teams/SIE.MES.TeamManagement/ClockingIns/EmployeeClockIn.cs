using SIE.Common.Configs;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using SIE.Resources.ShiftTypes;
using SIE.Resources.WipResources;
using System;

namespace SIE.MES.TeamManagement.ClockingIns
{
    /// <summary>
    /// 员工出勤
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(EmployeeClockInCriteria))]
    [EntityWithConfig(typeof(EmployeeClockInSetConfig))]
    [Label("员工出勤")]
    public partial class EmployeeClockIn : DataEntity
    {
        #region 日期 ClockInDate
        /// <summary>
        /// 日期
        /// </summary>
        [Required]
        [Label("日期")]
        public static readonly Property<DateTime> ClockInDateProperty = P<EmployeeClockIn>.Register(e => e.ClockInDate);

        /// <summary>
        /// 日期(不带时分秒)
        /// </summary>
        public DateTime ClockInDate
        {
            get { return GetProperty(ClockInDateProperty); }
            set { SetProperty(ClockInDateProperty, value); }
        }
        #endregion

        #region 上班打卡时间 OnDutyDate
        /// <summary>
        /// 上班打卡时间
        /// </summary>
        [Label("上班打卡时间")]
        public static readonly Property<DateTime?> OnDutyDateProperty = P<EmployeeClockIn>.Register(e => e.OnDutyDate);

        /// <summary>
        /// 上班打卡时间
        /// </summary>
        public DateTime? OnDutyDate
        {
            get { return GetProperty(OnDutyDateProperty); }
            set { SetProperty(OnDutyDateProperty, value); }
        }
        #endregion

        #region 下班打卡时间 OffDutyDate
        /// <summary>
        /// 下班打卡时间
        /// </summary>
        [Label("下班打卡时间")]
        public static readonly Property<DateTime?> OffDutyDateProperty = P<EmployeeClockIn>.Register(e => e.OffDutyDate);

        /// <summary>
        /// 下班打卡时间
        /// </summary>
        public DateTime? OffDutyDate
        {
            get { return GetProperty(OffDutyDateProperty); }
            set { SetProperty(OffDutyDateProperty, value); }
        }
        #endregion

        #region 出勤状态 OnDutyState
        /// <summary>
        /// 出勤状态
        /// </summary>
        [Label("出勤状态")]
        public static readonly Property<OnDutyState?> OnDutyStateProperty = P<EmployeeClockIn>.Register(e => e.OnDutyState);

        /// <summary>
        /// 出勤状态
        /// </summary>
        public OnDutyState? OnDutyState
        {
            get { return GetProperty(OnDutyStateProperty); }
            set { SetProperty(OnDutyStateProperty, value); }
        }
        #endregion

        #region 员工 Employee
        /// <summary>
        /// 员工Id
        /// </summary>
        public static readonly IRefIdProperty EmployeeIdProperty = P<EmployeeClockIn>.RegisterRefId(e => e.EmployeeId, ReferenceType.Normal);

        /// <summary>
        /// 员工Id
        /// </summary>
        public double EmployeeId
        {
            get { return (double)GetRefId(EmployeeIdProperty); }
            set { SetRefId(EmployeeIdProperty, value); }
        }

        /// <summary>
        /// 员工
        /// </summary>
        public static readonly RefEntityProperty<Employee> EmployeeProperty = P<EmployeeClockIn>.RegisterRef(e => e.Employee, EmployeeIdProperty);

        /// <summary>
        /// 员工
        /// </summary>
        public Employee Employee
        {
            get { return GetRefEntity(EmployeeProperty); }
            set { SetRefEntity(EmployeeProperty, value); }
        }
        #endregion

        #region 班组 WorkGroup
        /// <summary>
        /// 班组Id
        /// </summary>
        public static readonly IRefIdProperty WorkGroupIdProperty = P<EmployeeClockIn>.RegisterRefId(e => e.WorkGroupId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<WorkGroup> WorkGroupProperty = P<EmployeeClockIn>.RegisterRef(e => e.WorkGroup, WorkGroupIdProperty);

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
        public static readonly IRefIdProperty WipResourceIdProperty =
            P<EmployeeClockIn>.RegisterRefId(e => e.WipResourceId, ReferenceType.Normal);

        /// <summary>
        /// 资源Id
        /// </summary>
        public double? WipResourceId
        {
            get { return (double?)this.GetRefNullableId(WipResourceIdProperty); }
            set { this.SetRefNullableId(WipResourceIdProperty, value); }
        }

        /// <summary>
        /// 资源
        /// </summary>
        public static readonly RefEntityProperty<WipResource> WipResourceProperty =
            P<EmployeeClockIn>.RegisterRef(e => e.WipResource, WipResourceIdProperty);

        /// <summary>
        /// 资源
        /// </summary>
        public WipResource WipResource
        {
            get { return this.GetRefEntity(WipResourceProperty); }
            set { this.SetRefEntity(WipResourceProperty, value); }
        }
        #endregion

        #region 车间 WorkShop
        /// <summary>
        /// 车间Id
        /// </summary>
        [Label("车间")]
        public static readonly IRefIdProperty WorkShopIdProperty =
            P<EmployeeClockIn>.RegisterRefId(e => e.WorkShopId, ReferenceType.Normal);

        /// <summary>
        /// 车间Id
        /// </summary>
        public double? WorkShopId
        {
            get { return (double?)this.GetRefNullableId(WorkShopIdProperty); }
            set { this.SetRefNullableId(WorkShopIdProperty, value); }
        }

        /// <summary>
        /// 车间
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> WorkShopProperty =
            P<EmployeeClockIn>.RegisterRef(e => e.WorkShop, WorkShopIdProperty);

        /// <summary>
        /// 车间
        /// </summary>
        public Enterprise WorkShop
        {
            get { return this.GetRefEntity(WorkShopProperty); }
            set { this.SetRefEntity(WorkShopProperty, value); }
        }
        #endregion 

        #region 班次 Shift
        /// <summary>
        /// 班次Id
        /// </summary>
        public static readonly IRefIdProperty ShiftIdProperty = P<EmployeeClockIn>.RegisterRefId(e => e.ShiftId, ReferenceType.Normal);

        /// <summary>
        /// 班次Id
        /// </summary>
        public double? ShiftId
        {
            get { return (double?)GetRefNullableId(ShiftIdProperty); }
            set { SetRefNullableId(ShiftIdProperty, value); }
        }

        /// <summary>
        /// 班次
        /// </summary>
        public static readonly RefEntityProperty<Shift> ShiftProperty = P<EmployeeClockIn>.RegisterRef(e => e.Shift, ShiftIdProperty);

        /// <summary>
        /// 班次
        /// </summary>
        public Shift Shift
        {
            get { return GetRefEntity(ShiftProperty); }
            set { SetRefEntity(ShiftProperty, value); }
        }
        #endregion

        #region 班次开始时间 ShiftBegin
        /// <summary>
        /// 班次开始时间
        /// </summary>
        [Label("班次开始时间")]
        public static readonly Property<DateTime?> ShiftBeginProperty = P<EmployeeClockIn>.Register(e => e.ShiftBegin);

        /// <summary>
        /// 班次开始时间
        /// </summary>
        public DateTime? ShiftBegin
        {
            get { return this.GetProperty(ShiftBeginProperty); }
            set { this.SetProperty(ShiftBeginProperty, value); }
        }
        #endregion

        #region 班次结束时间 ShiftEnd
        /// <summary>
        /// 班次结束时间
        /// </summary>
        [Label("班次结束时间")]
        public static readonly Property<DateTime?> ShiftEndProperty = P<EmployeeClockIn>.Register(e => e.ShiftEnd);

        /// <summary>
        /// 班次结束时间
        /// </summary>
        public DateTime? ShiftEnd
        {
            get { return this.GetProperty(ShiftEndProperty); }
            set { this.SetProperty(ShiftEndProperty, value); }
        }
        #endregion

        #region 出勤工时 AttentHour
        /// <summary>
        /// 出勤工时
        /// </summary>
        [Label("出勤工时")]
        public static readonly Property<decimal?> AttentHourProperty = P<EmployeeClockIn>.Register(e => e.AttentHour);

        /// <summary>
        /// 出勤工时
        /// </summary>
        public decimal? AttentHour
        {
            get { return this.GetProperty(AttentHourProperty); }
            set { this.SetProperty(AttentHourProperty, value); }
        }
        #endregion

        #region 是否借调 IsLoan
        /// <summary>
        /// 是否借调
        /// </summary>
        [Label("是否借调")]
        public static readonly Property<YesNo?> IsLoanProperty = P<EmployeeClockIn>.Register(e => e.IsLoan);

        /// <summary>
        /// 是否借调
        /// </summary>
        public YesNo? IsLoan
        {
            get { return this.GetProperty(IsLoanProperty); }
            set { this.SetProperty(IsLoanProperty, value); }
        }
        #endregion

        #region 员工工号 EmployeeCode 方便卡机实时打卡识别
        /// <summary>
        /// 员工工号
        /// </summary>
        [Label("员工工号")]
        public static readonly Property<string> EmployeeCodeProperty = P<EmployeeClockIn>.Register(e => e.EmployeeCode);

        /// <summary>
        /// 员工工号
        /// </summary>
        public string EmployeeCode
        {
            get { return this.GetProperty(EmployeeCodeProperty); }
            set { this.SetProperty(EmployeeCodeProperty, value); }
        }
        #endregion

        #region 注册视图

        #region 员工姓名 EmployeeName
        /// <summary>
        /// 员工姓名
        /// </summary>
        [Label("员工姓名")]
        public static readonly Property<string> EmployeeNameProperty = P<EmployeeClockIn>.RegisterView(e => e.EmployeeName, e => e.Employee.Name);

        /// <summary>
        /// 员工姓名
        /// </summary>
        public string EmployeeName
        {
            get { return GetProperty(EmployeeNameProperty); }
        }
        #endregion

        #region 员工类型 EmployeeType
        /// <summary>
        /// 员工类型
        /// </summary>
        [Label("员工类型")]
        public static readonly Property<EmployeeType?> EmployeeTypeProperty = P<EmployeeClockIn>.RegisterView(e => e.EmployeeType, e => e.Employee.EmployeeType);

        /// <summary>
        /// 员工类型
        /// </summary>
        public EmployeeType? EmployeeType
        {
            get { return GetProperty(EmployeeTypeProperty); }
        }
        #endregion

        #region 员工性别 EmployeeSex
        /// <summary>
        /// 员工性别
        /// </summary>
        [Label("性别")]
        public static readonly Property<Sex> EmployeeSexProperty = P<EmployeeClockIn>.RegisterView(e => e.EmployeeSex, e => e.Employee.Sex);

        /// <summary>
        /// 员工性别
        /// </summary>
        public Sex EmployeeSex
        {
            get { return GetProperty(EmployeeSexProperty); }
        }
        #endregion

        #region 员工班组 WorkGroup
        /// <summary>
        /// 员工班组
        /// </summary>
        [Label("班组")]
        public static readonly Property<string> WorkGroupNameProperty = P<EmployeeClockIn>.RegisterView(e => e.WorkGroupName, e => e.WorkGroup.Name);

        /// <summary>
        /// 员工班组
        /// </summary>
        public string WorkGroupName
        {
            get { return GetProperty(WorkGroupNameProperty); }
        }
        #endregion

        #endregion

        #region 当前操作者员工类型 UserEmpType
        /// <summary>
        /// 当前操作者员工类型
        /// </summary>
        [Label("员工类型")]
        public static readonly Property<EmployeeType?> UserEmpTypeProperty = P<EmployeeClockIn>.Register(e => e.UserEmpType);

        /// <summary>
        /// 当前操作者员工类型
        /// </summary>
        public EmployeeType? UserEmpType
        {
            get { return this.GetProperty(UserEmpTypeProperty); }
            set { this.SetProperty(UserEmpTypeProperty, value); }
        }
        #endregion

        #region 打卡记录列表 ClockInDetail
        /// <summary>
        /// 打卡记录列表
        /// </summary>
        public static readonly ListProperty<EntityList<ClockInDetail>> ClockInDetailProperty = P<EmployeeClockIn>.RegisterList(e => e.ClockInDetail);

        /// <summary>
        /// 打卡记录列表
        /// </summary>
        public EntityList<ClockInDetail> ClockInDetail
        {
            get { return this.GetLazyList(ClockInDetailProperty); }
        }
        #endregion

        #region 班次时间 ShiftTimes
        /// <summary>
        /// 班次时间
        /// </summary>
        [Label("班次时间")]
        public static readonly Property<string> ShiftTimesProperty = P<EmployeeClockIn>.RegisterReadOnly(
            e => e.ShiftTimes, e => e.GetShiftTimes());
        /// <summary>
        /// 班次时间
        /// </summary>

        public string ShiftTimes
        {
            get { return this.GetProperty(ShiftTimesProperty); }
        }
        private string GetShiftTimes()
        {
            const string hm = "HH:mm";
            if (ShiftId.HasValue)
            {
                if (ShiftEnd.Value.Date > ShiftBegin.Value.Date)
                {
                    return ShiftBegin.Value.ToString(hm) + "-(次日)" + ShiftEnd.Value.ToString(hm);
                }
                else
                {
                    return ShiftBegin.Value.ToString(hm) + "-" + ShiftEnd.Value.ToString(hm);
                }
            }
            else
            {
                return string.Empty;
            }
        }
        #endregion

    }

    /// <summary>
    /// 员工出勤 实体配置
    /// </summary>
    internal class EmployeeClockInConfigs : EntityConfig<EmployeeClockIn>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMP_CLOCK_IN").MapAllProperties();
            Meta.Property(EmployeeClockIn.UserEmpTypeProperty).DontMapColumn();
            Meta.Property(EmployeeClockIn.ClockInDateProperty).ColumnMeta.HasIndex();
            Meta.EnablePhantoms();
        }
    }
}