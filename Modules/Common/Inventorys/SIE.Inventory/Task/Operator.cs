using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;

namespace SIE.Inventory.Task
{
    /// <summary>
    /// 任务操作人
    /// </summary>
    [ChildEntity, Serializable]
    [Label("任务操作人")]
    public partial class Operator : DataEntity
    {
        #region 是否主操作人 IsMaster
        /// <summary>
        /// 是否主操作人
        /// </summary>
        [Label("是否主操作人")]
        public static readonly Property<bool> IsMasterProperty = P<Operator>.Register(e => e.IsMaster);

        /// <summary>
        /// 是否主操作人
        /// </summary>
        public bool IsMaster
        {
            get { return GetProperty(IsMasterProperty); }
            set { SetProperty(IsMasterProperty, value); }
        }
        #endregion

        #region 员工 Employee
        /// <summary>
        /// 员工Id
        /// </summary>
        public static readonly IRefIdProperty EmployeeIdProperty = P<Operator>.RegisterRefId(e => e.EmployeeId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Employee> EmployeeProperty = P<Operator>.RegisterRef(e => e.Employee, EmployeeIdProperty);

        /// <summary>
        /// 员工
        /// </summary>
        public Employee Employee
        {
            get { return GetRefEntity(EmployeeProperty); }
            set { SetRefEntity(EmployeeProperty, value); }
        }
        #endregion

        #region 任务管理 TaskManagement
        /// <summary>
        /// 任务管理Id
        /// </summary>
        public static readonly IRefIdProperty TaskManagementIdProperty = P<Operator>.RegisterRefId(e => e.TaskManagementId, ReferenceType.Parent);

        /// <summary>
        /// 任务管理Id
        /// </summary>
        public double TaskManagementId
        {
            get { return (double)GetRefId(TaskManagementIdProperty); }
            set { SetRefId(TaskManagementIdProperty, value); }
        }

        /// <summary>
        /// 任务管理
        /// </summary>
        public static readonly RefEntityProperty<TaskManagement> TaskManagementProperty = P<Operator>.RegisterRef(e => e.TaskManagement, TaskManagementIdProperty);

        /// <summary>
        /// 任务管理
        /// </summary>
        public TaskManagement TaskManagement
        {
            get { return GetRefEntity(TaskManagementProperty); }
            set { SetRefEntity(TaskManagementProperty, value); }
        }
        #endregion

        #region 员工工号 EmployeeCode
        /// <summary>
        /// 员工工号
        /// </summary>
        [Label("员工工号")]
        public static readonly Property<string> EmployeeCodeProperty = P<Operator>.RegisterView(e => e.EmployeeCode, p => p.Employee.Code);

        /// <summary>
        /// 员工工号
        /// </summary>
        public string EmployeeCode
        {
            get { return this.GetProperty(EmployeeCodeProperty); }
        }
        #endregion

        #region 员工姓名 EmployeeName
        /// <summary>
        /// 员工姓名
        /// </summary>
        [Label("员工姓名")]
        public static readonly Property<string> EmployeeNameProperty = P<Operator>.RegisterView(e => e.EmployeeName, p => p.Employee.Name);

        /// <summary>
        /// 员工姓名
        /// </summary>
        public string EmployeeName
        {
            get { return this.GetProperty(EmployeeNameProperty); }
        }
        #endregion

        #region 员工组 EmployeeGroupName
        /// <summary>
        /// 员工组
        /// </summary>
        [Label("员工组")]
        public static readonly Property<string> EmployeeGroupNameProperty = P<Operator>.RegisterView(e => e.EmployeeGroupName, p => p.Employee.EmployeeGroup.Name);

        /// <summary>
        /// 员工组
        /// </summary>
        public string EmployeeGroupName
        {
            get { return this.GetProperty(EmployeeGroupNameProperty); }
        }
        #endregion

        #region 班组 WorkGroupName
        /// <summary>
        /// 班组
        /// </summary>
        [Label("班组")]
        public static readonly Property<string> WorkGroupNameProperty = P<Operator>.RegisterView(e => e.WorkGroupName, p => p.Employee.WorkGroup.Name);

        /// <summary>
        /// 班组
        /// </summary>
        public string WorkGroupName
        {
            get { return this.GetProperty(WorkGroupNameProperty); }
        }
        #endregion
    }

    /// <summary>
    /// 任务操作人 实体配置
    /// </summary>
    internal class OperatorConfig : EntityConfig<Operator>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("TASK_USER").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}