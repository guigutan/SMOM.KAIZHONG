using SIE.Domain;
using SIE.Fixtures;
using SIE.Fixtures.Models;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;

namespace SIE.EMS.InventoryPlans
{
    /// <summary>
    /// 盘点计划范围（工治具）
    /// </summary>
    [RootEntity, Serializable]
    [Label("盘点计划范围（工治具）")]
    public partial class InventoryPlanFixture : DataEntity
    {
        #region 资产分类 AssetsCategory
        /// <summary>
        /// 资产分类
        /// </summary>
        [Label("资产分类")]
        public static readonly Property<string> AssetsCategoryProperty = P<InventoryPlanFixture>.Register(e => e.AssetsCategory);

        /// <summary>
        /// 资产分类
        /// </summary>
        public string AssetsCategory
        {
            get { return GetProperty(AssetsCategoryProperty); }
            set { SetProperty(AssetsCategoryProperty, value); }
        }
        #endregion

        #region 工治具类型 FixtureTypes
        /// <summary>
        /// 工治具类型
        /// </summary>
        [MaxLength(2000)]
        [Label("工治具类型")]
        public static readonly Property<string> FixtureTypesProperty = P<InventoryPlanFixture>.Register(e => e.FixtureTypes);

        /// <summary>
        /// 工治具类型
        /// </summary>
        public string FixtureTypes
        {
            get { return GetProperty(FixtureTypesProperty); }
            set { SetProperty(FixtureTypesProperty, value); }
        }
        #endregion


        #region 工治具类型Ids FixtureTypeIds
        /// <summary>
        /// 工治具类型
        /// </summary>
        [MaxLength(1000)]
        [Label("工治具类型")]
        public static readonly Property<string> FixtureTypeIdsProperty = P<InventoryPlanFixture>.Register(e => e.FixtureTypeIds);

        /// <summary>
        /// 工治具类型
        /// </summary>
        public string FixtureTypeIds
        {
            get { return GetProperty(FixtureTypeIdsProperty); }
            set { SetProperty(FixtureTypeIdsProperty, value); }
        }
        #endregion


        #region 工治具型号 FixtureModels
        /// <summary>
        /// 工治具型号
        /// </summary>
        [MaxLength(1000)]
        [Label("工治具型号")]
        public static readonly Property<string> FixtureModelsProperty = P<InventoryPlanFixture>.Register(e => e.FixtureModels);

        /// <summary>
        /// 工治具型号
        /// </summary>
        public string FixtureModels
        {
            get { return GetProperty(FixtureModelsProperty); }
            set { SetProperty(FixtureModelsProperty, value); }
        }
        #endregion

        #region 工治具型号 FixtureModelIds
        /// <summary>
        /// 工治具型号
        /// </summary>
        [MaxLength(2000)]
        [Label("工治具型号")]
        public static readonly Property<string> FixtureModelIdsProperty = P<InventoryPlanFixture>.Register(e => e.FixtureModelIds);

        /// <summary>
        /// 工治具型号
        /// </summary>
        public string FixtureModelIds
        {
            get { return GetProperty(FixtureModelIdsProperty); }
            set { SetProperty(FixtureModelIdsProperty, value); }
        }
        #endregion


        #region 管理方式 ManageMode
        /// <summary>
        /// 管理方式
        /// </summary>
        [Label("管理方式")]
        public static readonly Property<ManageMode?> ManageModeProperty = P<InventoryPlanFixture>.Register(e => e.ManageMode);

        /// <summary>
        /// 管理方式
        /// </summary>
        public ManageMode? ManageMode
        {
            get { return GetProperty(ManageModeProperty); }
            set { SetProperty(ManageModeProperty, value); }
        }
        #endregion

        #region 固定资产 IsFixAsset
        /// <summary>
        /// 固定资产
        /// </summary>
        [Label("固定资产")]
        public static readonly Property<YesNo?> IsFixAssetProperty = P<InventoryPlanFixture>.Register(e => e.IsFixAsset);

        /// <summary>
        /// 固定资产
        /// </summary>
        public YesNo? IsFixAsset
        {
            get { return GetProperty(IsFixAssetProperty); }
            set { SetProperty(IsFixAssetProperty, value); }
        }
        #endregion

        #region 工治具编码 FixtureEncodes
        /// <summary>
        /// 工治具编码
        /// </summary>
        [MaxLength(1000)]
        [Label("工治具编码")]
        public static readonly Property<string> FixtureEncodesProperty = P<InventoryPlanFixture>.Register(e => e.FixtureEncodes);

        /// <summary>
        /// 工治具编码
        /// </summary>
        public string FixtureEncodes
        {
            get { return this.GetProperty(FixtureEncodesProperty); }
            set { this.SetProperty(FixtureEncodesProperty, value); }
        }
        #endregion

        #region 工治具编码 FixtureEncodeIds
        /// <summary>
        /// 工治具编码
        /// </summary>
        [MaxLength(2000)]
        [Label("工治具编码")]
        public static readonly Property<string> FixtureEncodeIdsProperty = P<InventoryPlanFixture>.Register(e => e.FixtureEncodeIds);

        /// <summary>
        /// 工治具编码
        /// </summary>
        public string FixtureEncodeIds
        {
            get { return this.GetProperty(FixtureEncodeIdsProperty); }
            set { this.SetProperty(FixtureEncodeIdsProperty, value); }
        }
        #endregion


        #region 资产责任人 AssetOwner
        /// <summary>
        /// 资产责任人Id
        /// </summary>
        [Label("资产责任人")]
        public static readonly IRefIdProperty AssetOwnerIdProperty = P<InventoryPlanFixture>.RegisterRefId(e => e.AssetOwnerId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Employee> AssetOwnerProperty = P<InventoryPlanFixture>.RegisterRef(e => e.AssetOwner, AssetOwnerIdProperty);

        /// <summary>
        /// 资产责任人
        /// </summary>
        public Employee AssetOwner
        {
            get { return GetRefEntity(AssetOwnerProperty); }
            set { SetRefEntity(AssetOwnerProperty, value); }
        }
        #endregion

        #region 盘点计划 InventoryPlan
        /// <summary>
        /// 盘点计划Id
        /// </summary>
        [Label("盘点计划")]
        public static readonly IRefIdProperty InventoryPlanIdProperty = P<InventoryPlanFixture>.RegisterRefId(e => e.InventoryPlanId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<InventoryPlan> InventoryPlanProperty = P<InventoryPlanFixture>.RegisterRef(e => e.InventoryPlan, InventoryPlanIdProperty);

        /// <summary>
        /// 盘点计划
        /// </summary>
        public InventoryPlan InventoryPlan
        {
            get { return GetRefEntity(InventoryPlanProperty); }
            set { SetRefEntity(InventoryPlanProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 盘点计划范围（工治具） 实体配置
    /// </summary>
    internal class InventoryPlanFixtureConfig : EntityConfig<InventoryPlanFixture>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_INV_PLAN_FIX").MapAllProperties();
            Meta.Property(InventoryPlanFixture.FixtureTypesProperty).ColumnMeta.HasLength(4000);
            Meta.Property(InventoryPlanFixture.FixtureTypeIdsProperty).ColumnMeta.HasLength(4000);
            Meta.Property(InventoryPlanFixture.FixtureEncodeIdsProperty).ColumnMeta.HasLength(4000);
            Meta.Property(InventoryPlanFixture.FixtureModelsProperty).ColumnMeta.HasLength(4000);
            Meta.Property(InventoryPlanFixture.FixtureModelIdsProperty).ColumnMeta.HasLength(4000);
            Meta.EnablePhantoms();
        }
    }
}