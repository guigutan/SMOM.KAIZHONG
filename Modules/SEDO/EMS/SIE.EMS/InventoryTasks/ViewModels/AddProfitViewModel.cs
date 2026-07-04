using SIE.Core.Enums;
using SIE.Domain;
using SIE.Equipments.EquipModels;
using SIE.Equipments.EquipTypes;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using SIE.Warehouses;
using System;

namespace SIE.EMS.InventoryTasks.ViewModels
{
    /// <summary>
    /// 新增盘盈 ViewModel
    /// </summary> 
    [Serializable, RootEntity]
    [Label("新增盘盈")]
    public class AddProfitViewModel : ViewModel
    {
        #region 盘点任务id InventoryTaskId
        /// <summary>
        /// 盘点任务id
        /// </summary>
        [Label("盘点任务id")]
        public static readonly Property<double> InventoryTaskIdProperty = P<AddProfitViewModel>.Register(e => e.InventoryTaskId);

        /// <summary>
        /// 盘点任务id
        /// </summary>
        public double InventoryTaskId
        {
            get { return this.GetProperty(InventoryTaskIdProperty); }
            set { this.SetProperty(InventoryTaskIdProperty, value); }
        }
        #endregion

        #region 工厂id FactoryId
        /// <summary>
        /// 工厂id
        /// </summary>
        [Label("工厂id")]
        public static readonly Property<double> FactoryIdProperty = P<AddProfitViewModel>.Register(e => e.FactoryId);

        /// <summary>
        /// 工厂id
        /// </summary>
        public double FactoryId
        {
            get { return this.GetProperty(FactoryIdProperty); }
            set { this.SetProperty(FactoryIdProperty, value); }
        }
        #endregion

        #region 界面设备状态 AddProfitUIState
        /// <summary>
        /// 界面设备状态
        /// </summary>
        [Label("界面设备状态")]
        public static readonly Property<AddProfitUIState> AddProfitUIStateProperty = P<AddProfitViewModel>.Register(e => e.AddProfitUIState);

        /// <summary>
        /// 界面设备状态
        /// </summary>
        public AddProfitUIState AddProfitUIState
        {
            get { return this.GetProperty(AddProfitUIStateProperty); }
            set { this.SetProperty(AddProfitUIStateProperty, value); }
        }
        #endregion

        #region 无设备编码 NoHaveCode
        /// <summary>
        /// 无设备编码
        /// </summary>
        [Label("无设备编码")]
        public static readonly Property<bool> NoHaveCodeProperty = P<AddProfitViewModel>.Register(e => e.NoHaveCode);

        /// <summary>
        /// 无设备编码
        /// </summary>
        public bool NoHaveCode
        {
            get { return this.GetProperty(NoHaveCodeProperty); }
            set { this.SetProperty(NoHaveCodeProperty, value); }
        }
        #endregion

        #region 设备编码 EquipmentCode
        /// <summary>
        /// 设备编码
        /// </summary>
        [Label("设备编码")]
        public static readonly Property<string> EquipmentCodeProperty = P<AddProfitViewModel>.Register(e => e.EquipmentCode);

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipmentCode
        {
            get { return GetProperty(EquipmentCodeProperty); }
            set { SetProperty(EquipmentCodeProperty, value); }
        }
        #endregion

        #region 设备名称 EquipmentName
        /// <summary>
        /// 设备名称
        /// </summary>
        [Label("设备名称")]
        public static readonly Property<string> EquipmentNameProperty = P<AddProfitViewModel>.Register(e => e.EquipmentName);

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipmentName
        {
            get { return this.GetProperty(EquipmentNameProperty); }
            set { this.SetProperty(EquipmentNameProperty, value); }
        }
        #endregion

        #region 管理状态 AccountUseState
        /// <summary>
        /// 管理状态
        /// </summary>
        [Label("管理状态")]
        public static readonly Property<AccountUseState?> AccountUseStateProperty = P<AddProfitViewModel>.Register(e => e.AccountUseState);

        /// <summary>
        /// 管理状态
        /// </summary>
        public AccountUseState? AccountUseState
        {
            get { return GetProperty(AccountUseStateProperty); }
            set { SetProperty(AccountUseStateProperty, value); }
        }
        #endregion

        #region 设备状态 AccountState
        /// <summary>
        /// 设备状态
        /// </summary>
        [Label("设备状态")]
        public static readonly Property<AccountState?> AccountStateProperty = P<AddProfitViewModel>.Register(e => e.AccountState);

        /// <summary>
        /// 设备状态
        /// </summary>
        public AccountState? AccountState
        {
            get { return GetProperty(AccountStateProperty); }
            set { SetProperty(AccountStateProperty, value); }
        }
        #endregion

        #region 任务管理部门 TaskManageDeptName
        /// <summary>
        /// 任务管理部门
        /// </summary>
        [Label("任务管理部门")]
        public static readonly Property<string> TaskManageDeptNameProperty = P<AddProfitViewModel>.Register(e => e.TaskManageDeptName);

        /// <summary>
        /// 任务管理部门
        /// </summary>
        public string TaskManageDeptName
        {
            get { return this.GetProperty(TaskManageDeptNameProperty); }
            set { this.SetProperty(TaskManageDeptNameProperty, value); }
        }
        #endregion

        #region 管理部门 ManageDeptName
        /// <summary>
        /// 管理部门
        /// </summary>
        [Label("管理部门")]
        public static readonly Property<string> ManageDeptNameProperty = P<AddProfitViewModel>.Register(e => e.ManageDeptName);

        /// <summary>
        /// 管理部门
        /// </summary>
        public string ManageDeptName
        {
            get { return this.GetProperty(ManageDeptNameProperty); }
            set { this.SetProperty(ManageDeptNameProperty, value); }
        }
        #endregion

        #region 设备型号 EquipModel
        /// <summary>
        /// 设备型号Id
        /// </summary>
        [Label("型号编码")]
        public static readonly IRefIdProperty EquipModelIdProperty = P<AddProfitViewModel>.RegisterRefId(e => e.EquipModelId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<EquipModel> EquipModelProperty = P<AddProfitViewModel>.RegisterRef(e => e.EquipModel, EquipModelIdProperty);

        /// <summary>
        /// 设备型号
        /// </summary>
        public EquipModel EquipModel
        {
            get { return GetRefEntity(EquipModelProperty); }
            set { SetRefEntity(EquipModelProperty, value); }
        }
        #endregion

        #region 型号名称 EquipModelName
        /// <summary>
        /// 型号名称
        /// </summary>
        [Label("型号名称")]
        public static readonly Property<string> EquipModelNameProperty = P<AddProfitViewModel>.Register(e => e.EquipModelName);

        /// <summary>
        /// 型号名称
        /// </summary>
        public string EquipModelName
        {
            get { return this.GetProperty(EquipModelNameProperty); }
            set { this.SetProperty(EquipModelNameProperty, value); }
        }
        #endregion

        #region 技术规格 Specifications
        /// <summary>
        /// 技术规格
        /// </summary>
        [Label("技术规格")]
        public static readonly Property<string> SpecificationsProperty = P<AddProfitViewModel>.Register(e => e.Specifications);

        /// <summary>
        /// 技术规格
        /// </summary>
        public string Specifications
        {
            get { return this.GetProperty(SpecificationsProperty); }
            set { this.SetProperty(SpecificationsProperty, value); }
        }
        #endregion

        #region 设备类型 EquipType
        /// <summary>
        /// 设备类型Id
        /// </summary>
        [Label("设备类型")]
        public static readonly IRefIdProperty EquipTypeIdProperty = P<AddProfitViewModel>.RegisterRefId(e => e.EquipTypeId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<EquipType> EquipTypeProperty = P<AddProfitViewModel>.RegisterRef(e => e.EquipType, EquipTypeIdProperty);

        /// <summary>
        /// 设备类型
        /// </summary>
        public EquipType EquipType
        {
            get { return GetRefEntity(EquipTypeProperty); }
            set { SetRefEntity(EquipTypeProperty, value); }
        }
        #endregion

        #region 设备类别 TypeCategory
        /// <summary>
        /// 设备类别
        /// </summary>
        [Label("设备类别")]
        public static readonly Property<string> TypeCategoryProperty = P<AddProfitViewModel>.Register(e => e.TypeCategory);

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
        public static readonly Property<string> UseLevelProperty = P<AddProfitViewModel>.Register(e => e.UseLevel);

        /// <summary>
        /// ABC分类
        /// </summary>
        public string UseLevel
        {
            get { return GetProperty(UseLevelProperty); }
            set { SetProperty(UseLevelProperty, value); }
        }
        #endregion

        #region 使用部门 UseDept
        /// <summary>
        /// 使用部门Id
        /// </summary>
        [Label("使用部门")]
        public static readonly IRefIdProperty UseDeptIdProperty = P<AddProfitViewModel>.RegisterRefId(e => e.UseDeptId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Enterprise> UseDeptProperty = P<AddProfitViewModel>.RegisterRef(e => e.UseDept, UseDeptIdProperty);

        /// <summary>
        /// 使用部门
        /// </summary>
        public Enterprise UseDept
        {
            get { return GetRefEntity(UseDeptProperty); }
            set { SetRefEntity(UseDeptProperty, value); }
        }
        #endregion

        #region 使用责任人 User
        /// <summary>
        /// 使用责任人Id
        /// </summary>
        [Label("使用责任人")]
        public static readonly IRefIdProperty UserIdProperty = P<AddProfitViewModel>.RegisterRefId(e => e.UserId, ReferenceType.Normal);

        /// <summary>
        /// 使用责任人Id
        /// </summary>
        public double? UserId
        {
            get { return (double?)GetRefNullableId(UserIdProperty); }
            set { SetRefNullableId(UserIdProperty, value); }
        }

        /// <summary>
        /// 使用责任人
        /// </summary>
        public static readonly RefEntityProperty<Employee> UserProperty = P<AddProfitViewModel>.RegisterRef(e => e.User, UserIdProperty);

        /// <summary>
        /// 使用责任人
        /// </summary>
        public Employee User
        {
            get { return GetRefEntity(UserProperty); }
            set { SetRefEntity(UserProperty, value); }
        }
        #endregion

        #region 车间 RealWorkShop
        /// <summary>
        /// 车间Id
        /// </summary>
        [Label("车间")]
        public static readonly IRefIdProperty RealWorkShopIdProperty = P<AddProfitViewModel>.RegisterRefId(e => e.RealWorkShopId, ReferenceType.Normal);

        /// <summary>
        /// 车间Id
        /// </summary>
        public double? RealWorkShopId
        {
            get { return (double?)GetRefNullableId(RealWorkShopIdProperty); }
            set { SetRefNullableId(RealWorkShopIdProperty, value); }
        }

        /// <summary>
        /// 车间
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> RealWorkShopProperty = P<AddProfitViewModel>.RegisterRef(e => e.RealWorkShop, RealWorkShopIdProperty);

        /// <summary>
        /// 车间
        /// </summary>
        public Enterprise RealWorkShop
        {
            get { return GetRefEntity(RealWorkShopProperty); }
            set { SetRefEntity(RealWorkShopProperty, value); }
        }
        #endregion

        #region 产线 RealResource
        /// <summary>
        /// 产线Id
        /// </summary>
        [Label("产线")]
        public static readonly IRefIdProperty RealResourceIdProperty = P<AddProfitViewModel>.RegisterRefId(e => e.RealResourceId, ReferenceType.Normal);

        /// <summary>
        /// 产线Id
        /// </summary>
        public double? RealResourceId
        {
            get { return (double?)GetRefNullableId(RealResourceIdProperty); }
            set { SetRefNullableId(RealResourceIdProperty, value); }
        }

        /// <summary>
        /// 产线
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> RealResourceProperty = P<AddProfitViewModel>.RegisterRef(e => e.RealResource, RealResourceIdProperty);

        /// <summary>
        /// 产线
        /// </summary>
        public Enterprise RealResource
        {
            get { return GetRefEntity(RealResourceProperty); }
            set { SetRefEntity(RealResourceProperty, value); }
        }
        #endregion

        #region 仓库 RealWarehouse
        /// <summary>
        /// 仓库Id
        /// </summary>
        [Label("仓库")]
        public static readonly IRefIdProperty RealWarehouseIdProperty = P<AddProfitViewModel>.RegisterRefId(e => e.RealWarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 仓库Id
        /// </summary>
        public double? RealWarehouseId
        {
            get { return (double?)GetRefNullableId(RealWarehouseIdProperty); }
            set { SetRefNullableId(RealWarehouseIdProperty, value); }
        }

        /// <summary>
        /// 仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> RealWarehouseProperty = P<AddProfitViewModel>.RegisterRef(e => e.RealWarehouse, RealWarehouseIdProperty);

        /// <summary>
        /// 仓库
        /// </summary>
        public Warehouse RealWarehouse
        {
            get { return GetRefEntity(RealWarehouseProperty); }
            set { SetRefEntity(RealWarehouseProperty, value); }
        }
        #endregion

        #region 库位 StorageLocation
        /// <summary>
        /// 库位Id
        /// </summary>
        [Label("库位")]
        public static readonly IRefIdProperty StorageLocationIdProperty = P<AddProfitViewModel>.RegisterRefId(e => e.StorageLocationId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<StorageLocation> StorageLocationProperty = P<AddProfitViewModel>.RegisterRef(e => e.StorageLocation, StorageLocationIdProperty);

        /// <summary>
        /// 库位
        /// </summary>
        public StorageLocation StorageLocation
        {
            get { return GetRefEntity(StorageLocationProperty); }
            set { SetRefEntity(StorageLocationProperty, value); }
        }
        #endregion

        #region 位置 RealLocation
        /// <summary>
        /// 位置
        /// </summary>
        [Label("位置")]
        public static readonly Property<string> RealLocationProperty = P<AddProfitViewModel>.Register(e => e.RealLocation);

        /// <summary>
        /// 位置
        /// </summary>
        public string RealLocation
        {
            get { return GetProperty(RealLocationProperty); }
            set { SetProperty(RealLocationProperty, value); }
        }
        #endregion

        #region 图片 PhotoFilePath
        /// <summary>
        /// 图片
        /// </summary>
        [Label("图片")]
        public static readonly Property<string> PhotoFilePathProperty = P<AddProfitViewModel>.Register(e => e.PhotoFilePath);

        /// <summary>
        /// 图片
        /// </summary>
        public string PhotoFilePath
        {
            get { return GetProperty(PhotoFilePathProperty); }
            set { SetProperty(PhotoFilePathProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 界面设备状态(值和逻辑按文档写的划分)
    /// </summary>
    public enum AddProfitUIState
    {
        /// <summary>
		/// A
		/// </summary>
		[Label("A")]
        A = 10,
        /// <summary>
        /// B
        /// </summary>
        [Label("B")]
        B = 20,
        /// <summary>
        /// C
        /// </summary>
        [Label("C")]
        C = 30,
    }
}
