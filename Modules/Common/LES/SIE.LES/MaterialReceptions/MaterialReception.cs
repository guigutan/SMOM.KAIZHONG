using SIE.Common.Configs;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.Domain.Query;
using SIE.Items;
using SIE.LES.MaterialReceptions.Configs;
using SIE.LES.StockOrders;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.WipResources;
using SIE.Warehouses;
using System;

namespace SIE.LES.MaterialReceptions
{
    /// <summary>
    /// 物料接收
    /// </summary>
    [RootEntity,Serializable]
    [ConditionQueryType(typeof(MaterialReceptionCriterial))]
    [EntityWithConfig(typeof(MaterialReceptionQtyConfig))]
    [Label("物料接收")]
    public partial class MaterialReception : Entity<Double>
    {
        #region 备料单 StockOrder
        /// <summary>
        /// 备料单Id
        /// </summary>
        [Label("备料单")]
        public static readonly IRefIdProperty StockOrderIdProperty =
            P<MaterialReception>.RegisterRefId(e => e.StockOrderId, ReferenceType.Normal);

        /// <summary>
        /// 备料单Id
        /// </summary>
        public double StockOrderId
        {
            get { return (double)this.GetRefId(StockOrderIdProperty); }
            set { this.SetRefId(StockOrderIdProperty, value); }
        }

        /// <summary>
        /// 备料单
        /// </summary>
        public static readonly RefEntityProperty<StockOrder> StockOrderProperty =
            P<MaterialReception>.RegisterRef(e => e.StockOrder, StockOrderIdProperty);

        /// <summary>
        /// 备料单
        /// </summary>
        public StockOrder StockOrder
        {
            get { return this.GetRefEntity(StockOrderProperty); }
            set { this.SetRefEntity(StockOrderProperty, value); }
        }
        #endregion

        #region 行号 LineNo
        /// <summary>
        /// 行号
        /// </summary>
        [Label("行号")]
        public static readonly Property<string> LineNoProperty = P<MaterialReception>.Register(e => e.LineNo);

        /// <summary>
        /// 行号
        /// </summary>
        public string LineNo
        {
            get { return this.GetProperty(LineNoProperty); }
            set { this.SetProperty(LineNoProperty, value); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<ReceiveState> StateProperty = P<MaterialReception>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public ReceiveState State
        {
            get { return this.GetProperty(StateProperty); }
            set { this.SetProperty(StateProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty =
            P<MaterialReception>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId
        {
            get { return (double)this.GetRefId(ItemIdProperty); }
            set { this.SetRefId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty =
            P<MaterialReception>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return this.GetRefEntity(ItemProperty); }
            set { this.SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 物料拓展属性 ItemExtProp
        /// <summary>
        /// 物料拓展属性
        /// </summary>
        [Label("物料拓展属性")]
        public static readonly Property<string> ItemExtPropProperty = P<MaterialReception>.Register(e => e.ItemExtProp);

        /// <summary>
        /// 物料拓展属性
        /// </summary>
        public string ItemExtProp
        {
            get { return this.GetProperty(ItemExtPropProperty); }
            set { this.SetProperty(ItemExtPropProperty, value); }
        }
        #endregion

        #region 标签号 LabelNo
        /// <summary>
        /// 标签号
        /// </summary>
        [Label("标签号")]
        public static readonly Property<string> LabelNoProperty = P<MaterialReception>.Register(e => e.LabelNo);

        /// <summary>
        /// 标签号
        /// </summary>
        public string LabelNo
        {
            get { return this.GetProperty(LabelNoProperty); }
            set { this.SetProperty(LabelNoProperty, value); }
        }
        #endregion

        #region 批次号 LotNo
        /// <summary>
        /// 批次号
        /// </summary>
        [Label("批次号")]
        public static readonly Property<string> LotNoProperty = P<MaterialReception>.Register(e => e.LotNo);

        /// <summary>
        /// 批次号
        /// </summary>
        public string LotNo
        {
            get { return this.GetProperty(LotNoProperty); }
            set { this.SetProperty(LotNoProperty, value); }
        }
        #endregion

        #region 接收数量 Qty
        /// <summary>
        /// 接收数量
        /// </summary>
        [Label("接收数量")]
        public static readonly Property<decimal> QtyProperty = P<MaterialReception>.Register(e => e.Qty);

        /// <summary>
        /// 接收数量
        /// </summary>
        public decimal Qty
        {
            get { return this.GetProperty(QtyProperty); }
            set { this.SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 发料数量 ShipQty
        /// <summary>
        /// 发料数量
        /// </summary>
        [Label("发料数量")]
        public static readonly Property<decimal> ShipQtyProperty = P<MaterialReception>.Register(e => e.ShipQty);

        /// <summary>
        /// 发料数量
        /// </summary>
        public decimal ShipQty
        {
            get { return this.GetProperty(ShipQtyProperty); }
            set { this.SetProperty(ShipQtyProperty, value); }
        }
        #endregion

        #region 接收仓库 Warehouse
        /// <summary>
        /// 接收仓库Id
        /// </summary>
        [Label("接收仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty =
            P<MaterialReception>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 接收仓库Id
        /// </summary>
        public double? WarehouseId
        {
            get { return (double?)this.GetRefNullableId(WarehouseIdProperty); }
            set { this.SetRefNullableId(WarehouseIdProperty, value); }
        }

        /// <summary>
        /// 接收仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty =
            P<MaterialReception>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 接收仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return this.GetRefEntity(WarehouseProperty); }
            set { this.SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 接收库位 StorageLocation
        /// <summary>
        /// 接收库位Id
        /// </summary>
        [Label("接收库位")]
        public static readonly IRefIdProperty StorageLocationIdProperty =
            P<MaterialReception>.RegisterRefId(e => e.StorageLocationId, ReferenceType.Normal);

        /// <summary>
        /// 接收库位Id
        /// </summary>
        public double? StorageLocationId
        {
            get { return (double?)this.GetRefNullableId(StorageLocationIdProperty); }
            set { this.SetRefNullableId(StorageLocationIdProperty, value); }
        }

        /// <summary>
        /// 接收库位
        /// </summary>
        public static readonly RefEntityProperty<StorageLocation> StorageLocationProperty =
            P<MaterialReception>.RegisterRef(e => e.StorageLocation, StorageLocationIdProperty);

        /// <summary>
        /// 接收库位
        /// </summary>
        public StorageLocation StorageLocation
        {
            get { return this.GetRefEntity(StorageLocationProperty); }
            set { this.SetRefEntity(StorageLocationProperty, value); }
        }
        #endregion

        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty =
            P<MaterialReception>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double? WorkOrderId
        {
            get { return (double?)this.GetRefNullableId(WorkOrderIdProperty); }
            set { this.SetRefNullableId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty =
            P<MaterialReception>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return this.GetRefEntity(WorkOrderProperty); }
            set { this.SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 生产资源 Resource
        /// <summary>
        /// 生产资源Id
        /// </summary>
        [Label("生产资源")]
        public static readonly IRefIdProperty ResourceIdProperty =
            P<MaterialReception>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 生产资源Id
        /// </summary>
        public double? ResourceId
        {
            get { return (double?)this.GetRefNullableId(ResourceIdProperty); }
            set { this.SetRefNullableId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 生产资源
        /// </summary>
        public static readonly RefEntityProperty<WipResource> ResourceProperty =
            P<MaterialReception>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 生产资源
        /// </summary>
        public WipResource Resource
        {
            get { return this.GetRefEntity(ResourceProperty); }
            set { this.SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 发运单号 SoNo
        /// <summary>
        /// 发运单号
        /// </summary>
        [Label("发运单号")]
        public static readonly Property<string> SoNoProperty = P<MaterialReception>.Register(e => e.SoNo);

        /// <summary>
        /// 发运单号
        /// </summary>
        public string SoNo
        {
            get { return this.GetProperty(SoNoProperty); }
            set { this.SetProperty(SoNoProperty, value); }
        }
        #endregion

        #region 发运单行号 SoLineNo
        /// <summary>
        /// 发运单行号
        /// </summary>
        [Label("发运单行号")]
        public static readonly Property<string> SoLineNoProperty = P<MaterialReception>.Register(e => e.SoLineNo);

        /// <summary>
        /// 发运单行号
        /// </summary>
        public string SoLineNo
        {
            get { return this.GetProperty(SoLineNoProperty); }
            set { this.SetProperty(SoLineNoProperty, value); }
        }
        #endregion

        #region 是否启用手工物料接收 IsManualRec
        /// <summary>
        /// 是否启用手工物料接收
        /// </summary>
        [Label("是否启用手工物料接收")]
        public static readonly Property<bool> IsManualRecProperty = P<MaterialReception>.Register(e => e.IsManualRec);

        /// <summary>
        /// 是否启用手工物料接收
        /// </summary>
        public bool IsManualRec
        {
            get { return this.GetProperty(IsManualRecProperty); }
            set { this.SetProperty(IsManualRecProperty, value); }
        }
        #endregion

        #region 接收人 Receiver
        /// <summary>
        /// 接收人Id
        /// </summary>
        [Label("接收人")]
        public static readonly IRefIdProperty ReceiverIdProperty =
            P<MaterialReception>.RegisterRefId(e => e.ReceiverId, ReferenceType.Normal);

        /// <summary>
        /// 接收人Id
        /// </summary>
        public double ReceiverId
        {
            get { return (double)this.GetRefId(ReceiverIdProperty); }
            set { this.SetRefId(ReceiverIdProperty, value); }
        }

        /// <summary>
        /// 接收人
        /// </summary>
        public static readonly RefEntityProperty<Employee> ReceiverProperty =
            P<MaterialReception>.RegisterRef(e => e.Receiver, ReceiverIdProperty);

        /// <summary>
        /// 接收人
        /// </summary>
        public Employee Receiver
        {
            get { return this.GetRefEntity(ReceiverProperty); }
            set { this.SetRefEntity(ReceiverProperty, value); }
        }
        #endregion

        #region 接收时间 ReceiveTime
        /// <summary>
        /// 接收时间
        /// </summary>
        [Label("接收时间")]
        public static readonly Property<DateTime?> ReceiveTimeProperty = P<MaterialReception>.Register(e => e.ReceiveTime);

        /// <summary>
        /// 接收时间
        /// </summary>
        public DateTime? ReceiveTime
        {
            get { return this.GetProperty(ReceiveTimeProperty); }
            set { this.SetProperty(ReceiveTimeProperty, value); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<MaterialReception>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }
        #endregion


        #region 备料单明细ID StockOrderDetailId
        /// <summary>
        /// 备料单明细ID
        /// </summary>
        [Label("备料单明细ID")]
        public static readonly Property<double> StockOrderDetailIdProperty = P<MaterialReception>.Register(e => e.StockOrderDetailId);

        /// <summary>
        /// 备料单明细ID
        /// </summary>
        public double StockOrderDetailId
        {
            get { return this.GetProperty(StockOrderDetailIdProperty); }
        }
        #endregion


        #region 工厂ID FactoryId
        /// <summary>
        /// 工厂ID
        /// </summary>
        [Label("工厂ID")]
        public static readonly Property<double> FactoryIdProperty = P<MaterialReception>.Register(e => e.FactoryId);

        /// <summary>
        /// 工厂ID
        /// </summary>
        public double FactoryId
        {
            get { return this.GetProperty(FactoryIdProperty); }
        }
        #endregion

        #region 视图属性

        #region 生产资源名称 ResourceName
        /// <summary>
        /// 生产资源名称
        /// </summary>
        [Label("生产资源名称")]
        public static readonly Property<string> ResourceNameProperty = P<MaterialReception>.RegisterView(e => e.ResourceName, p => p.Resource.Name);

        /// <summary>
        /// 生产资源名称
        /// </summary>
        public string ResourceName
        {
            get { return this.GetProperty(ResourceNameProperty); }
        }
        #endregion

        #endregion

    }

    /// <summary>
    /// 实体配置
    /// </summary>
    public class MaterialReceptionConfig : EntityConfig<MaterialReception>
    {
        /// <summary>
        /// 元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Func<IQuery> view = () => DB.Query<StockOrderSn>("sos")
                .Join<StockOrder>("so", (sos, so) => sos.StockOrderId == so.Id && so.SQL<int>("so.IS_PHANTOM") == 0 && so.SQL<int>("so.INV_ORG_ID") == RT.InvOrg)
                .Join<StockOrderDetail>("sod", (sos, sod) => sos.StockOrderDetailId == sod.Id && sod.SQL<int>("sod.IS_PHANTOM") == 0 && sod.SQL<int>("sod.INV_ORG_ID") == RT.InvOrg)
                .Select<StockOrder, StockOrderDetail>((sos, so, sod) => new
                {
                    sos.Id,
                    FACTORY_ID=so.FactoryId,
                    STOCK_ORDER_ID = so.Id,
                    STOCK_ORDER_DETAIL_ID= sod.Id,
                    LINE_NO = sos.LineNo,
                    STATE = sos.State,
                    ITEM_ID = sos.ItemId,
                    ITEM_EXT_PROP = sod.ItemExtPropName,
                    LABEL_NO = sos.Sn,
                    LOT_NO = sos.LotNo,
                    QTY = sos.Qty,
                    SHIP_QTY = sos.ShipQty,
                    WAREHOUSE_ID = sod.WarehouseId,
                    STORAGE_LOCATION_ID = sod.StorageLocationId,
                    WORK_ORDER_ID = so.WorkOrderId,
                    RESOURCE_ID = so.ResourceId,
                    SO_NO = sos.SoNo,
                    SO_LINE_NO = sos.SoLineNo,
                    IS_MANUAL_REC = sod.IsManualRec,
                    RECEIVER_ID = sos.ReceiveById,
                    RECEIVE_TIME = sos.ReceiveTime,
                })
                .Where(sos => sos.SQL<int>("sos.IS_PHANTOM") == 0)
                .Where(sos => sos.SQL<int>("sos.INV_ORG_ID") == RT.InvOrg)
                .ToQuery();
            Meta.MapView(view).MapAllProperties();
            Meta.IsTreeEntity = false;
            Meta.DisablePhantoms();
        }
    }
}
