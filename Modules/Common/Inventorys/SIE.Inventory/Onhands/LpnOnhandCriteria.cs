using SIE.Core.Enums;
using SIE.Domain;
using SIE.Core.Enums;
using SIE.Items;
using SIE.ObjectModel;
using SIE.Warehouses;
using System;

namespace SIE.Inventory.Onhands
{
    /// <summary>
    /// 批次和LPN库存
    /// </summary>
    [QueryEntity, Serializable]
    [Label("批次和LPN库存查询")]
    public partial class LpnOnhandCriteria : Criteria
    {
        #region 仓库 Warehouse
        /// <summary>
        /// 仓库Id
        /// </summary>
        [Label("仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty = P<LpnOnhandCriteria>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<LpnOnhandCriteria>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return GetRefEntity(WarehouseProperty); }
            set { SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 库区 StorageArea
        /// <summary>
        /// 库区
        /// </summary>
        [Label("库区")]
        public static readonly IRefIdProperty StorageAreaIdProperty = P<LpnOnhandCriteria>.RegisterRefId(e => e.StorageAreaId, ReferenceType.Normal);

        /// <summary>
        /// 库区
        /// </summary>
        public double? StorageAreaId
        {
            get { return (double?)GetRefNullableId(StorageAreaIdProperty); }
            set { SetRefNullableId(StorageAreaIdProperty, value); }
        }

        /// <summary>
        /// 库区
        /// </summary>
        public static readonly RefEntityProperty<StorageArea> StorageAreaProperty = P<LpnOnhandCriteria>.RegisterRef(e => e.StorageArea, StorageAreaIdProperty);

        /// <summary>
        /// 库区
        /// </summary>
        public StorageArea StorageArea
        {
            get { return this.GetRefEntity(StorageAreaProperty); }
            set { this.SetRefEntity(StorageAreaProperty, value); }
        }
        #endregion

        #region 库位 StorageLocation
        /// <summary>
        /// 库位Id
        /// </summary>
        [Label("库位")]
        public static readonly IRefIdProperty StorageLocationIdProperty = P<LpnOnhandCriteria>.RegisterRefId(e => e.StorageLocationId, ReferenceType.Normal);

        /// <summary>
        /// 库位Id
        /// </summary>
        public double? StorageLocationId
        {
            get { return (double?)GetRefNullableId(StorageLocationIdProperty); }
            set { SetRefNullableId(StorageLocationIdProperty, value); }
        }

        /// <summary>
        /// 库位
        /// </summary>
        public static readonly RefEntityProperty<StorageLocation> StorageLocationProperty = P<LpnOnhandCriteria>.RegisterRef(e => e.StorageLocation, StorageLocationIdProperty);

        /// <summary>
        /// 库位
        /// </summary>
        public StorageLocation StorageLocation
        {
            get { return GetRefEntity(StorageLocationProperty); }
            set { SetRefEntity(StorageLocationProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty = P<LpnOnhandCriteria>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料Id
        /// </summary>
        public double? ItemId
        {
            get { return (double?)GetRefNullableId(ItemIdProperty); }
            set { SetRefNullableId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty = P<LpnOnhandCriteria>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 批次 LotCode
        /// <summary>
        /// 批次
        /// </summary>
        [Label("批次")]
        public static readonly Property<string> LotCodeProperty = P<LpnOnhandCriteria>.Register(e => e.LotCode);

        /// <summary>
        /// 批次
        /// </summary>
        public string LotCode
        {
            get { return GetProperty(LotCodeProperty); }
            set { SetProperty(LotCodeProperty, value); }
        }
        #endregion

        #region 货主编码 StorerCode
        /// <summary>
        /// 货主编码
        /// </summary>
        [Label("货主编码")]
        public static readonly Property<string> StorerCodeProperty = P<LpnOnhandCriteria>.Register(e => e.StorerCode);

        /// <summary>
        /// 货主编码
        /// </summary>
        public string StorerCode
        {
            get { return GetProperty(StorerCodeProperty); }
            set { SetProperty(StorerCodeProperty, value); }
        }
        #endregion

        #region LPN Lpn
        /// <summary>
        /// LPN
        /// </summary>
        [Label("LPN")]
        public static readonly Property<string> LpnProperty = P<LpnOnhandCriteria>.Register(e => e.Lpn);

        /// <summary>
        /// LPN
        /// </summary>
        public string Lpn
        {
            get { return GetProperty(LpnProperty); }
            set { SetProperty(LpnProperty, value); }
        }
        #endregion

        #region 项目号 ProjectNo
        /// <summary>
        /// 项目号
        /// </summary>
        [Label("项目号")]
        public static readonly Property<string> ProjectNoProperty = P<LpnOnhandCriteria>.Register(e => e.ProjectNo);

        /// <summary>
        /// 项目号
        /// </summary>
        public string ProjectNo
        {
            get { return GetProperty(ProjectNoProperty); }
            set { SetProperty(ProjectNoProperty, value); }
        }
        #endregion

        #region 任务号 TaskNo
        /// <summary>
        /// 任务号
        /// </summary>
        [Label("任务号")]
        public static readonly Property<string> TaskNoProperty = P<LpnOnhandCriteria>.Register(e => e.TaskNo);

        /// <summary>
        /// 任务号
        /// </summary>
        public string TaskNo
        {
            get { return GetProperty(TaskNoProperty); }
            set { SetProperty(TaskNoProperty, value); }
        }
        #endregion

        #region 零库存
        /// <summary>
        /// 零库存
        /// </summary>
        [Label("零库存")]
        public static readonly Property<bool> IsZeroProperty = P<LpnOnhandCriteria>.Register(e => e.IsZero);

        /// <summary>
        /// 零库存
        /// </summary>
        public bool IsZero
        {
            get { return GetProperty(IsZeroProperty); }
            set { SetProperty(IsZeroProperty, value); }
        }
        #endregion

        #region 库存状态 State
        /// <summary>
        /// 库存状态
        /// </summary>
        [Label("库存状态")]
        public static readonly Property<OnhandState?> StateProperty = P<LpnOnhandCriteria>.Register(e => e.State);

        /// <summary>
        /// 库存状态
        /// </summary>
        public OnhandState? State
        {
            get { return this.GetProperty(StateProperty); }
            set { this.SetProperty(StateProperty, value); }
        }
        #endregion

        #region 单据大类 OrderType
        /// <summary>
        /// 单据大类
        /// </summary>
        [Label("单据大类")]
        public static readonly Property<OrderType?> OrderTypeProperty = P<LpnOnhandCriteria>.Register(e => e.OrderType);

        /// <summary>
        /// 单据大类
        /// </summary>
        public OrderType? OrderType
        {
            get { return this.GetProperty(OrderTypeProperty); }
            set { this.SetProperty(OrderTypeProperty, value); }
        }
        #endregion

        #region 状态不是未质检 IsNotNoneState
        /// <summary>
        /// 状态不是未质检
        /// </summary>
        [Label("状态不是未质检")]
        public static readonly Property<bool?> IsNotNoneStateProperty = P<LpnOnhandCriteria>.Register(e => e.IsNotNoneState);

        /// <summary>
        /// 状态不是未质检
        /// </summary>
        public bool? IsNotNoneState
        {
            get { return this.GetProperty(IsNotNoneStateProperty); }
            set { this.SetProperty(IsNotNoneStateProperty, value); }
        }
        #endregion

        #region 库位是否冻结 IsFrozen
        /// <summary>
        /// 库位是否冻结
        /// </summary>
        [Label("库位是否冻结")]
        public static readonly Property<bool?> IsFrozenProperty = P<LpnOnhandCriteria>.Register(e => e.IsFrozen);

        /// <summary>
        /// 库位是否冻结
        /// </summary>
        public bool? IsFrozen
        {
            get { return this.GetProperty(IsFrozenProperty); }
            set { this.SetProperty(IsFrozenProperty, value); }
        }
        #endregion

        #region 是否存在现有量 IsExistsAvailableQty
        /// <summary>
        /// 是否存在现有量
        /// </summary>
        [Label("是否存在现有量")]
        public static readonly Property<bool?> IsExistsAvailableQtyProperty = P<LpnOnhandCriteria>.Register(e => e.IsExistsAvailableQty);

        /// <summary>
        /// 是否存在现有量
        /// </summary>
        public bool? IsExistsAvailableQty
        {
            get { return this.GetProperty(IsExistsAvailableQtyProperty); }
            set { this.SetProperty(IsExistsAvailableQtyProperty, value); }
        }
        #endregion

        #region 是否非空货主 IsNotEmptyStorer
        /// <summary>
        /// 是否非空货主
        /// </summary>
        [Label("是否非空货主")]
        public static readonly Property<bool?> IsNotEmptyStorerProperty = P<LpnOnhandCriteria>.Register(e => e.IsNotEmptyStorer);

        /// <summary>
        /// 是否非空货主
        /// </summary>
        public bool? IsNotEmptyStorer
        {
            get { return this.GetProperty(IsNotEmptyStorerProperty); }
            set { this.SetProperty(IsNotEmptyStorerProperty, value); }
        }
        #endregion

        #region 库存批次排除LotDefault NoLotDefault
        /// <summary>
        /// 库存批次排除LotDefault
        /// </summary>     
        public static readonly Property<bool> NoLotDefaultProperty = P<LpnOnhandCriteria>.Register(e => e.NoLotDefault);

        /// <summary>
        /// 库存批次排除LotDefault
        /// </summary>
        public bool NoLotDefault
        {
            get { return this.GetProperty(NoLotDefaultProperty); }
            set { this.SetProperty(NoLotDefaultProperty, value); }
        }
        #endregion

        #region HideZero HideZero
        /// <summary>
        /// HideZero
        /// </summary>
        public static readonly Property<bool> HideZeroProperty = P<LpnOnhandCriteria>.Register(e => e.HideZero);

        /// <summary>
        /// HideZero
        /// </summary>
        public bool HideZero
        {
            get { return this.GetProperty(HideZeroProperty); }
            set { this.SetProperty(HideZeroProperty, value); }
        }
        #endregion

        #region 序列号物料 IsSerial
        /// <summary>
        /// 序列号物料
        /// </summary>        
        public static readonly Property<bool?> IsSerialProperty = P<LpnOnhandCriteria>.Register(e => e.IsSerial);

        /// <summary>
        /// 序列号物料
        /// </summary>
        public bool? IsSerial
        {
            get { return this.GetProperty(IsSerialProperty); }
            set { this.SetProperty(IsSerialProperty, value); }
        }
        #endregion

        #region 窗口 IsWindow 弹窗的需要赋值true，否则会多一栏合计栏位
        /// <summary>
        /// 窗口
        /// </summary>         
        public static readonly Property<bool> IsWindowProperty = P<LpnOnhandCriteria>.Register(e => e.IsWindow);

        /// <summary>
        /// 窗口
        /// </summary>
        public bool IsWindow
        {
            get { return this.GetProperty(IsWindowProperty); }
            set { this.SetProperty(IsWindowProperty, value); }
        }
        #endregion

        #region 物料及扩展属性集合 ItemIdandExtprops
        /// <summary>
        /// 物料及扩展属性集合(扩展指定项)
        /// </summary>
        // <remarks>数据格式物料Id^物料扩展属性^项目号^任务号^批次^Lpn+;</remarks>
        [Label("物料及扩展属性集合")]
        public static readonly Property<string> ItemIdandExtpropsProperty = P<LpnOnhandCriteria>.Register(e => e.ItemIdandExtprops);

        /// <summary>
        /// 物料及扩展属性集合
        /// </summary>
        public string ItemIdandExtprops
        {
            get { return this.GetProperty(ItemIdandExtpropsProperty); }
            set { this.SetProperty(ItemIdandExtpropsProperty, value); }
        }
        #endregion

        #region 超期复检 IsInspection
        /// <summary>
        /// 超期复检
        /// </summary>
        [Label("超期复检")]
        public static readonly Property<bool> IsInspectionProperty = P<LpnOnhandCriteria>.Register(e => e.IsInspection);

        /// <summary>
        /// 超期复检
        /// </summary>
        public bool IsInspection
        {
            get { return this.GetProperty(IsInspectionProperty); }
            set { this.SetProperty(IsInspectionProperty, value); }
        }
        #endregion

        #region 是否允许人工上架 IsAllowManualGrounding
        /// <summary>
        /// 是否允许人工上架
        /// </summary>
        [Label("是否允许人工上架")]
        public static readonly Property<bool?> IsAllowManualGroundingProperty = P<LpnOnhandCriteria>.Register(e => e.IsAllowManualGrounding);

        /// <summary>
        /// 是否允许人工上架
        /// </summary>
        public bool? IsAllowManualGrounding
        {
            get { return this.GetProperty(IsAllowManualGroundingProperty); }
            set { this.SetProperty(IsAllowManualGroundingProperty, value); }
        }
        #endregion

        #region 库位是否锁 IsLock
        /// <summary>
        /// 库位是否锁
        /// </summary>
        [Label("库位是否锁")]
        public static readonly Property<bool?> IsLockProperty = P<LpnOnhandCriteria>.Register(e => e.IsLock);

        /// <summary>
        /// 库位是否锁
        /// </summary>
        public bool? IsLock
        {
            get { return this.GetProperty(IsLockProperty); }
            set { this.SetProperty(IsLockProperty, value); }
        }
        #endregion

        #region 调整库位锁 IsAdjustLock
        /// <summary>
        /// 调整库位锁
        /// </summary>
        [Label("调整库位锁")]
        public static readonly Property<bool?> IsAdjustLockProperty = P<LpnOnhandCriteria>.Register(e => e.IsAdjustLock);

        /// <summary>
        /// 调整库位锁
        /// </summary>
        public bool? IsAdjustLock
        {
            get { return this.GetProperty(IsAdjustLockProperty); }
            set { this.SetProperty(IsAdjustLockProperty, value); }
        }
        #endregion

        #region 是否立库 IsAutomated
        /// <summary>
        /// 是否立库
        /// </summary>
        [Label("是否立库")]
        public static readonly Property<bool?> IsAutomatedProperty = P<LpnOnhandCriteria>.Register(e => e.IsAutomated);

        /// <summary>
        /// 是否立库
        /// </summary>
        public bool? IsAutomated
        {
            get { return this.GetProperty(IsAutomatedProperty); }
            set { this.SetProperty(IsAutomatedProperty, value); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料")]
        public static readonly Property<string> ItemCodeProperty = P<LpnOnhandCriteria>.Register(e => e.ItemCode);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
            set { this.SetProperty(ItemCodeProperty, value); }
        }
        #endregion

        #region 库区 StorageAreaCode
        /// <summary>
        /// 库区
        /// </summary>
        [Label("库区")]
        public static readonly Property<string> StorageAreaCodeProperty = P<LpnOnhandCriteria>.Register(e => e.StorageAreaCode);

        /// <summary>
        /// 库区
        /// </summary>
        public string StorageAreaCode
        {
            get { return this.GetProperty(StorageAreaCodeProperty); }
            set { this.SetProperty(StorageAreaCodeProperty, value); }
        }
        #endregion

        #region 库位 StorageLocationCode
        /// <summary>
        /// 库位
        /// </summary>
        [Label("库位")]
        public static readonly Property<string> StorageLocationCodeProperty = P<LpnOnhandCriteria>.Register(e => e.StorageLocationCode);

        /// <summary>
        /// 库位
        /// </summary>
        public string StorageLocationCode
        {
            get { return this.GetProperty(StorageLocationCodeProperty); }
            set { this.SetProperty(StorageLocationCodeProperty, value); }
        }
        #endregion

        #region 排除库存 ExpectIds
        /// <summary>
        /// 排除库存
        /// </summary>
        [Label("排除库存")]
        public static readonly Property<string> ExpectIdsProperty = P<LpnOnhandCriteria>.Register(e => e.ExpectIds);

        /// <summary>
        /// 排除库存
        /// </summary>
        public string ExpectIds
        {
            get { return this.GetProperty(ExpectIdsProperty); }
            set { this.SetProperty(ExpectIdsProperty, value); }
        }
        #endregion

        #region 仓库 WarehouseCode
        /// <summary>
        /// 仓库
        /// </summary>
        [Label("仓库")]
        public static readonly Property<string> WarehouseCodeProperty = P<LpnOnhandCriteria>.Register(e => e.WarehouseCode);

        /// <summary>
        /// 仓库
        /// </summary>
        public string WarehouseCode
        {
            get { return this.GetProperty(WarehouseCodeProperty); }
            set { this.SetProperty(WarehouseCodeProperty, value); }
        }
        #endregion         

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<InvOnhandController>().GetLpnOnhands(this);
        }
    }
}