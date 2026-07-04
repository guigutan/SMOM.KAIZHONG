using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Warehouses;
using System;

namespace SIE.Kit.MES.Storages
{
    /// <summary>
    /// 工位货区
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("工位货区")]
    [DisplayMember(nameof(Code))]
    [DataAuth.EntityDataAuth(typeof(Resources.Employees.EmployeeEnterprise), nameof(FactoryId), true)]
    public partial class StorageArea : DataEntity, IStateEntity
    {
        #region 工厂 Factory
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        [Required]
        public static readonly IRefIdProperty FactoryIdProperty =
            P<StorageArea>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

        /// <summary>
        /// 工厂Id
        /// </summary>
        public double? FactoryId
        {
            get { return (double?)this.GetRefNullableId(FactoryIdProperty); }
            set { this.SetRefNullableId(FactoryIdProperty, value); }
        }

        /// <summary>
        /// 工厂
        /// </summary>
        public static readonly RefEntityProperty<Resources.Enterprises.Enterprise> FactoryProperty =
            P<StorageArea>.RegisterRef(e => e.Factory, FactoryIdProperty);

        /// <summary>
        /// 工厂
        /// </summary>
        public Resources.Enterprises.Enterprise Factory
        {
            get { return this.GetRefEntity(FactoryProperty); }
            set { this.SetRefEntity(FactoryProperty, value); }
        }
        #endregion

        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Required, MaxLength(40), NotDuplicate]
        [Label("编码")]
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
        [Required, MaxLength(40), NotDuplicate]
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

        #region 产线货区类型 StorageAreaType
        /// <summary>
        /// 类型
        /// </summary>
        [Label("类型")]
        public static readonly Property<StorageAreaType> TypeProperty = P<StorageArea>.Register(e => e.Type);

        /// <summary>
        /// 类型
        /// </summary>
        public StorageAreaType Type
        {
            get { return GetProperty(TypeProperty); }
            set { SetProperty(TypeProperty, value); }
        }
        #endregion

        #region 仓库 Warehouse
        /// <summary>
        /// 仓库Id
        /// </summary>
        [Label("仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty =
            P<StorageArea>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

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
            P<StorageArea>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return this.GetRefEntity(WarehouseProperty); }
            set { this.SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 是否混放 IsMixItem
        /// <summary>
        /// 是否混放
        /// </summary>
        [Label("是否混放")]
        public static readonly Property<bool> IsMixItemProperty = P<StorageArea>.Register(e => e.IsMixItem);

        /// <summary>
        /// 是否混放
        /// </summary>
        public bool IsMixItem
        {
            get { return this.GetProperty(IsMixItemProperty); }
            set { this.SetProperty(IsMixItemProperty, value); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<State> StateProperty = P<StorageArea>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public State State
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 仓库名称 WarehouseName
        /// <summary>
        /// 仓库名称
        /// </summary>
        [Label("仓库")]
        public static readonly Property<string> WarehouseNameProperty = P<StorageArea>.RegisterView(e => e.WarehouseName, p => p.Warehouse.Name);

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
    /// 工位货区 实体配置
    /// </summary>
    internal class StorageAreaConfig : EntityConfig<StorageArea>
    {
        /// <summary>
        /// 配置数据库的映射
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_WH_AREA").MapAllProperties();
            Meta.EnablePhantoms();
            Meta.Property(StorageArea.CodeProperty).ColumnMeta.HasIndex();
            Meta.Property(StorageArea.NameProperty).ColumnMeta.HasIndex();
        }
    }
}