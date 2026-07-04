using SIE.Domain;
using SIE.LES.StockOrders;
using SIE.ObjectModel;
using System;

namespace SIE.LES.MaterialReceptions.ViewModels
{
    /// <summary>
    /// 添加按明细、按单接收
    /// </summary>
    [RootEntity, Serializable]
    [Label("添加按明细、按单接收")]
    public class MaterialReceptionAddViewModel : ViewModel
    {
        #region 明细行Id DetailId
        /// <summary>
        /// 明细行Id
        /// </summary>
        [Label("明细行Id")]
        public static readonly Property<double> DetailIdProperty = P<MaterialReceptionAddViewModel>.Register(e => e.DetailId);

        /// <summary>
        /// 明细行Id
        /// </summary>
        public double DetailId
        {
            get { return this.GetProperty(DetailIdProperty); }
            set { this.SetProperty(DetailIdProperty, value); }
        }
        #endregion

        #region 备料单Id StorkOrderId
        /// <summary>
        /// 备料单Id
        /// </summary>
        [Label("备料单Id")]
        public static readonly Property<double> StockOrderIdProperty = P<MaterialReceptionAddViewModel>.Register(e => e.StockOrderId);

        /// <summary>
        /// 备料单Id
        /// </summary>
        public double StockOrderId
        {
            get { return this.GetProperty(StockOrderIdProperty); }
            set { this.SetProperty(StockOrderIdProperty, value); }
        }
        #endregion

        #region 备料单号 StockOrderNo
        /// <summary>
        /// 备料单号
        /// </summary>
        [Label("备料单号")]
        public static readonly Property<string> StockOrderNoProperty = P<MaterialReceptionAddViewModel>.Register(e => e.StockOrderNo);

        /// <summary>
        /// 备料单号
        /// </summary>
        public string StockOrderNo
        {
            get { return this.GetProperty(StockOrderNoProperty); }
            set { this.SetProperty(StockOrderNoProperty, value); }
        }
        #endregion

        #region 行号 LineNo
        /// <summary>
        /// 行号
        /// </summary>
        [Label("行号")]
        public static readonly Property<string> LineNoProperty = P<MaterialReceptionAddViewModel>.Register(e => e.LineNo);

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
        public static readonly Property<ReceiveState?> StateProperty = P<MaterialReceptionAddViewModel>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public ReceiveState? State
        {
            get { return this.GetProperty(StateProperty); }
            set { this.SetProperty(StateProperty, value); }
        }
        #endregion

        #region 物料Id ItemId
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料Id")]
        public static readonly Property<double> ItemIdProperty = P<MaterialReceptionAddViewModel>.Register(e => e.ItemId);

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId
        {
            get { return this.GetProperty(ItemIdProperty); }
            set { this.SetProperty(ItemIdProperty, value); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<MaterialReceptionAddViewModel>.Register(e => e.ItemCode);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
            set { this.SetProperty(ItemCodeProperty, value); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<MaterialReceptionAddViewModel>.Register(e => e.ItemName);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
            set { this.SetProperty(ItemNameProperty, value); }
        }
        #endregion

        #region 物料拓展属性 ItemExtProp
        /// <summary>
        /// 物料拓展属性
        /// </summary>
        [Label("物料拓展属性")]
        public static readonly Property<string> ItemExtPropProperty = P<MaterialReceptionAddViewModel>.Register(e => e.ItemExtProp);

        /// <summary>
        /// 物料拓展属性
        /// </summary>
        public string ItemExtProp
        {
            get { return this.GetProperty(ItemExtPropProperty); }
            set { this.SetProperty(ItemExtPropProperty, value); }
        }
        #endregion

        #region 物料拓展属性(显示) ItemExtPropName
        /// <summary>
        /// 物料拓展属性(显示)
        /// </summary>
        [Label("物料拓展属性(显示)")]
        public static readonly Property<string> ItemExtPropNameProperty = P<MaterialReceptionAddViewModel>.Register(e => e.ItemExtPropName);

        /// <summary>
        /// 物料拓展属性(显示)
        /// </summary>
        public string ItemExtPropName
        {
            get { return this.GetProperty(ItemExtPropNameProperty); }
            set { this.SetProperty(ItemExtPropNameProperty, value); }
        }
        #endregion

        #region 标签号 LabelNo
        /// <summary>
        /// 
        /// </summary>
        [Label("标签号")]
        public static readonly Property<string> LabelNoProperty = P<MaterialReceptionAddViewModel>.Register(e => e.LabelNo);

        /// <summary>
        /// 
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
        public static readonly Property<string> LotNoProperty = P<MaterialReceptionAddViewModel>.Register(e => e.LotNo);

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
        public static readonly Property<decimal> QtyProperty = P<MaterialReceptionAddViewModel>.Register(e => e.Qty);

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
        public static readonly Property<decimal> ShipQtyProperty = P<MaterialReceptionAddViewModel>.Register(e => e.ShipQty);

        /// <summary>
        /// 发料数量
        /// </summary>
        public decimal ShipQty
        {
            get { return this.GetProperty(ShipQtyProperty); }
            set { this.SetProperty(ShipQtyProperty, value); }
        }
        #endregion

        #region 待收数量 StayQty
        /// <summary>
        /// 待收数量
        /// </summary>
        [Label("待收数量")]
        public static readonly Property<decimal> StayQtyProperty = P<MaterialReceptionAddViewModel>.Register(e => e.StayQty);

        /// <summary>
        /// 待收数量
        /// </summary>
        public decimal StayQty
        {
            get { return this.GetProperty(StayQtyProperty); }
            set { this.SetProperty(StayQtyProperty, value); }
        }
        #endregion

        #region 接收仓库Id WarehouseId
        /// <summary>
        /// 接收仓库Id
        /// </summary>
        [Label("接收仓库Id")]
        public static readonly Property<double?> WarehouseIdProperty = P<MaterialReceptionAddViewModel>.Register(e => e.WarehouseId);

        /// <summary>
        /// 接收仓库Id
        /// </summary>
        public double? WarehouseId
        {
            get { return this.GetProperty(WarehouseIdProperty); }
            set { this.SetProperty(WarehouseIdProperty, value); }
        }
        #endregion

        #region 接收仓库名称 WarehouseName
        /// <summary>
        /// 接收仓库名称
        /// </summary>
        [Label("接收仓库名称")]
        public static readonly Property<string> WarehouseNameProperty = P<MaterialReceptionAddViewModel>.Register(e => e.WarehouseName);

        /// <summary>
        /// 接收仓库名称
        /// </summary>
        public string WarehouseName
        {
            get { return this.GetProperty(WarehouseNameProperty); }
            set { this.SetProperty(WarehouseNameProperty, value); }
        }
        #endregion

        #region 接收库位Id StorageLocationId
        /// <summary>
        /// 接收库位Id
        /// </summary>
        [Label("接收库位Id")]
        public static readonly Property<double?> StorageLocationIdProperty = P<MaterialReceptionAddViewModel>.Register(e => e.StorageLocationId);

        /// <summary>
        /// 接收库位Id
        /// </summary>
        public double? StorageLocationId
        {
            get { return this.GetProperty(StorageLocationIdProperty); }
            set { this.SetProperty(StorageLocationIdProperty, value); }
        }
        #endregion

        #region 接收库位名称 StorageLocationName
        /// <summary>
        /// 接收库位名称
        /// </summary>
        [Label("接收库位名称")]
        public static readonly Property<string> StorageLocationNameProperty = P<MaterialReceptionAddViewModel>.Register(e => e.StorageLocationName);

        /// <summary>
        /// 接收库位名称
        /// </summary>
        public string StorageLocationName
        {
            get { return this.GetProperty(StorageLocationNameProperty); }
            set { this.SetProperty(StorageLocationNameProperty, value); }
        }
        #endregion

        #region 工单Id WorkOrderId
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单Id")]
        public static readonly Property<double?> WorkOrderIdProperty = P<MaterialReceptionAddViewModel>.Register(e => e.WorkOrderId);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double? WorkOrderId
        {
            get { return this.GetProperty(WorkOrderIdProperty); }
            set { this.SetProperty(WorkOrderIdProperty, value); }
        }
        #endregion

        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<MaterialReceptionAddViewModel>.Register(e => e.WorkOrderNo);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
            set { this.SetProperty(WorkOrderNoProperty, value); }
        }
        #endregion

        #region 资源Id ResourceId
        /// <summary>
        /// 资源Id
        /// </summary>
        [Label("资源Id")]
        public static readonly Property<double?> ResourceIdProperty = P<MaterialReceptionAddViewModel>.Register(e => e.ResourceId);

        /// <summary>
        /// 资源Id
        /// </summary>
        public double? ResourceId
        {
            get { return this.GetProperty(ResourceIdProperty); }
            set { this.SetProperty(ResourceIdProperty, value); }
        }
        #endregion

        #region 资源名称 ResourceName
        /// <summary>
        /// 资源名称
        /// </summary>
        [Label("资源名称")]
        public static readonly Property<string> ResourceNameProperty = P<MaterialReceptionAddViewModel>.Register(e => e.ResourceName);

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName
        {
            get { return this.GetProperty(ResourceNameProperty); }
            set { this.SetProperty(ResourceNameProperty, value); }
        }
        #endregion

        #region 发运单号 SoNo
        /// <summary>
        /// 发运单号
        /// </summary>
        [Label("发运单号")]
        public static readonly Property<string> SoNoProperty = P<MaterialReceptionAddViewModel>.Register(e => e.SoNo);

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
        public static readonly Property<string> SoLineNoProperty = P<MaterialReceptionAddViewModel>.Register(e => e.SoLineNo);

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
        public static readonly Property<bool> IsManualRecProperty = P<MaterialReceptionAddViewModel>.Register(e => e.IsManualRec);

        /// <summary>
        /// 是否启用手工物料接收
        /// </summary>
        public bool IsManualRec
        {
            get { return this.GetProperty(IsManualRecProperty); }
            set { this.SetProperty(IsManualRecProperty, value); }
        }
        #endregion

        #region 接收人Id ReceiverId
        /// <summary>
        /// 接收人Id
        /// </summary>
        [Label("接收人Id")]
        public static readonly Property<double?> ReceiverIdProperty = P<MaterialReceptionAddViewModel>.Register(e => e.ReceiverId);

        /// <summary>
        /// 接收人Id
        /// </summary>
        public double? ReceiverId
        {
            get { return this.GetProperty(ReceiverIdProperty); }
            set { this.SetProperty(ReceiverIdProperty, value); }
        }
        #endregion

        #region 接收人名称 ReceiverName
        /// <summary>
        /// 接收人名称
        /// </summary>
        [Label("接收人名称")]
        public static readonly Property<string> ReceiverNameProperty = P<MaterialReceptionAddViewModel>.Register(e => e.ReceiverName);

        /// <summary>
        /// 接收人名称
        /// </summary>
        public string ReceiverName
        {
            get { return this.GetProperty(ReceiverNameProperty); }
            set { this.SetProperty(ReceiverNameProperty, value); }
        }
        #endregion

        #region 接收时间 ReceiverTime
        /// <summary>
        /// 接收时间
        /// </summary>
        [Label("接收时间")]
        public static readonly Property<DateTime?> ReceiverTimeProperty = P<MaterialReceptionAddViewModel>.Register(e => e.ReceiverTime);

        /// <summary>
        /// 接收时间
        /// </summary>
        public DateTime? ReceiverTime
        {
            get { return this.GetProperty(ReceiverTimeProperty); }
            set { this.SetProperty(ReceiverTimeProperty, value); }
        }
        #endregion

        #region 物料需求Id StockOrderDetailId
        /// <summary>
        /// 物料需求Id
        /// </summary>
        [Label("物料需求Id")]
        public static readonly Property<double> StockOrderDetailIdProperty = P<MaterialReceptionAddViewModel>.Register(e => e.StockOrderDetailId);

        /// <summary>
        /// 物料需求Id
        /// </summary>
        public double StockOrderDetailId
        {
            get { return this.GetProperty(StockOrderDetailIdProperty); }
            set { this.SetProperty(StockOrderDetailIdProperty, value); }
        }
        #endregion

        #region 工厂Id FactoryId
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂Id")]
        public static readonly Property<double> FactoryIdProperty = P<MaterialReceptionAddViewModel>.Register(e => e.FactoryId);

        /// <summary>
        /// 工厂Id
        /// </summary>
        public double FactoryId
        {
            get { return this.GetProperty(FactoryIdProperty); }
            set { this.SetProperty(FactoryIdProperty, value); }
        }
        #endregion

        #region 备料单状态 StockOrderState
        /// <summary>
        /// 备料单状态
        /// </summary>
        [Label("备料单状态")]
        public static readonly Property<StockState> StockOrderStateProperty = P<MaterialReceptionAddViewModel>.Register(e => e.StockOrderState);

        /// <summary>
        /// 备料单状态
        /// </summary>
        public StockState StockOrderState
        {
            get { return this.GetProperty(StockOrderStateProperty); }
            set { this.SetProperty(StockOrderStateProperty, value); }
        }
        #endregion

    }
}
