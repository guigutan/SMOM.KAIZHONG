using SIE.Domain;
using SIE.EMS.Enums;
using SIE.Equipments.EquipAccounts;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using SIE.Tech.Processs;
using System;

namespace SIE.EMS.Checks.Records
{
    /// <summary>
    /// 设备点检记录查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("设备点检记录查询实体")]
    public partial class CheckRecordCriteria : Criteria
    {
        #region 点检单号 CheckPlanNo
        /// <summary>
        /// 点检单号
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("点检单号")]
        public static readonly Property<string> CheckPlanNoProperty = P<CheckRecordCriteria>.Register(e => e.CheckPlanNo);

        /// <summary>
        /// 点检单号
        /// </summary>
        public string CheckPlanNo
        {
            get { return GetProperty(CheckPlanNoProperty); }
            set { SetProperty(CheckPlanNoProperty, value); }
        }
        #endregion

        #region 设备编码 EquipAccount
        /// <summary>
        /// 设备编码Id
        /// </summary>
        [Label("设备编码")]
        public static readonly IRefIdProperty EquipAccountIdProperty = P<CheckRecordCriteria>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Normal);

        /// <summary>
        /// 设备编码Id
        /// </summary>
        public double? EquipAccountId
        {
            get { return (double?)GetRefNullableId(EquipAccountIdProperty); }
            set { SetRefNullableId(EquipAccountIdProperty, value); }
        }

        /// <summary>
        /// 设备编码
        /// </summary>
        public static readonly RefEntityProperty<EquipAccountSelect> EquipAccountProperty = P<CheckRecordCriteria>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

        /// <summary>
        /// 设备编码
        /// </summary>
        public EquipAccountSelect EquipAccount
        {
            get { return GetRefEntity(EquipAccountProperty); }
            set { SetRefEntity(EquipAccountProperty, value); }
        }
        #endregion

        #region 机台号 MachineNo
        /// <summary>
        /// 机台号
        /// </summary>
        [Label("机台号")]
        public static readonly Property<string> MachineNoProperty = P<CheckRecordCriteria>.Register(e => e.MachineNo);

        /// <summary>
        /// 机台号
        /// </summary>
        public string MachineNo
        {
            get { return GetProperty(MachineNoProperty); }
            set { SetProperty(MachineNoProperty, value); }
        }
        #endregion

        #region 车间 Workshop
        /// <summary>
        /// 车间Id
        /// </summary>
        [Label("车间")]
        public static readonly IRefIdProperty WorkshopIdProperty = P<CheckRecordCriteria>.RegisterRefId(e => e.WorkshopId, ReferenceType.Normal);

        /// <summary>
        /// 车间Id
        /// </summary>
        public double? WorkshopId
        {
            get { return (double?)GetRefNullableId(WorkshopIdProperty); }
            set { SetRefNullableId(WorkshopIdProperty, value); }
        }

        /// <summary>
        /// 车间
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> WorkshopProperty = P<CheckRecordCriteria>.RegisterRef(e => e.Workshop, WorkshopIdProperty);

        /// <summary>
        /// 车间
        /// </summary>
        public Enterprise Workshop
        {
            get { return GetRefEntity(WorkshopProperty); }
            set { SetRefEntity(WorkshopProperty, value); }
        }
        #endregion

        #region 产线 Line
        /// <summary>
        /// 产线Id
        /// </summary>
        [Label("产线")]
        public static readonly IRefIdProperty LineIdProperty = P<CheckRecordCriteria>.RegisterRefId(e => e.LineId, ReferenceType.Normal);

        /// <summary>
        /// 产线Id
        /// </summary>
        public double? LineId
        {
            get { return (double?)GetRefNullableId(LineIdProperty); }
            set { SetRefNullableId(LineIdProperty, value); }
        }

        /// <summary>
        /// 产线
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> LineProperty = P<CheckRecordCriteria>.RegisterRef(e => e.Line, LineIdProperty);

        /// <summary>
        /// 产线
        /// </summary>
        public Enterprise Line
        {
            get { return GetRefEntity(LineProperty); }
            set { SetRefEntity(LineProperty, value); }
        }
        #endregion

        #region 计划点检日期 CheckDate
        /// <summary>
        /// 计划点检日期
        /// </summary>
        [Label("计划点检日期")]
        public static readonly Property<DateRange> CheckDateProperty = P<CheckRecordCriteria>.Register(e => e.PlanCheckDate);

        /// <summary>
        /// 计划点检日期
        /// </summary>
        public DateRange PlanCheckDate
        {
            get { return GetProperty(CheckDateProperty); }
            set { SetProperty(CheckDateProperty, value); }
        }
        #endregion

        #region 点检状态 ExeState
        /// <summary>
        /// 点检状态
        /// </summary>
        [Label("点检状态")]
        public static readonly Property<CheckExeState?> ExeStateProperty = P<CheckRecordCriteria>.Register(e => e.ExeState);

        /// <summary>
        /// 点检状态
        /// </summary>
        public CheckExeState? ExeState
        {
            get { return GetProperty(ExeStateProperty); }
            set { SetProperty(ExeStateProperty, value); }
        }
        #endregion

        #region 点检部门 Department
        /// <summary>
        /// 点检部门Id
        /// </summary>
        [Label("点检部门")]
        public static readonly IRefIdProperty DepartmentIdProperty = P<CheckRecordCriteria>.RegisterRefId(e => e.DepartmentId, ReferenceType.Normal);

        /// <summary>
        /// 点检部门Id
        /// </summary>
        public double? DepartmentId
        {
            get { return (double?)GetRefNullableId(DepartmentIdProperty); }
            set { SetRefNullableId(DepartmentIdProperty, value); }
        }

        /// <summary>
        /// 点检部门
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> DepartmentProperty = P<CheckRecordCriteria>.RegisterRef(e => e.Department, DepartmentIdProperty);

        /// <summary>
        /// 点检部门
        /// </summary>
        public Enterprise Department
        {
            get { return GetRefEntity(DepartmentProperty); }
            set { SetRefEntity(DepartmentProperty, value); }
        }
        #endregion

        #region 点检执行人 CheckEmployee
        /// <summary>
        /// 点检执行人Id
        /// </summary>
        [Label("点检执行人")]
        public static readonly IRefIdProperty CheckEmployeeIdProperty =
            P<CheckRecordCriteria>.RegisterRefId(e => e.CheckEmployeeId, ReferenceType.Normal);

        /// <summary>
        /// 点检执行人Id
        /// </summary>
        public double? CheckEmployeeId
        {
            get { return (double?)this.GetRefNullableId(CheckEmployeeIdProperty); }
            set { this.SetRefNullableId(CheckEmployeeIdProperty, value); }
        }

        /// <summary>
        /// 点检执行人
        /// </summary>
        public static readonly RefEntityProperty<Employee> CheckEmployeeProperty =
            P<CheckRecordCriteria>.RegisterRef(e => e.CheckEmployee, CheckEmployeeIdProperty);

        /// <summary>
        /// 点检执行人
        /// </summary>
        public Employee CheckEmployee
        {
            get { return this.GetRefEntity(CheckEmployeeProperty); }
            set { this.SetRefEntity(CheckEmployeeProperty, value); }
        }
        #endregion

        #region 执行结果 ExeResult
        /// <summary>
        /// 执行结果
        /// </summary>
        [Label("执行结果")]
        public static readonly Property<ExeResult?> ExeResultProperty = P<CheckRecordCriteria>.Register(e => e.ExeResult);

        /// <summary>
        /// 执行结果
        /// </summary>
        public ExeResult? ExeResult
        {
            get { return this.GetProperty(ExeResultProperty); }
            set { this.SetProperty(ExeResultProperty, value); }
        }
        #endregion

        #region 确认结果 ConfirmResult
        /// <summary>
        /// 确认结果
        /// </summary>
        [Label("确认结果")]
        public static readonly Property<ConfirmResult?> ConfirmResultProperty = P<CheckRecordCriteria>.Register(e => e.ConfirmResult);

        /// <summary>
        /// 确认结果
        /// </summary>
        public ConfirmResult? ConfirmResult
        {
            get { return GetProperty(ConfirmResultProperty); }
            set { SetProperty(ConfirmResultProperty, value); }
        }
        #endregion

        /// <summary>
        /// 重写此方法实现查询
        /// </summary>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<CheckController>().QueryCheckPlanLog(this);
        }
    }
}
