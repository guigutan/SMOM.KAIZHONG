using SIE.Domain;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;

namespace SIE.MES.DashBoard.TeamManagement
{
    /// <summary>
    /// 工单准时达成率率报表 查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("评分统计表")]
    public class ScoreRecordVMCriteria : Criteria
    {
        public ScoreRecordVMCriteria()
        {
            OccurDate = new DateRange();
        }

        #region 班组 WorkGroup
        /// <summary>
        /// 班组Id
        /// </summary>
        [Label("班组")]
        [Required]
        public static readonly IRefIdProperty WorkGroupIdProperty =
            P<ScoreRecordVMCriteria>.RegisterRefId(e => e.WorkGroupId, ReferenceType.Normal);

        /// <summary>
        /// 班组Id
        /// </summary>
        public double WorkGroupId
        {
            get { return (double)this.GetRefId(WorkGroupIdProperty); }
            set { this.SetRefId(WorkGroupIdProperty, value); }
        }

        /// <summary>
        /// 班组
        /// </summary>
        public static readonly RefEntityProperty<SIE.Resources.Employees.WorkGroup> WorkGroupProperty =
            P<ScoreRecordVMCriteria>.RegisterRef(e => e.WorkGroup, WorkGroupIdProperty);

        /// <summary>
        /// 班组
        /// </summary>
        public SIE.Resources.Employees.WorkGroup WorkGroup
        {
            get { return this.GetRefEntity(WorkGroupProperty); }
            set { this.SetRefEntity(WorkGroupProperty, value); }
        }
        #endregion

        #region 员工 Employee
        /// <summary>
        /// 操作员Id
        /// </summary>
        [Label("员工")]
        public static readonly IRefIdProperty EmployeeIdProperty =
            P<ScoreRecordVMCriteria>.RegisterRefId(e => e.EmployeeId, ReferenceType.Normal);

        /// <summary>
        /// 操作员Id
        /// </summary>
        public double? EmployeeId
        {
            get { return (double?)this.GetRefNullableId(EmployeeIdProperty); }
            set { this.SetRefNullableId(EmployeeIdProperty, value); }
        }

        /// <summary>
        /// 操作员
        /// </summary>
        public static readonly RefEntityProperty<Employee> EmployeeProperty =
            P<ScoreRecordVMCriteria>.RegisterRef(e => e.Employee, EmployeeIdProperty);

        /// <summary>
        /// 操作员
        /// </summary>
        public Employee Employee
        {
            get { return this.GetRefEntity(EmployeeProperty); }
            set { this.SetRefEntity(EmployeeProperty, value); }
        }
        #endregion

        #region 日期 OccurDate
        /// <summary>
        /// 发生日期
        /// </summary>
        [Label("日期")]
        [Required]
        public static readonly Property<DateRange> OccurDateProperty = P<ScoreRecordVMCriteria>.Register(e => e.OccurDate);

        /// <summary>
        /// 日期
        /// </summary>
        public DateRange OccurDate
        {
            get { return GetProperty(OccurDateProperty); }
            set { SetProperty(OccurDateProperty, value); }
        }
        #endregion

        #region 类型 DateType
        /// <summary>
        /// 类型
        /// </summary>
        [Label("类型")]
        [Required]
        public static readonly Property<DateType> DateTypeProperty = P<ScoreRecordVMCriteria>.Register(e => e.DateType);

        /// <summary>
        /// 类型
        /// </summary>
        public DateType DateType
        {
            get { return this.GetProperty(DateTypeProperty); }
            set { this.SetProperty(DateTypeProperty, value); }
        }
        #endregion

        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<ScoreRecordVMController>().GetScoreRecordsVM(this);
        }
    }
}
