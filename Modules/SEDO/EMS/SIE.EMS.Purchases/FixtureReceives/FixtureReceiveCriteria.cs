using SIE;
using SIE.CSM.Customers;
using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.EMS.Purchases.Enums;
using SIE.Equipments.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources;
using SIE.Resources.Enterprises;
using System;

namespace SIE.EMS.Purchases.FixtureReceives
{
    /// <summary>
    /// 工治具接收查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("工治具接收查询实体")]
    public class FixtureReceiveCriteria : Criteria
    {

        #region 采购订单号 PurchaseOrderNo
        /// <summary>
        /// 采购订单号
        /// </summary>
        [Label("采购订单号")]
        public static readonly Property<string> PurchaseOrderNoProperty = P<FixtureReceiveCriteria>.Register(e => e.PurchaseOrderNo);

        /// <summary>
        /// 采购订单号
        /// </summary>
        public string PurchaseOrderNo
        {
            get { return GetProperty(PurchaseOrderNoProperty); }
            set { SetProperty(PurchaseOrderNoProperty, value); }
        }
        #endregion

        #region 供应商 Supplier
        /// <summary>
        /// 供应商Id
        /// </summary>
        [Label("供应商")]
        public static readonly IRefIdProperty SupplierIdProperty = P<FixtureReceiveCriteria>.RegisterRefId(e => e.SupplierId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Supplier> SupplierProperty = P<FixtureReceiveCriteria>.RegisterRef(e => e.Supplier, SupplierIdProperty);

        /// <summary>
        /// 供应商
        /// </summary>
        public Supplier Supplier
        {
            get { return GetRefEntity(SupplierProperty); }
            set { SetRefEntity(SupplierProperty, value); }
        }
        #endregion

        #region 工厂 Factory
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        public static readonly IRefIdProperty FactoryIdProperty = P<FixtureReceiveCriteria>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Enterprise> FactoryProperty = P<FixtureReceiveCriteria>.RegisterRef(e => e.Factory, FactoryIdProperty);

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
        public static readonly IRefIdProperty DepartmentIdProperty = P<FixtureReceiveCriteria>.RegisterRefId(e => e.DepartmentId, ReferenceType.Normal);

        /// <summary>
        /// 部门Id
        /// </summary>
        public double? DepartmentId
        {
            get { return (double?)GetRefNullableId(DepartmentIdProperty); }
            set { SetRefId(DepartmentIdProperty, value); }
        }

        /// <summary>
        /// 部门
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> DepartmentProperty = P<FixtureReceiveCriteria>.RegisterRef(e => e.Department, DepartmentIdProperty);

        /// <summary>
        /// 部门
        /// </summary>
        public Enterprise Department
        {
            get { return GetRefEntity(DepartmentProperty); }
            set { SetRefEntity(DepartmentProperty, value); }
        }
        #endregion

        #region 状态 ReceiveBillStatus
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<ReceiveBillStatus?> ReceiveBillStatusProperty = P<FixtureReceiveCriteria>.Register(e => e.ReceiveBillStatus);

        /// <summary>
        /// 状态
        /// </summary>
        public ReceiveBillStatus? ReceiveBillStatus
        {
            get { return GetProperty(ReceiveBillStatusProperty); }
            set { SetProperty(ReceiveBillStatusProperty, value); }
        }
        #endregion

        #region 客户 Customer
        /// <summary>
        /// 客户Id
        /// </summary>
        [Label("客户")]
        public static readonly IRefIdProperty CustomerIdProperty = P<FixtureReceiveCriteria>.RegisterRefId(e => e.CustomerId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Customer> CustomerProperty = P<FixtureReceiveCriteria>.RegisterRef(e => e.Customer, CustomerIdProperty);

        /// <summary>
        /// 客户
        /// </summary>
        public Customer Customer
        {
            get { return GetRefEntity(CustomerProperty); }
            set { SetRefEntity(CustomerProperty, value); }
        }
        #endregion


        #region 创建日期 CreateDate
        /// <summary>
        /// 创建日期
        /// </summary>
        [Label("创建日期")]
        public static readonly Property<DateRange> CreateDateProperty = P<FixtureReceiveCriteria>.Register(e => e.CreateDate);

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateRange CreateDate
        {
            get { return GetProperty(CreateDateProperty); }
            set { SetProperty(CreateDateProperty, value); }
        }
        #endregion

        #region 接收类型 ReceiveType
        /// <summary>
        /// 接收类型
        /// </summary>
        [Label("接收类型")]
        public static readonly Property<ReceiveType?> ReceiveTypeProperty = P<FixtureReceiveCriteria>.Register(e => e.ReceiveType);

        /// <summary>
        /// 接收类型
        /// </summary>
        public ReceiveType? ReceiveType
        {
            get { return GetProperty(ReceiveTypeProperty); }
            set { SetProperty(ReceiveTypeProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<FixtureReceiveController>().Fetch(this);
        }
    }
}