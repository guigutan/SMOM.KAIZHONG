using SIE.Domain;
using SIE.EMS.Purchases.PurchaseOrders;
using SIE.EMS.SpareParts.Enums;
using SIE.Equipments.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Warehouses;
using System;

namespace SIE.EMS.Purchases.SparePartAcceptances
{
    /// <summary>
    /// 备件验收明细
    /// </summary>
    [ChildEntity, Serializable]
    [Label("备件验收明细")]
    public partial class SparePartAcceptanceDetail : DataEntity
    {
        #region 备件验收 SparePartAcceptance
        /// <summary>
        /// 备件验收Id
        /// </summary>
        public static readonly IRefIdProperty SparePartAcceptanceIdProperty = P<SparePartAcceptanceDetail>.RegisterRefId(e => e.SparePartAcceptanceId, ReferenceType.Parent);

        /// <summary>
        /// 备件验收Id
        /// </summary>
        public double SparePartAcceptanceId
        {
            get { return (double)GetRefId(SparePartAcceptanceIdProperty); }
            set { SetRefId(SparePartAcceptanceIdProperty, value); }
        }

        /// <summary>
        /// 备件验收
        /// </summary>
        public static readonly RefEntityProperty<SparePartAcceptance> SparePartAcceptanceProperty = P<SparePartAcceptanceDetail>.RegisterRef(e => e.SparePartAcceptance, SparePartAcceptanceIdProperty);

        /// <summary>
        /// 备件验收
        /// </summary>
        public SparePartAcceptance SparePartAcceptance
        {
            get { return GetRefEntity(SparePartAcceptanceProperty); }
            set { SetRefEntity(SparePartAcceptanceProperty, value); }
        }
        #endregion

        #region 单价 Price
        /// <summary>
        /// 单价
        /// </summary>
        [Label("单价")]
        [MinValue(0)]
        public static readonly Property<decimal> PriceProperty = P<SparePartAcceptanceDetail>.Register(e => e.Price);

        /// <summary>
        /// 单价
        /// </summary>
        public decimal Price
        {
            get { return GetProperty(PriceProperty); }
            set { SetProperty(PriceProperty, value); }
        }
        #endregion

        #region 接收数量 ReceiveQty
        /// <summary>
        /// 接收数量
        /// </summary>
        [Label("接收数量")]
        [MinValue(0)]
        public static readonly Property<int> ReceiveQtyProperty = P<SparePartAcceptanceDetail>.Register(e => e.ReceiveQty);

        /// <summary>
        /// 接收数量
        /// </summary>
        public int ReceiveQty
        {
            get { return GetProperty(ReceiveQtyProperty); }
            set { SetProperty(ReceiveQtyProperty, value); }
        }
        #endregion

        #region 合格数量 PassQty
        /// <summary>
        /// 合格数量
        /// </summary>
        [Label("合格数量")]
        [MinValue(0)]
        public static readonly Property<int> PassQtyProperty = P<SparePartAcceptanceDetail>.Register(e => e.PassQty);

        /// <summary>
        /// 合格数量
        /// </summary>
        public int PassQty
        {
            get { return GetProperty(PassQtyProperty); }
            set { SetProperty(PassQtyProperty, value); }
        }
        #endregion

        #region 不合格数量 UnqualifiedQty
        /// <summary>
        /// 不合格数量
        /// </summary>
        [Label("不合格数量")]
        [MinValue(0)]
        public static readonly Property<int> UnqualifiedQtyProperty = P<SparePartAcceptanceDetail>.Register(e => e.UnqualifiedQty);

        /// <summary>
        /// 不合格数量
        /// </summary>
        public int UnqualifiedQty
        {
            get { return GetProperty(UnqualifiedQtyProperty); }
            set { SetProperty(UnqualifiedQtyProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(1000)]
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<SparePartAcceptanceDetail>.Register(e => e.Remark);

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
        [Label("采购订单")]
        public static readonly IRefIdProperty PurchaseOrderIdProperty = P<SparePartAcceptanceDetail>.RegisterRefId(e => e.PurchaseOrderId, ReferenceType.Normal);

        /// <summary>
        /// 采购订单Id
        /// </summary>
        public double? PurchaseOrderId
        {
            get { return (double?)GetRefNullableId(PurchaseOrderIdProperty); }
            set { SetRefNullableId(PurchaseOrderIdProperty, value); }
        }

        /// <summary>
        /// 采购订单
        /// </summary>
        public static readonly RefEntityProperty<PurchaseOrder> PurchaseOrderProperty = P<SparePartAcceptanceDetail>.RegisterRef(e => e.PurchaseOrder, PurchaseOrderIdProperty);

        /// <summary>
        /// 采购订单
        /// </summary>
        public PurchaseOrder PurchaseOrder
        {
            get { return GetRefEntity(PurchaseOrderProperty); }
            set { SetRefEntity(PurchaseOrderProperty, value); }
        }
        #endregion

        #region 采购订单明细 PurchaseOrderItem
        /// <summary>
        /// 采购订单明细Id
        /// </summary>
        [Label("采购订单明细")]
        public static readonly IRefIdProperty PurchaseOrderItemIdProperty = P<SparePartAcceptanceDetail>.RegisterRefId(e => e.PurchaseOrderItemId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<PurchaseOrderItem> PurchaseOrderItemProperty = P<SparePartAcceptanceDetail>.RegisterRef(e => e.PurchaseOrderItem, PurchaseOrderItemIdProperty);

        /// <summary>
        /// 采购订单明细
        /// </summary>
        public PurchaseOrderItem PurchaseOrderItem
        {
            get { return GetRefEntity(PurchaseOrderItemProperty); }
            set { SetRefEntity(PurchaseOrderItemProperty, value); }
        }
        #endregion

        #region 仓库 Warehouse
        /// <summary>
        /// 仓库Id
        /// </summary>
        [Label("仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty = P<SparePartAcceptanceDetail>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 仓库Id
        /// </summary>
        public double? WarehouseId
        {
            get { return (double?)GetRefNullableId(WarehouseIdProperty); }
            set { SetRefNullableId(WarehouseIdProperty, value); }
        }

        /// <summary>
        /// 仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<SparePartAcceptanceDetail>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return GetRefEntity(WarehouseProperty); }
            set { SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 批次明细列表 LotList
        /// <summary>
        /// 批次明细列表
        /// </summary>
        public static readonly ListProperty<EntityList<SparePartAcceptanceLot>> LotListProperty = P<SparePartAcceptanceDetail>.RegisterList(e => e.LotList);
        /// <summary>
        /// 批次明细列表
        /// </summary>
        public EntityList<SparePartAcceptanceLot> LotList
        {
            get { return this.GetLazyList(LotListProperty); }
        }
        #endregion

        #region 序列号明细列表 SnList
        /// <summary>
        /// 序列号明细列表
        /// </summary>
        public static readonly ListProperty<EntityList<SparePartAcceptanceSn>> SnListProperty = P<SparePartAcceptanceDetail>.RegisterList(e => e.SnList);
        /// <summary>
        /// 序列号明细列表
        /// </summary>
        public EntityList<SparePartAcceptanceSn> SnList
        {
            get { return this.GetLazyList(SnListProperty); }
        }
        #endregion

        #region 视图属性
        #region 采购对象 PurchaseObjectType
        /// <summary>
        /// 采购对象
        /// </summary>
        [Label("采购对象")]
        public static readonly Property<PurchaseObjectType?> PurchaseObjectTypeProperty = P<SparePartAcceptanceDetail>.RegisterView(e => e.PurchaseObjectType, p => p.PurchaseOrder.PurchaseObjectType);

        /// <summary>
        /// 采购对象
        /// </summary>
        public PurchaseObjectType? PurchaseObjectType
        {
            get { return this.GetProperty(PurchaseObjectTypeProperty); }
        }
        #endregion

        #region 单位 UnitName
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> UnitNameProperty = P<SparePartAcceptanceDetail>.RegisterView(e => e.UnitName, p => p.SparePartAcceptance.SparePart.Unit.Name);

        /// <summary>
        /// 单位
        /// </summary>
        public string UnitName
        {
            get { return this.GetProperty(UnitNameProperty); }
        }
        #endregion

        #region 审核状态 ApprovalStatus
        /// <summary>
        /// 审核状态
        /// </summary>
        [Label("审核状态")]
        public static readonly Property<ApprovalStatus> ApprovalStatusProperty = P<SparePartAcceptanceDetail>.RegisterView(e => e.ApprovalStatus, p => p.SparePartAcceptance.ApprovalStatus);

        /// <summary>
        /// 审核状态
        /// </summary>
        public ApprovalStatus ApprovalStatus
        {
            get { return this.GetProperty(ApprovalStatusProperty); }
        }
        #endregion

        #region 管控方式 ControlMethod
        /// <summary>
        /// 管控方式
        /// </summary>
        [Label("管控方式")]
        public static readonly Property<ControlMethod> ControlMethodProperty = P<SparePartAcceptanceDetail>.RegisterView(e => e.ControlMethod, p => p.SparePartAcceptance.SparePart.ControlMethod);

        /// <summary>
        /// 管控方式
        /// </summary>
        public ControlMethod ControlMethod
        {
            get { return this.GetProperty(ControlMethodProperty); }
        }
        #endregion

        #region 仓库名称 WarehouseName
        /// <summary>
        /// 仓库名称
        /// </summary>
        [Label("仓库名称")]
        public static readonly Property<string> WarehouseNameProperty = P<SparePartAcceptanceDetail>.RegisterView(e => e.WarehouseName, p => p.Warehouse.Name);

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WarehouseName
        {
            get { return this.GetProperty(WarehouseNameProperty); }
        }
        #endregion

        #region 采购单号 PurchaseOrderNo
        /// <summary>
        /// 采购单号
        /// </summary>
        [Label("采购单号")]
        public static readonly Property<string> PurchaseOrderNoProperty = P<SparePartAcceptanceDetail>.RegisterView(e => e.PurchaseOrderNo, p => p.PurchaseOrder.OrderNo);

        /// <summary>
        /// 采购单号
        /// </summary>
        public string PurchaseOrderNo
        {
            get { return this.GetProperty(PurchaseOrderNoProperty); }
        }
        #endregion

        #region 采购单行号 PurchaseOrderLine
        /// <summary>
        /// 采购单行号
        /// </summary>
        [Label("采购单行号")]
        public static readonly Property<string> PurchaseOrderLineProperty = P<SparePartAcceptanceDetail>.RegisterView(e => e.PurchaseOrderLine, p => p.PurchaseOrderItem.LineNo);

        /// <summary>
        /// 采购单行号
        /// </summary>
        public string PurchaseOrderLine
        {
            get { return this.GetProperty(PurchaseOrderLineProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 备件验收明细 实体配置
    /// </summary>
    internal class SparePartAcceptanceDetailConfig : EntityConfig<SparePartAcceptanceDetail>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_SP_ACPT_DTL").MapAllProperties();
            Meta.Property(SparePartAcceptanceDetail.RemarkProperty).ColumnMeta.HasLength(2000);
            Meta.EnablePhantoms();
        }
    }
}