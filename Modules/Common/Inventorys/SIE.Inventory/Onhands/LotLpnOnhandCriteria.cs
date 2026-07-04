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
    /// 批次和LPN库存，LPN可为空
    /// </summary>
    [QueryEntity, Serializable]
    [Label("批次和LPN库存查询")]
    public partial class LotLpnOnhandCriteria : Criteria
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public LotLpnOnhandCriteria()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public LotLpnOnhandCriteria(OrderType type)
        {
            this.type = type;
        }

        #region 仓库 Warehouse
        /// <summary>
        /// 仓库Id
        /// </summary>
        [Label("仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty = P<LotLpnOnhandCriteria>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<LotLpnOnhandCriteria>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return GetRefEntity(WarehouseProperty); }
            set { SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 库位 StorageLocation
        /// <summary>
        /// 库位Id
        /// </summary>
        [Label("库位")]
        public static readonly IRefIdProperty StorageLocationIdProperty = P<LotLpnOnhandCriteria>.RegisterRefId(e => e.StorageLocationId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<StorageLocation> StorageLocationProperty = P<LotLpnOnhandCriteria>.RegisterRef(e => e.StorageLocation, StorageLocationIdProperty);

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
        public static readonly IRefIdProperty ItemIdProperty = P<LotLpnOnhandCriteria>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Item> ItemProperty = P<LotLpnOnhandCriteria>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 货主编码 StorerCode
        /// <summary>
        /// 货主编码
        /// </summary>
        [Label("货主编码")]
        public static readonly Property<string> StorerCodeProperty = P<LotLpnOnhandCriteria>.Register(e => e.StorerCode);

        /// <summary>
        /// 货主编码
        /// </summary>
        public string StorerCode
        {
            get { return GetProperty(StorerCodeProperty); }
            set { SetProperty(StorerCodeProperty, value); }
        }
        #endregion

        #region 批次 Lot
        /// <summary>
        /// 批次
        /// </summary>
        [Label("批次")]
        public static readonly Property<string> LotProperty = P<LotLpnOnhandCriteria>.Register(e => e.Lot);

        /// <summary>
        /// 批次
        /// </summary>
        public string Lot
        {
            get { return GetProperty(LotProperty); }
            set { SetProperty(LotProperty, value); }
        }
        #endregion

        #region LPN Lpn
        /// <summary>
        /// LPN
        /// </summary>
        [Label("LPN")]
        public static readonly Property<string> LpnProperty = P<LotLpnOnhandCriteria>.Register(e => e.Lpn);

        /// <summary>
        /// LPN
        /// </summary>
        public string Lpn
        {
            get { return GetProperty(LpnProperty); }
            set { SetProperty(LpnProperty, value); }
        }
        #endregion

        #region 库存状态 State
        /// <summary>
        /// 库存状态
        /// </summary>
        [Label("库存状态")]
        public static readonly Property<OnhandState?> StateProperty = P<LotLpnOnhandCriteria>.Register(e => e.State);

        /// <summary>
        /// 库存状态
        /// </summary>
        public OnhandState? State
        {
            get { return this.GetProperty(StateProperty); }
            set { this.SetProperty(StateProperty, value); }
        }
        #endregion

        #region 项目号 ProjectNo
        /// <summary>
        /// 项目号
        /// </summary>
        [Label("项目号")]
        public static readonly Property<string> ProjectNoProperty = P<LotLpnOnhandCriteria>.Register(e => e.ProjectNo);

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
        public static readonly Property<string> TaskNoProperty = P<LotLpnOnhandCriteria>.Register(e => e.TaskNo);

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
        public static readonly Property<bool> IsZeroProperty = P<LotLpnOnhandCriteria>.Register(e => e.IsZero);

        /// <summary>
        /// 零库存
        /// </summary>
        public bool IsZero
        {
            get { return GetProperty(IsZeroProperty); }
            set { SetProperty(IsZeroProperty, value); }
        }
        #endregion

        #region 是否冻结 IsFrozen
        /// <summary>
        /// 是否冻结
        /// </summary>
        [Label("是否冻结")]
        public static readonly Property<bool?> IsFrozenProperty = P<LotLpnOnhandCriteria>.Register(e => e.IsFrozen);

        /// <summary>
        /// 是否冻结
        /// </summary>
        public bool? IsFrozen
        {
            get { return GetProperty(IsFrozenProperty); }
            set { SetProperty(IsFrozenProperty, value); }
        }
        #endregion

        #region 是否存在现有量 IsExistsAvailableQty
        /// <summary>
        /// 是否存在现有量
        /// </summary>
        [Label("是否存在现有量")]
        public static readonly Property<bool?> IsExistsAvailableQtyProperty = P<LotLpnOnhandCriteria>.Register(e => e.IsExistsAvailableQty);

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
        public static readonly Property<bool?> IsNotEmptyStorerProperty = P<LotLpnOnhandCriteria>.Register(e => e.IsNotEmptyStorer);

        /// <summary>
        /// 是否非空货主
        /// </summary>
        public bool? IsNotEmptyStorer
        {
            get { return this.GetProperty(IsNotEmptyStorerProperty); }
            set { this.SetProperty(IsNotEmptyStorerProperty, value); }
        }
        #endregion

        #region 类型 type
        /// <summary>
        /// 类型
        /// </summary>
        [Label("类型")]
        public static readonly Property<OrderType> typeProperty = P<LotLpnOnhandCriteria>.Register(e => e.type);

        /// <summary>
        /// 类型
        /// </summary>
        public OrderType type
        {
            get { return this.GetProperty(typeProperty); }
            set { this.SetProperty(typeProperty, value); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料")]
        public static readonly Property<string> ItemCodeProperty = P<LotLpnOnhandCriteria>.Register(e => e.ItemCode);

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
        public static readonly Property<string> StorageAreaCodeProperty = P<LotLpnOnhandCriteria>.Register(e => e.StorageAreaCode);

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
        public static readonly Property<string> StorageLocationCodeProperty = P<LotLpnOnhandCriteria>.Register(e => e.StorageLocationCode);

        /// <summary>
        /// 库位
        /// </summary>
        public string StorageLocationCode
        {
            get { return this.GetProperty(StorageLocationCodeProperty); }
            set { this.SetProperty(StorageLocationCodeProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>

        protected override EntityList Fetch()
        {
            if (type == OrderType.StorerAdjust)
            {
                return RT.Service.Resolve<InvOnhandController>().GetLotLpnOnhandsForOnhandAdjust(this);
            }

            if (type == OrderType.Frozen || type == OrderType.UnFrozen)
            {
                return RT.Service.Resolve<InvOnhandController>().GetLotLpnOnhandForFrozen(this);
            }
            return RT.Service.Resolve<InvOnhandController>().GetLotLpnOnhands(this);
        }
    }
}