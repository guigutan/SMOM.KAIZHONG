using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.CSM.Customers;
using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipModels;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Resources.Employees;
using SIE.Warehouses;
using System;

namespace SIE.EMS.Purchases.EquipmentInbounds
{
    /// <summary>
    /// 设备入库
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(EquipmentInboundCriteria))]
    [Label("设备入库")]
    [EntityWithConfig(typeof(NoConfig), "设备入库单号配置项", "设备入库单号生成规则")]
    public partial class EquipmentInbound : DataEntity
    {
        #region 入库单号 InboundNo
        /// <summary>
        /// 入库单号
        /// </summary>
        [Label("入库单号")]
        [Required]
        [NotDuplicate]
        public static readonly Property<string> InboundNoProperty = P<EquipmentInbound>.Register(e => e.InboundNo);

        /// <summary>
        /// 入库单号
        /// </summary>
        public string InboundNo
        {
            get { return GetProperty(InboundNoProperty); }
            set { SetProperty(InboundNoProperty, value); }
        }
        #endregion

        #region 入库数 Qty
        /// <summary>
        /// 入库数
        /// </summary>
        [Label("入库数")]
        public static readonly Property<int> QtyProperty = P<EquipmentInbound>.Register(e => e.Qty);

        /// <summary>
        /// 入库数
        /// </summary>
        public int Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 入库人 WarehouseOperator
        /// <summary>
        /// 入库人Id
        /// </summary>
        [Label("入库人")]
        public static readonly IRefIdProperty WarehouseOperatorIdProperty = P<EquipmentInbound>.RegisterRefId(e => e.WarehouseOperatorId, ReferenceType.Normal);

        /// <summary>
        /// 入库人Id
        /// </summary>
        public double? WarehouseOperatorId
        {
            get { return (double?)GetRefNullableId(WarehouseOperatorIdProperty); }
            set { SetRefNullableId(WarehouseOperatorIdProperty, value); }
        }

        /// <summary>
        /// 入库人
        /// </summary>
        public static readonly RefEntityProperty<Employee> WarehouseOperatorProperty = P<EquipmentInbound>.RegisterRef(e => e.WarehouseOperator, WarehouseOperatorIdProperty);

        /// <summary>
        /// 入库人
        /// </summary>
        public Employee WarehouseOperator
        {
            get { return GetRefEntity(WarehouseOperatorProperty); }
            set { SetRefEntity(WarehouseOperatorProperty, value); }
        }
        #endregion

        #region 入库日期 InboundDateTime
        /// <summary>
        /// 入库日期
        /// </summary>
        [Label("入库日期")]
        public static readonly Property<DateTime?> InboundDateTimeProperty = P<EquipmentInbound>.Register(e => e.InboundDateTime);

        /// <summary>
        /// 入库日期
        /// </summary>
        public DateTime? InboundDateTime
        {
            get { return GetProperty(InboundDateTimeProperty); }
            set { SetProperty(InboundDateTimeProperty, value); }
        }
        #endregion

        #region 入库仓库 Warehouse
        /// <summary>
        /// 入库仓库Id
        /// </summary>
        [Label("入库仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty = P<EquipmentInbound>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 入库仓库Id
        /// </summary>
        public double? WarehouseId
        {
            get { return (double?)GetRefNullableId(WarehouseIdProperty); }
            set { SetRefNullableId(WarehouseIdProperty, value); }
        }

        /// <summary>
        /// 入库仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<EquipmentInbound>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 入库仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return GetRefEntity(WarehouseProperty); }
            set { SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 接收车间 Workshop
        /// <summary>
        /// 接收车间Id
        /// </summary>
        [Label("接收车间")]
        public static readonly IRefIdProperty WorkshopIdProperty =
            P<EquipmentInbound>.RegisterRefId(e => e.WorkshopId, ReferenceType.Normal);

        /// <summary>
        /// 接收车间Id
        /// </summary>
        public double? WorkshopId
        {
            get { return (double?)this.GetRefNullableId(WorkshopIdProperty); }
            set { this.SetRefNullableId(WorkshopIdProperty, value); }
        }

        /// <summary>
        /// 接收车间
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> WorkshopProperty =
            P<EquipmentInbound>.RegisterRef(e => e.Workshop, WorkshopIdProperty);

        /// <summary>
        /// 接收车间
        /// </summary>
        public Enterprise Workshop
        {
            get { return this.GetRefEntity(WorkshopProperty); }
            set { this.SetRefEntity(WorkshopProperty, value); }
        }
        #endregion

        #region 入库状态 InboundStatus
        /// <summary>
        /// 入库状态
        /// </summary>
        [Label("入库状态")]
        public static readonly Property<InboundStatus> InboundStatusProperty = P<EquipmentInbound>.Register(e => e.InboundStatus);

        /// <summary>
        /// 入库状态
        /// </summary>
        public InboundStatus InboundStatus
        {
            get { return GetProperty(InboundStatusProperty); }
            set { SetProperty(InboundStatusProperty, value); }
        }
        #endregion

        #region 入库类型 InboundType
        /// <summary>
        /// 入库类型
        /// </summary>
        [Label("入库类型")]
        public static readonly Property<InboundType> InboundTypeProperty = P<EquipmentInbound>.Register(e => e.InboundType);

        /// <summary>
        /// 入库类型
        /// </summary>
        public InboundType InboundType
        {
            get { return GetProperty(InboundTypeProperty); }
            set { SetProperty(InboundTypeProperty, value); }
        }
        #endregion

        #region 开箱验收单号 AcceptanceNo
        /// <summary>
        /// 开箱验收单号
        /// </summary>
        [Label("开箱验收单号")]
        public static readonly Property<string> AcceptanceNoProperty = P<EquipmentInbound>.Register(e => e.AcceptanceNo);

        /// <summary>
        /// 开箱验收单号
        /// </summary>
        public string AcceptanceNo
        {
            get { return this.GetProperty(AcceptanceNoProperty); }
            set { this.SetProperty(AcceptanceNoProperty, value); }
        }
        #endregion

        #region 设备型号维护 EquipModel
        /// <summary>
        /// 设备型号维护Id
        /// </summary>
        [Label("设备型号")]
        public static readonly IRefIdProperty EquipModelIdProperty = P<EquipmentInbound>.RegisterRefId(e => e.EquipModelId, ReferenceType.Normal);

        /// <summary>
        /// 设备型号维护Id
        /// </summary>
        public double EquipModelId
        {
            get { return (double)GetRefId(EquipModelIdProperty); }
            set { SetRefId(EquipModelIdProperty, value); }
        }

        /// <summary>
        /// 设备型号维护
        /// </summary>
        public static readonly RefEntityProperty<EquipModel> EquipModelProperty = P<EquipmentInbound>.RegisterRef(e => e.EquipModel, EquipModelIdProperty);

        /// <summary>
        /// 设备型号维护
        /// </summary>
        public EquipModel EquipModel
        {
            get { return GetRefEntity(EquipModelProperty); }
            set { SetRefEntity(EquipModelProperty, value); }
        }
        #endregion

        #region 供应商 Supplier
        /// <summary>
        /// 供应商Id
        /// </summary>
        [Label("供应商")]
        public static readonly IRefIdProperty SupplierIdProperty = P<EquipmentInbound>.RegisterRefId(e => e.SupplierId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Supplier> SupplierProperty = P<EquipmentInbound>.RegisterRef(e => e.Supplier, SupplierIdProperty);

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
        public static readonly IRefIdProperty CustomerIdProperty = P<EquipmentInbound>.RegisterRefId(e => e.CustomerId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Customer> CustomerProperty = P<EquipmentInbound>.RegisterRef(e => e.Customer, CustomerIdProperty);

        /// <summary>
        /// 客户
        /// </summary>
        public Customer Customer
        {
            get { return GetRefEntity(CustomerProperty); }
            set { SetRefEntity(CustomerProperty, value); }
        }
        #endregion

        #region 接收类型 ReceiveType
        /// <summary>
        /// 接收类型
        /// </summary>
        [Label("接收类型")]
        public static readonly Property<ReceiveType> ReceiveTypeProperty = P<EquipmentInbound>.Register(e => e.ReceiveType);

        /// <summary>
        /// 接收类型
        /// </summary>
        public ReceiveType ReceiveType
        {
            get { return GetProperty(ReceiveTypeProperty); }
            set { SetProperty(ReceiveTypeProperty, value); }
        }
        #endregion

        #region 设备入库明细列表 DetailList
        /// <summary>
        /// 设备入库明细列表
        /// </summary>
        public static readonly ListProperty<EntityList<EquipmentInboundDetail>> DetailListProperty = P<EquipmentInbound>.RegisterList(e => e.DetailList);
        /// <summary>
        /// 设备入库明细列表
        /// </summary>
        public EntityList<EquipmentInboundDetail> DetailList
        {
            get { return this.GetLazyList(DetailListProperty); }
        }
        #endregion

        #region 视图属性
        #region 设备型号名称 EquipModelName
        /// <summary>
        /// 设备型号名称
        /// </summary>
        [Label("设备型号名称")]
        public static readonly Property<string> EquipModelNameProperty = P<EquipmentInbound>.RegisterView(e => e.EquipModelName, p => p.EquipModel.Name);

        /// <summary>
        /// 设备型号名称
        /// </summary>
        public string EquipModelName
        {
            get { return this.GetProperty(EquipModelNameProperty); }
        }
        #endregion

        #region 供应商名称 SupplierName
        /// <summary>
        /// 供应商名称
        /// </summary>
        [Label("供应商名称")]
        public static readonly Property<string> SupplierNameProperty = P<EquipmentInbound>.RegisterView(e => e.SupplierName, p => p.Supplier.Name);

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName
        {
            get { return this.GetProperty(SupplierNameProperty); }
        }
        #endregion

        #region 客户名称 CustomerName
        /// <summary>
        /// 客户名称
        /// </summary>
        [Label("客户名称")]
        public static readonly Property<string> CustomerNameProperty = P<EquipmentInbound>.RegisterView(e => e.CustomerName, p => p.Customer.Name);

        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName
        {
            get { return this.GetProperty(CustomerNameProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 设备入库 实体配置
    /// </summary>
    internal class EquipmentInboundConfig : EntityConfig<EquipmentInbound>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_EQP_IN").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}