using SIE.CSM.Customers;
using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipModels;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Purchases.EquipmentInbounds
{
    /// <summary>
    /// 设备入库查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("设备入库查询实体")]
    public partial class EquipmentInboundCriteria : Criteria
    {
        #region 入库单号 No
        /// <summary>
        /// 入库单号
        /// </summary>
        [Label("入库单号")]
        public static readonly Property<string> NoProperty = P<EquipmentInboundCriteria>.Register(e => e.No);

        /// <summary>
        /// 入库单号
        /// </summary>
        public string No
        {
            get { return this.GetProperty(NoProperty); }
            set { this.SetProperty(NoProperty, value); }
        }
        #endregion

        #region 入库类型 InboundType
        /// <summary>
        /// 入库类型
        /// </summary>
        [Label("入库类型")]
        public static readonly Property<InboundType?> InboundTypeProperty = P<EquipmentInboundCriteria>.Register(e => e.InboundType);

        /// <summary>
        /// 入库类型
        /// </summary>
        public InboundType? InboundType
        {
            get { return GetProperty(InboundTypeProperty); }
            set { SetProperty(InboundTypeProperty, value); }
        }
        #endregion

        #region 设备编码 EquipmentCode
        /// <summary>
        /// 设备编码
        /// </summary>
        [Label("设备编码")]
        public static readonly Property<string> EquipmentCodeProperty = P<EquipmentInboundCriteria>.Register(e => e.EquipmentCode);

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipmentCode
        {
            get { return GetProperty(EquipmentCodeProperty); }
            set { SetProperty(EquipmentCodeProperty, value); }
        }
        #endregion

        #region 设备型号维护 EquipModel
        /// <summary>
        /// 设备型号维护Id
        /// </summary>
        [Label("设备型号")]
        public static readonly IRefIdProperty EquipModelIdProperty = P<EquipmentInboundCriteria>.RegisterRefId(e => e.EquipModelId, ReferenceType.Normal);

        /// <summary>
        /// 设备型号维护Id
        /// </summary>
        public double? EquipModelId
        {
            get { return (double?)GetRefNullableId(EquipModelIdProperty); }
            set { SetRefNullableId(EquipModelIdProperty, value); }
        }

        /// <summary>
        /// 设备型号维护
        /// </summary>
        public static readonly RefEntityProperty<EquipModel> EquipModelProperty = P<EquipmentInboundCriteria>.RegisterRef(e => e.EquipModel, EquipModelIdProperty);

        /// <summary>
        /// 设备型号维护
        /// </summary>
        public EquipModel EquipModel
        {
            get { return GetRefEntity(EquipModelProperty); }
            set { SetRefEntity(EquipModelProperty, value); }
        }
        #endregion

        #region 采购单号 PurchaseOrderNo
        /// <summary>
        /// 采购单号
        /// </summary>
        [Label("采购单号")]
        public static readonly Property<string> PurchaseOrderNoProperty = P<EquipmentInboundCriteria>.Register(e => e.PurchaseOrderNo);

        /// <summary>
        /// 采购单号
        /// </summary>
        public string PurchaseOrderNo
        {
            get { return this.GetProperty(PurchaseOrderNoProperty); }
            set { this.SetProperty(PurchaseOrderNoProperty, value); }
        }
        #endregion

        #region 验收单号 AcceptanceNo
        /// <summary>
        /// 验收单号
        /// </summary>
        [Label("验收单号")]
        public static readonly Property<string> AcceptanceNoProperty = P<EquipmentInboundCriteria>.Register(e => e.AcceptanceNo);

        /// <summary>
        /// 验收单号
        /// </summary>
        public string AcceptanceNo
        {
            get { return this.GetProperty(AcceptanceNoProperty); }
            set { this.SetProperty(AcceptanceNoProperty, value); }
        }
        #endregion

        #region 供应商 Supplier
        /// <summary>
        /// 供应商Id
        /// </summary>
        [Label("供应商")]
        public static readonly IRefIdProperty SupplierIdProperty = P<EquipmentInboundCriteria>.RegisterRefId(e => e.SupplierId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Supplier> SupplierProperty = P<EquipmentInboundCriteria>.RegisterRef(e => e.Supplier, SupplierIdProperty);

        /// <summary>
        /// 供应商
        /// </summary>
        public Supplier Supplier
        {
            get { return GetRefEntity(SupplierProperty); }
            set { SetRefEntity(SupplierProperty, value); }
        }
        #endregion

        #region 客户 Customer
        /// <summary>
        /// 客户Id
        /// </summary>
        [Label("客户")]
        public static readonly IRefIdProperty CustomerIdProperty = P<EquipmentInboundCriteria>.RegisterRefId(e => e.CustomerId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Customer> CustomerProperty = P<EquipmentInboundCriteria>.RegisterRef(e => e.Customer, CustomerIdProperty);

        /// <summary>
        /// 客户
        /// </summary>
        public Customer Customer
        {
            get { return GetRefEntity(CustomerProperty); }
            set { SetRefEntity(CustomerProperty, value); }
        }
        #endregion

        #region 入库状态 InboundStatus
        /// <summary>
        /// 入库状态
        /// </summary>
        [Label("入库状态")]
        public static readonly Property<InboundStatus?> InboundStatusProperty = P<EquipmentInboundCriteria>.Register(e => e.InboundStatus);

        /// <summary>
        /// 入库状态
        /// </summary>
        public InboundStatus? InboundStatus
        {
            get { return GetProperty(InboundStatusProperty); }
            set { SetProperty(InboundStatusProperty, value); }
        }
        #endregion

        #region 入库日期 CreateDate
        /// <summary>
        /// 入库日期
        /// </summary>
        [Label("入库日期")]
        public static readonly Property<DateRange> CreateDateProperty = P<EquipmentInboundCriteria>.Register(e => e.CreateDate);

        /// <summary>
        /// 入库日期
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
            return RT.Service.Resolve<EquipmentInboundController>().CriteriaEquipmentInbounds(this);
        }
    }
}
