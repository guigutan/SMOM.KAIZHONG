using SIE.Domain;
using SIE.Items;
using SIE.ObjectModel;
using SIE.Warehouses;
using System;

namespace SIE.Inventory.Onhands
{
    /// <summary>
    /// 批次库存
    /// </summary>
    [QueryEntity, Serializable]
    [Label("批次库存查询")]
    public partial class LotOnhandCriteria : Criteria
    {
        #region 仓库 Warehouse
        /// <summary>
        /// 仓库Id
        /// </summary>
        [Label("仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty = P<LotOnhandCriteria>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<LotOnhandCriteria>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

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
        public static readonly IRefIdProperty StorageAreaIdProperty = P<LotOnhandCriteria>.RegisterRefId(e => e.StorageAreaId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<StorageArea> StorageAreaProperty = P<LotOnhandCriteria>.RegisterRef(e => e.StorageArea, StorageAreaIdProperty);

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
        public static readonly IRefIdProperty StorageLocationIdProperty = P<LotOnhandCriteria>.RegisterRefId(e => e.StorageLocationId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<StorageLocation> StorageLocationProperty = P<LotOnhandCriteria>.RegisterRef(e => e.StorageLocation, StorageLocationIdProperty);

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
        public static readonly IRefIdProperty ItemIdProperty = P<LotOnhandCriteria>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Item> ItemProperty = P<LotOnhandCriteria>.RegisterRef(e => e.Item, ItemIdProperty);

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
        public static readonly Property<string> LotCodeProperty = P<LotOnhandCriteria>.Register(e => e.LotCode);

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
        public static readonly Property<string> StorerCodeProperty = P<LotOnhandCriteria>.Register(e => e.StorerCode);

        /// <summary>
        /// 货主编码
        /// </summary>
        public string StorerCode
        {
            get { return GetProperty(StorerCodeProperty); }
            set { SetProperty(StorerCodeProperty, value); }
        }
        #endregion

        #region 项目号 ProjectNo
        /// <summary>
        /// 项目号
        /// </summary>
        [Label("项目号")]
        public static readonly Property<string> ProjectNoProperty = P<LotOnhandCriteria>.Register(e => e.ProjectNo);

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
        public static readonly Property<string> TaskNoProperty = P<LotOnhandCriteria>.Register(e => e.TaskNo);

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
        public static readonly Property<bool> IsZeroProperty = P<LotOnhandCriteria>.Register(e => e.IsZero);

        /// <summary>
        /// 零库存
        /// </summary>
        public bool IsZero
        {
            get { return GetProperty(IsZeroProperty); }
            set { SetProperty(IsZeroProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<InvOnhandController>().GetLotOnhands(this);
        }
    }
}