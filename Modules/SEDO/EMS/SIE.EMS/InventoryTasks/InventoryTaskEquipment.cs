using SIE.Core.Enums;
using SIE.Domain;
using SIE.EMS.Enums;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.EquipModels;
using SIE.Equipments.EquipTypes;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using SIE.Warehouses;
using System;

namespace SIE.EMS.InventoryTasks
{
    /// <summary>
    /// 盘点任务设备清单
    /// </summary>
    [ChildEntity, Serializable]
    [Label("盘点任务设备清单")]
    public partial class InventoryTaskEquipment : DataEntity
    {
        #region 设备台账 EquipAccount
        /// <summary>
        /// 设备台账Id
        /// </summary>
        [Label("设备台账")]
        public static readonly IRefIdProperty EquipAccountIdProperty =
            P<InventoryTaskEquipment>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Normal);

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
            P<InventoryTaskEquipment>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

        /// <summary>
        /// 设备台账
        /// </summary>
        public EquipAccountSelect EquipAccount
        {
            get { return this.GetRefEntity(EquipAccountProperty); }
            set { this.SetRefEntity(EquipAccountProperty, value); }
        }
        #endregion

        #region 设备编码 EquipmentCode
        /// <summary>
        /// 设备编码
        /// </summary>
        [Label("设备编码")]
        public static readonly Property<string> EquipmentCodeProperty = P<InventoryTaskEquipment>.Register(e => e.EquipmentCode);

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
        public static readonly Property<string> EquipmentNameProperty = P<InventoryTaskEquipment>.Register(e => e.EquipmentName);

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipmentName
        {
            get { return this.GetProperty(EquipmentNameProperty); }
            set { this.SetProperty(EquipmentNameProperty, value); }
        }
        #endregion

        #region 位置 RealLocation
        /// <summary>
        /// 位置
        /// </summary>
        [Label("位置")]
        public static readonly Property<string> RealLocationProperty = P<InventoryTaskEquipment>.Register(e => e.RealLocation);

        /// <summary>
        /// 位置
        /// </summary>
        public string RealLocation
        {
            get { return GetProperty(RealLocationProperty); }
            set { SetProperty(RealLocationProperty, value); }
        }
        #endregion

        #region 盘点时间 InventoryDateTime
        /// <summary>
        /// 盘点时间
        /// </summary>
        [Label("盘点时间")]
        public static readonly Property<DateTime?> InventoryDateTimeProperty = P<InventoryTaskEquipment>.Register(e => e.InventoryDateTime);

        /// <summary>
        /// 盘点时间
        /// </summary>
        public DateTime? InventoryDateTime
        {
            get { return GetProperty(InventoryDateTimeProperty); }
            set { SetProperty(InventoryDateTimeProperty, value); }
        }
        #endregion

        #region 复盘时间 SecondDateTime
        /// <summary>
        /// 复盘时间
        /// </summary>
        [Label("复盘时间")]
        public static readonly Property<DateTime?> SecondDateTimeProperty = P<InventoryTaskEquipment>.Register(e => e.SecondDateTime);

        /// <summary>
        /// 复盘时间
        /// </summary>
        public DateTime? SecondDateTime
        {
            get { return GetProperty(SecondDateTimeProperty); }
            set { SetProperty(SecondDateTimeProperty, value); }
        }
        #endregion

        #region 设备别名 Alias
        /// <summary>
        /// 设备别名
        /// </summary>
        [Label("设备别名")]
        public static readonly Property<string> AliasProperty = P<InventoryTaskEquipment>.Register(e => e.Alias);

        /// <summary>
        /// 设备别名
        /// </summary>
        public string Alias
        {
            get { return GetProperty(AliasProperty); }
            set { SetProperty(AliasProperty, value); }
        }
        #endregion

        #region 图片 PhotoFileName
        /// <summary>
        /// 图片
        /// </summary>
        [Label("图片")]
        public static readonly Property<string> PhotoFileNameProperty = P<InventoryTaskEquipment>.Register(e => e.PhotoFileName);

        /// <summary>
        /// 图片
        /// </summary>
        public string PhotoFileName
        {
            get { return GetProperty(PhotoFileNameProperty); }
            set { SetProperty(PhotoFileNameProperty, value); }
        }
        #endregion

        #region 图片文件路径 PhotoFilePath
        /// <summary>
        /// 图片文件路径
        /// </summary>
        [Label("图片文件路径")]
        [MaxLength(1000)]
        public static readonly Property<string> PhotoFilePathProperty = P<InventoryTaskEquipment>.Register(e => e.PhotoFilePath);

        /// <summary>
        /// 图片文件路径
        /// </summary>
        public string PhotoFilePath
        {
            get { return GetProperty(PhotoFilePathProperty); }
            set { SetProperty(PhotoFilePathProperty, value); }
        }
        #endregion

        #region 盘点状态 InventoryStatus
        /// <summary>
        /// 盘点状态
        /// </summary>
        [Label("盘点状态")]
        public static readonly Property<InventoryStatus> InventoryStatusProperty = P<InventoryTaskEquipment>.Register(e => e.InventoryStatus);

        /// <summary>
        /// 盘点状态
        /// </summary>
        public InventoryStatus InventoryStatus
        {
            get { return GetProperty(InventoryStatusProperty); }
            set { SetProperty(InventoryStatusProperty, value); }
        }
        #endregion

        #region 管理部门 RealManageDept
        /// <summary>
        /// 管理部门Id
        /// </summary>
        [Label("管理部门")]
        public static readonly IRefIdProperty RealManageDeptIdProperty = P<InventoryTaskEquipment>.RegisterRefId(e => e.RealManageDeptId, ReferenceType.Normal);

        /// <summary>
        /// 管理部门Id
        /// </summary>
        public double? RealManageDeptId
        {
            get { return (double?)GetRefNullableId(RealManageDeptIdProperty); }
            set { SetRefNullableId(RealManageDeptIdProperty, value); }
        }

        /// <summary>
        /// 管理部门
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> RealManageDeptProperty = P<InventoryTaskEquipment>.RegisterRef(e => e.RealManageDept, RealManageDeptIdProperty);

        /// <summary>
        /// 管理部门
        /// </summary>
        public Enterprise RealManageDept
        {
            get { return GetRefEntity(RealManageDeptProperty); }
            set { SetRefEntity(RealManageDeptProperty, value); }
        }
        #endregion

        #region 使用部门 RealUseDept
        /// <summary>
        /// 使用部门Id
        /// </summary>
        [Label("使用部门")]
        public static readonly IRefIdProperty RealUseDeptIdProperty = P<InventoryTaskEquipment>.RegisterRefId(e => e.RealUseDeptId, ReferenceType.Normal);

        /// <summary>
        /// 使用部门Id
        /// </summary>
        public double? RealUseDeptId
        {
            get { return (double?)GetRefNullableId(RealUseDeptIdProperty); }
            set { SetRefNullableId(RealUseDeptIdProperty, value); }
        }

        /// <summary>
        /// 使用部门
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> RealUseDeptProperty = P<InventoryTaskEquipment>.RegisterRef(e => e.RealUseDept, RealUseDeptIdProperty);

        /// <summary>
        /// 使用部门
        /// </summary>
        public Enterprise RealUseDept
        {
            get { return GetRefEntity(RealUseDeptProperty); }
            set { SetRefEntity(RealUseDeptProperty, value); }
        }
        #endregion

        #region 车间 RealWorkShop
        /// <summary>
        /// 车间Id
        /// </summary>
        [Label("车间")]
        public static readonly IRefIdProperty RealWorkShopIdProperty = P<InventoryTaskEquipment>.RegisterRefId(e => e.RealWorkShopId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Enterprise> RealWorkShopProperty = P<InventoryTaskEquipment>.RegisterRef(e => e.RealWorkShop, RealWorkShopIdProperty);

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
        public static readonly IRefIdProperty RealResourceIdProperty = P<InventoryTaskEquipment>.RegisterRefId(e => e.RealResourceId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Enterprise> RealResourceProperty = P<InventoryTaskEquipment>.RegisterRef(e => e.RealResource, RealResourceIdProperty);

        /// <summary>
        /// 产线
        /// </summary>
        public Enterprise RealResource
        {
            get { return GetRefEntity(RealResourceProperty); }
            set { SetRefEntity(RealResourceProperty, value); }
        }
        #endregion

        #region 盘点人 FirstCounter
        /// <summary>
        /// 盘点人Id
        /// </summary>
        [Label("盘点人")]
        public static readonly IRefIdProperty FirstCounterIdProperty = P<InventoryTaskEquipment>.RegisterRefId(e => e.FirstCounterId, ReferenceType.Normal);

        /// <summary>
        /// 盘点人Id
        /// </summary>
        public double? FirstCounterId
        {
            get { return (double?)GetRefNullableId(FirstCounterIdProperty); }
            set { SetRefNullableId(FirstCounterIdProperty, value); }
        }

        /// <summary>
        /// 盘点人
        /// </summary>
        public static readonly RefEntityProperty<Employee> FirstCounterProperty = P<InventoryTaskEquipment>.RegisterRef(e => e.FirstCounter, FirstCounterIdProperty);

        /// <summary>
        /// 盘点人
        /// </summary>
        public Employee FirstCounter
        {
            get { return GetRefEntity(FirstCounterProperty); }
            set { SetRefEntity(FirstCounterProperty, value); }
        }
        #endregion

        #region 管理状态 AccountUseState
        /// <summary>
        /// 管理状态
        /// </summary>
        [Label("管理状态")]
        public static readonly Property<AccountUseState?> AccountUseStateProperty = P<InventoryTaskEquipment>.Register(e => e.AccountUseState);

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
        public static readonly Property<AccountState?> AccountStateProperty = P<InventoryTaskEquipment>.Register(e => e.AccountState);

        /// <summary>
        /// 设备状态
        /// </summary>
        public AccountState? AccountState
        {
            get { return GetProperty(AccountStateProperty); }
            set { SetProperty(AccountStateProperty, value); }
        }
        #endregion

        #region 复盘人 SecondCounter
        /// <summary>
        /// 复盘人Id
        /// </summary>
        [Label("复盘人")]
        public static readonly IRefIdProperty SecondCounterIdProperty = P<InventoryTaskEquipment>.RegisterRefId(e => e.SecondCounterId, ReferenceType.Normal);

        /// <summary>
        /// 复盘人Id
        /// </summary>
        public double? SecondCounterId
        {
            get { return (double?)GetRefNullableId(SecondCounterIdProperty); }
            set { SetRefNullableId(SecondCounterIdProperty, value); }
        }

        /// <summary>
        /// 复盘人
        /// </summary>
        public static readonly RefEntityProperty<Employee> SecondCounterProperty = P<InventoryTaskEquipment>.RegisterRef(e => e.SecondCounter, SecondCounterIdProperty);

        /// <summary>
        /// 复盘人
        /// </summary>
        public Employee SecondCounter
        {
            get { return GetRefEntity(SecondCounterProperty); }
            set { SetRefEntity(SecondCounterProperty, value); }
        }
        #endregion

        #region 使用责任人 User
        /// <summary>
        /// 使用责任人Id
        /// </summary>
        [Label("使用责任人")]
        public static readonly IRefIdProperty UserIdProperty = P<InventoryTaskEquipment>.RegisterRefId(e => e.UserId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Employee> UserProperty = P<InventoryTaskEquipment>.RegisterRef(e => e.User, UserIdProperty);

        /// <summary>
        /// 使用责任人
        /// </summary>
        public Employee User
        {
            get { return GetRefEntity(UserProperty); }
            set { SetRefEntity(UserProperty, value); }
        }
        #endregion

        #region 仓库 RealWarehouse
        /// <summary>
        /// 仓库Id
        /// </summary>
        [Label("仓库")]
        public static readonly IRefIdProperty RealWarehouseIdProperty = P<InventoryTaskEquipment>.RegisterRefId(e => e.RealWarehouseId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Warehouse> RealWarehouseProperty = P<InventoryTaskEquipment>.RegisterRef(e => e.RealWarehouse, RealWarehouseIdProperty);

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
        public static readonly IRefIdProperty StorageLocationIdProperty = P<InventoryTaskEquipment>.RegisterRefId(e => e.StorageLocationId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<StorageLocation> StorageLocationProperty = P<InventoryTaskEquipment>.RegisterRef(e => e.StorageLocation, StorageLocationIdProperty);

        /// <summary>
        /// 库位
        /// </summary>
        public StorageLocation StorageLocation
        {
            get { return GetRefEntity(StorageLocationProperty); }
            set { SetRefEntity(StorageLocationProperty, value); }
        }
        #endregion

        #region 设备型号 EquipModel
        /// <summary>
        /// 设备型号Id
        /// </summary>
        [Label("设备型号")]
        public static readonly IRefIdProperty EquipModelIdProperty = P<InventoryTaskEquipment>.RegisterRefId(e => e.EquipModelId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<EquipModel> EquipModelProperty = P<InventoryTaskEquipment>.RegisterRef(e => e.EquipModel, EquipModelIdProperty);

        /// <summary>
        /// 设备型号
        /// </summary>
        public EquipModel EquipModel
        {
            get { return GetRefEntity(EquipModelProperty); }
            set { SetRefEntity(EquipModelProperty, value); }
        }
        #endregion

        #region 设备类型 EquipType
        /// <summary>
        /// 设备类型Id
        /// </summary>
        [Label("设备类型")]
        public static readonly IRefIdProperty EquipTypeIdProperty = P<InventoryTaskEquipment>.RegisterRefId(e => e.EquipTypeId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<EquipType> EquipTypeProperty = P<InventoryTaskEquipment>.RegisterRef(e => e.EquipType, EquipTypeIdProperty);

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
        public static readonly Property<string> TypeCategoryProperty = P<InventoryTaskEquipment>.Register(e => e.TypeCategory);

        /// <summary>
        /// 设备类别
        /// </summary>
        public string TypeCategory
        {
            get { return GetProperty(TypeCategoryProperty); }
            set { SetProperty(TypeCategoryProperty, value); }
        }
        #endregion

        #region 盘点资产来源 InventoryAssetSource
        /// <summary>
        /// 盘点资产来源
        /// </summary>
        [Label("盘点资产来源")]
        public static readonly Property<InventoryAssetSource> InventoryAssetSourceProperty = P<InventoryTaskEquipment>.Register(e => e.InventoryAssetSource);

        /// <summary>
        /// 盘点资产来源
        /// </summary>
        public InventoryAssetSource InventoryAssetSource
        {
            get { return GetProperty(InventoryAssetSourceProperty); }
            set { SetProperty(InventoryAssetSourceProperty, value); }
        }
        #endregion

        #region 盘点任务 InventoryTask
        /// <summary>
        /// 盘点任务Id
        /// </summary>
        [Label("盘点任务")]
        public static readonly IRefIdProperty InventoryTaskIdProperty = P<InventoryTaskEquipment>.RegisterRefId(e => e.InventoryTaskId, ReferenceType.Parent);

        /// <summary>
        /// 盘点任务Id
        /// </summary>
        public double InventoryTaskId
        {
            get { return (double)GetRefId(InventoryTaskIdProperty); }
            set { SetRefId(InventoryTaskIdProperty, value); }
        }

        /// <summary>
        /// 盘点任务
        /// </summary>
        public static readonly RefEntityProperty<InventoryTask> InventoryTaskProperty = P<InventoryTaskEquipment>.RegisterRef(e => e.InventoryTask, InventoryTaskIdProperty);

        /// <summary>
        /// 盘点任务
        /// </summary>
        public InventoryTask InventoryTask
        {
            get { return GetRefEntity(InventoryTaskProperty); }
            set { SetRefEntity(InventoryTaskProperty, value); }
        }
        #endregion

        #region 初盘结果 FirstInventoryResult
        /// <summary>
        /// 初盘结果
        /// </summary>
        [Label("初盘结果")]
        public static readonly Property<InventoryResult?> FirstInventoryResultProperty = P<InventoryTaskEquipment>.Register(e => e.FirstInventoryResult);

        /// <summary>
        /// 初盘结果
        /// </summary>
        public InventoryResult? FirstInventoryResult
        {
            get { return this.GetProperty(FirstInventoryResultProperty); }
            set { this.SetProperty(FirstInventoryResultProperty, value); }
        }
        #endregion

        #region 复盘结果 SecondInventoryResult
        /// <summary>
        /// 复盘结果
        /// </summary>
        [Label("复盘结果")]
        public static readonly Property<InventoryResult?> SecondInventoryResultProperty = P<InventoryTaskEquipment>.Register(e => e.SecondInventoryResult);

        /// <summary>
        /// 复盘结果
        /// </summary>
        public InventoryResult? SecondInventoryResult
        {
            get { return this.GetProperty(SecondInventoryResultProperty); }
            set { this.SetProperty(SecondInventoryResultProperty, value); }
        }
        #endregion

        #region 建议处理方式 SuggestProcessMethod
        /// <summary>
        /// 建议处理方式
        /// </summary>
        [Label("建议处理方式")]
        public static readonly Property<InventoryProcessMethod?> SuggestProcessMethodProperty = P<InventoryTaskEquipment>.Register(e => e.SuggestProcessMethod);

        /// <summary>
        /// 建议处理方式
        /// </summary>
        public InventoryProcessMethod? SuggestProcessMethod
        {
            get { return GetProperty(SuggestProcessMethodProperty); }
            set { SetProperty(SuggestProcessMethodProperty, value); }
        }
        #endregion


        #region 库位编码 StorageLocationCode
        /// <summary>
        /// 库位编码
        /// </summary>
        [Label("属性名")]
        public static readonly Property<string> StorageLocationCodeProperty = P<InventoryTaskEquipment>.Register(e => e.StorageLocationCode);

        /// <summary>
        /// 库位编码
        /// </summary>
        public string StorageLocationCode
        {
            get { return this.GetProperty(StorageLocationCodeProperty); }
            set { this.SetProperty(StorageLocationCodeProperty, value); }
        }
        #endregion


        #region 平账方式 InventoryProcessMethod
        /// <summary>
        /// 平账方式
        /// </summary>
        [Label("平账方式")]
        public static readonly Property<InventoryProcessMethod?> InventoryProcessMethodProperty = P<InventoryTaskEquipment>.Register(e => e.InventoryProcessMethod);

        /// <summary>
        /// 平账方式
        /// </summary>
        public InventoryProcessMethod? InventoryProcessMethod
        {
            get { return GetProperty(InventoryProcessMethodProperty); }
            set { SetProperty(InventoryProcessMethodProperty, value); }
        }
        #endregion

        #region 视图属性(原始设备台账)
        #region 管理部门 OldManageDeptId
        /// <summary>
        /// 管理部门
        /// </summary>
        [Label("管理部门")]
        public static readonly Property<double?> OldManageDeptIdProperty = P<InventoryTaskEquipment>.Register(e => e.OldManageDeptId);

        /// <summary>
        /// 管理部门
        /// </summary>
        public double? OldManageDeptId
        {
            get { return this.GetProperty(OldManageDeptIdProperty); }
            set { SetProperty(OldManageDeptIdProperty, value); }
        }
        #endregion

        #region 管理部门名称 OldManageDept
        /// <summary>
        /// 管理部门名称
        /// </summary>
        [Label("管理部门名称")]
        public static readonly Property<string> OldManageDeptProperty = P<InventoryTaskEquipment>.Register(e => e.OldManageDept);

        /// <summary>
        /// 管理部门名称
        /// </summary>
        public string OldManageDept
        {
            get { return this.GetProperty(OldManageDeptProperty); }
            set { SetProperty(OldManageDeptProperty, value); }
        }
        #endregion

        #region 使用部门 OldUseDeptId
        /// <summary>
        /// 使用部门
        /// </summary>
        [Label("使用部门")]
        public static readonly Property<double?> OldUseDeptIdProperty = P<InventoryTaskEquipment>.Register(e => e.OldUseDeptId);

        /// <summary>
        /// 使用部门
        /// </summary>
        public double? OldUseDeptId
        {
            get { return this.GetProperty(OldUseDeptIdProperty); }
            set { SetProperty(OldUseDeptIdProperty, value); }
        }
        #endregion

        #region 使用部门 OldUseDeptName
        /// <summary>
        /// 使用部门
        /// </summary>
        [Label("使用部门")]
        public static readonly Property<string> OldUseDeptNameProperty = P<InventoryTaskEquipment>.Register(e => e.OldUseDeptName);

        /// <summary>
        /// 使用部门
        /// </summary>
        public string OldUseDeptName
        {
            get { return this.GetProperty(OldUseDeptNameProperty); }
            set { SetProperty(OldUseDeptNameProperty, value); }
        }
        #endregion

        #region 管理状态 OldAccountUseState
        /// <summary>
        /// 管理状态
        /// </summary>
        [Label("管理状态")]
        public static readonly Property<AccountUseState?> OldAccountUseStateProperty = P<InventoryTaskEquipment>
            .Register(e => e.OldAccountUseState);

        /// <summary>
        /// 管理状态
        /// </summary>
        public AccountUseState? OldAccountUseState
        {
            get { return this.GetProperty(OldAccountUseStateProperty); }
            set { SetProperty(OldAccountUseStateProperty, value); }
        }
        #endregion

        #region 设备状态 OldAccountState
        /// <summary>
        /// 设备状态
        /// </summary>
        [Label("设备状态")]
        public static readonly Property<AccountState?> OldAccountStateProperty = P<InventoryTaskEquipment>.Register(e => e.OldAccountState);

        /// <summary>
        /// 设备状态
        /// </summary>
        public AccountState? OldAccountState
        {
            get { return this.GetProperty(OldAccountStateProperty); }
            set { SetProperty(OldAccountStateProperty, value); }
        }
        #endregion

        #region 使用责任人 OldUserId
        /// <summary>
        /// 使用责任人
        /// </summary>
        [Label("使用责任人")]
        public static readonly Property<double?> OldUserIdProperty = P<InventoryTaskEquipment>.Register(e => e.OldUserId);

        /// <summary>
        /// 使用责任人
        /// </summary>
        public double? OldUserId
        {
            get { return this.GetProperty(OldUserIdProperty); }
            set { SetProperty(OldUserIdProperty, value); }
        }
        #endregion

        #region 使用责任人 OldUserName
        /// <summary>
        /// 使用责任人
        /// </summary>
        [Label("使用责任人")]
        public static readonly Property<string> OldUserNameProperty = P<InventoryTaskEquipment>.Register(e => e.OldUserName);

        /// <summary>
        /// 使用责任人
        /// </summary>
        public string OldUserName
        {
            get { return this.GetProperty(OldUserNameProperty); }
            set { SetProperty(OldUserNameProperty, value); }
        }
        #endregion

        #region 车间 OldWorkShopId
        /// <summary>
        /// 车间
        /// </summary>
        [Label("车间")]
        public static readonly Property<double?> OldWorkShopIdProperty = P<InventoryTaskEquipment>.Register(e => e.OldWorkShopId);

        /// <summary>
        /// 车间
        /// </summary>
        public double? OldWorkShopId
        {
            get { return this.GetProperty(OldWorkShopIdProperty); }
            set { SetProperty(OldWorkShopIdProperty, value); }
        }
        #endregion

        #region 车间 OldWorkShopName
        /// <summary>
        /// 车间
        /// </summary>
        [Label("车间")]
        public static readonly Property<string> OldWorkShopNameProperty = P<InventoryTaskEquipment>.Register(e => e.OldWorkShopName);

        /// <summary>
        /// 车间
        /// </summary>
        public string OldWorkShopName
        {
            get { return this.GetProperty(OldWorkShopNameProperty); }
            set { SetProperty(OldWorkShopNameProperty, value); }
        }
        #endregion

        #region 产线 OldResourceId
        /// <summary>
        /// 产线
        /// </summary>
        [Label("产线")]
        public static readonly Property<double?> OldResourceIdProperty = P<InventoryTaskEquipment>.Register(e => e.OldResourceId);

        /// <summary>
        /// 产线
        /// </summary>
        public double? OldResourceId
        {
            get { return this.GetProperty(OldResourceIdProperty); }
            set { SetProperty(OldResourceIdProperty, value); }
        }
        #endregion

        #region 产线 OldResourceName
        /// <summary>
        /// 产线
        /// </summary>
        [Label("产线")]
        public static readonly Property<string> OldResourceNameProperty = P<InventoryTaskEquipment>.Register(e => e.OldResourceName);

        /// <summary>
        /// 产线
        /// </summary>
        public string OldResourceName
        {
            get { return this.GetProperty(OldResourceNameProperty); }
            set { SetProperty(OldResourceNameProperty, value); }
        }
        #endregion

        #region 仓库 OldWarehouseId
        /// <summary>
        /// 仓库
        /// </summary>
        [Label("仓库")]
        public static readonly Property<double?> OldWarehouseIdProperty = P<InventoryTaskEquipment>.Register(e => e.OldWarehouseId);

        /// <summary>
        /// 仓库
        /// </summary>
        public double? OldWarehouseId
        {
            get { return this.GetProperty(OldWarehouseIdProperty); }
            set { SetProperty(OldWarehouseIdProperty, value); }
        }
        #endregion

        #region 仓库名称 OldWarehouseCode
        /// <summary>
        /// 仓库名称
        /// </summary>
        [Label("仓库名称")]
        public static readonly Property<string> OldWarehouseCodeProperty = P<InventoryTaskEquipment>.Register(e => e.OldWarehouseCode);

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string OldWarehouseCode
        {
            get { return this.GetProperty(OldWarehouseCodeProperty); }
            set { SetProperty(OldWarehouseCodeProperty, value); }
        }
        #endregion

        #region 库位 OldStorageLocationId
        /// <summary>
        /// 库位
        /// </summary>
        [Label("库位")]
        public static readonly Property<double?> OldStorageLocationIdProperty = P<InventoryTaskEquipment>.Register(e => e.OldStorageLocationId);

        /// <summary>
        /// 库位
        /// </summary>
        public double? OldStorageLocationId
        {
            get { return this.GetProperty(OldStorageLocationIdProperty); }
            set { SetProperty(OldStorageLocationIdProperty, value); }
        }
        #endregion

        #region 库位 OldStorageLocationCode
        /// <summary>
        /// 库位
        /// </summary>
        [Label("库位")]
        public static readonly Property<string> OldStorageLocationCodeProperty = P<InventoryTaskEquipment>.Register(e => e.OldStorageLocationCode);

        /// <summary>
        /// 库位
        /// </summary>
        public string OldStorageLocationCode
        {
            get { return this.GetProperty(OldStorageLocationCodeProperty); }
            set { SetProperty(OldStorageLocationCodeProperty, value); }
        }
        #endregion

        #region 位置 OldLocation
        /// <summary>
        /// 位置
        /// </summary>
        [Label("位置")]
        public static readonly Property<string> OldLocationProperty = P<InventoryTaskEquipment>.Register(e => e.OldLocation);

        /// <summary>
        /// 位置
        /// </summary>
        public string OldLocation
        {
            get { return this.GetProperty(OldLocationProperty); }
            set { SetProperty(OldLocationProperty, value); }
        }
        #endregion
        #endregion

        #region 视图属性(实盘字段)
        #region 管理部门 RealManageDeptName
        /// <summary>
        /// 管理部门
        /// </summary>
        [Label("管理部门")]
        public static readonly Property<string> RealManageDeptNameProperty = P<InventoryTaskEquipment>.RegisterView(e => e.RealManageDeptName, p => p.RealManageDept.Name);

        /// <summary>
        /// 管理部门
        /// </summary>
        public string RealManageDeptName
        {
            get { return this.GetProperty(RealManageDeptNameProperty); }
        }
        #endregion

        #region 使用部门 RealUseDeptName
        /// <summary>
        /// 使用部门
        /// </summary>
        [Label("使用部门")]
        public static readonly Property<string> RealUseDeptNameProperty = P<InventoryTaskEquipment>.RegisterView(e => e.RealUseDeptName, p => p.RealUseDept.Name);

        /// <summary>
        /// 使用部门
        /// </summary>
        public string RealUseDeptName
        {
            get { return this.GetProperty(RealUseDeptNameProperty); }
        }
        #endregion

        #region 使用责任人 UserName
        /// <summary>
        /// 使用责任人
        /// </summary>
        [Label("使用责任人")]
        public static readonly Property<string> UserNameProperty = P<InventoryTaskEquipment>.RegisterView(e => e.UserName, p => p.User.Name);

        /// <summary>
        /// 使用责任人
        /// </summary>
        public string UserName
        {
            get { return this.GetProperty(UserNameProperty); }
        }
        #endregion

        #region 使用责任人编码 UserCode
        /// <summary>
        /// 使用责任人编码
        /// </summary>
        [Label("使用责任人编码")]
        public static readonly Property<string> UserCodeProperty = P<InventoryTaskEquipment>.RegisterView(e => e.UserCode, p => p.User.Code);

        /// <summary>
        /// 使用责任人编码
        /// </summary>
        public string UserCode
        {
            get { return this.GetProperty(UserCodeProperty); }
        }
        #endregion

        #region 车间名称 RealWorkShopName
        /// <summary>
        /// 车间名称
        /// </summary>
        [Label("车间名称")]
        public static readonly Property<string> RealWorkShopNameProperty = P<InventoryTaskEquipment>.RegisterView(e => e.RealWorkShopName, p => p.RealWorkShop.Name);

        /// <summary>
        /// 车间名称
        /// </summary>
        public string RealWorkShopName
        {
            get { return this.GetProperty(RealWorkShopNameProperty); }
        }
        #endregion

        #region 产线名称 RealResourceName
        /// <summary>
        /// 产线名称
        /// </summary>
        [Label("产线名称")]
        public static readonly Property<string> RealResourceNameProperty = P<InventoryTaskEquipment>.RegisterView(e => e.RealResourceName, p => p.RealResource.Name);

        /// <summary>
        /// 产线名称
        /// </summary>
        public string RealResourceName
        {
            get { return this.GetProperty(RealResourceNameProperty); }
        }
        #endregion

        #region 仓库名称 RealWarehouseName
        /// <summary>
        /// 仓库名称
        /// </summary>
        [Label("仓库名称")]
        public static readonly Property<string> RealWarehouseNameProperty = P<InventoryTaskEquipment>.RegisterView(e => e.RealWarehouseName, p => p.RealWarehouse.Name);

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string RealWarehouseName
        {
            get { return this.GetProperty(RealWarehouseNameProperty); }
        }
        #endregion

        #region 库位名称 StorageLocationName
        /// <summary>
        /// 库位名称
        /// </summary>
        [Label("库位名称")]
        public static readonly Property<string> StorageLocationNameProperty = P<InventoryTaskEquipment>.RegisterView(e => e.StorageLocationName, p => p.StorageLocation.Name);

        /// <summary>
        /// 库位名称
        /// </summary>
        public string StorageLocationName
        {
            get { return this.GetProperty(StorageLocationNameProperty); }
        }
        #endregion
        #endregion

        #region 视图属性
        #region 型号名称 EquipModelName
        /// <summary>
        /// 型号名称
        /// </summary>
        [Label("型号名称")]
        public static readonly Property<string> EquipModelNameProperty = P<InventoryTaskEquipment>.RegisterView(e => e.EquipModelName, p => p.EquipModel.Name);

        /// <summary>
        /// 型号名称
        /// </summary>
        public string EquipModelName
        {
            get { return this.GetProperty(EquipModelNameProperty); }
        }
        #endregion

        #region 型号规格 ModelSpecifications
        /// <summary>
        /// 型号规格
        /// </summary>
        [Label("型号规格")]
        public static readonly Property<string> ModelSpecificationsProperty = P<InventoryTaskEquipment>.RegisterView(e => e.ModelSpecifications, p => p.EquipModel.Specifications);

        /// <summary>
        /// 型号规格
        /// </summary>
        public string ModelSpecifications
        {
            get { return this.GetProperty(ModelSpecificationsProperty); }
        }
        #endregion

        #region 是否固定资产 IssAsset
        /// <summary>
        /// 是否固定资产
        /// </summary>
        [Label("是否固定资产")]
        public static readonly Property<bool> IssAssetProperty = P<InventoryTaskEquipment>.RegisterView(e => e.IssAsset, p => p.EquipAccount.IssAsset);

        /// <summary>
        /// 是否固定资产
        /// </summary>
        public bool IssAsset
        {
            get { return this.GetProperty(IssAssetProperty); }
        }
        #endregion

        #region 设备RFID EqRFID
        /// <summary>
        /// 设备RFID
        /// </summary>
        [Label("设备RFID")]
        public static readonly Property<string> EqRFIDProperty = P<InventoryTaskEquipment>.RegisterView(e => e.EqRFID, p => p.EquipAccount.RFID);

        /// <summary>
        /// 设备RFID
        /// </summary>
        public string EqRFID
        {
            get { return this.GetProperty(EqRFIDProperty); }
        }
        #endregion

        #region 固定资产编码 FixedAssetsAccountCode
        /// <summary>
        /// 固定资产编码
        /// </summary>
        [Label("固定资产编码")]
        public static readonly Property<string> FixedAssetsAccountCodeProperty = P<InventoryTaskEquipment>.RegisterView(e => e.FixedAssetsAccountCode, p => p.EquipAccount.FixedAssetsAccount.Code);

        /// <summary>
        /// 固定资产编码
        /// </summary>
        public string FixedAssetsAccountCode
        {
            get { return this.GetProperty(FixedAssetsAccountCodeProperty); }
        }
        #endregion

        #region 固定资产名称 FixedAssetsAccountName
        /// <summary>
        /// 固定资产名称
        /// </summary>
        [Label("固定资产名称")]
        public static readonly Property<string> FixedAssetsAccountNameProperty = P<InventoryTaskEquipment>.RegisterView(e => e.FixedAssetsAccountName, p => p.EquipAccount.FixedAssetsAccount.Name);

        /// <summary>
        /// 固定资产名称
        /// </summary>
        public string FixedAssetsAccountName
        {
            get { return this.GetProperty(FixedAssetsAccountNameProperty); }
        }
        #endregion

        #region 固定资产净值 NetAssetValue
        /// <summary>
        /// 固定资产净值
        /// </summary>
        [Label("固定资产净值")]
        public static readonly Property<decimal> NetAssetValueProperty = P<InventoryTaskEquipment>.RegisterView(e => e.NetAssetValue, p => p.EquipAccount.FixedAssetsAccount.NetAssetValue);

        /// <summary>
        /// 固定资产净值
        /// </summary>
        public decimal NetAssetValue
        {
            get { return this.GetProperty(NetAssetValueProperty); }
        }
        #endregion

        #region 盘点任务状态 InventoryTaskStatus
        /// <summary>
        /// 盘点任务状态
        /// </summary>
        [Label("盘点任务状态")]
        public static readonly Property<InventoryTaskStatus> InventoryTaskStatusProperty = P<InventoryTaskEquipment>.RegisterView(e => e.InventoryTaskStatus, p => p.InventoryTask.InventoryTaskStatus);

        /// <summary>
        /// 盘点任务状态
        /// </summary>
        public InventoryTaskStatus InventoryTaskStatus
        {
            get { return this.GetProperty(InventoryTaskStatusProperty); }
        }
        #endregion

        #region 工厂id FactoryId
        /// <summary>
        /// 工厂id
        /// </summary>
        [Label("工厂id")]
        public static readonly Property<double> FactoryIdProperty = P<InventoryTaskEquipment>.RegisterView(e => e.FactoryId, p => p.InventoryTask.FactoryId);

        /// <summary>
        /// 工厂id
        /// </summary>
        public double FactoryId
        {
            get { return this.GetProperty(FactoryIdProperty); }
        }
        #endregion

        #region 盘点计划id InventoryPlanId
        /// <summary>
        /// 盘点计划id
        /// </summary>
        [Label("盘点计划id")]
        public static readonly Property<double> InventoryPlanIdProperty = P<InventoryTaskEquipment>.RegisterView(e => e.InventoryPlanId, p => p.InventoryTask.InventoryPlanId);

        /// <summary>
        /// 盘点计划id
        /// </summary>
        public double InventoryPlanId
        {
            get { return this.GetProperty(InventoryPlanIdProperty); }
        }
        #endregion

        #region 设备台账保管人 AdministratorId
        /// <summary>
        /// 设备台账保管人
        /// </summary>
        [Label("设备台账保管人")]
        public static readonly Property<double?> AdministratorIdProperty = P<InventoryTaskEquipment>.RegisterView(e => e.AdministratorId, p => p.EquipAccount.AdministratorId);

        /// <summary>
        /// 设备台账保管人
        /// </summary>
        public double? AdministratorId
        {
            get { return this.GetProperty(AdministratorIdProperty); }
        }
        #endregion

        #region 盘点结果（导入） ImportResult
        /// <summary>
        /// 盘点结果（导入）
        /// </summary>
        [Label("盘点结果")]
        public static readonly Property<InventoryResult?> ImportResultProperty = P<InventoryTaskEquipment>.Register(e => e.ImportResult);

        /// <summary>
        /// 盘点结果（导入）
        /// </summary>
        public InventoryResult? ImportResult
        {
            get { return this.GetProperty(ImportResultProperty); }
            set { this.SetProperty(ImportResultProperty, value); }
        }
        #endregion

        #region 有初盘权限(界面属性) FirstPower
        /// <summary>
        /// 有初盘权限
        /// </summary>
        [Label("有初盘权限")]
        public static readonly Property<bool?> FirstPowerProperty = P<InventoryTaskEquipment>.Register(e => e.FirstPower);

        /// <summary>
        /// 有初盘权限
        /// </summary>
        public bool? FirstPower
        {
            get { return this.GetProperty(FirstPowerProperty); }
            set { this.SetProperty(FirstPowerProperty, value); }
        }
        #endregion

        #region 有复盘权限(界面属性) SecondPower
        /// <summary>
        /// 有复盘权限
        /// </summary>
        [Label("有复盘权限")]
        public static readonly Property<bool?> SecondPowerProperty = P<InventoryTaskEquipment>.Register(e => e.SecondPower);

        /// <summary>
        /// 有复盘权限
        /// </summary>
        public bool? SecondPower
        {
            get { return this.GetProperty(SecondPowerProperty); }
            set { this.SetProperty(SecondPowerProperty, value); }
        }
        #endregion

        #region 审核状态 ApprovalStatus
        /// <summary>
        /// 审核状态
        /// </summary>
        [Label("审核状态")]
        public static readonly Property<ApprovalStatus> ApprovalStatusProperty = P<InventoryTaskEquipment>.RegisterView(e => e.ApprovalStatus, p => p.InventoryTask.ApprovalStatus);

        /// <summary>
        /// 审核状态
        /// </summary>
        public ApprovalStatus ApprovalStatus
        {
            get { return this.GetProperty(ApprovalStatusProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 盘点任务设备清单 实体配置
    /// </summary>
    internal class InventoryTaskEquipmentConfig : EntityConfig<InventoryTaskEquipment>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_INV_TASK_EQP").MapAllProperties();
            Meta.Property(InventoryTaskEquipment.PhotoFilePathProperty).ColumnMeta.HasLength(2000);
            Meta.Property(InventoryTaskEquipment.ImportResultProperty).DontMapColumn();
            Meta.Property(InventoryTaskEquipment.FirstPowerProperty).DontMapColumn();
            Meta.Property(InventoryTaskEquipment.SecondPowerProperty).DontMapColumn();
            Meta.Property(InventoryTaskEquipment.StorageLocationCodeProperty).DontMapColumn();
            Meta.EnablePhantoms();
        }
    }
}