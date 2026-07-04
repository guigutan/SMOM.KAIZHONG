using SIE.CSM.Customers;
using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.EMS.Purchases.PurchaseOrders;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipModels;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Warehouses;
using System;

namespace SIE.EMS.Purchases.EquipmentReceives
{
    /// <summary>
    /// 接收明细
    /// </summary>
    [ChildEntity, Serializable]
    [Label("接收明细")]
    [DisplayMember(nameof(LineNo))]
    public partial class EquipmentReceiveDetail : DataEntity
    {
        #region 设备接收 EquipmentReceive
        /// <summary>
        /// 设备接收Id
        /// </summary>
        public static readonly IRefIdProperty EquipmentReceiveIdProperty = P<EquipmentReceiveDetail>.RegisterRefId(e => e.EquipmentReceiveId, ReferenceType.Parent);

        /// <summary>
        /// 设备接收Id
        /// </summary>
        public double EquipmentReceiveId
        {
            get { return (double)GetRefId(EquipmentReceiveIdProperty); }
            set { SetRefId(EquipmentReceiveIdProperty, value); }
        }

        /// <summary>
        /// 设备接收
        /// </summary>
        public static readonly RefEntityProperty<EquipmentReceive> EquipmentReceiveProperty = P<EquipmentReceiveDetail>.RegisterRef(e => e.EquipmentReceive, EquipmentReceiveIdProperty);

        /// <summary>
        /// 设备接收
        /// </summary>
        public EquipmentReceive EquipmentReceive
        {
            get { return GetRefEntity(EquipmentReceiveProperty); }
            set { SetRefEntity(EquipmentReceiveProperty, value); }
        }
        #endregion

        #region 行号 LineNo
        /// <summary>
        /// 行号
        /// </summary>
        [Label("行号")]
        public static readonly Property<int> LineNoProperty = P<EquipmentReceiveDetail>.Register(e => e.LineNo);

        /// <summary>
        /// 行号
        /// </summary>
        public int LineNo
        {
            get { return GetProperty(LineNoProperty); }
            set { SetProperty(LineNoProperty, value); }
        }
        #endregion

        #region 赠品 Giveaway
        /// <summary>
        /// 赠品
        /// </summary>
        [Label("赠品")]
        public static readonly Property<bool> GiveawayProperty = P<EquipmentReceiveDetail>.Register(e => e.Giveaway);

        /// <summary>
        /// 赠品
        /// </summary>
        public bool Giveaway
        {
            get { return GetProperty(GiveawayProperty); }
            set { SetProperty(GiveawayProperty, value); }
        }
        #endregion

        #region 单价 Price
        /// <summary>
        /// 单价
        /// </summary>
        [Label("单价")]
        public static readonly Property<decimal> PriceProperty = P<EquipmentReceiveDetail>.Register(e => e.Price);

        /// <summary>
        /// 单价
        /// </summary>
        public decimal Price
        {
            get { return GetProperty(PriceProperty); }
            set { SetProperty(PriceProperty, value); }
        }
        #endregion

        #region 接收数量 Qty
        /// <summary>
        /// 接收数量
        /// </summary>
        [Label("接收数量")]
        [MinValue(1)]
        public static readonly Property<int> QtyProperty = P<EquipmentReceiveDetail>.Register(e => e.Qty);

        /// <summary>
        /// 接收数量
        /// </summary>
        public int Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 已接收数量 RecivedQty
        /// <summary>
        /// 已接收数量
        /// </summary>
        [Label("已接收数量")]
        public static readonly Property<int> RecivedQtyProperty = P<EquipmentReceiveDetail>.Register(e => e.RecivedQty);

        /// <summary>
        /// 已接收数量
        /// </summary>
        public int RecivedQty
        {
            get { return GetProperty(RecivedQtyProperty); }
            set { SetProperty(RecivedQtyProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(1000)]
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<EquipmentReceiveDetail>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 采购订单 PurchaseOrder
        /// <summary>
        /// 采购订单Id
        /// </summary>
        [Label("采购单号")]
        public static readonly IRefIdProperty PurchaseOrderIdProperty =
            P<EquipmentReceiveDetail>.RegisterRefId(e => e.PurchaseOrderId, ReferenceType.Normal);

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
            P<EquipmentReceiveDetail>.RegisterRef(e => e.PurchaseOrder, PurchaseOrderIdProperty);

        /// <summary>
        /// 采购订单
        /// </summary>
        public PurchaseOrder PurchaseOrder
        {
            get { return this.GetRefEntity(PurchaseOrderProperty); }
            set { this.SetRefEntity(PurchaseOrderProperty, value); }
        }
        #endregion

        #region 采购订单明细 PurchaseOrderItem
        /// <summary>
        /// 采购订单明细Id
        /// </summary>
        public static readonly IRefIdProperty PurchaseOrderItemIdProperty = P<EquipmentReceiveDetail>.RegisterRefId(e => e.PurchaseOrderItemId, ReferenceType.Normal);

        /// <summary>
        /// 采购订单明细Id
        /// </summary>
        public double? PurchaseOrderItemId
        {
            get { return (double?)GetRefNullableId(PurchaseOrderItemIdProperty); }
            set { SetRefNullableId(PurchaseOrderItemIdProperty, value); }
        }

        /// <summary>
        /// 采购订单明细
        /// </summary>
        public static readonly RefEntityProperty<PurchaseOrderItem> PurchaseOrderItemProperty = P<EquipmentReceiveDetail>.RegisterRef(e => e.PurchaseOrderItem, PurchaseOrderItemIdProperty);

        /// <summary>
        /// 采购订单明细
        /// </summary>
        public PurchaseOrderItem PurchaseOrderItem
        {
            get { return GetRefEntity(PurchaseOrderItemProperty); }
            set { SetRefEntity(PurchaseOrderItemProperty, value); }
        }
        #endregion

        #region 接收仓库 Warehouse
        /// <summary>
        /// 接收仓库Id
        /// </summary>
        [Label("接收仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty = P<EquipmentReceiveDetail>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 接收仓库Id
        /// </summary>
        public double? WarehouseId
        {
            get { return (double?)GetRefNullableId(WarehouseIdProperty); }
            set { SetRefNullableId(WarehouseIdProperty, value); }
        }

        /// <summary>
        /// 接收仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<EquipmentReceiveDetail>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 接收仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return GetRefEntity(WarehouseProperty); }
            set { SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 接收车间 WorkShop
        /// <summary>
        /// 接收车间Id
        /// </summary>
        [Label("接收车间")]
        public static readonly IRefIdProperty WorkshopIdProperty =
            P<EquipmentReceiveDetail>.RegisterRefId(e => e.WorkshopId, ReferenceType.Normal);

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
            P<EquipmentReceiveDetail>.RegisterRef(e => e.Workshop, WorkshopIdProperty);

        /// <summary>
        /// 接收车间
        /// </summary>
        public Enterprise Workshop
        {
            get { return this.GetRefEntity(WorkshopProperty); }
            set { this.SetRefEntity(WorkshopProperty, value); }
        }
        #endregion


        #region 客户 Customer
        /// <summary>
        /// 客户Id
        /// </summary>
        public static readonly IRefIdProperty CustomerIdProperty = P<EquipmentReceiveDetail>.RegisterRefId(e => e.CustomerId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Customer> CustomerProperty = P<EquipmentReceiveDetail>.RegisterRef(e => e.Customer, CustomerIdProperty);

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
        public static readonly IRefIdProperty SupplierIdProperty = P<EquipmentReceiveDetail>.RegisterRefId(e => e.SupplierId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Supplier> SupplierProperty = P<EquipmentReceiveDetail>.RegisterRef(e => e.Supplier, SupplierIdProperty);

        /// <summary>
        /// 供应商
        /// </summary>
        public Supplier Supplier
        {
            get { return GetRefEntity(SupplierProperty); }
            set { SetRefEntity(SupplierProperty, value); }
        }
        #endregion

        #region 设备型号维护 EquipModel
        /// <summary>
        /// 设备型号维护Id
        /// </summary>
        public static readonly IRefIdProperty EquipModelIdProperty = P<EquipmentReceiveDetail>.RegisterRefId(e => e.EquipModelId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<EquipModel> EquipModelProperty = P<EquipmentReceiveDetail>.RegisterRef(e => e.EquipModel, EquipModelIdProperty);

        /// <summary>
        /// 设备型号维护
        /// </summary>
        public EquipModel EquipModel
        {
            get { return GetRefEntity(EquipModelProperty); }
            set { SetRefEntity(EquipModelProperty, value); }
        }
        #endregion

        #region 采购订单的设备型号id OrderEquipModelId
        /// <summary>
        /// 采购订单的设备型号id
        /// </summary>
        [Label("采购订单的设备型号id")]
        public static readonly Property<double?> OrderEquipModelIdProperty = P<EquipmentReceiveDetail>.Register(e => e.OrderEquipModelId);

        /// <summary>
        /// 采购订单的设备型号id
        /// </summary>
        public double? OrderEquipModelId
        {
            get { return this.GetProperty(OrderEquipModelIdProperty); }
            set { this.SetProperty(OrderEquipModelIdProperty, value); }
        }
        #endregion

        #region 序列号明细列表 SnList
        /// <summary>
        /// 序列号明细列表
        /// </summary>
        public static readonly ListProperty<EntityList<EquipmentReceiveSn>> SnListProperty = P<EquipmentReceiveDetail>.RegisterList(e => e.SnList);
        /// <summary>
        /// 序列号明细列表
        /// </summary>
        public EntityList<EquipmentReceiveSn> SnList
        {
            get { return this.GetLazyList(SnListProperty); }
        }
        #endregion

        #region 视图属性
        #region 型号编码 EquipModelCode
        /// <summary>
        /// 型号编码
        /// </summary>
        [Label("型号编码")]
        public static readonly Property<string> EquipModelCodeProperty = P<EquipmentReceiveDetail>.RegisterView(e => e.EquipModelCode, p => p.EquipModel.Code);

        /// <summary>
        /// 型号编码
        /// </summary>
        public string EquipModelCode
        {
            get { return this.GetProperty(EquipModelCodeProperty); }
        }
        #endregion

        #region 设备型号名称 EquipModelName
        /// <summary>
        /// 设备型号名称
        /// </summary>
        [Label("设备型号名称")]
        public static readonly Property<string> EquipModelNameProperty = P<EquipmentReceiveDetail>.RegisterView(e => e.EquipModelName, p => p.EquipModel.Name);

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
        public static readonly Property<string> SupplierNameProperty = P<EquipmentReceiveDetail>.RegisterView(e => e.SupplierName, p => p.Supplier.Name);

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
        public static readonly Property<string> CustomerNameProperty = P<EquipmentReceiveDetail>.RegisterView(e => e.CustomerName, p => p.Customer.Name);

        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName
        {
            get { return this.GetProperty(CustomerNameProperty); }
        }
        #endregion

        #region 采购单号 PurOrderNo
        /// <summary>
        /// 采购单号
        /// </summary>
        [Label("采购单号")]
        public static readonly Property<string> PurOrderNoProperty = P<EquipmentReceiveDetail>.RegisterView(e => e.PurOrderNo, p => p.PurchaseOrder.OrderNo);

        /// <summary>
        /// 采购单号
        /// </summary>
        public string PurOrderNo
        {
            get { return this.GetProperty(PurOrderNoProperty); }
        }
        #endregion

        #region 工厂id FactoryId
        /// <summary>
        /// 工厂id
        /// </summary>
        [Label("工厂id")]
        public static readonly Property<double> FactoryIdProperty = P<EquipmentReceiveDetail>.RegisterView(e => e.FactoryId, p => p.EquipmentReceive.FactoryId);

        /// <summary>
        /// 工厂id
        /// </summary>
        public double FactoryId
        {
            get { return this.GetProperty(FactoryIdProperty); }
        }
        #endregion

        #region 部门id DepartmentId
        /// <summary>
        /// 部门id
        /// </summary>
        [Label("部门id")]
        public static readonly Property<double> DepartmentIdProperty = P<EquipmentReceiveDetail>.RegisterView(e => e.DepartmentId, p => p.EquipmentReceive.DepartmentId);

        /// <summary>
        /// 部门id
        /// </summary>
        public double DepartmentId
        {
            get { return this.GetProperty(DepartmentIdProperty); }
        }
        #endregion

        #region 接收类型 ReceiveType
        /// <summary>
        /// 接收类型
        /// </summary>
        [Label("接收类型")]
        public static readonly Property<ReceiveType> ReceiveTypeProperty = P<EquipmentReceiveDetail>.RegisterView(e => e.ReceiveType, p => p.EquipmentReceive.ReceiveType);

        /// <summary>
        /// 接收类型
        /// </summary>
        public ReceiveType ReceiveType
        {
            get { return this.GetProperty(ReceiveTypeProperty); }
        }
        #endregion

        #region 订单描述 PurchaseOrderDescription
        /// <summary>
        /// 订单描述
        /// </summary>
        [Label("订单描述")]
        public static readonly Property<string> PurchaseOrderDescriptionProperty = P<EquipmentReceiveDetail>.RegisterView(e => e.PurchaseOrderDescription, p => p.PurchaseOrder.Description);

        /// <summary>
        /// 订单描述
        /// </summary>
        public string PurchaseOrderDescription
        {
            get { return this.GetProperty(PurchaseOrderDescriptionProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 接收明细 实体配置
    /// </summary>
    internal class EquipmentReceiveDetailConfig : EntityConfig<EquipmentReceiveDetail>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_EQP_RECV_DTL").MapAllProperties();
            Meta.Property(EquipmentReceiveDetail.RemarkProperty).ColumnMeta.HasLength(2000);
            Meta.EnablePhantoms();
        }
    }
}