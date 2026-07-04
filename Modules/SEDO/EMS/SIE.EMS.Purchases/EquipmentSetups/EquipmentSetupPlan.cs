using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.EMS.Purchases.Enums;
using SIE.EMS.Purchases.PurchaseOrders;
using SIE.Equipments.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Purchases.EquipmentSetups
{
    /// <summary>
    /// 安装调试工作计划
    /// </summary>
    [ChildEntity, Serializable]
    [Label("安装调试工作计划")]
    [DisplayMember(nameof(TodoItem))]
    public partial class EquipmentSetupPlan : DataEntity
    {
        #region 工作计划 EquipmentSetup
        /// <summary>
        /// 工作计划Id
        /// </summary>
        public static readonly IRefIdProperty EquipmentSetupIdProperty = P<EquipmentSetupPlan>.RegisterRefId(e => e.EquipmentSetupId, ReferenceType.Parent);

        /// <summary>
        /// 工作计划Id
        /// </summary>
        public double EquipmentSetupId
        {
            get { return (double)GetRefId(EquipmentSetupIdProperty); }
            set { SetRefId(EquipmentSetupIdProperty, value); }
        }

        /// <summary>
        /// 工作计划
        /// </summary>
        public static readonly RefEntityProperty<EquipmentSetup> EquipmentSetupProperty = P<EquipmentSetupPlan>.RegisterRef(e => e.EquipmentSetup, EquipmentSetupIdProperty);

        /// <summary>
        /// 工作计划
        /// </summary>
        public EquipmentSetup EquipmentSetup
        {
            get { return GetRefEntity(EquipmentSetupProperty); }
            set { SetRefEntity(EquipmentSetupProperty, value); }
        }
        #endregion

        #region 工作节点 TodoItem
        /// <summary>
        /// 工作节点
        /// </summary>
        [Label("工作节点")]
        [Required]
        public static readonly Property<string> TodoItemProperty = P<EquipmentSetupPlan>.Register(e => e.TodoItem);

        /// <summary>
        /// 工作节点
        /// </summary>
        public string TodoItem
        {
            get { return GetProperty(TodoItemProperty); }
            set { SetProperty(TodoItemProperty, value); }
        }
        #endregion

        #region 计划开始时间 PlanStartDateTime
        /// <summary>
        /// 计划开始时间
        /// </summary>
        [Label("计划开始时间")]
        public static readonly Property<DateTime> PlanStartDateTimeProperty = P<EquipmentSetupPlan>.Register(e => e.PlanStartDateTime);

        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateTime PlanStartDateTime
        {
            get { return GetProperty(PlanStartDateTimeProperty); }
            set { SetProperty(PlanStartDateTimeProperty, value); }
        }
        #endregion

        #region 计划结束时间 PlanEndDateTime
        /// <summary>
        /// 计划结束时间
        /// </summary>
        [Label("计划结束时间")]
        public static readonly Property<DateTime> PlanEndDateTimeProperty = P<EquipmentSetupPlan>.Register(e => e.PlanEndDateTime);

        /// <summary>
        /// 计划结束时间
        /// </summary>
        public DateTime PlanEndDateTime
        {
            get { return GetProperty(PlanEndDateTimeProperty); }
            set { SetProperty(PlanEndDateTimeProperty, value); }
        }
        #endregion

        #region 需求工时(h) WorkHours
        /// <summary>
        /// 需求工时(h)
        /// </summary>
        [Label("需求工时")]
        [MinValue(0)]
        public static readonly Property<decimal> WorkHoursProperty = P<EquipmentSetupPlan>.Register(e => e.WorkHours);

        /// <summary>
        /// 需求工时(h)
        /// </summary>
        public decimal WorkHours
        {
            get { return GetProperty(WorkHoursProperty); }
            set { SetProperty(WorkHoursProperty, value); }
        }
        #endregion

        #region 实际开始时间 ActualStartDateTime
        /// <summary>
        /// 实际开始时间
        /// </summary>
        [Label("实际开始时间")]
        public static readonly Property<DateTime?> ActualStartDateTimeProperty = P<EquipmentSetupPlan>.Register(e => e.ActualStartDateTime);

        /// <summary>
        /// 实际开始时间
        /// </summary>
        public DateTime? ActualStartDateTime
        {
            get { return GetProperty(ActualStartDateTimeProperty); }
            set { SetProperty(ActualStartDateTimeProperty, value); }
        }
        #endregion

        #region 实际结束时间 ActualEndDateTime
        /// <summary>
        /// 实际结束时间
        /// </summary>
        [Label("实际结束时间")]
        public static readonly Property<DateTime?> ActualEndDateTimeProperty = P<EquipmentSetupPlan>.Register(e => e.ActualEndDateTime);

        /// <summary>
        /// 实际结束时间
        /// </summary>
        public DateTime? ActualEndDateTime
        {
            get { return GetProperty(ActualEndDateTimeProperty); }
            set { SetProperty(ActualEndDateTimeProperty, value); }
        }
        #endregion

        #region 员工数量 EmployeeCount
        /// <summary>
        /// 员工数量
        /// </summary>
        [Label("员工数量")]
        public static readonly Property<int?> EmployeeCountProperty = P<EquipmentSetupPlan>.Register(e => e.EmployeeCount);

        /// <summary>
        /// 员工数量
        /// </summary>
        public int? EmployeeCount
        {
            get { return GetProperty(EmployeeCountProperty); }
            set { SetProperty(EmployeeCountProperty, value); }
        }
        #endregion

        #region 耗费工时(h) ActualWorkHours
        /// <summary>
        /// 耗费工时(h)
        /// </summary>
        [Label("耗费工时(h)")]
        public static readonly Property<decimal?> ActualWorkHoursProperty = P<EquipmentSetupPlan>.Register(e => e.ActualWorkHours);

        /// <summary>
        /// 耗费工时(h)
        /// </summary>
        public decimal? ActualWorkHours
        {
            get { return GetProperty(ActualWorkHoursProperty); }
            set { SetProperty(ActualWorkHoursProperty, value); }
        }
        #endregion

        #region 联系人 ContactPerson
        /// <summary>
        /// 联系人
        /// </summary>
        [Label("联系人")]
        public static readonly Property<string> ContactPersonProperty = P<EquipmentSetupPlan>.Register(e => e.ContactPerson);

        /// <summary>
        /// 联系人
        /// </summary>
        public string ContactPerson
        {
            get { return GetProperty(ContactPersonProperty); }
            set { SetProperty(ContactPersonProperty, value); }
        }
        #endregion

        #region 联系方式 ContactDetail
        /// <summary>
        /// 联系方式
        /// </summary>
        [Label("联系方式")]
        public static readonly Property<string> ContactDetailProperty = P<EquipmentSetupPlan>.Register(e => e.ContactDetail);

        /// <summary>
        /// 联系方式
        /// </summary>
        public string ContactDetail
        {
            get { return GetProperty(ContactDetailProperty); }
            set { SetProperty(ContactDetailProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(1000)]
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<EquipmentSetupPlan>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 状态 WorkStatus
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<WorkStatus> WorkStatusProperty = P<EquipmentSetupPlan>.Register(e => e.WorkStatus);

        /// <summary>
        /// 状态
        /// </summary>
        public WorkStatus WorkStatus
        {
            get { return GetProperty(WorkStatusProperty); }
            set { SetProperty(WorkStatusProperty, value); }
        }
        #endregion

        #region 供应商 Supplier
        /// <summary>
        /// 供应商Id
        /// </summary>
        [Label("供应商")]
        public static readonly IRefIdProperty SupplierIdProperty = P<EquipmentSetupPlan>.RegisterRefId(e => e.SupplierId, ReferenceType.Normal);

        /// <summary>
        /// 供应商Id
        /// </summary>
        public double? SupplierId
        {
            get { return (double?)GetRefNullableId(SupplierIdProperty); }
            set { SetRefNullableId(SupplierIdProperty, value); }
        }

        /// <summary>
        /// 供应商
        /// </summary>
        public static readonly RefEntityProperty<Supplier> SupplierProperty = P<EquipmentSetupPlan>.RegisterRef(e => e.Supplier, SupplierIdProperty);

        /// <summary>
        /// 供应商
        /// </summary>
        public Supplier Supplier
        {
            get { return GetRefEntity(SupplierProperty); }
            set { SetRefEntity(SupplierProperty, value); }
        }
        #endregion

        #region 采购订单 PurchaseOrder
        /// <summary>
        /// 采购订单Id
        /// </summary>
        [Label("采购订单")]
        public static readonly IRefIdProperty PurchaseOrderIdProperty =
            P<EquipmentSetupPlan>.RegisterRefId(e => e.PurchaseOrderId, ReferenceType.Normal);

        /// <summary>
        /// 采购订单Id
        /// </summary>
        public double? PurchaseOrderId
        {
            get { return (double?)this.GetRefNullableId(PurchaseOrderIdProperty); }
            set { this.SetRefNullableId(PurchaseOrderIdProperty, value); }
        }

        /// <summary>
        /// 采购订单
        /// </summary>
        public static readonly RefEntityProperty<PurchaseOrder> PurchaseOrderProperty =
            P<EquipmentSetupPlan>.RegisterRef(e => e.PurchaseOrder, PurchaseOrderIdProperty);

        /// <summary>
        /// 采购订单
        /// </summary>
        public PurchaseOrder PurchaseOrder
        {
            get { return this.GetRefEntity(PurchaseOrderProperty); }
            set { this.SetRefEntity(PurchaseOrderProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 供应商名称 SupplierName
        /// <summary>
        /// 供应商名称
        /// </summary>
        [Label("供应商名称")]
        public static readonly Property<string> SupplierNameProperty = P<EquipmentSetupPlan>.RegisterView(e => e.SupplierName, p => p.Supplier.Name);

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName
        {
            get { return this.GetProperty(SupplierNameProperty); }
        }
        #endregion

        #region 委外 OutSource
        /// <summary>
        /// 委外
        /// </summary>
        [Label("委外")]
        public static readonly Property<bool> OutSourceProperty = P<EquipmentSetupPlan>.RegisterView(e => e.OutSource, p => p.EquipmentSetup.OutSource);

        /// <summary>
        /// 委外
        /// </summary>
        public bool OutSource
        {
            get { return this.GetProperty(OutSourceProperty); }
        }
        #endregion

        #region 审核状态 ApprovalStatus
        /// <summary>
        /// 审核状态
        /// </summary>
        [Label("审核状态")]
        public static readonly Property<ApprovalStatus> ApprovalStatusProperty = P<EquipmentSetupPlan>.RegisterView(e => e.ApprovalStatus, p => p.EquipmentSetup.ApprovalStatus);

        /// <summary>
        /// 审核状态
        /// </summary>
        public ApprovalStatus ApprovalStatus
        {
            get { return this.GetProperty(ApprovalStatusProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 安装调试工作计划 实体配置
    /// </summary>
    internal class EquipmentSetupPlanConfig : EntityConfig<EquipmentSetupPlan>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_SETUP_PLAN").MapAllProperties();
            Meta.Property(EquipmentSetupPlan.RemarkProperty).ColumnMeta.HasLength(2000);
            Meta.EnablePhantoms();
        }
    }
}