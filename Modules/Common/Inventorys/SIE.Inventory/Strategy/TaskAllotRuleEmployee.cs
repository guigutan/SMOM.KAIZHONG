using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Warehouses;
using System;

namespace SIE.Inventory.Strategy
{
    /// <summary>
    /// 员工
    /// </summary>
    [ChildEntity, Serializable]
    [Label("员工")]
    public partial class TaskAllotRuleEmployee : DataEntity
    {
        #region 员工 Employee
        /// <summary>
        /// 员工Id
        /// </summary>
        [Label("员工")]
        public static readonly IRefIdProperty EmployeeIdProperty = P<TaskAllotRuleEmployee>.RegisterRefId(e => e.EmployeeId, ReferenceType.Normal);

        /// <summary>
        /// 员工Id
        /// </summary>
        public double EmployeeId
        {
            get { return (double)GetRefNullableId(EmployeeIdProperty); }
            set { SetRefNullableId(EmployeeIdProperty, value); }
        }

        /// <summary>
        /// 员工
        /// </summary>        
        public static readonly RefEntityProperty<Employee> EmployeeProperty = P<TaskAllotRuleEmployee>.RegisterRef(e => e.Employee, EmployeeIdProperty);

        /// <summary>
        /// 员工
        /// </summary>
        public Employee Employee
        {
            get { return GetRefEntity(EmployeeProperty); }
            set { SetRefEntity(EmployeeProperty, value); }
        }
        #endregion

        #region 任务分配规则 TaskAllotRule
        /// <summary>
        /// 任务分配规则Id
        /// </summary>
        [Label("任务分配规则")]
        public static readonly IRefIdProperty TaskAllotRuleIdProperty = P<TaskAllotRuleEmployee>.RegisterRefId(e => e.TaskAllotRuleId, ReferenceType.Parent);

        /// <summary>
        /// 任务分配规则Id
        /// </summary>
        public double TaskAllotRuleId
        {
            get { return (double)GetRefNullableId(TaskAllotRuleIdProperty); }
            set { SetRefNullableId(TaskAllotRuleIdProperty, value); }
        }

        /// <summary>
        /// 任务分配规则
        /// </summary>
        public static readonly RefEntityProperty<TaskAllotRule> TaskAllotRuleProperty = P<TaskAllotRuleEmployee>.RegisterRef(e => e.TaskAllotRule, TaskAllotRuleIdProperty);

        /// <summary>
        /// 任务分配规则
        /// </summary>
        public TaskAllotRule TaskAllotRule
        {
            get { return GetRefEntity(TaskAllotRuleProperty); }
            set { SetRefEntity(TaskAllotRuleProperty, value); }
        }
        #endregion

        #region 工号 EmployeeCode
        /// <summary>
        /// 工号
        /// </summary>
        [Label("工号")]
        public static readonly Property<string> EmployeeCodeProperty = P<TaskAllotRuleEmployee>.RegisterView(e => e.EmployeeCode, p => p.Employee.Code);

        /// <summary>
        /// 工号
        /// </summary>
        public string EmployeeCode
        {
            get { return this.GetProperty(EmployeeCodeProperty); }
        }
        #endregion

        #region 姓名 EmployeeName
        /// <summary>
        /// 姓名
        /// </summary>
        [Label("姓名")]
        public static readonly Property<string> EmployeeNameProperty = P<TaskAllotRuleEmployee>.RegisterView(e => e.EmployeeName, p => p.Employee.Name);

        /// <summary>
        /// 姓名
        /// </summary>
        public string EmployeeName
        {
            get { return this.GetProperty(EmployeeNameProperty); }
        }
        #endregion

        #region 员工状态 EmployeeStatus
        /// <summary>
        /// 员工状态
        /// </summary>
        [Label("员工状态")]
        public static readonly Property<EmployeeStatus> EmployeeStatusProperty = P<TaskAllotRuleEmployee>.RegisterView(e => e.EmployeeStatus, p => p.Employee.EmployeeStatus);

        /// <summary>
        /// 员工状态
        /// </summary>
        public EmployeeStatus EmployeeStatus
        {
            get { return this.GetProperty(EmployeeStatusProperty); }
        }
        #endregion

        #region 员工组名称 EmployeeGroupName
        /// <summary>
        /// 员工组名称
        /// </summary>
        [Label("员工组名称")]
        public static readonly Property<string> EmployeeGroupNameProperty = P<TaskAllotRuleEmployee>.RegisterView(e => e.EmployeeGroupName, p => p.Employee.EmployeeGroup.Name);

        /// <summary>
        /// 员工组名称
        /// </summary>
        public string EmployeeGroupName
        {
            get { return this.GetProperty(EmployeeGroupNameProperty); }
        }
        #endregion

        #region 班组 EmployeeWrokGroupName
        /// <summary>
        /// 班组
        /// </summary>
        [Label("班组")]
        public static readonly Property<string> EmployeeWrokGroupNameProperty = P<TaskAllotRuleEmployee>.RegisterView(e => e.EmployeeWrokGroupName, p => p.Employee.WorkGroup.Name);

        /// <summary>
        /// 班组
        /// </summary>
        public string EmployeeWrokGroupName
        {
            get { return this.GetProperty(EmployeeWrokGroupNameProperty); }
        }
        #endregion

        #region 用户账号 UserCode
        /// <summary>
        /// 用户账号
        /// </summary>
        [Label("用户账号")]
        public static readonly Property<string> UserCodeProperty = P<TaskAllotRuleEmployee>.RegisterView(e => e.UserCode, p => p.Employee.User.Code);

        /// <summary>
        /// 用户账号
        /// </summary>
        public string UserCode
        {
            get { return this.GetProperty(UserCodeProperty); }
        }
        #endregion

    }

    /// <summary>
    /// 任务分配规则员工 实体配置
    /// </summary>
    internal class TaskAllotRuleEmployeeConfig : EntityConfig<TaskAllotRuleEmployee>
    {
        /// <summary>
        /// 配置数据库的映射
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("TASK_ALLOT_RULE_EMP").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}