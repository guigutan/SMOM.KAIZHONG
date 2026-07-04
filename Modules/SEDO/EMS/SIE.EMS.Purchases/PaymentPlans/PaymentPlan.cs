using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.CSM.Suppliers;
using SIE.DataAuth;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.DataAuth;
using SIE.EMS.Purchases.PurchaseOrders;
using SIE.Equipments.Configs;
using SIE.Equipments.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using System;

namespace SIE.EMS.Purchases.PaymentPlans
{
    /// <summary>
    /// 付款计划
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(PaymentPlanCriteria))]
    [Label("付款计划")]
    [EntityWithConfig(typeof(ApprovalConfig))]
    [EntityWithConfig(typeof(NoConfig), "付款计划单号配置项", "付款计划单号生成规则")]
    [EntityDataAuth(typeof(EmployeeEnterprise), nameof(FactoryId), true)]
    [BudgetDepartmentAuth(nameof(DepartmentId), true)]
    public partial class PaymentPlan : DataEntity
    {
        #region 工厂 Factory
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        public static readonly IRefIdProperty FactoryIdProperty = P<PaymentPlan>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

        /// <summary>
        /// 工厂Id
        /// </summary>
        public double FactoryId
        {
            get { return (double)GetRefId(FactoryIdProperty); }
            set { SetRefId(FactoryIdProperty, value); }
        }

        /// <summary>
        /// 工厂
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> FactoryProperty = P<PaymentPlan>.RegisterRef(e => e.Factory, FactoryIdProperty);

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
        public static readonly IRefIdProperty DepartmentIdProperty = P<PaymentPlan>.RegisterRefId(e => e.DepartmentId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Enterprise> DepartmentProperty = P<PaymentPlan>.RegisterRef(e => e.Department, DepartmentIdProperty);

        /// <summary>
        /// 部门
        /// </summary>
        public Enterprise Department
        {
            get { return GetRefEntity(DepartmentProperty); }
            set { SetRefEntity(DepartmentProperty, value); }
        }
        #endregion

        #region 付款计划单号 PaymentOrderNo
        /// <summary>
        /// 付款计划单号
        /// </summary>
        [Label("付款计划单号")]
        [Required]
        [NotDuplicate]
        public static readonly Property<string> PaymentOrderNoProperty = P<PaymentPlan>.Register(e => e.PaymentOrderNo);

        /// <summary>
        /// 付款计划单号
        /// </summary>
        public string PaymentOrderNo
        {
            get { return GetProperty(PaymentOrderNoProperty); }
            set { SetProperty(PaymentOrderNoProperty, value); }
        }
        #endregion

        #region 采购订单 PurchaseOrder
        /// <summary>
        /// 采购订单Id
        /// </summary>
        public static readonly IRefIdProperty PurchaseOrderIdProperty = P<PaymentPlan>.RegisterRefId(e => e.PurchaseOrderId, ReferenceType.Normal);

        /// <summary>
        /// 采购订单Id
        /// </summary>
        public double? PurchaseOrderId
        {
            get { return (double?)GetRefNullableId(PurchaseOrderIdProperty); }
            set { SetRefNullableId(PurchaseOrderIdProperty, value); }
        }

        /// <summary>
        /// 采购订单
        /// </summary>
        public static readonly RefEntityProperty<PurchaseOrder> PurchaseOrderProperty = P<PaymentPlan>.RegisterRef(e => e.PurchaseOrder, PurchaseOrderIdProperty);

        /// <summary>
        /// 采购订单
        /// </summary>
        public PurchaseOrder PurchaseOrder
        {
            get { return GetRefEntity(PurchaseOrderProperty); }
            set { SetRefEntity(PurchaseOrderProperty, value); }
        }
        #endregion

        #region 审核状态 ApprovalStatus
        /// <summary>
        /// 审核状态
        /// </summary>
        [Label("审核状态")]
        public static readonly Property<ApprovalStatus> ApprovalStatusProperty = P<PaymentPlan>.Register(e => e.ApprovalStatus);

        /// <summary>
        /// 审核状态
        /// </summary>
        public ApprovalStatus ApprovalStatus
        {
            get { return GetProperty(ApprovalStatusProperty); }
            set { SetProperty(ApprovalStatusProperty, value); }
        }
        #endregion

        #region 审核时间 ApprovedTime
        /// <summary>
        /// 审核时间
        /// </summary>
        [Label("审核时间")]
        public static readonly Property<DateTime?> ApprovedTimeProperty = P<PaymentPlan>.Register(e => e.ApprovedTime);

        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? ApprovedTime
        {
            get { return GetProperty(ApprovedTimeProperty); }
            set { SetProperty(ApprovedTimeProperty, value); }
        }
        #endregion

        #region 供应商 Supplier
        /// <summary>
        /// 供应商Id
        /// </summary>
        [Label("供应商")]
        public static readonly IRefIdProperty SupplierIdProperty = P<PaymentPlan>.RegisterRefId(e => e.SupplierId, ReferenceType.Normal);

        /// <summary>
        /// 供应商Id
        /// </summary>
        public double SupplierId
        {
            get { return (double)GetRefId(SupplierIdProperty); }
            set { SetRefId(SupplierIdProperty, value); }
        }

        /// <summary>
        /// 供应商
        /// </summary>
        public static readonly RefEntityProperty<Supplier> SupplierProperty = P<PaymentPlan>.RegisterRef(e => e.Supplier, SupplierIdProperty);

        /// <summary>
        /// 供应商
        /// </summary>
        public Supplier Supplier
        {
            get { return GetRefEntity(SupplierProperty); }
            set { SetRefEntity(SupplierProperty, value); }
        }
        #endregion

        #region 计划付款金额 Amount
        /// <summary>
        /// 计划付款金额
        /// </summary>
        [Label("计划付款金额")]
        [MinValue(0)]
        [Required]
        public static readonly Property<decimal> AmountProperty = P<PaymentPlan>.Register(e => e.Amount);

        /// <summary>
        /// 计划付款金额
        /// </summary>
        public decimal Amount
        {
            get { return GetProperty(AmountProperty); }
            set { SetProperty(AmountProperty, value); }
        }
        #endregion

        #region 付款条件 PaymentTerms
        /// <summary>
        /// 付款条件Id
        /// </summary>
        public static readonly IRefIdProperty PaymentTermsIdProperty = P<PaymentPlan>.RegisterRefId(e => e.PaymentTermsId, ReferenceType.Normal);

        /// <summary>
        /// 付款条件Id
        /// </summary>
        public double? PaymentTermsId
        {
            get { return (double?)GetRefNullableId(PaymentTermsIdProperty); }
            set { SetRefNullableId(PaymentTermsIdProperty, value); }
        }

        /// <summary>
        /// 付款条件
        /// </summary>
        public static readonly RefEntityProperty<PaymentTerms> PaymentTermsProperty = P<PaymentPlan>.RegisterRef(e => e.PaymentTerms, PaymentTermsIdProperty);

        /// <summary>
        /// 付款条件
        /// </summary>
        public PaymentTerms PaymentTerms
        {
            get { return GetRefEntity(PaymentTermsProperty); }
            set { SetRefEntity(PaymentTermsProperty, value); }
        }
        #endregion

        #region 计划付款日期 PaymentDate
        /// <summary>
        /// 计划付款日期
        /// </summary>
        [Label("计划付款日期")]
        [Required]
        public static readonly Property<DateTime?> PaymentDateProperty = P<PaymentPlan>.Register(e => e.PaymentDate);

        /// <summary>
        /// 计划付款日期
        /// </summary>
        public DateTime? PaymentDate
        {
            get { return GetProperty(PaymentDateProperty); }
            set { SetProperty(PaymentDateProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<PaymentPlan>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 付款计划附件列表 AttachmentList
        /// <summary>
        /// 付款计划附件列表
        /// </summary>
        public static readonly ListProperty<EntityList<PaymentPlanAttachment>> AttachmentListProperty = P<PaymentPlan>.RegisterList(e => e.AttachmentList);
        /// <summary>
        /// 付款计划附件列表
        /// </summary>
        public EntityList<PaymentPlanAttachment> AttachmentList
        {
            get { return this.GetLazyList(AttachmentListProperty); }
        }
        #endregion

        #region 视图属性
        #region 供应商编码 SupplierCode
        /// <summary>
        /// 供应商编码
        /// </summary>
        [Label("供应商编码")]
        public static readonly Property<string> SupplierCodeProperty = P<PaymentPlan>.RegisterView(e => e.SupplierCode, p => p.Supplier.Code);

        /// <summary>
        /// 供应商编码
        /// </summary>
        public string SupplierCode
        {
            get { return this.GetProperty(SupplierCodeProperty); }
        }
        #endregion

        #region 供应商名称 SupplierName
        /// <summary>
        /// 供应商名称
        /// </summary>
        [Label("供应商名称")]
        public static readonly Property<string> SupplierNameProperty = P<PaymentPlan>.RegisterView(e => e.SupplierName, p => p.Supplier.Name);

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName
        {
            get { return this.GetProperty(SupplierNameProperty); }
        }
        #endregion

        #region 订单金额 TotalAmount
        /// <summary>
        /// 订单金额
        /// </summary>
        [Label("订单金额")]
        public static readonly Property<decimal?> TotalAmountProperty = P<PaymentPlan>.RegisterView(e => e.TotalAmount, p => p.PurchaseOrder.TotalAmount);

        /// <summary>
        /// 订单金额
        /// </summary>
        public decimal? TotalAmount
        {
            get { return this.GetProperty(TotalAmountProperty); }
        }
        #endregion

        #region 币种 Currency
        /// <summary>
        /// 币种
        /// </summary>
        [Label("币种")]
        public static readonly Property<Currency?> CurrencyProperty = P<PaymentPlan>.RegisterView(e => e.Currency, p => p.PurchaseOrder.Currency);

        /// <summary>
        /// 币种
        /// </summary>
        public Currency? Currency
        {
            get { return this.GetProperty(CurrencyProperty); }
        }
        #endregion

        #region 金额单位 AmountUnit
        /// <summary>
        /// 金额单位
        /// </summary>
        [Label("金额单位")]
        public static readonly Property<AmountUnit?> AmountUnitProperty = P<PaymentPlan>.RegisterView(e => e.AmountUnit, p => p.PurchaseOrder.Currency);

        /// <summary>
        /// 金额单位
        /// </summary>
        public AmountUnit? AmountUnit
        {
            get { return this.GetProperty(AmountUnitProperty); }
        }
        #endregion

        #region 已计划总额 PlanAmount
        /// <summary>
        /// 已计划总额
        /// </summary>
        [Label("已计划总额")]
        public static readonly Property<decimal?> PlanAmountProperty = P<PaymentPlan>.RegisterView(e => e.PlanAmount, p => p.PurchaseOrder.TotalPlanned);

        /// <summary>
        /// 已计划总额
        /// </summary>
        public decimal? PlanAmount
        {
            get { return this.GetProperty(PlanAmountProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 付款计划 实体配置
    /// </summary>
    internal class PaymentPlanConfig : EntityConfig<PaymentPlan>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_PAYMENT_PLAN").MapAllProperties();
            Meta.EnablePhantoms();
        }

        /// <summary>
        /// 验证规则
        /// </summary>
        /// <param name="rules">验证规则</param>
        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.AddRule(PaymentPlan.AmountProperty, new PositiveNumberRule());
        }
    }
}