using SIE.Domain;
using SIE.EMS.SpareParts;
using SIE.EMS.SpareParts.Enums;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;

namespace SIE.EMS.InventoryPlans
{
    /// <summary>
    /// 盘点计划范围（备件）
    /// </summary>
    [RootEntity, Serializable]
    [Label("盘点计划范围（备件）")]
    public partial class InventoryPlanSparePart : DataEntity
    {
        #region 仓库 Warehouses
        /// <summary>
        /// 仓库
        /// </summary>
        [Label("仓库")]
        [MaxLength(2000)]
      //  [Required]
        public static readonly Property<string> WarehousesProperty = P<InventoryPlanSparePart>.Register(e => e.Warehouses);

        /// <summary>
        /// 仓库
        /// </summary>
        public string Warehouses
        {
            get { return GetProperty(WarehousesProperty); }
            set { SetProperty(WarehousesProperty, value); }
        }
        #endregion

        #region 仓库ID列表 WarehouseIds
        /// <summary>
        /// 仓库ID列表
        /// </summary>
        [Label("仓库ID列表")]
        [MaxLength(2000)]
        public static readonly Property<string> WarehouseIdsProperty = P<InventoryPlanSparePart>.Register(e => e.WarehouseIds);

        /// <summary>
        /// 仓库ID列表
        /// </summary>
        public string WarehouseIds
        {
            get { return GetProperty(WarehouseIdsProperty); }
            set { SetProperty(WarehouseIdsProperty, value); }
        }
        #endregion

        #region 库区 StorageAreas
        /// <summary>
        /// 库区
        /// </summary>
        [Label("库区")]
        [MaxLength(2000)]
        public static readonly Property<string> StorageAreasProperty = P<InventoryPlanSparePart>.Register(e => e.StorageAreas);

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
        [MaxLength(2000)]
        public static readonly Property<string> StorageAreaIdsProperty = P<InventoryPlanSparePart>.Register(e => e.StorageAreaIds);

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
        [MaxLength(2000)]
        public static readonly Property<string> StorageLocationsProperty = P<InventoryPlanSparePart>.Register(e => e.StorageLocations);

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
        [MaxLength(2000)]
        public static readonly Property<string> StorageLocationIdsProperty = P<InventoryPlanSparePart>.Register(e => e.StorageLocationIds);

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
        public static readonly Property<string> AssetsCategoryProperty = P<InventoryPlanSparePart>.Register(e => e.AssetsCategory);

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
        public static readonly Property<SparePartType?> PartTypeProperty = P<InventoryPlanSparePart>.Register(e => e.PartType);

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
        public static readonly Property<ControlMethod?> ControlMethodProperty = P<InventoryPlanSparePart>.Register(e => e.ControlMethod);

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
        public static readonly IRefIdProperty SparePartIdProperty = P<InventoryPlanSparePart>.RegisterRefId(e => e.SparePartId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<SparePart> SparePartProperty = P<InventoryPlanSparePart>.RegisterRef(e => e.SparePart, SparePartIdProperty);

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
        public static readonly IRefIdProperty AssetOwnerIdProperty = P<InventoryPlanSparePart>.RegisterRefId(e => e.AssetOwnerId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Employee> AssetOwnerProperty = P<InventoryPlanSparePart>.RegisterRef(e => e.AssetOwner, AssetOwnerIdProperty);

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
        [Label("固定资产")]
        public static readonly Property<YesNo?> IsFixAssetProperty = P<InventoryPlanSparePart>.Register(e => e.IsFixAsset);

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
        public static readonly IRefIdProperty ItemCategoryIdProperty = P<InventoryPlanSparePart>.RegisterRefId(e => e.ItemCategoryId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<ItemCategory> ItemCategoryProperty = P<InventoryPlanSparePart>.RegisterRef(e => e.ItemCategory, ItemCategoryIdProperty);

        /// <summary>
        /// 分类
        /// </summary>
        public ItemCategory ItemCategory
        {
            get { return GetRefEntity(ItemCategoryProperty); }
            set { SetRefEntity(ItemCategoryProperty, value); }
        }
        #endregion

        #region 盘点计划 InventoryPlan
        /// <summary>
        /// 盘点计划Id
        /// </summary>
        [Label("盘点计划")]
        public static readonly IRefIdProperty InventoryPlanIdProperty = P<InventoryPlanSparePart>.RegisterRefId(e => e.InventoryPlanId, ReferenceType.Normal);

        /// <summary>
        /// 盘点计划Id
        /// </summary>
        public double InventoryPlanId
        {
            get { return (double)GetRefId(InventoryPlanIdProperty); }
            set { SetRefId(InventoryPlanIdProperty, value); }
        }

        /// <summary>
        /// 盘点计划
        /// </summary>
        public static readonly RefEntityProperty<InventoryPlan> InventoryPlanProperty = P<InventoryPlanSparePart>.RegisterRef(e => e.InventoryPlan, InventoryPlanIdProperty);

        /// <summary>
        /// 盘点计划
        /// </summary>
        public InventoryPlan InventoryPlan
        {
            get { return GetRefEntity(InventoryPlanProperty); }
            set { SetRefEntity(InventoryPlanProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 分类 ItemCategoryName
        /// <summary>
        /// 分类
        /// </summary>
        [Label("分类")]
        public static readonly Property<string> ItemCategoryNameProperty = P<InventoryPlanSparePart>.RegisterView(e => e.ItemCategoryName,p=>p.ItemCategory.Name);

        /// <summary>
        /// 分类
        /// </summary>
        public string ItemCategoryName
        {
            get { return this.GetProperty(ItemCategoryNameProperty); }
            set { this.SetProperty(ItemCategoryNameProperty, value); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 盘点计划范围（备件） 实体配置
    /// </summary>
    internal class InventoryPlanSparePartConfig : EntityConfig<InventoryPlanSparePart>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_INV_PLAN_SP").MapAllProperties();
            Meta.Property(InventoryPlanSparePart.WarehouseIdsProperty).ColumnMeta.HasLength(4000);
            Meta.Property(InventoryPlanSparePart.WarehousesProperty).ColumnMeta.HasLength(4000);
            Meta.Property(InventoryPlanSparePart.StorageAreaIdsProperty).ColumnMeta.HasLength(4000);
            Meta.Property(InventoryPlanSparePart.StorageAreasProperty).ColumnMeta.HasLength(4000);
            Meta.Property(InventoryPlanSparePart.StorageLocationsProperty).ColumnMeta.HasLength(4000);
            Meta.Property(InventoryPlanSparePart.StorageLocationIdsProperty).ColumnMeta.HasLength(4000);
            Meta.EnablePhantoms();
        }
    }
}