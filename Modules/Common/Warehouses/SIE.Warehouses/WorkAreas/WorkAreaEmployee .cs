using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.ShiftTypes;
using System;

namespace SIE.Warehouses
{
    /// <summary>
	/// 
	/// </summary>
	[ChildEntity, Serializable]
    //[CriteriaQuery]
    [Label("工作区与员工关系")]
    public partial class WorkAreaEmployee : DataEntity
    {
        #region 员工 Employee
        /// <summary>
        /// 员工Id
        /// </summary>
        [Label("员工")]
        public static readonly IRefIdProperty EmployeeIdProperty = P<WorkAreaEmployee>.RegisterRefId(e => e.EmployeeId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Employee> EmployeeProperty = P<WorkAreaEmployee>.RegisterRef(e => e.Employee, EmployeeIdProperty);

        /// <summary>
        /// 员工
        /// </summary>
        public Employee Employee
        {
            get { return GetRefEntity(EmployeeProperty); }
            set { SetRefEntity(EmployeeProperty, value); }
        }
        #endregion

        #region 工号 EmployeeCode
        /// <summary>
        /// 工号
        /// </summary>
        [Label("工号")]
        public static readonly Property<string> EmployeeCodeProperty = P<WorkAreaEmployee>.RegisterView(e => e.EmployeeCode, p => p.Employee.Code);

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
        public static readonly Property<string> EmployeeNameProperty = P<WorkAreaEmployee>.RegisterView(e => e.EmployeeName, p => p.Employee.Name);

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
        public static readonly Property<EmployeeStatus> EmployeeStatusProperty = P<WorkAreaEmployee>.RegisterView(e => e.EmployeeStatus, p => p.Employee.EmployeeStatus);

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
        public static readonly Property<string> EmployeeGroupNameProperty = P<WorkAreaEmployee>.RegisterView(e => e.EmployeeGroupName, p => p.Employee.EmployeeGroup.Name);

        /// <summary>
        /// 员工组名称
        /// </summary>
        public string EmployeeGroupName
        {
            get { return this.GetProperty(EmployeeGroupNameProperty); }
        }
        #endregion

        #region 班组名称 WorkGroupName
        /// <summary>
        /// 班组名称
        /// </summary>
        [Label("班组名称")]
        public static readonly Property<string> WorkGroupNameProperty = P<WorkAreaEmployee>.RegisterView(e => e.WorkGroupName, p => p.Employee.WorkGroup.Name);

        /// <summary>
        /// 班组名称
        /// </summary>
        public string WorkGroupName
        {
            get { return this.GetProperty(WorkGroupNameProperty); }
        }
        #endregion

        #region 用户账号 UserCode
        /// <summary>
        /// 用户账号
        /// </summary>
        [Label("用户账号")]
        public static readonly Property<string> UserCodeProperty = P<WorkAreaEmployee>.RegisterView(e => e.UserCode, p => p.Employee.User.Code);

        /// <summary>
        /// 用户账号
        /// </summary>
        public string UserCode
        {
            get { return this.GetProperty(UserCodeProperty); }
        }
        #endregion

        #region 用户状态 UserState
        /// <summary>
        /// 用户状态
        /// </summary>
        [Label("用户状态")]
        public static readonly Property<State> UserStateProperty = P<WorkAreaEmployee>.RegisterView(e => e.UserState, p => p.Employee.User.State);

        /// <summary>
        /// 用户状态
        /// </summary>
        public State UserState
        {
            get { return this.GetProperty(UserStateProperty); }
        }
        #endregion

        #region 在岗情况 WorkSituation
        /// <summary>
        /// 在岗情况
        /// </summary>
        [Label("在岗情况")]
        public static readonly Property<WorkSituation> WorkSituationProperty = P<WorkAreaEmployee>.Register(e => e.WorkSituation);

        /// <summary>
        /// 在岗情况
        /// </summary>
        public WorkSituation WorkSituation
        {
            get { return GetProperty(WorkSituationProperty); }
            set { SetProperty(WorkSituationProperty, value); }
        }
        #endregion

        #region 班制 ShiftType
        /// <summary>
        /// 班制Id
        /// </summary>
        [Label("班制名称")]
        public static readonly IRefIdProperty ShiftTypeIdProperty = P<WorkAreaEmployee>.RegisterRefId(e => e.ShiftTypeId, ReferenceType.Normal);

        /// <summary>
        /// 班制Id
        /// </summary>
        public double? ShiftTypeId
        {
            get { return (double?)GetRefNullableId(ShiftTypeIdProperty); }
            set { SetRefNullableId(ShiftTypeIdProperty, value); }
        }

        /// <summary>
        /// 班制
        /// </summary>
        public static readonly RefEntityProperty<ShiftType> ShiftTypeProperty = P<WorkAreaEmployee>.RegisterRef(e => e.ShiftType, ShiftTypeIdProperty);

        /// <summary>
        /// 班制
        /// </summary>
        public ShiftType ShiftType
        {
            get { return GetRefEntity(ShiftTypeProperty); }
            set { SetRefEntity(ShiftTypeProperty, value); }
        }
        #endregion

        #region  WorkArea
        /// <summary>
        /// Id
        /// </summary>
        public static readonly IRefIdProperty WorkAreaIdProperty = P<WorkAreaEmployee>.RegisterRefId(e => e.WorkAreaId, ReferenceType.Parent);

        /// <summary>
        /// Id
        /// </summary>
        public double WorkAreaId
        {
            get { return (double)GetRefId(WorkAreaIdProperty); }
            set { SetRefId(WorkAreaIdProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly RefEntityProperty<WorkArea> WorkAreaProperty = P<WorkAreaEmployee>.RegisterRef(e => e.WorkArea, WorkAreaIdProperty);

        /// <summary>
        /// 
        /// </summary>
        public WorkArea WorkArea
        {
            get { return GetRefEntity(WorkAreaProperty); }
            set { SetRefEntity(WorkAreaProperty, value); }
        }
        #endregion
    }

    /// <summary>
    ///  实体配置
    /// </summary>
    internal class WorkAreaEmployeeConfig : EntityConfig<WorkAreaEmployee>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WH_WORKAREA_EMP").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}