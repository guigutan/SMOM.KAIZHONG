using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.MES.TeamManagement.ClockingIns
{
    /// <summary>
    /// 查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("员工出勤查询实体")]
    public class EmployeeClockInCriteria : Criteria
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public EmployeeClockInCriteria()
        {
            ShiftRange = new DateRange();
            ShiftRange.DateTimePart = DateTimePart.Time;
        }

        #region 工号 EmployeeCode
        /// <summary>
        /// 工号
        /// </summary>
        [Label("工号")]
        public static readonly Property<string> EmployeeCodeProperty = P<EmployeeClockInCriteria>.Register(e => e.EmployeeCode);

        /// <summary>
        /// 工号
        /// </summary>
        public string EmployeeCode
        {
            get { return this.GetProperty(EmployeeCodeProperty); }
            set { this.SetProperty(EmployeeCodeProperty, value); }
        }
        #endregion

        #region 姓名 EmployeeName
        /// <summary>
        /// 姓名
        /// </summary>
        [Label("姓名")]
        public static readonly Property<string> EmployeeNameProperty = P<EmployeeClockInCriteria>.Register(e => e.EmployeeName);

        /// <summary>
        /// 姓名
        /// </summary>
        public string EmployeeName
        {
            get { return this.GetProperty(EmployeeNameProperty); }
            set { this.SetProperty(EmployeeNameProperty, value); }
        }
        #endregion

        #region 班组 WorkGroup
        /// <summary>
        /// 班组Id
        /// </summary>
        [Label("班组")]
        public static readonly IRefIdProperty WorkGroupIdProperty =
            P<EmployeeClockInCriteria>.RegisterRefId(e => e.WorkGroupId, ReferenceType.Normal);

        /// <summary>
        /// 班组Id
        /// </summary>
        public double? WorkGroupId
        {
            get { return (double?)this.GetRefNullableId(WorkGroupIdProperty); }
            set { this.SetRefNullableId(WorkGroupIdProperty, value); }
        }

        /// <summary>
        /// 班组
        /// </summary>
        public static readonly RefEntityProperty<SIE.Resources.Employees.WorkGroup> WorkGroupProperty =
            P<EmployeeClockInCriteria>.RegisterRef(e => e.WorkGroup, WorkGroupIdProperty);

        /// <summary>
        /// 班组
        /// </summary>
        public SIE.Resources.Employees.WorkGroup WorkGroup
        {
            get { return this.GetRefEntity(WorkGroupProperty); }
            set { this.SetRefEntity(WorkGroupProperty, value); }
        }
        #endregion

        #region 日期 EmployeeDate
        /// <summary>
        /// 日期
        /// </summary>
        [Label("日期")]
        public static readonly Property<DateTime?> EmployeeDateProperty = P<EmployeeClockInCriteria>.Register(e => e.EmployeeDate);

        /// <summary>
        /// 日期
        /// </summary>
        public DateTime? EmployeeDate
        {
            get { return this.GetProperty(EmployeeDateProperty); }
            set { this.SetProperty(EmployeeDateProperty, value); }
        }
        #endregion

        #region 班次时间 ShiftRange
        /// <summary>
        /// 班次时间
        /// </summary>
        [Label("时间范围")]
        public static readonly Property<DateRange> ShiftRangeProperty = P<EmployeeClockInCriteria>.Register(e => e.ShiftRange);

        /// <summary>
        /// 班次时间
        /// </summary>
        public DateRange ShiftRange
        {
            get { return this.GetProperty(ShiftRangeProperty); }
            set { this.SetProperty(ShiftRangeProperty, value); }
        }
        #endregion

        #region 出勤状态 OnDutyState
        /// <summary>
        /// 出勤状态
        /// </summary>
        [Label("出勤状态")]
        public static readonly Property<OnDutyState?> OnDutyStateProperty = P<EmployeeClockInCriteria>.Register(e => e.OnDutyState);

        /// <summary>
        /// 出勤状态
        /// </summary>
        public OnDutyState? OnDutyState
        {
            get { return GetProperty(OnDutyStateProperty); }
            set { SetProperty(OnDutyStateProperty, value); }
        }
        #endregion

        /// <summary>
        /// 获取查询结果
        /// </summary>
        /// <returns>员工出勤列表</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<ClockInController>().GetEmployeeClockIns(this);
        }
    }
}