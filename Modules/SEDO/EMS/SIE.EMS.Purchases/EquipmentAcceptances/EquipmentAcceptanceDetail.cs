using SIE.Common;
using SIE.Domain;
using SIE.EMS.Purchases.PurchaseOrders;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Warehouses;
using System;
using SIE.Resources.Enterprises;

namespace SIE.EMS.Purchases.EquipmentAcceptances
{
    /// <summary>
    /// 设备开箱验收明细
    /// </summary>
    [ChildEntity, Serializable]
    [Label("设备开箱验收明细")]
    public partial class EquipmentAcceptanceDetail : DataEntity
    {
        #region 设备编码 EquipmentCode
        /// <summary>
        /// 设备编码
        /// </summary>
        [Label("设备编码")]
        [Required]
        public static readonly Property<string> EquipmentCodeProperty = P<EquipmentAcceptanceDetail>.Register(e => e.EquipmentCode);

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipmentCode
        {
            get { return GetProperty(EquipmentCodeProperty); }
            set { SetProperty(EquipmentCodeProperty, value); }
        }
        #endregion

        #region 赠品 Giveaway
        /// <summary>
        /// 赠品
        /// </summary>
        [Label("赠品")]
        public static readonly Property<bool> GiveawayProperty = P<EquipmentAcceptanceDetail>.Register(e => e.Giveaway);

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
        public static readonly Property<decimal> PriceProperty = P<EquipmentAcceptanceDetail>.Register(e => e.Price);

        /// <summary>
        /// 单价
        /// </summary>
        public decimal Price
        {
            get { return GetProperty(PriceProperty); }
            set { SetProperty(PriceProperty, value); }
        }
        #endregion

        #region 原厂序列号 OriginalSn
        /// <summary>
        /// 原厂序列号
        /// </summary>
        [Label("原厂序列号")]
        public static readonly Property<string> OriginalSnProperty = P<EquipmentAcceptanceDetail>.Register(e => e.OriginalSn);

        /// <summary>
        /// 原厂序列号
        /// </summary>
        public string OriginalSn
        {
            get { return GetProperty(OriginalSnProperty); }
            set { SetProperty(OriginalSnProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(1000)]
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<EquipmentAcceptanceDetail>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 采购订单明细 PurchaseOrderItem
        /// <summary>
        /// 采购订单明细Id
        /// </summary>
        public static readonly IRefIdProperty PurchaseOrderItemIdProperty = P<EquipmentAcceptanceDetail>.RegisterRefId(e => e.PurchaseOrderItemId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<PurchaseOrderItem> PurchaseOrderItemProperty = P<EquipmentAcceptanceDetail>.RegisterRef(e => e.PurchaseOrderItem, PurchaseOrderItemIdProperty);

        /// <summary>
        /// 采购订单明细
        /// </summary>
        public PurchaseOrderItem PurchaseOrderItem
        {
            get { return GetRefEntity(PurchaseOrderItemProperty); }
            set { SetRefEntity(PurchaseOrderItemProperty, value); }
        }
        #endregion

        #region 验收状态 AcceptanceStatus
        /// <summary>
        /// 验收状态
        /// </summary>
        [Label("验收状态")]
        public static readonly Property<InspectionResult?> AcceptanceStatusProperty = P<EquipmentAcceptanceDetail>.Register(e => e.AcceptanceStatus);

        /// <summary>
        /// 验收状态
        /// </summary>
        public InspectionResult? AcceptanceStatus
        {
            get { return GetProperty(AcceptanceStatusProperty); }
            set { SetProperty(AcceptanceStatusProperty, value); }
        }
        #endregion

        #region 接收仓库 Warehouse
        /// <summary>
        /// 接收仓库Id
        /// </summary>
        [Label("接收仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty = P<EquipmentAcceptanceDetail>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<EquipmentAcceptanceDetail>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 接收仓库
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
            P<EquipmentAcceptanceDetail>.RegisterRefId(e => e.WorkshopId, ReferenceType.Normal);

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
            P<EquipmentAcceptanceDetail>.RegisterRef(e => e.Workshop, WorkshopIdProperty);

        /// <summary>
        /// 接收车间
        /// </summary>
        public Enterprise Workshop
        {
            get { return this.GetRefEntity(WorkshopProperty); }
            set { this.SetRefEntity(WorkshopProperty, value); }
        }
        #endregion


        #region 采购订单 PurchaseOrder
        /// <summary>
        /// 采购订单Id
        /// </summary>
        public static readonly IRefIdProperty PurchaseOrderIdProperty = P<EquipmentAcceptanceDetail>.RegisterRefId(e => e.PurchaseOrderId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<PurchaseOrder> PurchaseOrderProperty = P<EquipmentAcceptanceDetail>.RegisterRef(e => e.PurchaseOrder, PurchaseOrderIdProperty);

        /// <summary>
        /// 采购订单
        /// </summary>
        public PurchaseOrder PurchaseOrder
        {
            get { return GetRefEntity(PurchaseOrderProperty); }
            set { SetRefEntity(PurchaseOrderProperty, value); }
        }
        #endregion

        #region 设备开箱验收 EquipmentAcceptance
        /// <summary>
        /// 设备开箱验收Id
        /// </summary>
        public static readonly IRefIdProperty EquipmentAcceptanceIdProperty = P<EquipmentAcceptanceDetail>.RegisterRefId(e => e.EquipmentAcceptanceId, ReferenceType.Parent);

        /// <summary>
        /// 设备开箱验收Id
        /// </summary>
        public double EquipmentAcceptanceId
        {
            get { return (double)GetRefId(EquipmentAcceptanceIdProperty); }
            set { SetRefId(EquipmentAcceptanceIdProperty, value); }
        }

        /// <summary>
        /// 设备开箱验收
        /// </summary>
        public static readonly RefEntityProperty<EquipmentAcceptance> EquipmentAcceptanceProperty = P<EquipmentAcceptanceDetail>.RegisterRef(e => e.EquipmentAcceptance, EquipmentAcceptanceIdProperty);

        /// <summary>
        /// 设备开箱验收
        /// </summary>
        public EquipmentAcceptance EquipmentAcceptance
        {
            get { return GetRefEntity(EquipmentAcceptanceProperty); }
            set { SetRefEntity(EquipmentAcceptanceProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 设备开箱验收明细 实体配置
    /// </summary>
    internal class EquipmentAcceptanceDetailConfig : EntityConfig<EquipmentAcceptanceDetail>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_EQP_ACPT_DTL").MapAllProperties();
            Meta.Property(EquipmentAcceptanceDetail.RemarkProperty).ColumnMeta.HasLength(2000);
            Meta.EnablePhantoms();
        }
    }
}