using SIE.Domain;
using SIE.Equipments.EquipModels;
using SIE.Equipments.EquipTypes;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using SIE.Warehouses;
using System;

namespace SIE.EMS.InventoryPlans
{
    /// <summary>
    /// 盘点计划范围（设备）
    /// </summary>
    [RootEntity, Serializable]
    [Label("盘点计划范围（设备）")]
    public partial class InventoryPlanEquipment : DataEntity
    {
        #region 盘点计划 InventoryPlan
        /// <summary>
        /// 盘点计划Id
        /// </summary>
        [Label("盘点计划")]
        public static readonly IRefIdProperty InventoryPlanIdProperty =
            P<InventoryPlanEquipment>.RegisterRefId(e => e.InventoryPlanId, ReferenceType.Normal);

        /// <summary>
        /// 盘点计划Id
        /// </summary>
        public double InventoryPlanId
        {
            get { return (double)this.GetRefId(InventoryPlanIdProperty); }
            set { this.SetRefId(InventoryPlanIdProperty, value); }
        }

        /// <summary>
        /// 盘点计划
        /// </summary>
        public static readonly RefEntityProperty<InventoryPlan> InventoryPlanProperty =
            P<InventoryPlanEquipment>.RegisterRef(e => e.InventoryPlan, InventoryPlanIdProperty);

        /// <summary>
        /// 盘点计划
        /// </summary>
        public InventoryPlan InventoryPlan
        {
            get { return this.GetRefEntity(InventoryPlanProperty); }
            set { this.SetRefEntity(InventoryPlanProperty, value); }
        }
        #endregion

        #region 设备类别 TypeCategory
        /// <summary>
        /// 设备类别
        /// </summary>
        [Label("设备类别")]
        public static readonly Property<string> TypeCategoryProperty = P<InventoryPlanEquipment>.Register(e => e.TypeCategory);

        /// <summary>
        /// 设备类别
        /// </summary>
        public string TypeCategory
        {
            get { return GetProperty(TypeCategoryProperty); }
            set { SetProperty(TypeCategoryProperty, value); }
        }
        #endregion

        #region ABC分类 UseLevel
        /// <summary>
        /// ABC分类
        /// </summary>
        [Label("ABC分类")]
        public static readonly Property<string> UseLevelProperty = P<InventoryPlanEquipment>.Register(e => e.UseLevel);

        /// <summary>
        /// ABC分类
        /// </summary>
        public string UseLevel
        {
            get { return GetProperty(UseLevelProperty); }
            set { SetProperty(UseLevelProperty, value); }
        }
        #endregion

        #region 资产分类 AssetsCategory
        /// <summary>
        /// 资产分类
        /// </summary>
        [Label("资产分类")]
        public static readonly Property<string> AssetsCategoryProperty = P<InventoryPlanEquipment>.Register(e => e.AssetsCategory);

        /// <summary>
        /// 资产分类
        /// </summary>
        public string AssetsCategory
        {
            get { return GetProperty(AssetsCategoryProperty); }
            set { SetProperty(AssetsCategoryProperty, value); }
        }
        #endregion

        #region 仓库 Warehouse
        /// <summary>
        /// 仓库Id
        /// </summary>
        [Label("仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty = P<InventoryPlanEquipment>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<InventoryPlanEquipment>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return GetRefEntity(WarehouseProperty); }
            set { SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 固定资产 IsAsset
        /// <summary>
        /// 固定资产
        /// </summary>
        [Label("固定资产")]
        public static readonly Property<YesNo?> IsAssetProperty = P<InventoryPlanEquipment>.Register(e => e.IsAsset);

        /// <summary>
        /// 固定资产
        /// </summary>
        public YesNo? IsAsset
        {
            get { return GetProperty(IsAssetProperty); }
            set { SetProperty(IsAssetProperty, value); }
        }
        #endregion

        #region 管理部门 ManageDept
        /// <summary>
        /// 管理部门Id
        /// </summary>
        [Label("管理部门")]
        public static readonly IRefIdProperty ManageDeptIdProperty = P<InventoryPlanEquipment>.RegisterRefId(e => e.ManageDeptId, ReferenceType.Normal);

        /// <summary>
        /// 管理部门Id
        /// </summary>
        public double? ManageDeptId
        {
            get { return (double?)GetRefNullableId(ManageDeptIdProperty); }
            set { SetRefNullableId(ManageDeptIdProperty, value); }
        }

        /// <summary>
        /// 管理部门
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> ManageDeptProperty = P<InventoryPlanEquipment>.RegisterRef(e => e.ManageDept, ManageDeptIdProperty);

        /// <summary>
        /// 管理部门
        /// </summary>
        public Enterprise ManageDept
        {
            get { return GetRefEntity(ManageDeptProperty); }
            set { SetRefEntity(ManageDeptProperty, value); }
        }
        #endregion

        #region 设备类型 EquipType
        /// <summary>
        /// 设备类型Id
        /// </summary>
        [Label("设备类型")]
        public static readonly IRefIdProperty EquipTypeIdProperty = P<InventoryPlanEquipment>.RegisterRefId(e => e.EquipTypeId, ReferenceType.Normal);

        /// <summary>
        /// 设备类型Id
        /// </summary>
        public double? EquipTypeId
        {
            get { return (double?)GetRefNullableId(EquipTypeIdProperty); }
            set { SetRefNullableId(EquipTypeIdProperty, value); }
        }

        /// <summary>
        /// 设备类型
        /// </summary>
        public static readonly RefEntityProperty<EquipType> EquipTypeProperty = P<InventoryPlanEquipment>.RegisterRef(e => e.EquipType, EquipTypeIdProperty);

        /// <summary>
        /// 设备类型
        /// </summary>
        public EquipType EquipType
        {
            get { return GetRefEntity(EquipTypeProperty); }
            set { SetRefEntity(EquipTypeProperty, value); }
        }
        #endregion

        #region 使用部门 UseDept
        /// <summary>
        /// 使用部门Id
        /// </summary>
        [Label("使用部门")]
        public static readonly IRefIdProperty UseDeptIdProperty = P<InventoryPlanEquipment>.RegisterRefId(e => e.UseDeptId, ReferenceType.Normal);

        /// <summary>
        /// 使用部门Id
        /// </summary>
        public double? UseDeptId
        {
            get { return (double?)GetRefNullableId(UseDeptIdProperty); }
            set { SetRefNullableId(UseDeptIdProperty, value); }
        }

        /// <summary>
        /// 使用部门
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> UseDeptProperty = P<InventoryPlanEquipment>.RegisterRef(e => e.UseDept, UseDeptIdProperty);

        /// <summary>
        /// 使用部门
        /// </summary>
        public Enterprise UseDept
        {
            get { return GetRefEntity(UseDeptProperty); }
            set { SetRefEntity(UseDeptProperty, value); }
        }
        #endregion

        #region 车间 WorkShop
        /// <summary>
        /// 车间Id
        /// </summary>
        [Label("车间")]
        public static readonly IRefIdProperty WorkShopIdProperty = P<InventoryPlanEquipment>.RegisterRefId(e => e.WorkShopId, ReferenceType.Normal);

        /// <summary>
        /// 车间Id
        /// </summary>
        public double? WorkShopId
        {
            get { return (double?)GetRefNullableId(WorkShopIdProperty); }
            set { SetRefNullableId(WorkShopIdProperty, value); }
        }

        /// <summary>
        /// 车间
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> WorkShopProperty = P<InventoryPlanEquipment>.RegisterRef(e => e.WorkShop, WorkShopIdProperty);

        /// <summary>
        /// 车间
        /// </summary>
        public Enterprise WorkShop
        {
            get { return GetRefEntity(WorkShopProperty); }
            set { SetRefEntity(WorkShopProperty, value); }
        }
        #endregion

        #region 设备型号 EquipModel
        /// <summary>
        /// 设备型号Id
        /// </summary>
        [Label("设备型号")]
        public static readonly IRefIdProperty EquipModelIdProperty = P<InventoryPlanEquipment>.RegisterRefId(e => e.EquipModelId, ReferenceType.Normal);

        /// <summary>
        /// 设备型号Id
        /// </summary>
        public double? EquipModelId
        {
            get { return (double?)GetRefNullableId(EquipModelIdProperty); }
            set { SetRefNullableId(EquipModelIdProperty, value); }
        }

        /// <summary>
        /// 设备型号
        /// </summary>
        public static readonly RefEntityProperty<EquipModel> EquipModelProperty = P<InventoryPlanEquipment>.RegisterRef(e => e.EquipModel, EquipModelIdProperty);

        /// <summary>
        /// 设备型号
        /// </summary>
        public EquipModel EquipModel
        {
            get { return GetRefEntity(EquipModelProperty); }
            set { SetRefEntity(EquipModelProperty, value); }
        }
        #endregion

        #region 资产责任人 AssetOwner
        /// <summary>
        /// 资产责任人Id
        /// </summary>
        [Label("资产责任人")]
        public static readonly IRefIdProperty AssetOwnerIdProperty = P<InventoryPlanEquipment>.RegisterRefId(e => e.AssetOwnerId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Employee> AssetOwnerProperty = P<InventoryPlanEquipment>.RegisterRef(e => e.AssetOwner, AssetOwnerIdProperty);

        /// <summary>
        /// 资产责任人
        /// </summary>
        public Employee AssetOwner
        {
            get { return GetRefEntity(AssetOwnerProperty); }
            set { SetRefEntity(AssetOwnerProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 工厂id FactoryId
        /// <summary>
        /// 工厂id
        /// </summary>
        [Label("工厂id")]
        public static readonly Property<double> FactoryIdProperty = P<InventoryPlanEquipment>.RegisterView(e => e.FactoryId, p => p.InventoryPlan.FactoryId);

        /// <summary>
        /// 工厂id
        /// </summary>
        public double FactoryId
        {
            get { return this.GetProperty(FactoryIdProperty); }
        }
        #endregion

        #region 管理部门名称 ManageDeptName
        /// <summary>
        /// 管理部门名称
        /// </summary>
        [Label("管理部门")]
        public static readonly Property<string> ManageDeptNameProperty = P<InventoryPlanEquipment>.RegisterView(e => e.ManageDeptName, p => p.ManageDept.Name);

        /// <summary>
        /// 管理部门名称
        /// </summary>
        public string ManageDeptName
        {
            get { return this.GetProperty(ManageDeptNameProperty); }
            set { SetProperty(ManageDeptNameProperty, value); }
        }
        #endregion

        #region 管理部门编码 ManageDeptCode
        /// <summary>
        /// 管理部门编码
        /// </summary>
        [Label("管理部门编码")]
        public static readonly Property<string> ManageDeptCodeProperty = P<InventoryPlanEquipment>.RegisterView(e => e.ManageDeptCode, p => p.ManageDept.Code);

        /// <summary>
        /// 管理部门编码
        /// </summary>
        public string ManageDeptCode
        {
            get { return this.GetProperty(ManageDeptCodeProperty); }
            set { SetProperty(ManageDeptCodeProperty, value); }
        }
        #endregion

        #region 使用部门编码 UseDeptCode
        /// <summary>
        /// 使用部门编码
        /// </summary>
        [Label("使用部门编码")]
        public static readonly Property<string> UseDeptCodeProperty = P<InventoryPlanEquipment>.RegisterView(e => e.UseDeptCode, p => p.UseDept.Code);

        /// <summary>
        /// 使用部门编码
        /// </summary>
        public string UseDeptCode
        {
            get { return this.GetProperty(UseDeptCodeProperty); }
            set { SetProperty(UseDeptCodeProperty, value); }
        }
        #endregion

        #region 使用部门名称 UseDeptName
        /// <summary>
        /// 使用部门名称
        /// </summary>
        [Label("使用部门名称")]
        public static readonly Property<string> UseDeptNameProperty = P<InventoryPlanEquipment>.RegisterView(e => e.UseDeptName, p => p.UseDept.Name);

        /// <summary>
        /// 使用部门名称
        /// </summary>
        public string UseDeptName
        {
            get { return this.GetProperty(UseDeptNameProperty); }
        }
        #endregion

        #region 仓库名称 WarehouseName
        /// <summary>
        /// 仓库名称
        /// </summary>
        [Label("仓库名称")]
        public static readonly Property<string> WarehouseNameProperty = P<InventoryPlanEquipment>.RegisterView(e => e.WarehouseName, p => p.Warehouse.Name);

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WarehouseName
        {
            get { return this.GetProperty(WarehouseNameProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 盘点计划范围（设备） 实体配置
    /// </summary>
    internal class InventoryPlanEquipmentConfig : EntityConfig<InventoryPlanEquipment>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_INV_PLAN_EQP").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}