using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.CSM.Suppliers;
using SIE.DataAuth;
using SIE.Domain;
using SIE.EMS.DataAuth;
using SIE.EMS.Purchases.Enums;
using SIE.Equipments.Configs;
using SIE.Equipments.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using System;

namespace SIE.EMS.Purchases.PurchaseOrders
{
    /// <summary>
    /// 采购订单
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(PurchaseOrderCriteria))]
    [Label("采购订单")]
    [DisplayMember(nameof(OrderNo))]
    [EntityWithConfig(typeof(ApprovalConfig))]
    [EntityWithConfig(typeof(NoConfig), "采购订单单号配置项", "采购订单单号生成规则")]
    [EntityDataAuth(typeof(EmployeeEnterprise), nameof(FactoryId), true)]
    [BudgetDepartmentAuth(nameof(DepartmentId), true)]
    public partial class PurchaseOrder : DataEntity
    {
        /// <summary>
        /// 快码类型：采购分类
        /// </summary>
        public static string PurchaseClassify { get { return "PURCHASE_CLASSIFY"; } }

        #region 订单号 OrderNo
        /// <summary>
        /// 订单号
        /// </summary>
        [Label("订单号")]
        [Required]
        [NotDuplicate]
        public static readonly Property<string> OrderNoProperty = P<PurchaseOrder>.Register(e => e.OrderNo);

        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo
        {
            get { return GetProperty(OrderNoProperty); }
            set { SetProperty(OrderNoProperty, value); }
        }
        #endregion

        #region 订单描述 Description
        /// <summary>
        /// 订单描述
        /// </summary>
        [MaxLength(1000)]
        [Required]
        [Label("订单描述")]
        public static readonly Property<string> DescriptionProperty = P<PurchaseOrder>.Register(e => e.Description);

        /// <summary>
        /// 订单描述
        /// </summary>
        public string Description
        {
            get { return this.GetProperty(DescriptionProperty); }
            set { this.SetProperty(DescriptionProperty, value); }
        }
        #endregion

        #region 审核时间 ApprovedDate
        /// <summary>
        /// 审核时间
        /// </summary>
        [Label("审核时间")]
        public static readonly Property<DateTime?> ApprovedDateProperty = P<PurchaseOrder>.Register(e => e.ApprovedDate);

        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? ApprovedDate
        {
            get { return GetProperty(ApprovedDateProperty); }
            set { SetProperty(ApprovedDateProperty, value); }
        }
        #endregion

        #region 采购分类 PurchaseCategroy
        /// <summary>
        /// 采购分类
        /// </summary>
        [Label("采购分类")]
        [Required]
        public static readonly Property<string> PurchaseCategroyProperty = P<PurchaseOrder>.Register(e => e.PurchaseCategroy);

        /// <summary>
        /// 采购分类
        /// </summary>
        public string PurchaseCategroy
        {
            get { return GetProperty(PurchaseCategroyProperty); }
            set { SetProperty(PurchaseCategroyProperty, value); }
        }
        #endregion

        #region 采购对象 PurchaseObjectType
        /// <summary>
        /// 采购对象
        /// </summary>
        [Label("采购对象")]
        public static readonly Property<PurchaseObjectType> PurchaseObjectTypeProperty = P<PurchaseOrder>.Register(e => e.PurchaseObjectType);

        /// <summary>
        /// 采购对象
        /// </summary>
        public PurchaseObjectType PurchaseObjectType
        {
            get { return GetProperty(PurchaseObjectTypeProperty); }
            set { SetProperty(PurchaseObjectTypeProperty, value); }
        }
        #endregion

        #region 品种数 VarietyQuantity
        /// <summary>
        /// 品种数
        /// </summary>
        [Label("品种数")]
        [MinValue(0)]
        public static readonly Property<int> VarietyQuantityProperty = P<PurchaseOrder>.Register(e => e.VarietyQuantity);

        /// <summary>
        /// 品种数
        /// </summary>
        public int VarietyQuantity
        {
            get { return GetProperty(VarietyQuantityProperty); }
            set { SetProperty(VarietyQuantityProperty, value); }
        }
        #endregion

        #region 总数量 TotalQty
        /// <summary>
        /// 总数量
        /// </summary>
        [Label("总数量")]
        [MinValue(0)]
        public static readonly Property<decimal> TotalQtyProperty = P<PurchaseOrder>.Register(e => e.TotalQty);

        /// <summary>
        /// 总数量
        /// </summary>
        public decimal TotalQty
        {
            get { return GetProperty(TotalQtyProperty); }
            set { SetProperty(TotalQtyProperty, value); }
        }
        #endregion

        #region 总金额 TotalAmount
        /// <summary>
        /// 总金额
        /// </summary>
        [Label("总金额")]
        [MinValue(0)]
        public static readonly Property<decimal> TotalAmountProperty = P<PurchaseOrder>.Register(e => e.TotalAmount);

        /// <summary>
        /// 总金额
        /// </summary>
        public decimal TotalAmount
        {
            get { return GetProperty(TotalAmountProperty); }
            set { SetProperty(TotalAmountProperty, value); }
        }
        #endregion

        #region 已计划总额 TotalPlanned
        /// <summary>
        /// 已计划总额
        /// </summary>
        [Label("已计划总额")]
        public static readonly Property<decimal> TotalPlannedProperty = P<PurchaseOrder>.Register(e => e.TotalPlanned);

        /// <summary>
        /// 已计划总额
        /// </summary>
        public decimal TotalPlanned
        {
            get { return this.GetProperty(TotalPlannedProperty); }
            set { this.SetProperty(TotalPlannedProperty, value); }
        }
        #endregion

        #region 已付金额 AmountPaid
        /// <summary>
        /// 已付金额
        /// </summary>
        [Label("已付金额")]
        [MinValue(0)]
        public static readonly Property<decimal> AmountPaidProperty = P<PurchaseOrder>.Register(e => e.AmountPaid);

        /// <summary>
        /// 已付金额
        /// </summary>
        public decimal AmountPaid
        {
            get { return GetProperty(AmountPaidProperty); }
            set { SetProperty(AmountPaidProperty, value); }
        }
        #endregion

        #region 合同编码 ContractCode
        /// <summary>
        /// 合同编码
        /// </summary>
        [Label("合同编码")]
        [Required]
        public static readonly Property<string> ContractCodeProperty = P<PurchaseOrder>.Register(e => e.ContractCode);

        /// <summary>
        /// 合同编码
        /// </summary>
        public string ContractCode
        {
            get { return GetProperty(ContractCodeProperty); }
            set { SetProperty(ContractCodeProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(1000)]
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<PurchaseOrder>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 审核状态 ApprovalStatus
        /// <summary>
        /// 审核状态
        /// </summary>
        [Label("审核状态")]
        public static readonly Property<ApprovalStatus> ApprovalStatusProperty = P<PurchaseOrder>.Register(e => e.ApprovalStatus);

        /// <summary>
        /// 审核状态
        /// </summary>
        public ApprovalStatus ApprovalStatus
        {
            get { return GetProperty(ApprovalStatusProperty); }
            set { SetProperty(ApprovalStatusProperty, value); }
        }
        #endregion

        #region 工厂 Factory
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        public static readonly IRefIdProperty FactoryIdProperty = P<PurchaseOrder>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Enterprise> FactoryProperty = P<PurchaseOrder>.RegisterRef(e => e.Factory, FactoryIdProperty);

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
        public static readonly IRefIdProperty DepartmentIdProperty = P<PurchaseOrder>.RegisterRefId(e => e.DepartmentId, ReferenceType.Normal);

        /// <summary>
        /// 部门Id
        /// </summary>
        public double DepartmentId
        {
            get { return (double)GetRefId(DepartmentIdProperty); }
            set { SetRefId(DepartmentIdProperty, value); }
        }

        /// <summary>
        /// 部门
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> DepartmentProperty = P<PurchaseOrder>.RegisterRef(e => e.Department, DepartmentIdProperty);

        /// <summary>
        /// 部门
        /// </summary>
        public Enterprise Department
        {
            get { return GetRefEntity(DepartmentProperty); }
            set { SetRefEntity(DepartmentProperty, value); }
        }
        #endregion

        #region 币种 Currency
        /// <summary>
        /// 币种
        /// </summary>
        [Label("币种")]
        public static readonly Property<Currency> CurrencyProperty = P<PurchaseOrder>.Register(e => e.Currency);

        /// <summary>
        /// 币种
        /// </summary>
        public Currency Currency
        {
            get { return GetProperty(CurrencyProperty); }
            set { SetProperty(CurrencyProperty, value); }
        }
        #endregion

        #region 采购订单状态 PurchaseOrderStatus
        /// <summary>
        /// 采购订单状态
        /// </summary>
        [Label("采购订单状态")]
        public static readonly Property<PurchaseOrderStatus> PurchaseOrderStatusProperty = P<PurchaseOrder>.Register(e => e.PurchaseOrderStatus);

        /// <summary>
        /// 采购订单状态
        /// </summary>
        public PurchaseOrderStatus PurchaseOrderStatus
        {
            get { return GetProperty(PurchaseOrderStatusProperty); }
            set { SetProperty(PurchaseOrderStatusProperty, value); }
        }
        #endregion

        #region 采购员 Buyer
        /// <summary>
        /// 采购员Id
        /// </summary>
        [Label("采购员")]
        public static readonly IRefIdProperty BuyerIdProperty = P<PurchaseOrder>.RegisterRefId(e => e.BuyerId, ReferenceType.Normal);

        /// <summary>
        /// 采购员Id
        /// </summary>
        public double BuyerId
        {
            get { return (double)GetRefId(BuyerIdProperty); }
            set { SetRefId(BuyerIdProperty, value); }
        }

        /// <summary>
        /// 采购员
        /// </summary>
        public static readonly RefEntityProperty<Employee> BuyerProperty = P<PurchaseOrder>.RegisterRef(e => e.Buyer, BuyerIdProperty);

        /// <summary>
        /// 采购员
        /// </summary>
        public Employee Buyer
        {
            get { return GetRefEntity(BuyerProperty); }
            set { SetRefEntity(BuyerProperty, value); }
        }
        #endregion

        #region 供应商 Supplier
        /// <summary>
        /// 供应商Id
        /// </summary>
        [Label("供应商")]
        public static readonly IRefIdProperty SupplierIdProperty = P<PurchaseOrder>.RegisterRefId(e => e.SupplierId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Supplier> SupplierProperty = P<PurchaseOrder>.RegisterRef(e => e.Supplier, SupplierIdProperty);

        /// <summary>
        /// 供应商
        /// </summary>
        public Supplier Supplier
        {
            get { return GetRefEntity(SupplierProperty); }
            set { SetRefEntity(SupplierProperty, value); }
        }
        #endregion

        #region 采购订单明细列表 DetailList
        /// <summary>
        /// 采购订单明细列表
        /// </summary>
        public static readonly ListProperty<EntityList<PurchaseOrderItem>> DetailListProperty = P<PurchaseOrder>.RegisterList(e => e.DetailList);
        /// <summary>
        /// 采购订单明细列表
        /// </summary>
        public EntityList<PurchaseOrderItem> DetailList
        {
            get { return this.GetLazyList(DetailListProperty); }
        }
        #endregion

        #region 付款条件列表 PaymentTermsList
        /// <summary>
        /// 付款条件列表
        /// </summary>
        public static readonly ListProperty<EntityList<PaymentTerms>> PaymentTermsListProperty = P<PurchaseOrder>.RegisterList(e => e.PaymentTermsList);
        /// <summary>
        /// 付款条件列表
        /// </summary>
        public EntityList<PaymentTerms> PaymentTermsList
        {
            get { return this.GetLazyList(PaymentTermsListProperty); }
        }
        #endregion

        #region 附件列表 AttachmentList
        /// <summary>
        /// 附件列表
        /// </summary>
        [Label("附件列表")]
        public static readonly ListProperty<EntityList<PurchaseAttachment>> AttachmentListProperty = P<PurchaseOrder>.RegisterList(e => e.AttachmentList);

        /// <summary>
        /// 附件列表
        /// </summary>
        public EntityList<PurchaseAttachment> AttachmentList
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
        public static readonly Property<string> SupplierCodeProperty = P<PurchaseOrder>.RegisterView(e => e.SupplierCode, p => p.Supplier.Code);

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
        public static readonly Property<string> SupplierNameProperty = P<PurchaseOrder>.RegisterView(e => e.SupplierName, p => p.Supplier.Name);

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName
        {
            get { return this.GetProperty(SupplierNameProperty); }
        }
        #endregion

        #region 工厂名称 FactoryName
        /// <summary>
        /// 工厂名称
        /// </summary>
        [Label("工厂名称")]
        public static readonly Property<string> FactoryNameProperty = P<PurchaseOrder>.RegisterView(e => e.FactoryName, p => p.Factory.Name);

        /// <summary>
        /// 工厂名称
        /// </summary>
        public string FactoryName
        {
            get { return this.GetProperty(FactoryNameProperty); }
        }
        #endregion

        #region 部门名称 DepartmentName
        /// <summary>
        /// 部门名称
        /// </summary>
        [Label("部门名称")]
        public static readonly Property<string> DepartmentNameProperty = P<PurchaseOrder>.RegisterView(e => e.DepartmentName, p => p.Department.Name);

        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartmentName
        {
            get { return this.GetProperty(DepartmentNameProperty); }
        }
        #endregion

        #region 金额单位 AmountUnit
        /// <summary>
        /// 金额单位
        /// </summary>
        [Label("金额单位")]
        public static readonly Property<AmountUnit> AmountUnitProperty = P<PurchaseOrder>.RegisterReadOnly(
            e => e.AmountUnit, e => e.GetAmountUnit(), CurrencyProperty);
        /// <summary>
        /// 金额单位
        /// </summary>
        public AmountUnit AmountUnit
        {
            get { return this.GetProperty(AmountUnitProperty); }
        }
        private AmountUnit GetAmountUnit()
        {
            return (AmountUnit)Currency;
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 采购订单 实体配置
    /// </summary>
    internal class PurchaseOrderConfig : EntityConfig<PurchaseOrder>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_PO").MapAllProperties();
            Meta.Property(PurchaseOrder.RemarkProperty).ColumnMeta.HasLength(4000);
            Meta.Property(PurchaseOrder.DescriptionProperty).ColumnMeta.HasLength(4000);
            Meta.EnablePhantoms();
        }
    }
}