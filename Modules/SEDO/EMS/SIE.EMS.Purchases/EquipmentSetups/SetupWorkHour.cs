using SIE.Domain;
using SIE.EMS.Purchases.Enums;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipAccounts;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;

namespace SIE.EMS.Purchases.EquipmentSetups
{
    /// <summary>
    /// 安装调试工时登记
    /// </summary>
    [ChildEntity, Serializable]
    [Label("安装调试工时登记")]
    public partial class SetupWorkHour : DataEntity
    {
        #region 工时 EquipmentSetup
        /// <summary>
        /// 工时Id
        /// </summary>
        public static readonly IRefIdProperty EquipmentSetupIdProperty = P<SetupWorkHour>.RegisterRefId(e => e.EquipmentSetupId, ReferenceType.Parent);

        /// <summary>
        /// 工时Id
        /// </summary>
        public double EquipmentSetupId
        {
            get { return (double)GetRefId(EquipmentSetupIdProperty); }
            set { SetRefId(EquipmentSetupIdProperty, value); }
        }

        /// <summary>
        /// 工时
        /// </summary>
        public static readonly RefEntityProperty<EquipmentSetup> EquipmentSetupProperty = P<SetupWorkHour>.RegisterRef(e => e.EquipmentSetup, EquipmentSetupIdProperty);

        /// <summary>
        /// 工时
        /// </summary>
        public EquipmentSetup EquipmentSetup
        {
            get { return GetRefEntity(EquipmentSetupProperty); }
            set { SetRefEntity(EquipmentSetupProperty, value); }
        }
        #endregion

        #region 开始时间 StartDateTime
        /// <summary>
        /// 开始时间
        /// </summary>
        [Label("开始时间")]
        public static readonly Property<DateTime> StartDateTimeProperty = P<SetupWorkHour>.Register(e => e.StartDateTime);

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartDateTime
        {
            get { return GetProperty(StartDateTimeProperty); }
            set { SetProperty(StartDateTimeProperty, value); }
        }
        #endregion

        #region 结束时间 EndDateTime
        /// <summary>
        /// 结束时间
        /// </summary>
        [Label("结束时间")]
        public static readonly Property<DateTime> EndDateTimeProperty = P<SetupWorkHour>.Register(e => e.EndDateTime);

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndDateTime
        {
            get { return GetProperty(EndDateTimeProperty); }
            set { SetProperty(EndDateTimeProperty, value); }
        }
        #endregion

        #region 工时(H) Hours
        /// <summary>
        /// 工时(H)
        /// </summary>
        [Label("工时(h)")]
        [MinValue(0)]
        public static readonly Property<decimal> HoursProperty = P<SetupWorkHour>.Register(e => e.Hours);

        /// <summary>
        /// 工时(H)
        /// </summary>
        public decimal Hours
        {
            get { return GetProperty(HoursProperty); }
            set { SetProperty(HoursProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(1000)]
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<SetupWorkHour>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 工作计划 EquipmentSetupPlan
        /// <summary>
        /// 工作计划Id
        /// </summary>
        [Label("工作节点")]
        public static readonly IRefIdProperty EquipmentSetupPlanIdProperty = P<SetupWorkHour>.RegisterRefId(e => e.EquipmentSetupPlanId, ReferenceType.Normal);

        /// <summary>
        /// 工作计划Id
        /// </summary>
        public double EquipmentSetupPlanId
        {
            get { return (double)GetRefId(EquipmentSetupPlanIdProperty); }
            set { SetRefId(EquipmentSetupPlanIdProperty, value); }
        }

        /// <summary>
        /// 工作计划
        /// </summary>
        public static readonly RefEntityProperty<EquipmentSetupPlan> EquipmentSetupPlanProperty = P<SetupWorkHour>.RegisterRef(e => e.EquipmentSetupPlan, EquipmentSetupPlanIdProperty);

        /// <summary>
        /// 工作计划
        /// </summary>
        public EquipmentSetupPlan EquipmentSetupPlan
        {
            get { return GetRefEntity(EquipmentSetupPlanProperty); }
            set { SetRefEntity(EquipmentSetupPlanProperty, value); }
        }
        #endregion

        #region 员工 Employee
        /// <summary>
        /// 员工Id
        /// </summary>
        [Label("员工")]
        public static readonly IRefIdProperty EmployeeIdProperty = P<SetupWorkHour>.RegisterRefId(e => e.EmployeeId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Employee> EmployeeProperty = P<SetupWorkHour>.RegisterRef(e => e.Employee, EmployeeIdProperty);

        /// <summary>
        /// 员工
        /// </summary>
        public Employee Employee
        {
            get { return GetRefEntity(EmployeeProperty); }
            set { SetRefEntity(EmployeeProperty, value); }
        }
        #endregion

        #region 设备 EquipAccount
        /// <summary>
        /// 设备Id
        /// </summary>
        [Label("设备")]
        public static readonly IRefIdProperty EquipAccountIdProperty = P<SetupWorkHour>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Normal);

        /// <summary>
        /// 设备Id
        /// </summary>
        public double? EquipAccountId
        {
            get { return (double?)GetRefNullableId(EquipAccountIdProperty); }
            set { SetRefNullableId(EquipAccountIdProperty, value); }
        }

        /// <summary>
        /// 设备
        /// </summary>
        public static readonly RefEntityProperty<EquipAccountSelect> EquipAccountProperty = P<SetupWorkHour>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

        /// <summary>
        /// 设备
        /// </summary>
        public EquipAccountSelect EquipAccount
        {
            get { return GetRefEntity(EquipAccountProperty); }
            set { SetRefEntity(EquipAccountProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 员工编码 EmployeeCode
        /// <summary>
        /// 员工编码
        /// </summary>
        [Label("员工编码")]
        public static readonly Property<string> EmployeeCodeProperty = P<SetupWorkHour>.RegisterView(e => e.EmployeeCode, p => p.Employee.Code);

        /// <summary>
        /// 员工编码
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
        public static readonly Property<string> EmployeeNameProperty = P<SetupWorkHour>.RegisterView(e => e.EmployeeName, p => p.Employee.Name);

        /// <summary>
        /// 员工姓名
        /// </summary>
        public string EmployeeName
        {
            get { return this.GetProperty(EmployeeNameProperty); }
        }
        #endregion

        #region 设备名称 EquipAccountName
        /// <summary>
        /// 设备名称
        /// </summary>
        [Label("设备名称")]
        public static readonly Property<string> EquipAccountNameProperty = P<SetupWorkHour>.RegisterView(e => e.EquipAccountName, p => p.EquipAccount.Name);

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipAccountName
        {
            get { return this.GetProperty(EquipAccountNameProperty); }
        }
        #endregion

        #region 审核状态 ApprovalStatus
        /// <summary>
        /// 审核状态
        /// </summary>
        [Label("审核状态")]
        public static readonly Property<ApprovalStatus> ApprovalStatusProperty = P<SetupWorkHour>.RegisterView(e => e.ApprovalStatus, p => p.EquipmentSetup.ApprovalStatus);

        /// <summary>
        /// 审核状态
        /// </summary>
        public ApprovalStatus ApprovalStatus
        {
            get { return this.GetProperty(ApprovalStatusProperty); }
        }
        #endregion

        #region 安装调试状态 SetupStatus
        /// <summary>
        /// 安装调试状态
        /// </summary>
        [Label("安装调试状态")]
        public static readonly Property<SetupStatus> SetupStatusProperty = P<SetupWorkHour>.RegisterView(e => e.SetupStatus, p => p.EquipmentSetup.SetupStatus);

        /// <summary>
        /// 安装调试状态
        /// </summary>
        public SetupStatus SetupStatus
        {
            get { return this.GetProperty(SetupStatusProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 安装调试工时登记 实体配置
    /// </summary>
    internal class SetupWorkHourConfig : EntityConfig<SetupWorkHour>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_SETUP_WH").MapAllProperties();
            Meta.Property(SetupWorkHour.RemarkProperty).ColumnMeta.HasLength(2000);
            Meta.EnablePhantoms();
        }
    }
}