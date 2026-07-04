using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Warehouses;
using System;

namespace SIE.LES.StockOrders
{
    /// <summary>
    /// 备料单序列号接收记录
    /// </summary>
    [RootEntity, Serializable]
    [Label("备料单序列号接收记录")]
    public class StockOrderSn : DataEntity
    {
        #region 行号 LineNo
        /// <summary>
        /// 行号
        /// </summary>
        [Label("行号")]
        public static readonly Property<string> LineNoProperty = P<StockOrderSn>.Register(e => e.LineNo);

        /// <summary>
        /// 行号
        /// </summary>
        public string LineNo
        {
            get { return this.GetProperty(LineNoProperty); }
            set { this.SetProperty(LineNoProperty, value); }
        }
        #endregion

        #region 物料标签 Sn
        /// <summary>
        /// 物料标签
        /// </summary>
        [Label("物料标签")]
        public static readonly Property<string> SnProperty = P<StockOrderSn>.Register(e => e.Sn);

        /// <summary>
        /// 物料标签
        /// </summary>
        public string Sn
        {
            get { return this.GetProperty(SnProperty); }
            set { this.SetProperty(SnProperty, value); }
        }
        #endregion

        #region 上级条码 PackageNo
        /// <summary>
        /// 上级条码
        /// </summary>
        [Label("上级条码")]
        public static readonly Property<string> PackageNoProperty = P<StockOrderSn>.Register(e => e.PackageNo);

        /// <summary>
        /// 上级条码
        /// </summary>
        public string PackageNo
        {
            get { return this.GetProperty(PackageNoProperty); }
            set { this.SetProperty(PackageNoProperty, value); }
        }
        #endregion

        #region 接收数量 Qty
        /// <summary>
        /// 接收数量
        /// </summary>
        [Label("接收数量")]
        public static readonly Property<decimal> QtyProperty = P<StockOrderSn>.Register(e => e.Qty);

        /// <summary>
        /// 接收数量
        /// </summary>
        public decimal Qty
        {
            get { return this.GetProperty(QtyProperty); }
            set { this.SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 备料单 StockOrder
        /// <summary>
        /// 备料单Id
        /// </summary>
        public static readonly IRefIdProperty StockOrderIdProperty = P<StockOrderSn>.RegisterRefId(e => e.StockOrderId, ReferenceType.Normal);

        /// <summary>
        /// 备料单Id
        /// </summary>
        public double StockOrderId
        {
            get { return (double)GetRefId(StockOrderIdProperty); }
            set { SetRefId(StockOrderIdProperty, value); }
        }

        /// <summary>
        /// 备料单
        /// </summary>
        public static readonly RefEntityProperty<StockOrder> StockOrderProperty = P<StockOrderSn>.RegisterRef(e => e.StockOrder, StockOrderIdProperty);

        /// <summary>
        /// 备料单
        /// </summary>
        public StockOrder StockOrder
        {
            get { return GetRefEntity(StockOrderProperty); }
            set { SetRefEntity(StockOrderProperty, value); }
        }
        #endregion

        #region 需求明细 StockOrderDetail
        /// <summary>
        /// 需求明细Id
        /// </summary>
        public static readonly IRefIdProperty StockOrderDetailIdProperty = P<StockOrderSn>.RegisterRefId(e => e.StockOrderDetailId, ReferenceType.Normal);

        /// <summary>
        /// 需求明细Id
        /// </summary>
        public double StockOrderDetailId
        {
            get { return (double)GetRefId(StockOrderDetailIdProperty); }
            set { SetRefId(StockOrderDetailIdProperty, value); }
        }

        /// <summary>
        /// 需求明细
        /// </summary>
        public static readonly RefEntityProperty<StockOrderDetail> StockOrderDetailProperty = P<StockOrderSn>.RegisterRef(e => e.StockOrderDetail, StockOrderDetailIdProperty);

        /// <summary>
        /// 需求明细
        /// </summary>
        public StockOrderDetail StockOrderDetail
        {
            get { return GetRefEntity(StockOrderDetailProperty); }
            set { SetRefEntity(StockOrderDetailProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料编码")]
        public static readonly IRefIdProperty ItemIdProperty = P<StockOrderSn>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId
        {
            get { return (double)GetRefId(ItemIdProperty); }
            set { SetRefId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty = P<StockOrderSn>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 批次号 LotNo
        /// <summary>
        /// 批次号
        /// </summary>
        [Label("批次号")]
        public static readonly Property<string> LotNoProperty = P<StockOrderSn>.Register(e => e.LotNo);

        /// <summary>
        /// 批次号
        /// </summary>
        public string LotNo
        {
            get { return this.GetProperty(LotNoProperty); }
            set { this.SetProperty(LotNoProperty, value); }
        }
        #endregion

        #region 发货数量 ShipQty
        /// <summary>
        /// 发货数量
        /// </summary>
        [Label("发货数量")]
        public static readonly Property<decimal> ShipQtyProperty = P<StockOrderSn>.Register(e => e.ShipQty);

        /// <summary>
        /// 发货数量
        /// </summary>
        public decimal ShipQty
        {
            get { return this.GetProperty(ShipQtyProperty); }
            set { this.SetProperty(ShipQtyProperty, value); }
        }
        #endregion

        #region 发运单号 SoNo
        /// <summary>
        /// 发运单号
        /// </summary>
        [Label("发运单号")]
        public static readonly Property<string> SoNoProperty = P<StockOrderSn>.Register(e => e.SoNo);

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
        public static readonly Property<string> SoLineNoProperty = P<StockOrderSn>.Register(e => e.SoLineNo);

        /// <summary>
        /// 发运单行号
        /// </summary>
        public string SoLineNo
        {
            get { return this.GetProperty(SoLineNoProperty); }
            set { this.SetProperty(SoLineNoProperty, value); }
        }
        #endregion

        #region 接收人 ReceiveBy
        /// <summary>
        /// 接收人Id
        /// </summary>
        [Label("接收人")]
        public static readonly IRefIdProperty ReceiveByIdProperty =
            P<StockOrderSn>.RegisterRefId(e => e.ReceiveById, ReferenceType.Normal);

        /// <summary>
        /// 接收人Id
        /// </summary>
        public double? ReceiveById
        {
            get { return (double?)this.GetRefId(ReceiveByIdProperty); }
            set { this.SetRefId(ReceiveByIdProperty, value); }
        }

        /// <summary>
        /// 接收人
        /// </summary>
        public static readonly RefEntityProperty<Employee> ReceiveByProperty =
            P<StockOrderSn>.RegisterRef(e => e.ReceiveBy, ReceiveByIdProperty);

        /// <summary>
        /// 接收人
        /// </summary>
        public Employee ReceiveBy
        {
            get { return this.GetRefEntity(ReceiveByProperty); }
            set { this.SetRefEntity(ReceiveByProperty, value); }
        }
        #endregion

        #region 接收时间 ReceiveTime
        /// <summary>
        /// 接收时间
        /// </summary>
        [Label("接收时间")]
        public static readonly Property<DateTime?> ReceiveTimeProperty = P<StockOrderSn>.Register(e => e.ReceiveTime);

        /// <summary>
        /// 接收时间
        /// </summary>
        public DateTime? ReceiveTime
        {
            get { return this.GetProperty(ReceiveTimeProperty); }
            set { this.SetProperty(ReceiveTimeProperty, value); }
        }
        #endregion


        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<ReceiveState?> StateProperty = P<StockOrderSn>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public ReceiveState? State
        {
            get { return this.GetProperty(StateProperty); }
            set { this.SetProperty(StateProperty, value); }
        }
        #endregion

        #region 序列号管理 IsSerialNumber
        /// <summary>
        /// 序列号管理
        /// </summary>
        [Label("序列号管理")]
        public static readonly Property<bool> IsSerialNumberProperty = P<StockOrderSn>.Register(e => e.IsSerialNumber);

        /// <summary>
        /// 序列号管理
        /// </summary>
        public bool IsSerialNumber
        {
            get { return this.GetProperty(IsSerialNumberProperty); }
            set { this.SetProperty(IsSerialNumberProperty, value); }
        }
        #endregion

        #region 批次管理 IsBatch
        /// <summary>
        /// 批次管理
        /// </summary>
        [Label("批次管理")]
        public static readonly Property<bool> IsBatchProperty = P<StockOrderSn>.Register(e => e.IsBatch);

        /// <summary>
        /// 批次管理
        /// </summary>
        public bool IsBatch
        {
            get { return this.GetProperty(IsBatchProperty); }
            set { this.SetProperty(IsBatchProperty, value); }
        }
        #endregion

        #region 发货仓库 Warehouse
        /// <summary>
        /// 发货仓库Id
        /// </summary>
        [Label("发货仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty =
            P<StockOrderSn>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 发货仓库Id
        /// </summary>
        public double? WarehouseId
        {
            get { return (double?)this.GetRefNullableId(WarehouseIdProperty); }
            set { this.SetRefNullableId(WarehouseIdProperty, value); }
        }

        /// <summary>
        /// 发货仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty =
            P<StockOrderSn>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 发货仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return this.GetRefEntity(WarehouseProperty); }
            set { this.SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 配送单号 DistributionNo
        /// <summary>
        /// 配送单号
        /// </summary>
        [Label("配送单号")]
        public static readonly Property<string> DistributionNoProperty = P<StockOrderSn>.Register(e => e.DistributionNo);

        /// <summary>
        /// 配送单号
        /// </summary>
        public string DistributionNo
        {
            get { return this.GetProperty(DistributionNoProperty); }
            set { this.SetProperty(DistributionNoProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 备料单号 StockOrderNo
        /// <summary>
        /// 备料单号
        /// </summary>
        [Label("备料单号")]
        public static readonly Property<string> StockOrderNoProperty = P<StockOrderSn>.RegisterView(e => e.StockOrderNo, p => p.StockOrder.No);

        /// <summary>
        /// 备料单号
        /// </summary>
        public string StockOrderNo
        {
            get { return this.GetProperty(StockOrderNoProperty); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<StockOrderSn>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<StockOrderSn>.RegisterView(e => e.ItemCode, p => p.Item.Code);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
        }
        #endregion

        #region 物料扩展属性 ItemExtPropName
        /// <summary>
        /// 物料扩展属性
        /// </summary>
        [Label("物料扩展属性")]
        public static readonly Property<string> ItemExtPropNameProperty = P<StockOrderSn>.RegisterView(e => e.ItemExtPropName, p => p.StockOrderDetail.ItemExtPropName);

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtPropName
        {
            get { return this.GetProperty(ItemExtPropNameProperty); }
        }
        #endregion

        #region 工单 WoNo
        /// <summary>
        /// 工单
        /// </summary>
        [Label("工单")]
        public static readonly Property<string> WoNoProperty = P<StockOrderSn>.RegisterView(e => e.WoNo, p => p.StockOrder.WorkOrder.No);

        /// <summary>
        /// 工单
        /// </summary>
        public string WoNo
        {
            get { return this.GetProperty(WoNoProperty); }
        }
        #endregion

        #region 发货仓库 WarehouseCode
        /// <summary>
        /// 发货仓库
        /// </summary>
        [Label("发货仓库")]
        public static readonly Property<string> WarehouseCodeProperty = P<StockOrderSn>.RegisterView(e => e.WarehouseCode, p => p.Warehouse.Code);

        /// <summary>
        /// 发货仓库
        /// </summary>
        public string WarehouseCode
        {
            get { return this.GetProperty(WarehouseCodeProperty); }
        }
        #endregion

        #region 接收人 ReceiveByName
        /// <summary>
        /// 接收人
        /// </summary>
        [Label("接收人")]
        public static readonly Property<string> ReceiveByNameProperty = P<StockOrderSn>.RegisterView(e => e.ReceiveByName, p => p.ReceiveBy.Name);

        /// <summary>
        /// 接收人
        /// </summary>
        public string ReceiveByName
        {
            get { return this.GetProperty(ReceiveByNameProperty); }
        }
        #endregion

        #region 车间 WorkShopName
        /// <summary>
        /// 车间
        /// </summary>
        [Label("车间")]
        public static readonly Property<string> WorkShopNameProperty = P<StockOrderSn>.RegisterView(e => e.WorkShopName, p => p.StockOrder.WorkShop.Name);

        /// <summary>
        /// 车间
        /// </summary>
        public string WorkShopName
        {
            get { return this.GetProperty(WorkShopNameProperty); }
        }
        #endregion

        #region 资源 ResourceName
        /// <summary>
        /// 资源
        /// </summary>
        [Label("资源")]
        public static readonly Property<string> ResourceNameProperty = P<StockOrderSn>.RegisterView(e => e.ResourceName, p => p.StockOrder.Resource.Name);

        /// <summary>
        /// 资源
        /// </summary>
        public string ResourceName
        {
            get { return this.GetProperty(ResourceNameProperty); }
        }
        #endregion

        #region 工厂 FactoryName
        /// <summary>
        /// 工厂
        /// </summary>
        [Label("工厂")]
        public static readonly Property<string> FactoryNameProperty = P<StockOrderSn>.RegisterView(e => e.FactoryName, p => p.StockOrder.Factory.Name);

        /// <summary>
        /// 工厂
        /// </summary>
        public string FactoryName
        {
            get { return this.GetProperty(FactoryNameProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    ///  实体配置
    /// </summary>
    internal class StockOrderSnConfig : EntityConfig<StockOrderSn>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("STOCK_ORDER_SN").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
