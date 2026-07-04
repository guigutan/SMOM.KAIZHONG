using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.EMS.Purchases.Enums;
using SIE.Equipments.Enums;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using System;

namespace SIE.EMS.Purchases.PurchaseOrders
{
    /// <summary>
    /// 采购订单查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("采购订单查询实体")]
    public partial class PurchaseOrderCriteria : Criteria
    {
        #region 工厂 Factory
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        public static readonly IRefIdProperty FactoryIdProperty = P<PurchaseOrderCriteria>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Enterprise> FactoryProperty = P<PurchaseOrderCriteria>.RegisterRef(e => e.Factory, FactoryIdProperty);

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
        public static readonly IRefIdProperty DepartmentIdProperty = P<PurchaseOrderCriteria>.RegisterRefId(e => e.DepartmentId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Enterprise> DepartmentProperty = P<PurchaseOrderCriteria>.RegisterRef(e => e.Department, DepartmentIdProperty);

        /// <summary>
        /// 部门
        /// </summary>
        public Enterprise Department
        {
            get { return GetRefEntity(DepartmentProperty); }
            set { SetRefEntity(DepartmentProperty, value); }
        }
        #endregion

        #region 编号 No
        /// <summary>
        /// 编号
        /// </summary>
        [Label("采购订单号")]
        public static readonly Property<string> NoProperty = P<PurchaseOrderCriteria>.Register(e => e.No);

        /// <summary>
        /// 编号
        /// </summary>
        public string No
        {
            get { return this.GetProperty(NoProperty); }
            set { this.SetProperty(NoProperty, value); }
        }
        #endregion

        #region 采购分类 PurchaseCategroy
        /// <summary>
        /// 采购分类
        /// </summary>
        [Label("采购分类")]
        public static readonly Property<string> PurchaseCategroyProperty = P<PurchaseOrderCriteria>.Register(e => e.PurchaseCategroy);

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
        public static readonly Property<PurchaseObjectType?> PurchaseObjectTypeProperty = P<PurchaseOrderCriteria>.Register(e => e.PurchaseObjectType);

        /// <summary>
        /// 采购对象
        /// </summary>
        public PurchaseObjectType? PurchaseObjectType
        {
            get { return GetProperty(PurchaseObjectTypeProperty); }
            set { SetProperty(PurchaseObjectTypeProperty, value); }
        }
        #endregion

        #region 供应商 Supplier
        /// <summary>
        /// 供应商Id
        /// </summary>
        [Label("供应商")]
        public static readonly IRefIdProperty SupplierIdProperty = P<PurchaseOrderCriteria>.RegisterRefId(e => e.SupplierId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Supplier> SupplierProperty = P<PurchaseOrderCriteria>.RegisterRef(e => e.Supplier, SupplierIdProperty);

        /// <summary>
        /// 供应商
        /// </summary>
        public Supplier Supplier
        {
            get { return GetRefEntity(SupplierProperty); }
            set { SetRefEntity(SupplierProperty, value); }
        }
        #endregion

        #region 采购订单状态 PurchaseOrderStatus
        /// <summary>
        /// 采购订单状态
        /// </summary>
        [Label("订单状态")]
        public static readonly Property<PurchaseOrderStatus?> PurchaseOrderStatusProperty = P<PurchaseOrderCriteria>.Register(e => e.PurchaseOrderStatus);

        /// <summary>
        /// 采购订单状态
        /// </summary>
        public PurchaseOrderStatus? PurchaseOrderStatus
        {
            get { return GetProperty(PurchaseOrderStatusProperty); }
            set { SetProperty(PurchaseOrderStatusProperty, value); }
        }
        #endregion

        #region 审核状态 ApprovalStatus
        /// <summary>
        /// 审核状态
        /// </summary>
        [Label("审核状态")]
        public static readonly Property<ApprovalStatus?> ApprovalStatusProperty = P<PurchaseOrderCriteria>.Register(e => e.ApprovalStatus);

        /// <summary>
        /// 审核状态
        /// </summary>
        public ApprovalStatus? ApprovalStatus
        {
            get { return GetProperty(ApprovalStatusProperty); }
            set { SetProperty(ApprovalStatusProperty, value); }
        }
        #endregion

        #region 创建日期 CreateDate
        /// <summary>
        /// 创建日期
        /// </summary>
        [Label("创建日期")]
        public static readonly Property<DateRange> CreateDateProperty = P<PurchaseOrderCriteria>.Register(e => e.CreateDate);

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
            return RT.Service.Resolve<PurchaseOrderController>().CriteriaPurchaseOrders(this);
        }
    }
}
