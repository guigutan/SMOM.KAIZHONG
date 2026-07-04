using SIE.Domain;
using SIE.EMS.Purchases.Enums;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipAccounts;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using System;

namespace SIE.EMS.Purchases.EquipmentSetups
{
    /// <summary>
    /// 安装调试查询
    /// </summary>
    [QueryEntity, Serializable]
    [Label("安装调试查询")]
    public partial class EquipmentSetupCriteria : Criteria
    {
        #region 工厂 Factory
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        public static readonly IRefIdProperty FactoryIdProperty = P<EquipmentSetupCriteria>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

        /// <summary>
        /// 工厂Id
        /// </summary>
        public double? FactoryId
        {
            get { return (double?)GetRefNullableId(FactoryIdProperty); }
            set { SetRefNullableId(FactoryIdProperty, value); }
        }

        /// <summary>
        /// 工厂
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> FactoryProperty = P<EquipmentSetupCriteria>.RegisterRef(e => e.Factory, FactoryIdProperty);

        /// <summary>
        /// 工厂
        /// </summary>
        public Enterprise Factory
        {
            get { return GetRefEntity(FactoryProperty); }
            set { SetRefEntity(FactoryProperty, value); }
        }
        #endregion

        #region 部门 Department
        /// <summary>
        /// 部门Id
        /// </summary>
        [Label("部门")]
        public static readonly IRefIdProperty DepartmentIdProperty = P<EquipmentSetupCriteria>.RegisterRefId(e => e.DepartmentId, ReferenceType.Normal);

        /// <summary>
        /// 部门Id
        /// </summary>
        public double? DepartmentId
        {
            get { return (double?)GetRefNullableId(DepartmentIdProperty); }
            set { SetRefNullableId(DepartmentIdProperty, value); }
        }

        /// <summary>
        /// 部门
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> DepartmentProperty = P<EquipmentSetupCriteria>.RegisterRef(e => e.Department, DepartmentIdProperty);

        /// <summary>
        /// 部门
        /// </summary>
        public Enterprise Department
        {
            get { return GetRefEntity(DepartmentProperty); }
            set { SetRefEntity(DepartmentProperty, value); }
        }
        #endregion

        #region 单号 No
        /// <summary>
        /// 单号
        /// </summary>
        [Label("单号")]
        public static readonly Property<string> NoProperty = P<EquipmentSetupCriteria>.Register(e => e.No);

        /// <summary>
        /// 单号
        /// </summary>
        public string No
        {
            get { return this.GetProperty(NoProperty); }
            set { this.SetProperty(NoProperty, value); }
        }
        #endregion

        #region 设备编码 EquipAccount
        /// <summary>
        /// 设备编码Id
        /// </summary>
        [Label("设备编码")]
        public static readonly IRefIdProperty EquipAccountIdProperty =
            P<EquipmentSetupCriteria>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Normal);

        /// <summary>
        /// 设备编码Id
        /// </summary>
        public double? EquipAccountId
        {
            get { return (double?)this.GetRefNullableId(EquipAccountIdProperty); }
            set { this.SetRefNullableId(EquipAccountIdProperty, value); }
        }

        /// <summary>
        /// 设备编码
        /// </summary>
        public static readonly RefEntityProperty<EquipAccountSelect> EquipAccountProperty =
            P<EquipmentSetupCriteria>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

        /// <summary>
        /// 设备编码
        /// </summary>
        public EquipAccountSelect EquipAccount
        {
            get { return this.GetRefEntity(EquipAccountProperty); }
            set { this.SetRefEntity(EquipAccountProperty, value); }
        }
        #endregion

        #region 审核状态 ApprovalStatus
        /// <summary>
        /// 审核状态
        /// </summary>
        [Label("审核状态")]
        public static readonly Property<ApprovalStatus?> ApprovalStatusProperty = P<EquipmentSetupCriteria>.Register(e => e.ApprovalStatus);

        /// <summary>
        /// 审核状态
        /// </summary>
        public ApprovalStatus? ApprovalStatus
        {
            get { return GetProperty(ApprovalStatusProperty); }
            set { SetProperty(ApprovalStatusProperty, value); }
        }
        #endregion

        #region 状态 SetupStatus
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<SetupStatus?> SetupStatusProperty = P<EquipmentSetupCriteria>.Register(e => e.SetupStatus);

        /// <summary>
        /// 状态
        /// </summary>
        public SetupStatus? SetupStatus
        {
            get { return GetProperty(SetupStatusProperty); }
            set { SetProperty(SetupStatusProperty, value); }
        }
        #endregion

        #region 负责人 Principal
        /// <summary>
        /// 负责人Id
        /// </summary>
        [Label("负责人")]
        public static readonly IRefIdProperty PrincipalIdProperty = P<EquipmentSetupCriteria>.RegisterRefId(e => e.PrincipalId, ReferenceType.Normal);

        /// <summary>
        /// 负责人Id
        /// </summary>
        public double? PrincipalId
        {
            get { return (double?)GetRefNullableId(PrincipalIdProperty); }
            set { SetRefNullableId(PrincipalIdProperty, value); }
        }

        /// <summary>
        /// 负责人
        /// </summary>
        public static readonly RefEntityProperty<Employee> PrincipalProperty = P<EquipmentSetupCriteria>.RegisterRef(e => e.Principal, PrincipalIdProperty);

        /// <summary>
        /// 负责人
        /// </summary>
        public Employee Principal
        {
            get { return GetRefEntity(PrincipalProperty); }
            set { SetRefEntity(PrincipalProperty, value); }
        }
        #endregion

        #region 超时 Overtime
        /// <summary>
        /// 超时
        /// </summary>
        [Label("超时")]
        public static readonly Property<bool> OvertimeProperty = P<EquipmentSetupCriteria>.Register(e => e.Overtime);

        /// <summary>
        /// 超时
        /// </summary>
        public bool Overtime
        {
            get { return this.GetProperty(OvertimeProperty); }
            set { this.SetProperty(OvertimeProperty, value); }
        }
        #endregion

        #region 创建日期 CreateDate
        /// <summary>
        /// 创建日期
        /// </summary>
        [Label("创建日期")]
        public static readonly Property<DateRange> CreateDateProperty = P<EquipmentSetupCriteria>.Register(e => e.CreateDate);

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateRange CreateDate
        {
            get { return GetProperty(CreateDateProperty); }
            set { SetProperty(CreateDateProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询逻辑
        /// </summary>
        /// <returns>返回查询后的数据</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<EquipmentSetupController>().CriteriaEquipmentSetups(this);
        }
    }
}
