using SIE.Common.Configs;
using SIE.Domain;
using SIE.ManagedProperty;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Warehouses.Configs;
using System;

namespace SIE.Warehouses
{
    /// <summary>
    /// 库位
    /// </summary>
    [RootEntity, Serializable]
    [EntityWithConfig(typeof(StorageLocationCodeConfig))]
    [ConditionQueryType(typeof(StorageLocationCriteria))]
    [Label("库位")]
    [DisplayMember(nameof(Code))]
    public partial class StorageLocation : DataEntity, IStateEntity
    {
        /// <summary>
        /// 立体库位编码前缀
        /// </summary>
        public const string AutomatedPrefix = "Cell";

        /// <summary>
        /// 构造函数
        /// </summary>
        public StorageLocation() { State = State.Enable; RowNo = 0; ColumnNo = 0; LayerNo = 0; Depth = 0; }

        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Required]
        [Label("编码")]
        [MaxLength(80)]
        public static readonly Property<string> CodeProperty = P<StorageLocation>.Register(e => e.Code);

        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 名称 Name
        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [Label("名称")]
        [MaxLength(80)]
        public static readonly Property<string> NameProperty = P<StorageLocation>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 是否冻结 IsFrozen
        /// <summary>
        /// 是否冻结
        /// </summary>
        [Label("是否冻结")]
        public static readonly Property<bool> IsFrozenProperty = P<StorageLocation>.Register(e => e.IsFrozen);

        /// <summary>
        /// 是否冻结
        /// </summary>
        public bool IsFrozen
        {
            get { return GetProperty(IsFrozenProperty); }
            set { SetProperty(IsFrozenProperty, value); }
        }
        #endregion

        #region 对应ERP库存组织 ErpInvOrg
        /// <summary>
        /// 对应ERP库存组织
        /// </summary>
        [Label("对应ERP库存组织")]
        [MaxLength(80)]
        public static readonly Property<string> ErpInvOrgProperty = P<StorageLocation>.Register(e => e.ErpInvOrg);

        /// <summary>
        /// 对应ERP库存组织
        /// </summary>
        public string ErpInvOrg
        {
            get { return GetProperty(ErpInvOrgProperty); }
            set { SetProperty(ErpInvOrgProperty, value); }
        }
        #endregion

        #region 对应ERP子库 ErpSubLibrary
        /// <summary>
        /// 对应ERP子库
        /// </summary>
        [Label("对应ERP子库")]
        [MaxLength(80)]
        public static readonly Property<string> ErpSubLibraryProperty = P<StorageLocation>.Register(e => e.ErpSubLibrary);

        /// <summary>
        /// 对应ERP子库
        /// </summary>
        public string ErpSubLibrary
        {
            get { return GetProperty(ErpSubLibraryProperty); }
            set { SetProperty(ErpSubLibraryProperty, value); }
        }
        #endregion

        #region 对应ERP库位 ErpLocation
        /// <summary>
        /// 对应ERP库位
        /// </summary>
        [Label("对应ERP库位")]
        [MaxLength(80)]
        public static readonly Property<string> ErpLocationProperty = P<StorageLocation>.Register(e => e.ErpLocation);

        /// <summary>
        /// 对应ERP库位
        /// </summary>
        public string ErpLocation
        {
            get { return GetProperty(ErpLocationProperty); }
            set { SetProperty(ErpLocationProperty, value); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<State> StateProperty = P<StorageLocation>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public State State
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }
        #endregion

        #region 仓库 Warehouse
        /// <summary>
        /// 仓库Id
        /// </summary>
        [Label("仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty = P<StorageLocation>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 仓库Id
        /// </summary>
        public double WarehouseId
        {
            get { return (double)GetRefId(WarehouseIdProperty); }
            set { SetRefId(WarehouseIdProperty, value); }
        }

        /// <summary>
        /// 仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<StorageLocation>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return GetRefEntity(WarehouseProperty); }
            set { SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 类型 LibraryType
        /// <summary>
        /// 类型
        /// </summary>
        [Label("类型")]
        public static readonly Property<LibraryType> LibraryTypeProperty = P<StorageLocation>.Register(e => e.LibraryType);

        /// <summary>
        /// 类型
        /// </summary>
        public LibraryType LibraryType
        {
            get { return GetProperty(LibraryTypeProperty); }
            set { SetProperty(LibraryTypeProperty, value); }
        }
        #endregion

        #region 库区 Area
        /// <summary>
        /// 库区Id
        /// </summary>
        [Label("库区")]
        public static readonly IRefIdProperty AreaIdProperty = P<StorageLocation>.RegisterRefId(e => e.AreaId, ReferenceType.Normal);

        /// <summary>
        /// 库区Id
        /// </summary>
        public double AreaId
        {
            get { return (double)GetRefNullableId(AreaIdProperty); }
            set { SetRefNullableId(AreaIdProperty, value); }
        }

        /// <summary>
        /// 库区
        /// </summary>
        public static readonly RefEntityProperty<StorageArea> AreaProperty = P<StorageLocation>.RegisterRef(e => e.Area, AreaIdProperty);

        /// <summary>
        /// 库区
        /// </summary>
        public StorageArea Area
        {
            get { return GetRefEntity(AreaProperty); }
            set { SetRefEntity(AreaProperty, value); }
        }
        #endregion

        #region 库位专储物料清单列表 StorageLocationItemListList
        /// <summary>
        /// 库位专储物料清单列表
        /// </summary>
        public static readonly ListProperty<EntityList<StorageLocationItemList>> StorageLocationItemListListProperty = P<StorageLocation>.RegisterList(e => e.StorageLocationItemListList);

        /// <summary>
        /// 库位专储物料清单列表
        /// </summary>
        public EntityList<StorageLocationItemList> StorageLocationItemListList
        {
            get { return this.GetLazyList(StorageLocationItemListListProperty); }
        }
        #endregion

        #region 是否储存 IsLayIn
        /// <summary>
        /// 是否储存
        /// </summary>
        [Label("是否储存")]
        public static readonly Property<bool> IsLayInProperty = P<StorageLocation>.Register(e => e.IsLayIn);

        /// <summary>
        /// 是否储存
        /// </summary>
        public bool IsLayIn
        {
            get { return GetProperty(IsLayInProperty); }
            set { SetProperty(IsLayInProperty, value); }
        }
        #endregion

        #region 是否拣货 IsPick
        /// <summary>
        /// 是否拣货
        /// </summary>
        [Label("是否拣货")]
        public static readonly Property<bool> IsPickProperty = P<StorageLocation>.Register(e => e.IsPick);

        /// <summary>
        /// 是否拣货
        /// </summary>
        public bool IsPick
        {
            get { return GetProperty(IsPickProperty); }
            set { SetProperty(IsPickProperty, value); }
        }
        #endregion

        #region 发货暂存 IsFocus
        /// <summary>
        /// 发货暂存
        /// </summary>
        [Label("发货暂存")]
        public static readonly Property<bool> IsFocusProperty = P<StorageLocation>.Register(e => e.IsFocus);

        /// <summary>
        /// 发货暂存
        /// </summary>
        public bool IsFocus
        {
            get { return GetProperty(IsFocusProperty); }
            set { SetProperty(IsFocusProperty, value); }
        }
        #endregion

        #region 收货暂存 IsTemporary
        /// <summary>
        /// 收货暂存
        /// </summary>
        [Label("收货暂存")]
        public static readonly Property<bool> IsTemporaryProperty = P<StorageLocation>.Register(e => e.IsTemporary);

        /// <summary>
        /// 收货暂存
        /// </summary>
        public bool IsTemporary
        {
            get { return GetProperty(IsTemporaryProperty); }
            set { SetProperty(IsTemporaryProperty, value); }
        }
        #endregion

        #region 来源主键 SourceKey
        /// <summary>
        /// 来源主键, 一般用于接口平台事务上传
        /// </summary>
        [Label("来源主键")]
        public static readonly Property<string> SourceKeyProperty = P<StorageLocation>.Register(e => e.SourceKey);

        /// <summary>
        /// 来源主键, 一般用于接口平台事务上传
        /// </summary>
        public string SourceKey
        {
            get { return this.GetProperty(SourceKeyProperty); }
            set { this.SetProperty(SourceKeyProperty, value); }
        }
        #endregion

        #region 排 RowNo
        /// <summary>
        /// 排
        /// </summary>
        [Label("排编号")]
        [MinValue(0)]
        public static readonly Property<int> RowNoProperty = P<StorageLocation>.Register(e => e.RowNo);

        /// <summary>
        /// 排
        /// </summary>
        public int RowNo
        {
            get { return this.GetProperty(RowNoProperty); }
            set { this.SetProperty(RowNoProperty, value); }
        }
        #endregion

        #region 层 LayerNo
        /// <summary>
        /// 层
        /// </summary>
        [Label("层编号")]
        [MinValue(0)]
        public static readonly Property<int> LayerNoProperty = P<StorageLocation>.Register(e => e.LayerNo);

        /// <summary>
        /// 层
        /// </summary>
        public int LayerNo
        {
            get { return this.GetProperty(LayerNoProperty); }
            set { this.SetProperty(LayerNoProperty, value); }
        }
        #endregion

        #region 列 ColumnNo
        /// <summary>
        /// 列
        /// </summary>
        [Label("列编号")]
        [MinValue(0)]
        public static readonly Property<int> ColumnNoProperty = P<StorageLocation>.Register(e => e.ColumnNo);

        /// <summary>
        /// 列
        /// </summary>
        public int ColumnNo
        {
            get { return this.GetProperty(ColumnNoProperty); }
            set { this.SetProperty(ColumnNoProperty, value); }
        }
        #endregion

        #region 深度 Depth
        /// <summary>
        /// 深度
        /// </summary>
        [Label("深度")]
        [MinValue(0)]
        public static readonly Property<int> DeepProperty = P<StorageLocation>.Register(e => e.Depth);

        /// <summary>
        /// 深度
        /// </summary>
        public int Depth
        {
            get { return this.GetProperty(DeepProperty); }
            set { this.SetProperty(DeepProperty, value); }
        }
        #endregion

        #region 入库锁 IsInLock
        /// <summary>
        /// 入库锁
        /// </summary>
        [Label("入库锁")]
        public static readonly Property<bool> IsInLockProperty = P<StorageLocation>.Register(e => e.IsInLock);

        /// <summary>
        /// 入库锁
        /// </summary>
        public bool IsInLock
        {
            get { return this.GetProperty(IsInLockProperty); }
            set { this.SetProperty(IsInLockProperty, value); }
        }
        #endregion

        #region 出库锁 IsOutLock
        /// <summary>
        /// 出库锁
        /// </summary>
        [Label("出库锁")]
        public static readonly Property<bool> IsOutLockProperty = P<StorageLocation>.Register(e => e.IsOutLock);

        /// <summary>
        /// 出库锁
        /// </summary>
        public bool IsOutLock
        {
            get { return this.GetProperty(IsOutLockProperty); }
            set { this.SetProperty(IsOutLockProperty, value); }
        }
        #endregion

        #region 盘点锁 IsCountLock
        /// <summary>
        /// 盘点锁
        /// </summary>
        [Label("盘点锁")]
        public static readonly Property<bool> IsCountLockProperty = P<StorageLocation>.Register(e => e.IsCountLock);

        /// <summary>
        /// 盘点锁
        /// </summary>
        public bool IsCountLock
        {
            get { return this.GetProperty(IsCountLockProperty); }
            set { this.SetProperty(IsCountLockProperty, value); }
        }
        #endregion

        #region 预留库位 IsBackup
        /// <summary>
        /// 预留库位
        /// </summary>
        [Label("预留库位")]
        public static readonly Property<bool> IsBackupProperty = P<StorageLocation>.Register(e => e.IsBackup);

        /// <summary>
        /// 预留库位
        /// </summary>
        public bool IsBackup
        {
            get { return this.GetProperty(IsBackupProperty); }
            set { this.SetProperty(IsBackupProperty, value); }
        }
        #endregion

        #region 巷道 Routeway
        /// <summary>
        /// 巷道Id
        /// </summary>
        [Label("巷道")]
        public static readonly IRefIdProperty RoutewayIdProperty =
            P<StorageLocation>.RegisterRefId(e => e.RoutewayId, ReferenceType.Normal);

        /// <summary>
        /// 巷道Id
        /// </summary>
        public double? RoutewayId
        {
            get { return (double?)this.GetRefNullableId(RoutewayIdProperty); }
            set { this.SetRefNullableId(RoutewayIdProperty, value); }
        }

        /// <summary>
        /// 巷道
        /// </summary>
        public static readonly RefEntityProperty<Routeway> RoutewayProperty =
            P<StorageLocation>.RegisterRef(e => e.Routeway, RoutewayIdProperty);

        /// <summary>
        /// 巷道
        /// </summary>
        public Routeway Routeway
        {
            get { return this.GetRefEntity(RoutewayProperty); }
            set { this.SetRefEntity(RoutewayProperty, value); }
        }
        #endregion

        #region 最深度库位 IsMaxDepth
        /// <summary>
        /// 最深度库位
        /// </summary>
        [Label("最深度库位")]
        public static readonly Property<bool> IsMaxDepthProperty = P<StorageLocation>.Register(e => e.IsMaxDepth);

        /// <summary>
        /// 最深度库位
        /// </summary>
        public bool IsMaxDepth
        {
            get { return this.GetProperty(IsMaxDepthProperty); }
            set { this.SetProperty(IsMaxDepthProperty, value); }
        }
        #endregion

        #region 是否立库 IsAutomatedStorage
        /// <summary>
        /// 是否立库
        /// </summary>
        [Label("是否立库")]
        public static readonly Property<bool> IsAutomatedStorageProperty = P<StorageLocation>.Register(e => e.IsAutomatedStorage);

        /// <summary>
        /// 是否立库
        /// </summary>
        public bool IsAutomatedStorage
        {
            get { return this.GetProperty(IsAutomatedStorageProperty); }
            set { this.SetProperty(IsAutomatedStorageProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 仓库编码 WarehouseCode
        /// <summary>
        /// 仓库编码
        /// </summary>
        [Label("仓库编码")]
        public static readonly Property<string> WarehouseCodeProperty = P<StorageLocation>.RegisterView(e => e.WarehouseCode, p => p.Warehouse.Code);

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string WarehouseCode
        {
            get { return this.GetProperty(WarehouseCodeProperty); }
        }
        #endregion

        #region 仓库名称 WarehouseName
        /// <summary>
        /// 仓库名称
        /// </summary>
        [Label("仓库名称")]
        public static readonly Property<string> WarehouseNameProperty = P<StorageLocation>.RegisterView(e => e.WarehouseName, p => p.Warehouse.Name);

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WarehouseName
        {
            get { return this.GetProperty(WarehouseNameProperty); }
        }
        #endregion        

        #region 库区编码 AreaCode
        /// <summary>
        /// 库区编码
        /// </summary>
        [Label("库区编码")]
        public static readonly Property<string> AreaCodeProperty = P<StorageLocation>.RegisterView(e => e.AreaCode, p => p.Area.Code);

        /// <summary>
        /// 库区编码
        /// </summary>
        public string AreaCode
        {
            get { return this.GetProperty(AreaCodeProperty); }
        }
        #endregion

        #region 库区名称 AreaName
        /// <summary>
        /// 库区名称
        /// </summary>
        [Label("库区名称")]
        public static readonly Property<string> AreaNameProperty = P<StorageLocation>.RegisterView(e => e.AreaName, p => p.Area.Name);

        /// <summary>
        /// 库区名称
        /// </summary>
        public string AreaName
        {
            get { return this.GetProperty(AreaNameProperty); }
        }
        #endregion

        #region 允许人工上架 IsAllowManualGrounding
        /// <summary>
        /// 允许人工上架
        /// </summary>
        [Label("允许人工上架")]
        public static readonly Property<bool> IsAllowManualGroundingProperty = P<StorageLocation>.RegisterView(e => e.IsAllowManualGrounding, p => p.Area.IsAllowManualGrounding);

        /// <summary>
        /// 允许人工上架
        /// </summary>
        public bool IsAllowManualGrounding
        {
            get { return this.GetProperty(IsAllowManualGroundingProperty); }
        }
        #endregion

        #region 库区冻结 AreaIsFrozen
        /// <summary>
        /// 库区冻结
        /// </summary>
        [Label("库区冻结")]
        public static readonly Property<bool> AreaIsFrozenProperty = P<StorageLocation>.RegisterView(e => e.AreaIsFrozen, p => p.Area.IsFrozen);

        /// <summary>
        /// 库区冻结
        /// </summary>
        public bool AreaIsFrozen
        {
            get { return this.GetProperty(AreaIsFrozenProperty); }
        }
        #endregion

        #region 仓库冻结 WarehouseIsFrozen
        /// <summary>
        /// 仓库冻结
        /// </summary>
        [Label("仓库冻结")]
        public static readonly Property<bool> WarehouseIsFrozenProperty = P<StorageLocation>.RegisterView(e => e.WarehouseIsFrozen, p => p.Warehouse.IsFrozen);

        /// <summary>
        /// 仓库冻结
        /// </summary>
        public bool WarehouseIsFrozen
        {
            get { return this.GetProperty(WarehouseIsFrozenProperty); }
        }
        #endregion

        #endregion

        #region 不映射数据库
        #region 长M Long
        /// <summary>
        /// 长M
        /// </summary>
        [Label("长M")]
        public static readonly Property<decimal> LongProperty = P<StorageLocation>.Register(e => e.Long);

        /// <summary>
        /// 长M
        /// </summary>
        public decimal Long
        {
            get { return this.GetProperty(LongProperty); }
            set { this.SetProperty(LongProperty, value); }
        }
        #endregion

        #region 宽M Width
        /// <summary>
        /// 宽M
        /// </summary>
        [Label("宽M")]
        public static readonly Property<decimal> WidthProperty = P<StorageLocation>.Register(e => e.Width);

        /// <summary>
        /// 宽M
        /// </summary>
        public decimal Width
        {
            get { return this.GetProperty(WidthProperty); }
            set { this.SetProperty(WidthProperty, value); }
        }
        #endregion

        #region 高M Height
        /// <summary>
        /// 高M
        /// </summary>
        [Label("高M")]
        public static readonly Property<decimal> HeightProperty = P<StorageLocation>.Register(e => e.Height);

        /// <summary>
        /// 高M
        /// </summary>
        public decimal Height
        {
            get { return this.GetProperty(HeightProperty); }
            set { this.SetProperty(HeightProperty, value); }
        }
        #endregion

        #region 重量KG Weight
        /// <summary>
        /// 重量KG
        /// </summary>
        [Label("重量KG")]
        public static readonly Property<decimal> WeightProperty = P<StorageLocation>.Register(e => e.Weight);

        /// <summary>
        /// 重量KG
        /// </summary>
        public decimal Weight
        {
            get { return this.GetProperty(WeightProperty); }
            set { this.SetProperty(WeightProperty, value); }
        }
        #endregion

        #region 巷道号 RouteNo
        /// <summary>
        /// 巷道号
        /// </summary>
        [Label("巷道号")]
        public static readonly Property<int> RouteNoProperty = P<StorageLocation>.RegisterView(e => e.RouteNo, p => p.Routeway.RoutewayNumber);

        /// <summary>
        /// 巷道号
        /// </summary>
        public int RouteNo
        {
            get { return this.GetProperty(RouteNoProperty); }
        }
        #endregion

        #region 巷道编码 RouteCode
        /// <summary>
        /// 巷道编码
        /// </summary>
        [Label("巷道编码")]
        public static readonly Property<string> RouteCodeProperty = P<StorageLocation>.RegisterView(e => e.RouteCode, p => p.Routeway.Code);

        /// <summary>
        /// 巷道编码
        /// </summary>
        public string RouteCode
        {
            get { return this.GetProperty(RouteCodeProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 库位 实体配置
    /// </summary>
    internal class StorageLocationConfig : EntityConfig<StorageLocation>
    {
        /// <summary>
        /// 对Meta属性配置
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("WH_LOCATION").MapAllProperties();
            Meta.Property(StorageLocation.LongProperty).DontMapColumn();
            Meta.Property(StorageLocation.WidthProperty).DontMapColumn();
            Meta.Property(StorageLocation.HeightProperty).DontMapColumn();
            Meta.Property(StorageLocation.WeightProperty).DontMapColumn();
            Meta.Property(StorageLocation.CodeProperty).ColumnMeta.HasIndex(IndexTypeMeta.Indexed);
            Meta.EnablePhantoms();
        }
    }

    /// <summary>
    /// 库位 基本资料扩展属性
    /// </summary>
    [CompiledPropertyDeclarer]
    public class StorageLocationDetailProperty
    {
        #region 基本资料 BaseInfoProperty
        /// <summary>
        /// 扩展基本资料属性
        /// </summary>
        public static readonly Property<StorageLocationInfo> BaseInfoProperty =
            P<StorageLocation>.RegisterExtension<StorageLocationInfo>("BaseInfo", typeof(StorageLocationDetailProperty));

        /// <summary>
        /// 获取基本资料对象
        /// </summary>
        /// <param name="me">库位对象</param>
        /// <returns>返回基本资料对象</returns>
        public static StorageLocationInfo GetBaseInfo(StorageLocation me)
        {
            return me.GetProperty(BaseInfoProperty);
        }

        /// <summary>
        /// 设置基本资料对象
        /// </summary>
        /// <param name="me">库位对象</param>
        /// <param name="value">需要设置的基本资料对象</param>
        public static void SetBaseInfo(StorageLocation me, StorageLocationInfo value)
        {
            me.SetProperty(BaseInfoProperty, value);
        }
        #endregion

        #region 仓储资料 LayinInfoProperty
        /// <summary>
        /// 扩展仓储资料属性
        /// </summary>
        public static readonly Property<StorageLocationLayinInfo> LayinInfoProperty =
            P<StorageLocation>.RegisterExtension<StorageLocationLayinInfo>("LayinInfo", typeof(StorageLocationDetailProperty));

        /// <summary>
        /// 获取仓储资料对象
        /// </summary>
        /// <param name="me">库位对象</param>
        /// <returns>返回仓储资料对象</returns>
        public static StorageLocationLayinInfo GetLayinInfo(StorageLocation me)
        {
            return me.GetProperty(LayinInfoProperty);
        }

        /// <summary>
        /// 设置仓储资料对象
        /// </summary>
        /// <param name="me">库位对象</param>
        /// <param name="value">需要设置的仓储资料对象</param>
        public static void SetLayinInfo(StorageLocation me, StorageLocationLayinInfo value)
        {
            me.SetProperty(LayinInfoProperty, value);
        }
        #endregion

        #region 操作管理 OperationInfoProperty
        /// <summary>
        /// 扩展操作管理属性
        /// </summary>
        public static readonly Property<StorageLocationOperation> OperationInfoProperty =
            P<StorageLocation>.RegisterExtension<StorageLocationOperation>("OperationInfo", typeof(StorageLocationDetailProperty));

        /// <summary>
        /// 获取操作管理对象
        /// </summary>
        /// <param name="me">库位对象</param>
        /// <returns>返回操作管理对象</returns>
        public static StorageLocationOperation GetOperationInfo(StorageLocation me)
        {
            return me.GetProperty(OperationInfoProperty);
        }

        /// <summary>
        /// 设置操作管理对象
        /// </summary>
        /// <param name="me">库位对象</param>
        /// <param name="value">需要设置的操作管理对象</param>
        public static void SetOperationInfo(StorageLocation me, StorageLocationOperation value)
        {
            me.SetProperty(OperationInfoProperty, value);
        }
        #endregion

        #region 冻结原因列表 StorageLocationFrozenReasonListProperty
        /// <summary>
        /// 冻结原因列表
        /// </summary>
        public static readonly ListProperty<EntityList<StorageLocationFrozenReason>> StorageLocationFrozenReasonListProperty =
            P<StorageLocation>.RegisterExtensionList<EntityList<StorageLocationFrozenReason>>("StorageLocationFrozenReasonList", typeof(StorageLocationDetailProperty));

        /// <summary>
        /// 获取冻结原因列表
        /// </summary>
        /// <param name="me">库位对象</param>
        /// <returns>返回冻结原因列表</returns>
        public static EntityList<StorageLocationFrozenReason> GetStorageLocationFrozenReasonList(StorageLocation me)
        {
            return me.GetProperty(StorageLocationFrozenReasonListProperty);
        }

        /// <summary>
        /// 设置冻结原因列表
        /// </summary>
        /// <param name="me">库位对象</param>
        /// <param name="value">需要设置的冻结原因列表</param>
        public static void SetStorageLocationFrozenReasonList(StorageLocation me, EntityList<StorageLocationFrozenReason> value)
        {
            me.SetProperty(StorageLocationFrozenReasonListProperty, value);
        }
        #endregion

        /// <summary>
        /// 库位 基本资料扩展属性 实体配置
        /// </summary>
        internal class StorageLocationInfoDetailPropertyConfig : EntityConfig<StorageLocation>
        {
            /// <summary>
            /// 属性元数据配置
            /// </summary>
            protected override void ConfigMeta()
            {
                Meta.Property(StorageLocationDetailProperty.BaseInfoProperty).DontMapColumn();
                Meta.Property(StorageLocationDetailProperty.LayinInfoProperty).DontMapColumn();
                Meta.Property(StorageLocationDetailProperty.OperationInfoProperty).DontMapColumn();
                Meta.Property(StorageLocationDetailProperty.StorageLocationFrozenReasonListProperty).DontMapColumn();
            }
        }
    }

    /// <summary>
    /// 库位扩展
    /// </summary>
    [RootEntity, Serializable]    
    [Label("库位扩展")]    
    public partial class StorageLocationExt : StorageLocation//!为了自定义查询块与列表块，多个join使用
    {

    }

    /// <summary>
    /// 库位扩展
    /// </summary>
    [RootEntity, Serializable]
    [Label("库位扩展")]
    public partial class TargetLocationExt : StorageLocation//!为了自定义查询块与列表块，多个join使用
    {

    }
}