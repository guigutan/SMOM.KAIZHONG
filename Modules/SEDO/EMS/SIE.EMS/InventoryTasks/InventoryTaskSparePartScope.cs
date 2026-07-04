using SIE.Domain;
using SIE.EMS.SpareParts;
using SIE.EMS.SpareParts.Enums;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Warehouses;
using System;

namespace SIE.EMS.InventoryTasks
{
    /// <summary>
    /// 盘点任务备件盘点范围
    /// </summary>
    [RootEntity, Serializable]
    [Label("盘点任务备件盘点范围")]
    public partial class InventoryTaskSparePartScope : DataEntity
    {
        #region 仓库 Warehouse
        /// <summary>
        /// 仓库Id
        /// </summary>
        [Label("仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty =
            P<InventoryTaskSparePartScope>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 仓库Id
        /// </summary>
        public double WarehouseId
        {
            get { return (double)this.GetRefId(WarehouseIdProperty); }
            set { this.SetRefId(WarehouseIdProperty, value); }
        }

        /// <summary>
        /// 仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty =
            P<InventoryTaskSparePartScope>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return this.GetRefEntity(WarehouseProperty); }
            set { this.SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 库区 StorageAreas
        /// <summary>
        /// 库区
        /// </summary>
        [Label("库区")]
        public static readonly Property<string> StorageAreasProperty = P<InventoryTaskSparePartScope>.Register(e => e.StorageAreas);

        /// <summary>
        /// 库区
        /// </summary>
        public string StorageAreas
        {
            get { return GetProperty(StorageAreasProperty); }
            set { SetProperty(StorageAreasProperty, value); }
        }
        #endregion

        #region 库区ID列表 StorageAreaIds
        /// <summary>
        /// 库区ID列表
        /// </summary>
        [Label("库区ID列表")]
        public static readonly Property<string> StorageAreaIdsProperty = P<InventoryTaskSparePartScope>.Register(e => e.StorageAreaIds);

        /// <summary>
        /// 库区ID列表
        /// </summary>
        public string StorageAreaIds
        {
            get { return GetProperty(StorageAreaIdsProperty); }
            set { SetProperty(StorageAreaIdsProperty, value); }
        }
        #endregion

        #region 库位 StorageLocations
        /// <summary>
        /// 库位
        /// </summary>
        [Label("库位")]
        public static readonly Property<string> StorageLocationsProperty = P<InventoryTaskSparePartScope>.Register(e => e.StorageLocations);

        /// <summary>
        /// 库位
        /// </summary>
        public string StorageLocations
        {
            get { return GetProperty(StorageLocationsProperty); }
            set { SetProperty(StorageLocationsProperty, value); }
        }
        #endregion

        #region 库位ID列表 StorageLocationIds
        /// <summary>
        /// 库位ID列表
        /// </summary>
        [Label("库位ID列表")]
        public static readonly Property<string> StorageLocationIdsProperty = P<InventoryTaskSparePartScope>.Register(e => e.StorageLocationIds);

        /// <summary>
        /// 库位ID列表
        /// </summary>
        public string StorageLocationIds
        {
            get { return GetProperty(StorageLocationIdsProperty); }
            set { SetProperty(StorageLocationIdsProperty, value); }
        }
        #endregion

        #region 资产分类 AssetsCategory
        /// <summary>
        /// 资产分类
        /// </summary>
        [Label("资产分类")]
        public static readonly Property<string> AssetsCategoryProperty = P<InventoryTaskSparePartScope>.Register(e => e.AssetsCategory);

        /// <summary>
        /// 资产分类
        /// </summary>
        public string AssetsCategory
        {
            get { return GetProperty(AssetsCategoryProperty); }
            set { SetProperty(AssetsCategoryProperty, value); }
        }
        #endregion

        #region 类型 PartType
        /// <summary>
        /// 类型
        /// </summary>
        [Label("类型")]
        public static readonly Property<SparePartType?> PartTypeProperty = P<InventoryTaskSparePartScope>.Register(e => e.PartType);

        /// <summary>
        /// 类型
        /// </summary>
        public SparePartType? PartType
        {
            get { return GetProperty(PartTypeProperty); }
            set { SetProperty(PartTypeProperty, value); }
        }
        #endregion

        #region 管控方式 ControlMethod
        /// <summary>
        /// 管控方式
        /// </summary>
        [Label("管控方式")]
        public static readonly Property<ControlMethod?> ControlMethodProperty = P<InventoryTaskSparePartScope>.Register(e => e.ControlMethod);

        /// <summary>
        /// 管控方式
        /// </summary>
        public ControlMethod? ControlMethod
        {
            get { return GetProperty(ControlMethodProperty); }
            set { SetProperty(ControlMethodProperty, value); }
        }
        #endregion

        #region 备件 SparePart
        /// <summary>
        /// 备件Id
        /// </summary>
        [Label("备件")]
        public static readonly IRefIdProperty SparePartIdProperty = P<InventoryTaskSparePartScope>.RegisterRefId(e => e.SparePartId, ReferenceType.Normal);

        /// <summary>
        /// 备件Id
        /// </summary>
        public double? SparePartId
        {
            get { return (double?)GetRefNullableId(SparePartIdProperty); }
            set { SetRefNullableId(SparePartIdProperty, value); }
        }

        /// <summary>
        /// 备件
        /// </summary>
        public static readonly RefEntityProperty<SparePart> SparePartProperty = P<InventoryTaskSparePartScope>.RegisterRef(e => e.SparePart, SparePartIdProperty);

        /// <summary>
        /// 备件
        /// </summary>
        public SparePart SparePart
        {
            get { return GetRefEntity(SparePartProperty); }
            set { SetRefEntity(SparePartProperty, value); }
        }
        #endregion

        #region 资产责任人 AssetOwner
        /// <summary>
        /// 资产责任人Id
        /// </summary>
        [Label("资产责任人")]
        public static readonly IRefIdProperty AssetOwnerIdProperty = P<InventoryTaskSparePartScope>.RegisterRefId(e => e.AssetOwnerId, ReferenceType.Normal);

        /// <summary>
        /// 资产责任人Id
        /// </summary>
        public double? AssetOwnerId
        {
            get { return (double?)GetRefNullableId(AssetOwnerIdProperty); }
            set { SetRefNullableId(AssetOwnerIdProperty, value); }
        }

        /// <summary>
        /// 资产责任人
        /// </summary>
        public static readonly RefEntityProperty<Employee> AssetOwnerProperty = P<InventoryTaskSparePartScope>.RegisterRef(e => e.AssetOwner, AssetOwnerIdProperty);

        /// <summary>
        /// 资产责任人
        /// </summary>
        public Employee AssetOwner
        {
            get { return GetRefEntity(AssetOwnerProperty); }
            set { SetRefEntity(AssetOwnerProperty, value); }
        }
        #endregion

        #region 固定资产 IsFixAsset
        /// <summary>
        /// 固定资产
        /// </summary>
        [Label("状态是否")]
        public static readonly Property<YesNo?> IsFixAssetProperty = P<InventoryTaskSparePartScope>.Register(e => e.IsFixAsset);

        /// <summary>
        /// 固定资产
        /// </summary>
        public YesNo? IsFixAsset
        {
            get { return GetProperty(IsFixAssetProperty); }
            set { SetProperty(IsFixAssetProperty, value); }
        }
        #endregion

        #region 分类 ItemCategory
        /// <summary>
        /// 分类Id
        /// </summary>
        [Label("分类")]
        public static readonly IRefIdProperty ItemCategoryIdProperty = P<InventoryTaskSparePartScope>.RegisterRefId(e => e.ItemCategoryId, ReferenceType.Normal);

        /// <summary>
        /// 分类Id
        /// </summary>
        public double? ItemCategoryId
        {
            get { return (double?)GetRefNullableId(ItemCategoryIdProperty); }
            set { SetRefNullableId(ItemCategoryIdProperty, value); }
        }

        /// <summary>
        /// 分类
        /// </summary>
        public static readonly RefEntityProperty<ItemCategory> ItemCategoryProperty = P<InventoryTaskSparePartScope>.RegisterRef(e => e.ItemCategory, ItemCategoryIdProperty);

        /// <summary>
        /// 分类
        /// </summary>
        public ItemCategory ItemCategory
        {
            get { return GetRefEntity(ItemCategoryProperty); }
            set { SetRefEntity(ItemCategoryProperty, value); }
        }
        #endregion

        #region 盘点任务 InventoryTask
        /// <summary>
        /// 盘点任务Id
        /// </summary>
        [Label("盘点任务")]
        public static readonly IRefIdProperty InventoryTaskIdProperty =
            P<InventoryTaskSparePartScope>.RegisterRefId(e => e.InventoryTaskId, ReferenceType.Normal);

        /// <summary>
        /// 盘点任务Id
        /// </summary>
        public double InventoryTaskId
        {
            get { return (double)this.GetRefId(InventoryTaskIdProperty); }
            set { this.SetRefId(InventoryTaskIdProperty, value); }
        }

        /// <summary>
        /// 盘点任务
        /// </summary>
        public static readonly RefEntityProperty<InventoryTask> InventoryTaskProperty =
            P<InventoryTaskSparePartScope>.RegisterRef(e => e.InventoryTask, InventoryTaskIdProperty);

        /// <summary>
        /// 盘点任务
        /// </summary>
        public InventoryTask InventoryTask
        {
            get { return this.GetRefEntity(InventoryTaskProperty); }
            set { this.SetRefEntity(InventoryTaskProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 仓库编码 WarehouseCode
        /// <summary>
        /// 仓库编码
        /// </summary>
        [Label("仓库编码")]
        public static readonly Property<string> WarehouseCodeProperty = P<InventoryTaskSparePartScope>.RegisterView(e => e.WarehouseCode, p => p.Warehouse.Code);

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string WarehouseCode
        {
            get { return this.GetProperty(WarehouseCodeProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 盘点计划范围（备件） 实体配置
    /// </summary>
    internal class InventoryTaskSparePartScopeConfig : EntityConfig<InventoryTaskSparePartScope>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_INV_TSK_SP_SCOP").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}