using SIE.Core.Enums;
using SIE.CSM.Customers;
using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Warehouses;
using System;

namespace SIE.ShipPlan
{
    /// <summary>
    /// 发货计划
    /// </summary>
    [QueryEntity, Serializable]
    public partial class DeliveryPlanCriteria : Criteria
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DeliveryPlanCriteria()
        {
            DeliveryDate = new DateRange();
            DeliveryDate.DateRangeType = DateRangeType.LastMonth;
            DeliveryDate.DateTimePart = DateTimePart.Date;
            IsFilter = true;
        }

        #region 单号 No
        /// <summary>
        /// 单号
        /// </summary>
        [Label("单号")]
        public static readonly Property<string> NoProperty = P<DeliveryPlanCriteria>.Register(e => e.No);

        /// <summary>
        /// 单号
        /// </summary>
        public string No
        {
            get { return GetProperty(NoProperty); }
            set { SetProperty(NoProperty, value); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<string> StateProperty = P<DeliveryPlanCriteria>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public string State
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }
        #endregion

        #region 单据类型 OrderType
        /// <summary>
        /// 单据类型
        /// </summary>
        [Label("单据类型")]
        public static readonly Property<OrderType?> OrderTypeProperty = P<DeliveryPlanCriteria>.Register(e => e.OrderType);

        /// <summary>
        /// 单据类型
        /// </summary>
        public OrderType? OrderType
        {
            get { return GetProperty(OrderTypeProperty); }
            set { SetProperty(OrderTypeProperty, value); }
        }
        #endregion

        #region 发货仓库 Warehouse
        /// <summary>
        /// 发货仓库Id
        /// </summary>
        [Label("发货仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty = P<DeliveryPlanCriteria>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 发货仓库Id
        /// </summary>
        public double? WarehouseId
        {
            get { return (double?)GetRefNullableId(WarehouseIdProperty); }
            set { SetRefNullableId(WarehouseIdProperty, value); }
        }

        /// <summary>
        /// 发货仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<DeliveryPlanCriteria>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 发货仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return GetRefEntity(WarehouseProperty); }
            set { SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 生产部门 Enterprise
        /// <summary>
        /// 生产部门Id
        /// </summary>
        [Label("生产部门")]
        public static readonly IRefIdProperty EnterpriseIdProperty = P<DeliveryPlanCriteria>.RegisterRefId(e => e.EnterpriseId, ReferenceType.Normal);

        /// <summary>
        /// 生产部门Id
        /// </summary>
        public double? EnterpriseId
        {
            get { return (double?)GetRefNullableId(EnterpriseIdProperty); }
            set { SetRefNullableId(EnterpriseIdProperty, value); }
        }

        /// <summary>
        /// 生产部门
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> EnterpriseProperty = P<DeliveryPlanCriteria>.RegisterRef(e => e.Enterprise, EnterpriseIdProperty);

        /// <summary>
        /// 生产部门
        /// </summary>
        public Enterprise Enterprise
        {
            get { return GetRefEntity(EnterpriseProperty); }
            set { SetRefEntity(EnterpriseProperty, value); }
        }
        #endregion

        #region 客户 Customer
        /// <summary>
        /// 客户Id
        /// </summary>
        [Label("客户")]
        public static readonly IRefIdProperty CustomerIdProperty = P<DeliveryPlanCriteria>.RegisterRefId(e => e.CustomerId, ReferenceType.Normal);

        /// <summary>
        /// 客户Id
        /// </summary>
        public double? CustomerId
        {
            get { return (double?)GetRefNullableId(CustomerIdProperty); }
            set { SetRefNullableId(CustomerIdProperty, value); }
        }

        /// <summary>
        /// 客户
        /// </summary>
        public static readonly RefEntityProperty<Customer> CustomerProperty = P<DeliveryPlanCriteria>.RegisterRef(e => e.Customer, CustomerIdProperty);

        /// <summary>
        /// 客户
        /// </summary>
        public Customer Customer
        {
            get { return GetRefEntity(CustomerProperty); }
            set { SetRefEntity(CustomerProperty, value); }
        }
        #endregion

        #region 供应商 Supplier
        /// <summary>
        /// 供应商Id
        /// </summary>
        [Label("供应商")]
        public static readonly IRefIdProperty SupplierIdProperty = P<DeliveryPlanCriteria>.RegisterRefId(e => e.SupplierId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Supplier> SupplierProperty = P<DeliveryPlanCriteria>.RegisterRef(e => e.Supplier, SupplierIdProperty);

        /// <summary>
        /// 供应商
        /// </summary>
        public Supplier Supplier
        {
            get { return GetRefEntity(SupplierProperty); }
            set { SetRefEntity(SupplierProperty, value); }
        }
        #endregion

        #region 目标仓库 TargetWarehouse
        /// <summary>
        /// 目标仓库Id
        /// </summary>
        [Label("目标仓库")]
        public static readonly IRefIdProperty TargetWarehouseIdProperty = P<DeliveryPlanCriteria>.RegisterRefId(e => e.TargetWarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 目标仓库Id
        /// </summary>
        public double? TargetWarehouseId
        {
            get { return (double?)GetRefNullableId(TargetWarehouseIdProperty); }
            set { SetRefNullableId(TargetWarehouseIdProperty, value); }
        }

        /// <summary>
        /// 目标仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> TargetWarehouseProperty = P<DeliveryPlanCriteria>.RegisterRef(e => e.TargetWarehouse, TargetWarehouseIdProperty);

        /// <summary>
        /// 目标仓库
        /// </summary>
        public Warehouse TargetWarehouse
        {
            get { return GetRefEntity(TargetWarehouseProperty); }
            set { SetRefEntity(TargetWarehouseProperty, value); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<DeliveryPlanCriteria>.Register(e => e.ItemCode);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return GetProperty(ItemCodeProperty); }
            set { SetProperty(ItemCodeProperty, value); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<DeliveryPlanCriteria>.Register(e => e.ItemName);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return GetProperty(ItemNameProperty); }
            set { SetProperty(ItemNameProperty, value); }
        }
        #endregion

        #region 发货时间 DeliveryDate
        /// <summary>
        /// 发货时间
        /// </summary>
        [Label("发货时间")]
        public static readonly Property<DateRange> DeliveryDateProperty = P<DeliveryPlanCriteria>.Register(e => e.DeliveryDate);

        /// <summary>
        /// 发货时间
        /// </summary>
        public DateRange DeliveryDate
        {
            get { return GetProperty(DeliveryDateProperty); }
            set { SetProperty(DeliveryDateProperty, value); }
        }
        #endregion

        #region 相关单号 OrderNo
        /// <summary>
        /// 相关单号
        /// </summary>
        [Label("相关单号")]
        public static readonly Property<string> OrderNoProperty = P<DeliveryPlanCriteria>.Register(e => e.OrderNo);

        /// <summary>
        /// 相关单号
        /// </summary>
        public string OrderNo
        {
            get { return this.GetProperty(OrderNoProperty); }
            set { this.SetProperty(OrderNoProperty, value); }
        }
        #endregion


        #region 过滤完成、取消状态数据 IsFilter
        /// <summary>
        /// 过滤完成、取消状态数据
        /// </summary>
        [Label("过滤完成、取消状态数据")]
        public static readonly Property<bool> IsFilterProperty = P<DeliveryPlanCriteria>.Register(e => e.IsFilter);

        /// <summary>
        /// 过滤完成、取消状态数据
        /// </summary>
        public bool IsFilter
        {
            get { return GetProperty(IsFilterProperty); }
            set { SetProperty(IsFilterProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询逻辑
        /// </summary>
        /// <returns>返回查询后的数据</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<DeliveryPlanController>().GetDeliveryPlans(this);
        }
    }
}