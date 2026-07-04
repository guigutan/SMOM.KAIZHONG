using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.LES.LesStockCounts.ViewModels
{
    /// <summary>
    /// 差异调账
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("差异调账")]
    public class DiffAdjustViewModel : ViewModel
    {
        #region 标签号 LabelNo
        /// <summary>
        /// 标签号
        /// </summary>
        [Label("标签号")]
        public static readonly Property<string> LabelNoProperty = P<DiffAdjustViewModel>.Register(e => e.LabelNo);

        /// <summary>
        /// 标签号
        /// </summary>
        public string LabelNo
        {
            get { return this.GetProperty(LabelNoProperty); }
            set { this.SetProperty(LabelNoProperty, value); }
        }
        #endregion

        #region 物料Id ItemId
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料Id")]
        public static readonly Property<double> ItemIdProperty = P<DiffAdjustViewModel>.Register(e => e.ItemId);

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
        public static readonly Property<string> ItemCodeProperty = P<DiffAdjustViewModel>.Register(e => e.ItemCode);

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
        public static readonly Property<string> ItemNameProperty = P<DiffAdjustViewModel>.Register(e => e.ItemName);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
            set { this.SetProperty(ItemNameProperty, value); }
        }
        #endregion

        #region 物料扩展属性 ItemExtPropName
        /// <summary>
        /// 物料扩展属性
        /// </summary>
        [Label("物料扩展属性")]
        public static readonly Property<string> ItemExtPropNameProperty = P<DiffAdjustViewModel>.Register(e => e.ItemExtPropName);

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtPropName
        {
            get { return this.GetProperty(ItemExtPropNameProperty); }
            set { this.SetProperty(ItemExtPropNameProperty, value); }
        }
        #endregion

        #region 差异数量 Qty
        /// <summary>
        /// 差异数量
        /// </summary>
        [Label("差异数量")]
        public static readonly Property<decimal> QtyProperty = P<DiffAdjustViewModel>.Register(e => e.Qty);

        /// <summary>
        /// 差异数量
        /// </summary>
        public decimal Qty
        {
            get { return this.GetProperty(QtyProperty); }
            set { this.SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 工厂 Factory
        /// <summary>
        /// 工厂
        /// </summary>
        [Label("工厂")]
        public static readonly Property<string> FactoryProperty = P<DiffAdjustViewModel>.Register(e => e.Factory);

        /// <summary>
        /// 工厂
        /// </summary>
        public string Factory
        {
            get { return this.GetProperty(FactoryProperty); }
            set { this.SetProperty(FactoryProperty, value); }
        }
        #endregion

        #region 仓库 WarehouseName
        /// <summary>
        /// 仓库
        /// </summary>
        [Label("仓库")]
        public static readonly Property<string> WarehouseNameProperty = P<DiffAdjustViewModel>.Register(e => e.WarehouseName);

        /// <summary>
        /// 仓库
        /// </summary>
        public string WarehouseName
        {
            get { return this.GetProperty(WarehouseNameProperty); }
            set { this.SetProperty(WarehouseNameProperty, value); }
        }
        #endregion

        #region 仓库Id WarehouseId
        /// <summary>
        /// 仓库Id
        /// </summary>
        [Label("仓库Id")]
        public static readonly Property<double> WarehouseIdProperty = P<DiffAdjustViewModel>.Register(e => e.WarehouseId);

        /// <summary>
        /// 仓库Id
        /// </summary>
        public double WarehouseId
        {
            get { return this.GetProperty(WarehouseIdProperty); }
            set { this.SetProperty(WarehouseIdProperty, value); }
        }
        #endregion

        #region 库位 StorageLocation
        /// <summary>
        /// 库位
        /// </summary>
        [Label("库位")]
        public static readonly Property<string> StorageLocationProperty = P<DiffAdjustViewModel>.Register(e => e.StorageLocation);

        /// <summary>
        /// 库位
        /// </summary>
        public string StorageLocation
        {
            get { return this.GetProperty(StorageLocationProperty); }
            set { this.SetProperty(StorageLocationProperty, value); }
        }
        #endregion

        #region 库位Id StorageLocationId
        /// <summary>
        /// 库位Id
        /// </summary>
        [Label("库位Id")]
        public static readonly Property<double> StorageLocationIdProperty = P<DiffAdjustViewModel>.Register(e => e.StorageLocationId);

        /// <summary>
        /// 库位Id
        /// </summary>
        public double StorageLocationId
        {
            get { return this.GetProperty(StorageLocationIdProperty); }
            set { this.SetProperty(StorageLocationIdProperty, value); }
        }
        #endregion

        #region 可用数 AvaiableQty
        /// <summary>
        /// 可用数
        /// </summary>
        [Label("标签可用数量")]
        public static readonly Property<decimal> AvaiableQtyProperty = P<DiffAdjustViewModel>.Register(e => e.AvaiableQty);

        /// <summary>
        /// 可用数
        /// </summary>
        public decimal AvaiableQty
        {
            get { return this.GetProperty(AvaiableQtyProperty); }
            set { this.SetProperty(AvaiableQtyProperty, value); }
        }
        #endregion         

        #region 盘点明细Id DtlId
        /// <summary>
        /// 盘点明细Id
        /// </summary>
        [Label("盘点明细Id")]
        public static readonly Property<double> DtlIdProperty = P<DiffAdjustViewModel>.Register(e => e.DtlId);

        /// <summary>
        /// 盘点明细Id
        /// </summary>
        public double DtlId
        {
            get { return this.GetProperty(DtlIdProperty); }
            set { this.SetProperty(DtlIdProperty, value); }
        }
        #endregion

        #region 物料投入工单 AdjustWorkOrder
        /// <summary>
        /// 物料投入工单
        /// </summary>
        [Label("物料投入工单")]
        public static readonly ListProperty<EntityList<AdjustWorkOrderViewModel>> AdjustWorkOrderProperty = P<DiffAdjustViewModel>.RegisterList(e => e.AdjustWorkOrder);

        /// <summary>
        /// 物料投入工单
        /// </summary>
        public EntityList<AdjustWorkOrderViewModel> AdjustWorkOrder
        {
            get { return this.GetLazyList(AdjustWorkOrderProperty); }
        }
        #endregion
    }

    /// <summary>
    /// 物料投入工单
    /// </summary>
    [ChildEntity, Serializable]
    [Label("物料投入工单")]
    public class AdjustWorkOrderViewModel : ViewModel
    {
        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty =
            P<AdjustWorkOrderViewModel>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double WorkOrderId
        {
            get { return (double)this.GetRefId(WorkOrderIdProperty); }
            set { this.SetRefId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty =
            P<AdjustWorkOrderViewModel>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return this.GetRefEntity(WorkOrderProperty); }
            set { this.SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 数量 Qty
        /// <summary>
        /// 数量
        /// </summary>
        [Label("现有数量")]
        public static readonly Property<decimal> QtyProperty = P<AdjustWorkOrderViewModel>.Register(e => e.Qty);

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty
        {
            get { return this.GetProperty(QtyProperty); }
            set { this.SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 实际数量 ActualyQty
        /// <summary>
        /// 实际数量
        /// </summary>
        [Label("实际数量")]
        public static readonly Property<decimal> ActualyQtyProperty = P<AdjustWorkOrderViewModel>.Register(e => e.ActualyQty);

        /// <summary>
        /// 实际数量
        /// </summary>
        public decimal ActualyQty
        {
            get { return this.GetProperty(ActualyQtyProperty); }
            set { this.SetProperty(ActualyQtyProperty, value); }
        }
        #endregion

        #region 差异数量 DiffQty
        /// <summary>
        /// 差异数量
        /// </summary>
        [Label("差异数量")]
        public static readonly Property<decimal> DiffQtyProperty = P<AdjustWorkOrderViewModel>.Register(e => e.DiffQty);

        /// <summary>
        /// 差异数量
        /// </summary>
        public decimal DiffQty
        {
            get { return this.GetProperty(DiffQtyProperty); }
            set { this.SetProperty(DiffQtyProperty, value); }
        }
        #endregion

        #region 自动生成的数据 IsAuto
        /// <summary>
        /// 自动生成的数据
        /// </summary>
        [Label("自动生成的数据")]
        public static readonly Property<bool> IsAutoProperty = P<AdjustWorkOrderViewModel>.Register(e => e.IsAuto);

        /// <summary>
        /// 自动生成的数据
        /// </summary>
        public bool IsAuto
        {
            get { return this.GetProperty(IsAutoProperty); }
            set { this.SetProperty(IsAutoProperty, value); }
        }
        #endregion

        #region 盘点明细Id DtlId
        /// <summary>
        /// 盘点明细Id
        /// </summary>
        [Label("盘点明细Id")]
        public static readonly Property<double> DtlIdProperty = P<AdjustWorkOrderViewModel>.Register(e => e.DtlId);

        /// <summary>
        /// 盘点明细Id
        /// </summary>
        public double DtlId
        {
            get { return this.GetProperty(DtlIdProperty); }
            set { this.SetProperty(DtlIdProperty, value); }
        }
        #endregion

        #region 差异调账 DiffAdjust
        /// <summary>
        /// 差异调账Id
        /// </summary>
        [Label("差异调账")]
        public static readonly IRefIdProperty DiffAdjustIdProperty =
            P<AdjustWorkOrderViewModel>.RegisterRefId(e => e.DiffAdjustId, ReferenceType.Parent);

        /// <summary>
        /// 差异调账Id
        /// </summary>
        public double DiffAdjustId
        {
            get { return (double)this.GetRefId(DiffAdjustIdProperty); }
            set { this.SetRefId(DiffAdjustIdProperty, value); }
        }

        /// <summary>
        /// 差异调账
        /// </summary>
        public static readonly RefEntityProperty<DiffAdjustViewModel> DiffAdjustProperty =
            P<AdjustWorkOrderViewModel>.RegisterRef(e => e.DiffAdjust, DiffAdjustIdProperty);

        /// <summary>
        /// 差异调账
        /// </summary>
        public DiffAdjustViewModel DiffAdjust
        {
            get { return this.GetRefEntity(DiffAdjustProperty); }
            set { this.SetRefEntity(DiffAdjustProperty, value); }
        }
        #endregion

        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<AdjustWorkOrderViewModel>.Register(e => e.WorkOrderNo);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
            set { this.SetProperty(WorkOrderNoProperty, value); }
        }
        #endregion

    }
}
