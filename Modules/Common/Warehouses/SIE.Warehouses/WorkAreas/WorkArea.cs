using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Warehouses
{
    /// <summary>
	/// 工作区
	/// </summary>
	[RootEntity, Serializable]
    [EntityWithConfig(typeof(NoConfig))]
    [ConditionQueryType(typeof(WorkAreaCriteria))]
    [Label("工作区")]
    public partial class WorkArea : DataEntity, IStateEntity
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<WorkArea>.Register(e => e.Code);

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
        public static readonly Property<string> NameProperty = P<WorkArea>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 描述 Desc
        /// <summary>
        /// 描述
        /// </summary>
        [Label("描述")]
        public static readonly Property<string> DescProperty = P<WorkArea>.Register(e => e.Desc);

        /// <summary>
        /// 描述
        /// </summary>
        public string Desc
        {
            get { return GetProperty(DescProperty); }
            set { SetProperty(DescProperty, value); }
        }
        #endregion

        #region 仓库 Warehouse
        /// <summary>
        /// 仓库Id
        /// </summary>
        [Label("仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty = P<WorkArea>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<WorkArea>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

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
        public static readonly Property<string> WarehouseCodeProperty = P<WorkArea>.RegisterView(e => e.WarehouseCode, p => p.Warehouse.Code);

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string WarehouseCode
        {
            get { return this.GetProperty(WarehouseCodeProperty); }
        }
        #endregion

        #region 列表 WorkAreaLocationList
        /// <summary>
        /// 列表
        /// </summary>
        public static readonly ListProperty<EntityList<WorkAreaLocation>> WorkAreaLocationListProperty = P<WorkArea>.RegisterList(e => e.WorkAreaLocationList);
        /// <summary>
        /// 列表
        /// </summary>
        public EntityList<WorkAreaLocation> WorkAreaLocationList
        {
            get { return this.GetLazyList(WorkAreaLocationListProperty); }
        }
        #endregion

        #region 列表 WorkAreaEmployeeList
        /// <summary>
        /// 列表
        /// </summary>
        public static readonly ListProperty<EntityList<WorkAreaEmployee>> WorkAreaEmployeeListProperty = P<WorkArea>.RegisterList(e => e.WorkAreaEmployeeList);
        /// <summary>
        /// 列表
        /// </summary>
        public EntityList<WorkAreaEmployee> WorkAreaEmployeeList
        {
            get { return this.GetLazyList(WorkAreaEmployeeListProperty); }
        }
        #endregion

        #region 启用/禁用
        /// <summary>
        /// 启用/禁用
        /// </summary>
        [Label("状态")]
        public static readonly Property<State> StateProperty = P<WorkArea>.Register(e => e.State);

        /// <summary>
        /// 启用/禁用
        /// </summary>
        public State State
        {
            get { return GetProperty(StateProperty); }

            set { SetProperty(StateProperty, value); }
        }
        #endregion
    }

    /// <summary>
    ///  实体配置
    /// </summary>
    internal class WorkAreaConfig : EntityConfig<WorkArea>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WH_WORKAREA").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}