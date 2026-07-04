using SIE.Domain;
using SIE.Inventory.Onhands;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Packages.Packages;
using SIE.Warehouses;
using System;

namespace SIE.Inventory.Strategy
{
    /// <summary>
    /// 分配规则明细
    /// </summary>
    [ChildEntity, Serializable]
    [Label("分配规则明细")]
    public partial class AssignRuleDetail : DataEntity
    {
        #region 行号 LineNo
        /// <summary>
        /// 行号
        /// </summary>
        [Required]
        [Label("行号")]
        public static readonly Property<int> LineNoProperty = P<AssignRuleDetail>.Register(e => e.LineNo);

        /// <summary>
        /// 行号
        /// </summary>
        public int LineNo
        {
            get { return GetProperty(LineNoProperty); }
            set { SetProperty(LineNoProperty, value); }
        }
        #endregion

        #region 带LPN库存 WithLpnType
        /// <summary>
        /// 带LPN库存
        /// </summary>
        [Label("带LPN库存")]
        public static readonly Property<WithLpnType?> WithLpnTypeProperty = P<AssignRuleDetail>.Register(e => e.WithLpnType);

        /// <summary>
        /// 带LPN库存
        /// </summary>
        public WithLpnType? WithLpnType
        {
            get { return GetProperty(WithLpnTypeProperty); }
            set { SetProperty(WithLpnTypeProperty, value); }
        }
        #endregion

        #region 立库库存 AutomatedStock
        /// <summary>
        /// 立库库存
        /// </summary>
        [Label("立库库存")]
        public static readonly Property<WithLpnType?> AutomatedStockProperty = P<AssignRuleDetail>.Register(e => e.AutomatedStock);

        /// <summary>
        /// 立库库存
        /// </summary>
        public WithLpnType? AutomatedStock
        {
            get { return GetProperty(AutomatedStockProperty); }
            set { SetProperty(AutomatedStockProperty, value); }
        }
        #endregion

        #region 特采库存 SpecialBasisStock
        /// <summary>
        /// 特采库存
        /// </summary>
        [Label("特采库存")]
        public static readonly Property<WithLpnType?> SpecialBasisStockProperty = P<AssignRuleDetail>.Register(e => e.SpecialBasisStock);

        /// <summary>
        /// 特采库存
        /// </summary>
        public WithLpnType? SpecialBasisStock
        {
            get { return GetProperty(SpecialBasisStockProperty); }
            set { SetProperty(SpecialBasisStockProperty, value); }
        }
        #endregion

        #region 库区 StorageArea
        /// <summary>
        /// 库区Id
        /// </summary>
        public static readonly IRefIdProperty StorageAreaIdProperty = P<AssignRuleDetail>.RegisterRefId(e => e.StorageAreaId, ReferenceType.Normal);

        /// <summary>
        /// 库区Id
        /// </summary>
        public double? StorageAreaId
        {
            get { return (double?)GetRefNullableId(StorageAreaIdProperty); }
            set { SetRefNullableId(StorageAreaIdProperty, value); }
        }

        /// <summary>
        /// 库区
        /// </summary>
        public static readonly RefEntityProperty<StorageArea> StorageAreaProperty = P<AssignRuleDetail>.RegisterRef(e => e.StorageArea, StorageAreaIdProperty);

        /// <summary>
        /// 库区
        /// </summary>
        public StorageArea StorageArea
        {
            get { return GetRefEntity(StorageAreaProperty); }
            set { SetRefEntity(StorageAreaProperty, value); }
        }
        #endregion

        #region 分配顺序 AssignSortType
        /// <summary>
        /// 分配顺序
        /// </summary>
        [Label("分配顺序")]
        public static readonly Property<AssignSortType> AssignSortTypeProperty = P<AssignRuleDetail>.Register(e => e.AssignSortType);

        /// <summary>
        /// 分配顺序
        /// </summary>
        public AssignSortType AssignSortType
        {
            get { return GetProperty(AssignSortTypeProperty); }
            set { SetProperty(AssignSortTypeProperty, value); }
        }
        #endregion

        #region 排序2 Sort2
        /// <summary>
        /// 排序2
        /// </summary>
        [Label("分配顺序")]
        public static readonly Property<AssignSortType?> Sort2Property = P<AssignRuleDetail>.Register(e => e.Sort2);

        /// <summary>
        /// 排序2
        /// </summary>
        public AssignSortType? Sort2
        {
            get { return GetProperty(Sort2Property); }
            set { SetProperty(Sort2Property, value); }
        }
        #endregion

        #region 可用数量匹配规则 LpnQtyMatchType
        /// <summary>
        /// 可用数量匹配规则
        /// </summary>
        [Label("可用数量匹配规则")]
        public static readonly Property<LpnQtyMatchType?> LpnQtyMatchTypeProperty = P<AssignRuleDetail>.Register(e => e.LpnQtyMatchType);

        /// <summary>
        /// LPN数量匹配规则
        /// </summary>
        public LpnQtyMatchType? LpnQtyMatchType
        {
            get { return GetProperty(LpnQtyMatchTypeProperty); }
            set { SetProperty(LpnQtyMatchTypeProperty, value); }
        }
        #endregion

        #region 排序1 Sort1
        /// <summary>
        /// 排序1
        /// </summary>
        [Label("分配顺序")]
        [Required]
        public static readonly Property<AssignSortType?> Sort1Property = P<AssignRuleDetail>.Register(e => e.Sort1);

        /// <summary>
        /// 排序1
        /// </summary>
        public AssignSortType? Sort1
        {
            get { return GetProperty(Sort1Property); }
            set { SetProperty(Sort1Property, value); }
        }
        #endregion

        #region 库位拣货处理 PickProcessType
        /// <summary>
        /// 库位拣货处理
        /// </summary>
        [Label("拣货处理类型")]
        public static readonly Property<PickProcessType?> PickProcessTypeProperty = P<AssignRuleDetail>.Register(e => e.PickProcessType);

        /// <summary>
        /// 库位拣货处理
        /// </summary>
        public PickProcessType? PickProcessType
        {
            get { return GetProperty(PickProcessTypeProperty); }
            set { SetProperty(PickProcessTypeProperty, value); }
        }
        #endregion

        #region 排序3 Sort3
        /// <summary>
        /// 排序3
        /// </summary>
        [Label("分配顺序")]
        public static readonly Property<AssignSortType?> Sort3Property = P<AssignRuleDetail>.Register(e => e.Sort3);

        /// <summary>
        /// 排序3
        /// </summary>
        public AssignSortType? Sort3
        {
            get { return GetProperty(Sort3Property); }
            set { SetProperty(Sort3Property, value); }
        }
        #endregion

        #region 分配规则 AssignRule
        /// <summary>
        /// 分配规则Id
        /// </summary>
        public static readonly IRefIdProperty AssignRuleIdProperty = P<AssignRuleDetail>.RegisterRefId(e => e.AssignRuleId, ReferenceType.Parent);

        /// <summary>
        /// 分配规则Id
        /// </summary>
        public double AssignRuleId
        {
            get { return (double)GetRefId(AssignRuleIdProperty); }
            set { SetRefId(AssignRuleIdProperty, value); }
        }

        /// <summary>
        /// 分配规则
        /// </summary>
        public static readonly RefEntityProperty<AssignRule> AssignRuleProperty = P<AssignRuleDetail>.RegisterRef(e => e.AssignRule, AssignRuleIdProperty);

        /// <summary>
        /// 分配规则
        /// </summary>
        public AssignRule AssignRule
        {
            get { return GetRefEntity(AssignRuleProperty); }
            set { SetRefEntity(AssignRuleProperty, value); }
        }
        #endregion

        #region 分配规则编码 AssignRuleCode
        /// <summary>
        /// 分配规则编码
        /// </summary>
        [Label("分配规则编码")]
        public static readonly Property<string> AssignRuleCodeProperty = P<AssignRuleDetail>.RegisterView(e => e.AssignRuleCode, p => p.AssignRule.Code);

        /// <summary>
        /// 分配规则编码
        /// </summary>
        public string AssignRuleCode
        {
            get { return this.GetProperty(AssignRuleCodeProperty); }
        }
        #endregion

        #region 是否默认 IsDefault
        /// <summary>
        /// 是否默认
        /// </summary>
        [Label("是否默认")]
        public static readonly Property<bool> IsDefaultProperty = P<AssignRuleDetail>.RegisterView(e => e.IsDefault, p => p.AssignRule.IsDefault);

        /// <summary>
        /// 是否默认
        /// </summary>
        public bool IsDefault
        {
            get { return this.GetProperty(IsDefaultProperty); }
        }
        #endregion

        #region 仓库 WarehouseName
        /// <summary>
        /// 仓库
        /// </summary>
        [Label("仓库")]
        public static readonly Property<string> WarehouseNameProperty = P<AssignRuleDetail>.RegisterView(e => e.WarehouseName, p => p.StorageArea.Warehouse.Name);

        /// <summary>
        /// 仓库
        /// </summary>
        public string WarehouseName
        {
            get { return this.GetProperty(WarehouseNameProperty); }
        }
        #endregion

        #region 分配原则 AssignBase
        /// <summary>
        /// 分配原则
        /// </summary>
        [Label("分配原则")]
        public static readonly Property<AssignBase> AssignBaseProperty = P<AssignRuleDetail>.Register(e => e.AssignBase);

        /// <summary>
        /// 分配原则
        /// </summary>
        public AssignBase AssignBase
        {
            get { return this.GetProperty(AssignBaseProperty); }
            set { this.SetProperty(AssignBaseProperty, value); }
        }
        #endregion

        #region 分配包装层级 PackageLevel
        /// <summary>
        /// 分配包装层级
        /// </summary>
        [Label("分配包装层级")]
        public static readonly Property<PackageUnitType?> PackageLevelProperty = P<AssignRuleDetail>.Register(e => e.PackageLevel);

        /// <summary>
        /// 分配包装层级
        /// </summary>
        public PackageUnitType? PackageLevel
        {
            get { return this.GetProperty(PackageLevelProperty); }
            set { this.SetProperty(PackageLevelProperty, value); }
        }
        #endregion

        #region 库存状态 State
        /// <summary>
        /// 库存状态
        /// </summary>
        [Label("库存状态")]
        public static readonly Property<OnhandState?> StateProperty = P<AssignRuleDetail>.Register(e => e.State);

        /// <summary>
        /// 库存状态
        /// </summary>
        public OnhandState? State
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 子规则关系 RelationType
        /// <summary>
        /// 子规则关系
        /// </summary>
        [Label("子规则关系")]
        public static readonly Property<RelationType> RelationTypeProperty = P<AssignRuleDetail>.RegisterView(e => e.RelationType, p => p.AssignRule.RelationType);

        /// <summary>
        /// 子规则关系
        /// </summary>
        public RelationType RelationType
        {
            get { return this.GetProperty(RelationTypeProperty); }
        }
        #endregion

        #region 批次上限 LotCountUL
        /// <summary>
        /// 批次上限
        /// </summary>
        [Label("批次上限")]
        public static readonly Property<int?> LotCountULProperty = P<AssignRuleDetail>.RegisterView(e => e.LotCountUL, p => p.AssignRule.LotCountUL);

        /// <summary>
        /// 批次上限
        /// </summary>
        public int? LotCountUL
        {
            get { return this.GetProperty(LotCountULProperty); }
        }
        #endregion

        #region 生产批次上限 ProductBatchUL
        /// <summary>
        /// 生产批次上限
        /// </summary>
        [Label("生产批次上限")]
        public static readonly Property<int?> ProductBatchULProperty = P<AssignRuleDetail>.RegisterView(e => e.ProductBatchUL, p => p.AssignRule.ProductBatchUL);

        /// <summary>
        /// 生产批次上限
        /// </summary>
        public int? ProductBatchUL
        {
            get { return this.GetProperty(ProductBatchULProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 分配规则明细 实体配置
    /// </summary>
    internal class AssignRuleDetailConfig : EntityConfig<AssignRuleDetail>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("ASSIGN_RULE_DTL").MapAllProperties();
            Meta.EnableSort();
            Meta.EnablePhantoms();
        }
    }
}