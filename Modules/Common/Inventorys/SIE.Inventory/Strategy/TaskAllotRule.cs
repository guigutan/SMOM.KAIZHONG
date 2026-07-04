using SIE.Common.Configs.CommonConfigs;
using SIE.Common.Configs;
using SIE.ObjectModel;
using System;
using SIE.Domain;
using SIE.Inventory.Task;
using SIE.Warehouses;
using SIE.Items;
using SIE.MetaModel;

namespace SIE.Inventory.Strategy
{
    /// <summary>
    /// 任务分配规则
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(TaskAllotRuleCriteria))]
    [EntityWithConfig(typeof(NoConfig))]
    [DisplayMember(nameof(Name))]
    [Label("任务分配规则")]
    public class TaskAllotRule : DataEntity, IStateEntity
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<TaskAllotRule>.Register(e => e.Code);

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
        [NotDuplicate]
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<TaskAllotRule>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 描述 Description
        /// <summary>
        /// 描述
        /// </summary>
        [MaxLength(1000)]
        [Label("描述")]
        public static readonly Property<string> DescriptionProperty = P<TaskAllotRule>.Register(e => e.Description);

        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get { return GetProperty(DescriptionProperty); }
            set { SetProperty(DescriptionProperty, value); }
        }
        #endregion

        #region 任务类型 OperationType
        /// <summary>
        /// 任务类型
        /// </summary>
        [Label("任务类型")]
        public static readonly Property<OperationType?> OperationTypeProperty = P<TaskAllotRule>.Register(e => e.OperationType);

        /// <summary>
        /// 任务类型
        /// </summary>
        public OperationType? OperationType
        {
            get { return this.GetProperty(OperationTypeProperty); }
            set { this.SetProperty(OperationTypeProperty, value); }
        }
        #endregion

        #region 货主 StorerCode
        /// <summary>
        /// 货主
        /// </summary>
        [Label("货主")]
        public static readonly Property<string> StorerCodeProperty = P<TaskAllotRule>.Register(e => e.StorerCode);

        /// <summary>
        /// 货主
        /// </summary>
        public string StorerCode
        {
            get { return GetProperty(StorerCodeProperty); }
            set { SetProperty(StorerCodeProperty, value); }
        }
        #endregion

        #region 仓库 Warehouse
        /// <summary>
        /// 仓库Id
        /// </summary>
        [Label("仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty = P<TaskAllotRule>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<TaskAllotRule>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return GetRefEntity(WarehouseProperty); }
            set { SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 逻辑分区 LogicArea
        /// <summary>
        /// 逻辑分区Id
        /// </summary>
        [Label("逻辑分区")]
        public static readonly IRefIdProperty LogicAreaIdProperty = P<TaskAllotRule>.RegisterRefId(e => e.LogicAreaId, ReferenceType.Normal);

        /// <summary>
        /// 逻辑分区Id
        /// </summary>
        public double? LogicAreaId
        {
            get { return (double?)GetRefNullableId(LogicAreaIdProperty); }
            set { SetRefNullableId(LogicAreaIdProperty, value); }
        }

        /// <summary>
        /// 逻辑分区
        /// </summary>
        public static readonly RefEntityProperty<LogicArea> LogicAreaProperty = P<TaskAllotRule>.RegisterRef(e => e.LogicArea, LogicAreaIdProperty);

        /// <summary>
        /// 逻辑分区
        /// </summary>
        public LogicArea LogicArea
        {
            get { return GetRefEntity(LogicAreaProperty); }
            set { SetRefEntity(LogicAreaProperty, value); }
        }
        #endregion

        #region 逻辑分区编码 LogicAreaCode
        /// <summary>
        /// 逻辑分区编码
        /// </summary>
        [Label("逻辑分区编码")]
        public static readonly Property<string> LogicAreaCodeProperty = P<TaskAllotRule>.RegisterView(e => e.LogicAreaCode, p => p.LogicArea.Code);

        /// <summary>
        /// 逻辑分区编码
        /// </summary>
        public string LogicAreaCode
        {
            get { return this.GetProperty(LogicAreaCodeProperty); }
        }
        #endregion

        #region 库存类别 ItemCategory
        /// <summary>
        /// 库存类别Id
        /// </summary>
        [Label("库存类别")]
        public static readonly IRefIdProperty ItemCategoryIdProperty = P<TaskAllotRule>.RegisterRefId(e => e.ItemCategoryId, ReferenceType.Normal);

        /// <summary>
        /// 库存类别Id
        /// </summary>
        public double? ItemCategoryId
        {
            get { return (double?)GetRefNullableId(ItemCategoryIdProperty); }
            set { SetRefNullableId(ItemCategoryIdProperty, value); }
        }

        /// <summary>
        /// 库存类别
        /// </summary>
        public static readonly RefEntityProperty<ItemCategory> ItemCategoryProperty = P<TaskAllotRule>.RegisterRef(e => e.ItemCategory, ItemCategoryIdProperty);

        /// <summary>
        /// 库存类别
        /// </summary>
        public ItemCategory ItemCategory
        {
            get { return GetRefEntity(ItemCategoryProperty); }
            set { SetRefEntity(ItemCategoryProperty, value); }
        }
        #endregion

        #region 库存分类层级Id ItemCategoryLevelId
        /// <summary>
        /// 库存分类层级Id
        /// </summary>
        [Label("库存分类层级Id")]
        public static readonly Property<double> ItemCategoryLevelIdProperty = P<TaskAllotRule>.RegisterView(e => e.ItemCategoryLevelId, p => p.ItemCategory.LevelId);

        /// <summary>
        /// 库存分类层级Id
        /// </summary>
        public double ItemCategoryLevelId
        {
            get { return this.GetProperty(ItemCategoryLevelIdProperty); }
        }
        #endregion

        #region 优先级 Priority
        /// <summary>
        /// 优先级
        /// </summary>
        [MinValue(1)]
        [Label("优先级")]
        public static readonly Property<int> PriorityProperty = P<TaskAllotRule>.Register(e => e.Priority);

        /// <summary>
        /// 优先级
        /// </summary>
        public int Priority
        {
            get { return GetProperty(PriorityProperty); }
            set { SetProperty(PriorityProperty, value); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<State> StateProperty = P<TaskAllotRule>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public State State
        {
            get { return this.GetProperty(StateProperty); }
            set { this.SetProperty(StateProperty, value); }
        }
        #endregion

        #region 员工列表 EmployeeList
        /// <summary>
        /// 员工列表
        /// </summary>
        [Label("员工")]
        public static readonly ListProperty<EntityList<TaskAllotRuleEmployee>> EmployeeListProperty = P<TaskAllotRule>.RegisterList(e => e.EmployeeList);
        /// <summary>
        /// 员工列表
        /// </summary>
        public EntityList<TaskAllotRuleEmployee> EmployeeList
        {
            get { return this.GetLazyList(EmployeeListProperty); }
        }
        #endregion
    }

    /// <summary>
    /// 任务分配规则 实体配置
    /// </summary>
    internal class TaskAllotRuleConfig : EntityConfig<TaskAllotRule>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("TASK_ALLOT_RULE").MapAllProperties();
            Meta.Property(TaskAllotRule.DescriptionProperty).ColumnMeta.HasLength(2000);
            Meta.EnablePhantoms();
        }
    }
}
