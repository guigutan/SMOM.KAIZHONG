using SIE.Domain;
using SIE.Inventory.Commom;
using SIE.Items;
using SIE.ObjectModel;
using SIE.Packages.Boxs;
using SIE.Warehouses;
using System;

namespace SIE.Inventory.Piles
{
    /// <summary>
    /// 垛查询实体
    /// </summary>
    [RootEntity, Serializable]
    public class PileCriteria : Criteria
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Required]
        [NotDuplicate]
        [MaxLength(80)]
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<PileCriteria>.Register(e => e.Code);

        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 型号 Model
        /// <summary>
        /// 型号
        /// </summary>
        [Required]
        [MaxLength(240)]
        [Label("型号")]
        public static readonly Property<string> ModelProperty = P<PileCriteria>.Register(e => e.Model);

        /// <summary>
        /// 型号
        /// </summary>
        public string Model
        {
            get { return GetProperty(ModelProperty); }
            set { SetProperty(ModelProperty, value); }
        }
        #endregion

        #region 垛状态 PileState
        /// <summary>
        /// 垛状态
        /// </summary>
        [Label("垛状态")]
        public static readonly Property<BoxState?> PileStateProperty = P<PileCriteria>.Register(e => e.PileState);

        /// <summary>
        /// 垛状态
        /// </summary>
        public BoxState? PileState
        {
            get { return this.GetProperty(PileStateProperty); }
            set { this.SetProperty(PileStateProperty, value); }
        }
        #endregion

        #region 周转容器 TurnoverContainer
        /// <summary>
        /// 周转容器
        /// </summary>
        [Label("周转容器")]
        public static readonly Property<bool?> TurnoverContainerProperty = P<PileCriteria>.Register(e => e.TurnoverContainer);

        /// <summary>
        /// 周转容器
        /// </summary>
        public bool? TurnoverContainer
        {
            get { return GetProperty(TurnoverContainerProperty); }
            set { SetProperty(TurnoverContainerProperty, value); }
        }
        #endregion

        #region 单据号 BillNo
        /// <summary>
        /// 单据号
        /// </summary>
        [Label("单据号")]
        public static readonly Property<string> BillNoProperty = P<PileCriteria>.Register(e => e.BillNo);

        /// <summary>
        /// 单据号
        /// </summary>
        public string BillNo
        {
            get { return GetProperty(BillNoProperty); }
            set { SetProperty(BillNoProperty, value); }
        }
        #endregion

        #region 当前位置 CurLocation
        /// <summary>
        /// 当前位置
        /// </summary>
        [MaxLength(80)]
        [Label("当前位置")]
        public static readonly Property<string> CurLocationProperty = P<PileCriteria>.Register(e => e.CurLocation);

        /// <summary>
        /// 当前位置
        /// </summary>
        public string CurLocation
        {
            get { return GetProperty(CurLocationProperty); }
            set { SetProperty(CurLocationProperty, value); }
        }
        #endregion

        #region 库位 StorageLocation
        /// <summary>
        /// 库位Id
        /// </summary>
        [Label("库位")]
        public static readonly IRefIdProperty StorageLocationIdProperty = P<PileCriteria>.RegisterRefId(e => e.StorageLocationId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<StorageLocation> StorageLocationProperty = P<PileCriteria>.RegisterRef(e => e.StorageLocation, StorageLocationIdProperty);

        /// <summary>
        /// 库位
        /// </summary>
        public StorageLocation StorageLocation
        {
            get { return GetRefEntity(StorageLocationProperty); }
            set { SetRefEntity(StorageLocationProperty, value); }
        }
        #endregion

        #region 库区 StorageArea
        /// <summary>
        /// 库区Id
        /// </summary>
        [Label("库区")]
        public static readonly IRefIdProperty StorageAreaIdProperty = P<PileCriteria>.RegisterRefId(e => e.StorageAreaId, ReferenceType.Normal);

        /// <summary>
        /// 库区Id
        /// </summary>
        public double? StorageAreaId
        {
            get { return (double?)GetRefNullableId(StorageAreaIdProperty); }
            set { SetRefNullableId(StorageAreaIdProperty, value); }
        }

        /// <summary>
        /// 库区
        /// </summary>
        public static readonly RefEntityProperty<StorageArea> StorageAreaProperty = P<PileCriteria>.RegisterRef(e => e.StorageArea, StorageAreaIdProperty);

        /// <summary>
        /// 库区
        /// </summary>
        public StorageArea StorageArea
        {
            get { return GetRefEntity(StorageAreaProperty); }
            set { SetRefEntity(StorageAreaProperty, value); }
        }
        #endregion

        #region 仓库 Warehouse
        /// <summary>
        /// 仓库Id
        /// </summary>
        [Label("仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty = P<PileCriteria>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<PileCriteria>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return GetRefEntity(WarehouseProperty); }
            set { SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料编码")]
        public static readonly IRefIdProperty ItemIdProperty =
            P<PileCriteria>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料Id
        /// </summary>
        public double? ItemId
        {
            get { return (double?)this.GetRefNullableId(ItemIdProperty); }
            set { this.SetRefNullableId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty =
            P<PileCriteria>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return this.GetRefEntity(ItemProperty); }
            set { this.SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 物料状态 ItemState
        /// <summary>
        /// 物料状态
        /// </summary>
        [Label("物料状态")]
        public static readonly Property<ItemState?> ItemStateProperty = P<PileCriteria>.Register(e => e.ItemState);

        /// <summary>
        /// 物料状态
        /// </summary>
        public ItemState? ItemState
        {
            get { return this.GetProperty(ItemStateProperty); }
            set { this.SetProperty(ItemStateProperty, value); }
        }
        #endregion

        #region 批次号 Lot
        /// <summary>
        /// 批次号Id
        /// </summary>
        [Label("批次号")]
        public static readonly IRefIdProperty LotIdProperty =
            P<PileCriteria>.RegisterRefId(e => e.LotId, ReferenceType.Normal);

        /// <summary>
        /// 批次号Id
        /// </summary>
        public double? LotId
        {
            get { return (double?)this.GetRefNullableId(LotIdProperty); }
            set { this.SetRefNullableId(LotIdProperty, value); }
        }

        /// <summary>
        /// 批次号
        /// </summary>
        public static readonly RefEntityProperty<Lot> LotProperty =
            P<PileCriteria>.RegisterRef(e => e.Lot, LotIdProperty);

        /// <summary>
        /// 批次号
        /// </summary>
        public Lot Lot
        {
            get { return this.GetRefEntity(LotProperty); }
            set { this.SetRefEntity(LotProperty, value); }
        }
        #endregion

        #region 创建日期 CreateDate
        /// <summary>
        /// 创建日期
        /// </summary>
        [Label("创建日期")]
        public static readonly Property<DateRange> CreateDateProperty = P<PileCriteria>.Register(e => e.CreateDate);

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateRange CreateDate
        {
            get { return this.GetProperty(CreateDateProperty); }
            set { this.SetProperty(CreateDateProperty, value); }
        }
        #endregion


        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<PileController>().GetPiles(this);
        }
    }
}
