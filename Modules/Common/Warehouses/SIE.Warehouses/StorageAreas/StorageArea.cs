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
    /// 库区
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(StorageAreaCriteria))]
    [EntityWithConfig(typeof(StorageAreaCodeConfig))]
    [Label("库区")]
    [DisplayMember(nameof(Name))]
    public partial class StorageArea : DataEntity, IStateEntity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public StorageArea() { State = State.Enable; IsAllowManualGrounding = true; }

        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Required]
        [Label("编码")]
        [MaxLength(80)]
        public static readonly Property<string> CodeProperty = P<StorageArea>.Register(e => e.Code);

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
        [MaxLength(80)]
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<StorageArea>.Register(e => e.Name);

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
        public static readonly Property<bool> IsFrozenProperty = P<StorageArea>.Register(e => e.IsFrozen);

        /// <summary>
        /// 是否冻结
        /// </summary>
        public bool IsFrozen
        {
            get { return GetProperty(IsFrozenProperty); }
            set { SetProperty(IsFrozenProperty, value); }
        }
        #endregion

        #region 仓库 Warehouse
        /// <summary>
        /// 仓库Id
        /// </summary>
        [Label("仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty = P<StorageArea>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 仓库Id
        /// </summary>
        public double WarehouseId
        {
            get { return (double)GetRefNullableId(WarehouseIdProperty); }
            set { SetRefNullableId(WarehouseIdProperty, value); }
        }

        /// <summary>
        /// 仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<StorageArea>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return GetRefEntity(WarehouseProperty); }
            set { SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 仓库编码 WarehouseCode
        /// <summary>
        /// 仓库编码
        /// </summary>
        [Label("仓库编码")]
        public static readonly Property<string> WarehouseCodeProperty = P<StorageArea>.RegisterView(e => e.WarehouseCode, p => p.Warehouse.Code);

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
        public static readonly Property<string> WarehouseNameProperty = P<StorageArea>.RegisterView(e => e.WarehouseName, p => p.Warehouse.Name);

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WarehouseName
        {
            get { return this.GetProperty(WarehouseNameProperty); }
        }
        #endregion

        #region 类型 LibraryType
        /// <summary>
        /// 类型
        /// </summary>
        [Label("类型")]
        public static readonly Property<LibraryType> LibraryTypeProperty = P<StorageArea>.Register(e => e.LibraryType);

        /// <summary>
        /// 类型
        /// </summary>
        public LibraryType LibraryType
        {
            get { return GetProperty(LibraryTypeProperty); }
            set { SetProperty(LibraryTypeProperty, value); }
        }
        #endregion

        #region 启用/禁用
        /// <summary>
        /// 启用/禁用
        /// </summary>
        [Label("状态")]
        public static readonly Property<State> StateProperty = P<StorageArea>.Register(e => e.State);

        /// <summary>
        /// 启用/禁用
        /// </summary>
        public State State
        {
            get { return GetProperty(StateProperty); }

            set { SetProperty(StateProperty, value); }
        }
        #endregion

        #region 仓库是否冻结 IsWarehouseFrozen
        /// <summary>
        /// 仓库是否冻结
        /// </summary>
        [Label("仓库是否冻结")]
        public static readonly Property<bool> IsWarehouseFrozenProperty = P<StorageArea>.RegisterView(e => e.IsWarehouseFrozen, p => p.Warehouse.IsFrozen);

        /// <summary>
        /// 仓库是否冻结
        /// </summary>
        public bool IsWarehouseFrozen
        {
            get { return this.GetProperty(IsWarehouseFrozenProperty); }
        }
        #endregion

        #region 是否立库库区 IsAutomatedArea
        /// <summary>
        /// 是否立库库区
        /// </summary>
        [Label("是否立库库区")]
        public static readonly Property<bool> IsAutomatedAreaProperty = P<StorageArea>.Register(e => e.IsAutomatedArea);

        /// <summary>
        /// 是否立库库区
        /// </summary>
        public bool IsAutomatedArea
        {
            get { return this.GetProperty(IsAutomatedAreaProperty); }
            set { this.SetProperty(IsAutomatedAreaProperty, value); }
        }
        #endregion

        #region 优先上架库位 GroundingType
        /// <summary>
        /// 优先上架库位
        /// </summary>
        [Label("上架顺序")]
        public static readonly Property<GroundingType> GroundingTypeProperty = P<StorageArea>.Register(e => e.GroundingType);

        /// <summary>
        /// 优先上架库位
        /// </summary>
        public GroundingType GroundingType
        {
            get { return this.GetProperty(GroundingTypeProperty); }
            set { this.SetProperty(GroundingTypeProperty, value); }
        }
        #endregion   

        #region 允许人工上架 IsAllowManualGrounding
        /// <summary>
        /// 允许人工上架
        /// </summary>
        [Label("允许人工上架")]
        public static readonly Property<bool> IsAllowManualGroundingProperty = P<StorageArea>.Register(e => e.IsAllowManualGrounding);

        /// <summary>
        /// 允许人工上架
        /// </summary>
        public bool IsAllowManualGrounding
        {
            get { return this.GetProperty(IsAllowManualGroundingProperty); }
            set { this.SetProperty(IsAllowManualGroundingProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 货区 实体配置
    /// </summary>
    internal class StorageAreaConfig : EntityConfig<StorageArea>
    {
        /// <summary>
        /// 对Meta属性的配置
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("WH_AREA").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }

    #region 库区扩展基本资料
    /// <summary>
    /// 扩展库区基本资料
    /// </summary>
    [CompiledPropertyDeclarer]
    public class StorageAreaInfoDetailProperty
    {
        /// <summary>
        /// 扩展库区基本资料
        /// </summary>
        [Label("基本资料")]
        public static readonly Property<StorageAreaInfo> StorageAreaInfoProperty =
            P<StorageArea>.RegisterExtension<StorageAreaInfo>("StorageAreaInfoTem", typeof(StorageAreaInfoDetailProperty));

        /// <summary>
        /// 获取库区资料对象
        /// </summary>
        /// <param name="me">库区对象</param>
        /// <returns>返回库区资料对象</returns>
        public static StorageAreaInfo GetStorageAreaInfoDetail(StorageArea me)
        {
            return me.GetProperty(StorageAreaInfoProperty);
        }

        /// <summary>
        /// 设置库区资料对象
        /// </summary>
        /// <param name="me">库区对象</param>
        /// <param name="value">需要设置的库区对象</param>
        public static void SetStorageAreaInfoDetail(StorageArea me, StorageAreaInfo value)
        {
            me.SetProperty(StorageAreaInfoProperty, value);
        }

        /// <summary>
        /// 扩展库区资料 实体配置
        /// </summary>
        internal class StorageAreaInfoDetailPropertyConfig : EntityConfig<StorageArea>
        {
            /// <summary>
            /// 属性元数据配置
            /// </summary>
            protected override void ConfigMeta()
            {
                Meta.Property(StorageAreaInfoDetailProperty.StorageAreaInfoProperty).DontMapColumn();
            }
        }
    }
    #endregion

    #region 库区扩展操作管理
    /// <summary>
    /// 扩展库区操作管理
    /// </summary>
    [CompiledPropertyDeclarer]
    public class StorageAreaOperationDetailProperty
    {
        /// <summary>
        /// 扩展库区操作管理
        /// </summary>
        [Label("操作管理")]
        public static readonly Property<StorageAreaOperation> StorageAreaOperationProperty =
            P<StorageArea>.RegisterExtension<StorageAreaOperation>("StorageAreaOperationTem", typeof(StorageAreaOperationDetailProperty));

        /// <summary>
        /// 获取仓库资料对象
        /// </summary>
        /// <param name="me">仓库对象</param>
        /// <returns>返回仓库资料对象</returns>
        public static StorageAreaOperation GetStorageAreaOperationDetail(StorageArea me)
        {
            return me.GetProperty(StorageAreaOperationProperty);
        }

        /// <summary>
        /// 设置仓库资料对象
        /// </summary>
        /// <param name="me">仓库对象</param>
        /// <param name="value">需要设置的仓库对象</param>
        public static void SetStorageAreaOperationDetail(StorageArea me, StorageAreaOperation value)
        {
            me.SetProperty(StorageAreaOperationProperty, value);
        }

        /// <summary>
        /// 扩展库区资料 实体配置
        /// </summary>
        internal class StorageAreaOperationDetailPropertyConfig : EntityConfig<StorageArea>
        {
            /// <summary>
            /// 属性元数据配置
            /// </summary>
            protected override void ConfigMeta()
            {
                Meta.Property(StorageAreaOperationDetailProperty.StorageAreaOperationProperty).DontMapColumn();
            }
        }
    }
    #endregion

    #region 库区扩展立库配置
    /// <summary>
    /// 扩展库区立库配置
    /// </summary>
    [CompiledPropertyDeclarer]
    public class StorageAreaWcsDetailProperty
    {
        /// <summary>
        /// 扩展库区立库配置
        /// </summary>
        [Label("立库配置")]
        public static readonly Property<StorageAreaWcs> StorageAreaWcsProperty =
            P<StorageArea>.RegisterExtension<StorageAreaWcs>("StorageAreaWcsTem", typeof(StorageAreaWcsDetailProperty));

        /// <summary>
        /// 获取仓库资料对象
        /// </summary>
        /// <param name="me">仓库对象</param>
        /// <returns>返回仓库资料对象</returns>
        public static StorageAreaWcs GetStorageAreaWcsDetail(StorageArea me)
        {
            return me.GetProperty(StorageAreaWcsProperty);
        }

        /// <summary>
        /// 设置仓库资料对象
        /// </summary>
        /// <param name="me">仓库对象</param>
        /// <param name="value">需要设置的仓库对象</param>
        public static void SetStorageAreaWcsDetail(StorageArea me, StorageAreaWcs value)
        {
            me.SetProperty(StorageAreaWcsProperty, value);
        }

        /// <summary>
        /// 扩展库区资料 实体配置
        /// </summary>
        internal class StorageAreaWcsDetailPropertyConfig : EntityConfig<StorageArea>
        {
            /// <summary>
            /// 属性元数据配置
            /// </summary>
            protected override void ConfigMeta()
            {
                Meta.Property(StorageAreaWcsDetailProperty.StorageAreaWcsProperty).DontMapColumn();
            }
        }
    }
    #endregion
}