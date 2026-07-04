using SIE.Domain;
using SIE.EMS.Purchases.EquipmentAcceptances;
using SIE.EMS.Purchases.PurchaseOrders;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipAccounts;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Warehouses;
using System;

namespace SIE.EMS.Purchases.EquipmentInbounds
{
    /// <summary>
    /// 设备入库明细
    /// </summary>
    [ChildEntity, Serializable]
    [Label("设备入库明细")]
    public partial class EquipmentInboundDetail : DataEntity
    {
        #region 设备入库 EquipmentInbound
        /// <summary>
        /// 设备入库Id
        /// </summary>
        public static readonly IRefIdProperty EquipmentInboundIdProperty = P<EquipmentInboundDetail>.RegisterRefId(e => e.EquipmentInboundId, ReferenceType.Parent);

        /// <summary>
        /// 设备入库Id
        /// </summary>
        public double EquipmentInboundId
        {
            get { return (double)GetRefId(EquipmentInboundIdProperty); }
            set { SetRefId(EquipmentInboundIdProperty, value); }
        }

        /// <summary>
        /// 设备入库
        /// </summary>
        public static readonly RefEntityProperty<EquipmentInbound> EquipmentInboundProperty = P<EquipmentInboundDetail>.RegisterRef(e => e.EquipmentInbound, EquipmentInboundIdProperty);

        /// <summary>
        /// 设备入库
        /// </summary>
        public EquipmentInbound EquipmentInbound
        {
            get { return GetRefEntity(EquipmentInboundProperty); }
            set { SetRefEntity(EquipmentInboundProperty, value); }
        }
        #endregion

        #region 赠品 Giveaway
        /// <summary>
        /// 赠品
        /// </summary>
        [Label("赠品")]
        public static readonly Property<bool> GiveawayProperty = P<EquipmentInboundDetail>.Register(e => e.Giveaway);

        /// <summary>
        /// 赠品
        /// </summary>
        public bool Giveaway
        {
            get { return GetProperty(GiveawayProperty); }
            set { SetProperty(GiveawayProperty, value); }
        }
        #endregion

        #region 设备编码 EquipmentCode
        /// <summary>
        /// 设备编码
        /// </summary>
        [Label("设备编码")]
        public static readonly Property<string> EquipmentCodeProperty = P<EquipmentInboundDetail>.Register(e => e.EquipmentCode);

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipmentCode
        {
            get { return GetProperty(EquipmentCodeProperty); }
            set { SetProperty(EquipmentCodeProperty, value); }
        }
        #endregion

        #region 设备台账 EquipAccount
        /// <summary>
        /// 设备台账Id
        /// </summary>
        [Label("设备台账")]
        public static readonly IRefIdProperty EquipAccountIdProperty =
            P<EquipmentInboundDetail>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Normal);

        /// <summary>
        /// 设备台账Id
        /// </summary>
        public double? EquipAccountId
        {
            get { return (double?)this.GetRefNullableId(EquipAccountIdProperty); }
            set { this.SetRefNullableId(EquipAccountIdProperty, value); }
        }

        /// <summary>
        /// 设备台账
        /// </summary>
        public static readonly RefEntityProperty<EquipAccountSelect> EquipAccountProperty =
            P<EquipmentInboundDetail>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

        /// <summary>
        /// 设备台账
        /// </summary>
        public EquipAccountSelect EquipAccount
        {
            get { return this.GetRefEntity(EquipAccountProperty); }
            set { this.SetRefEntity(EquipAccountProperty, value); }
        }
        #endregion

        #region 单价 Price
        /// <summary>
        /// 单价
        /// </summary>
        [Label("单价")]
        public static readonly Property<decimal> PriceProperty = P<EquipmentInboundDetail>.Register(e => e.Price);

        /// <summary>
        /// 单价
        /// </summary>
        public decimal Price
        {
            get { return GetProperty(PriceProperty); }
            set { SetProperty(PriceProperty, value); }
        }
        #endregion

        #region 入库库位 StorageLocation
        /// <summary>
        /// 入库库位Id
        /// </summary>
        [Label("入库库位")]
        public static readonly IRefIdProperty StorageLocationIdProperty = P<EquipmentInboundDetail>.RegisterRefId(e => e.StorageLocationId, ReferenceType.Normal);

        /// <summary>
        /// 入库库位Id
        /// </summary>
        public double? StorageLocationId
        {
            get { return (double?)GetRefNullableId(StorageLocationIdProperty); }
            set { SetRefNullableId(StorageLocationIdProperty, value); }
        }

        /// <summary>
        /// 入库库位
        /// </summary>
        public static readonly RefEntityProperty<StorageLocation> StorageLocationProperty = P<EquipmentInboundDetail>.RegisterRef(e => e.StorageLocation, StorageLocationIdProperty);

        /// <summary>
        /// 入库库位
        /// </summary>
        public StorageLocation StorageLocation
        {
            get { return GetRefEntity(StorageLocationProperty); }
            set { SetRefEntity(StorageLocationProperty, value); }
        }
        #endregion

        #region 采购订单明细 PurchaseOrderItem
        /// <summary>
        /// 采购订单明细Id
        /// </summary>
        public static readonly IRefIdProperty PurchaseOrderItemIdProperty = P<EquipmentInboundDetail>.RegisterRefId(e => e.PurchaseOrderItemId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<PurchaseOrderItem> PurchaseOrderItemProperty = P<EquipmentInboundDetail>.RegisterRef(e => e.PurchaseOrderItem, PurchaseOrderItemIdProperty);

        /// <summary>
        /// 采购订单明细
        /// </summary>
        public PurchaseOrderItem PurchaseOrderItem
        {
            get { return GetRefEntity(PurchaseOrderItemProperty); }
            set { SetRefEntity(PurchaseOrderItemProperty, value); }
        }
        #endregion

        #region 采购订单 PurchaseOrder
        /// <summary>
        /// 采购订单Id
        /// </summary>
        public static readonly IRefIdProperty PurchaseOrderIdProperty = P<EquipmentInboundDetail>.RegisterRefId(e => e.PurchaseOrderId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<PurchaseOrder> PurchaseOrderProperty = P<EquipmentInboundDetail>.RegisterRef(e => e.PurchaseOrder, PurchaseOrderIdProperty);

        /// <summary>
        /// 采购订单
        /// </summary>
        public PurchaseOrder PurchaseOrder
        {
            get { return GetRefEntity(PurchaseOrderProperty); }
            set { SetRefEntity(PurchaseOrderProperty, value); }
        }
        #endregion

        #region 设备开箱验收明细 EquipmentAcceptanceDetail
        /// <summary>
        /// 设备开箱验收明细Id
        /// </summary>
        public static readonly IRefIdProperty EquipmentAcceptanceDetailIdProperty = P<EquipmentInboundDetail>.RegisterRefId(e => e.EquipmentAcceptanceDetailId, ReferenceType.Normal);

        /// <summary>
        /// 设备开箱验收明细Id
        /// </summary>
        public double EquipmentAcceptanceDetailId
        {
            get { return (double)GetRefId(EquipmentAcceptanceDetailIdProperty); }
            set { SetRefId(EquipmentAcceptanceDetailIdProperty, value); }
        }

        /// <summary>
        /// 设备开箱验收明细
        /// </summary>
        public static readonly RefEntityProperty<EquipmentAcceptanceDetail> EquipmentAcceptanceDetailProperty = P<EquipmentInboundDetail>.RegisterRef(e => e.EquipmentAcceptanceDetail, EquipmentAcceptanceDetailIdProperty);

        /// <summary>
        /// 设备开箱验收明细
        /// </summary>
        public EquipmentAcceptanceDetail EquipmentAcceptanceDetail
        {
            get { return GetRefEntity(EquipmentAcceptanceDetailProperty); }
            set { SetRefEntity(EquipmentAcceptanceDetailProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 设备名称 EquipAccountName
        /// <summary>
        /// 设备名称
        /// </summary>
        [Label("设备名称")]
        public static readonly Property<string> EquipAccountNameProperty = P<EquipmentInboundDetail>.RegisterView(e => e.EquipAccountName, p => p.EquipAccount.Name);

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipAccountName
        {
            get { return this.GetProperty(EquipAccountNameProperty); }
            set { SetProperty(EquipAccountNameProperty, value); }
        }
        #endregion

        #region ABC分类 UseLevel
        /// <summary>
        /// ABC分类
        /// </summary>
        [Label("ABC分类")]
        public static readonly Property<string> UseLevelProperty = P<EquipmentInboundDetail>.RegisterView(e => e.UseLevel, p => p.EquipAccount.UseLevel);

        /// <summary>
        /// ABC分类
        /// </summary>
        public string UseLevel
        {
            get { return this.GetProperty(UseLevelProperty); }
        }
        #endregion

        #region 使用部门 UseDepartmentName
        /// <summary>
        /// 使用部门
        /// </summary>
        [Label("使用部门")]
        public static readonly Property<string> UseDepartmentNameProperty = P<EquipmentInboundDetail>.RegisterView(e => e.UseDepartmentName, p => p.EquipAccount.UseDepartment.Name);

        /// <summary>
        /// 使用部门
        /// </summary>
        public string UseDepartmentName
        {
            get { return this.GetProperty(UseDepartmentNameProperty); }
        }
        #endregion

        #region 位置 InstallationLocation
        /// <summary>
        /// 位置
        /// </summary>
        [Label("位置")]
        public static readonly Property<string> InstallationLocationProperty = P<EquipmentInboundDetail>.RegisterView(e => e.InstallationLocation, p => p.EquipAccount.InstallationLocation);

        /// <summary>
        /// 位置
        /// </summary>
        public string InstallationLocation
        {
            get { return this.GetProperty(InstallationLocationProperty); }
        }
        #endregion

        #region 原厂序列号 OriginalSerialNumber
        /// <summary>
        /// 原厂序列号
        /// </summary>
        [Label("原厂序列号")]
        public static readonly Property<string> OriginalSerialNumberProperty = P<EquipmentInboundDetail>.RegisterView(e => e.OriginalSerialNumber, p => p.EquipAccount.OriginalSerialNumber);

        /// <summary>
        /// 原厂序列号
        /// </summary>
        public string OriginalSerialNumber
        {
            get { return this.GetProperty(OriginalSerialNumberProperty); }
        }
        #endregion

        #region 生产日期 EnterDate
        /// <summary>
        /// 生产日期
        /// </summary>
        [Label("生产日期")]
        public static readonly Property<DateTime?> EnterDateProperty = P<EquipmentInboundDetail>.RegisterView(e => e.EnterDate, p => p.EquipAccount.EnterDate);

        /// <summary>
        /// 生产日期
        /// </summary>
        public DateTime? EnterDate
        {
            get { return this.GetProperty(EnterDateProperty); }
        }
        #endregion

        #region 生产厂家 Manufacturer
        /// <summary>
        /// 生产厂家
        /// </summary>
        [Label("生产厂家")]
        public static readonly Property<string> ManufacturerProperty = P<EquipmentInboundDetail>.RegisterView(e => e.Manufacturer, p => p.EquipAccount.Manufacturer);

        /// <summary>
        /// 生产厂家
        /// </summary>
        public string Manufacturer
        {
            get { return this.GetProperty(ManufacturerProperty); }
        }
        #endregion

        #region 入库状态 InboundStatus
        /// <summary>
        /// 入库状态
        /// </summary>
        [Label("入库状态")]
        public static readonly Property<InboundStatus> InboundStatusProperty = P<EquipmentInboundDetail>.RegisterView(e => e.InboundStatus, p => p.EquipmentInbound.InboundStatus);

        /// <summary>
        /// 入库状态
        /// </summary>
        public InboundStatus InboundStatus
        {
            get { return this.GetProperty(InboundStatusProperty); }
        }
        #endregion

        #region 入库仓库 WarehouseId
        /// <summary>
        /// 入库仓库
        /// </summary>
        [Label("入库仓库")]
        public static readonly Property<double?> WarehouseIdProperty = P<EquipmentInboundDetail>.RegisterView(e => e.WarehouseId, p => p.EquipmentInbound.WarehouseId);

        /// <summary>
        /// 入库仓库
        /// </summary>
        public double? WarehouseId
        {
            get { return this.GetProperty(WarehouseIdProperty); }
        }
        #endregion

        #region 接收车间 WorkshopId
        /// <summary>
        /// 接收车间
        /// </summary>
        [Label("接收车间")]
        public static readonly Property<double?> WorkshopIdProperty = P<EquipmentInboundDetail>.RegisterView(e => e.WorkshopId, p => p.EquipmentInbound.WorkshopId);

        /// <summary>
        /// 接收车间
        /// </summary>
        public double? WorkshopId
        {
            get { return this.GetProperty(WorkshopIdProperty); }
        }
        #endregion


        #endregion
    }

    /// <summary>
    /// 设备入库明细 实体配置
    /// </summary>
    internal class EquipmentInboundDetailConfig : EntityConfig<EquipmentInboundDetail>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_EQP_IN_DTL").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}