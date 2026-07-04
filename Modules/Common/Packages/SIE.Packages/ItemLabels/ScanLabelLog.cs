using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Packages.Packages;
using SIE.Warehouses;
using System;

namespace SIE.Packages.ItemLabels
{
    /// <summary>
    /// 扫描标签日志查询
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [DisplayMember(nameof(No))]
    [Label("扫描标签日志")]
    public partial class ScanLabelLog : DataEntity
    {
        #region 最上级条码 HighestNo
        /// <summary>
        /// 最上级条码
        /// </summary>
        [Label("最上级条码")]
        public static readonly Property<string> HighestNoProperty = P<ScanLabelLog>.Register(e => e.HighestNo);

        /// <summary>
        /// 最上级条码
        /// </summary>
        public string HighestNo
        {
            get { return this.GetProperty(HighestNoProperty); }
            set { this.SetProperty(HighestNoProperty, value); }
        }
        #endregion

        #region 条码 No
        /// <summary>
        /// 条码
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("条码")]
        public static readonly Property<string> NoProperty = P<ScanLabelLog>.Register(e => e.No);

        /// <summary>
        /// 条码
        /// </summary>
        public string No
        {
            get { return GetProperty(NoProperty); }
            set { SetProperty(NoProperty, value); }
        }
        #endregion

        #region 数量 Qty
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        public static readonly Property<decimal> QtyProperty = P<ScanLabelLog>.Register(e => e.Qty);

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty
        {
            get { return this.GetProperty(QtyProperty); }
            set { this.SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 操作类型 LabelOpType
        /// <summary>
        /// 操作类型
        /// </summary>
        [Label("操作类型")]
        public static readonly Property<LabelOpType?> LabelOpTypeProperty = P<ScanLabelLog>.Register(e => e.LabelOpType);

        /// <summary>
        /// 操作类型
        /// </summary>
        public LabelOpType? LabelOpType
        {
            get { return this.GetProperty(LabelOpTypeProperty); }
            set { this.SetProperty(LabelOpTypeProperty, value); }
        }
        #endregion

        #region 单据号 BillNo
        /// <summary>
        /// 单据号
        /// </summary>
        [Label("单据号")]
        public static readonly Property<string> BillNoProperty = P<ScanLabelLog>.Register(e => e.BillNo);

        /// <summary>
        /// 单据号
        /// </summary>
        public string BillNo
        {
            get { return this.GetProperty(BillNoProperty); }
            set { this.SetProperty(BillNoProperty, value); }
        }
        #endregion

        #region 明细行号 LineNo
        /// <summary>
        /// 明细行号
        /// </summary>
        [Label("明细行号")]
        public static readonly Property<string> LineNoProperty = P<ScanLabelLog>.Register(e => e.LineNo);

        /// <summary>
        /// 明细行号
        /// </summary>
        public string LineNo
        {
            get { return this.GetProperty(LineNoProperty); }
            set { this.SetProperty(LineNoProperty, value); }
        }
        #endregion

        #region 类型 PackingType
        /// <summary>
        /// 类型Id
        /// </summary>
        [Label("类型")]
        public static readonly IRefIdProperty PackingTypeIdProperty = P<ScanLabelLog>.RegisterRefId(e => e.PackingTypeId, ReferenceType.Normal);

        /// <summary>
        /// 类型Id
        /// </summary>
        public double? PackingTypeId
        {
            get { return (double?)GetRefNullableId(PackingTypeIdProperty); }
            set { SetRefNullableId(PackingTypeIdProperty, value); }
        }

        /// <summary>
        /// 类型
        /// </summary>
        public static readonly RefEntityProperty<PackingUnit> PackingTypeProperty = P<ScanLabelLog>.RegisterRef(e => e.PackingType, PackingTypeIdProperty);

        /// <summary>
        /// 类型
        /// </summary>
        public PackingUnit PackingType
        {
            get { return GetRefEntity(PackingTypeProperty); }
            set { SetRefEntity(PackingTypeProperty, value); }
        }
        #endregion

        #region 物料包装规则 ItemPackageRule
        /// <summary>
        /// 物料包装规则Id
        /// </summary>
        [Label("物料包装规则")]
        public static readonly IRefIdProperty ItemPackageRuleIdProperty =
            P<ScanLabelLog>.RegisterRefId(e => e.ItemPackageRuleId, ReferenceType.Normal);

        /// <summary>
        /// 物料包装规则Id
        /// </summary>
        public double? ItemPackageRuleId
        {
            get { return (double?)this.GetRefNullableId(ItemPackageRuleIdProperty); }
            set { this.SetRefNullableId(ItemPackageRuleIdProperty, value); }
        }

        /// <summary>
        /// 物料包装规则
        /// </summary>
        public static readonly RefEntityProperty<ItemPackageRule> ItemPackageRuleProperty =
            P<ScanLabelLog>.RegisterRef(e => e.ItemPackageRule, ItemPackageRuleIdProperty);

        /// <summary>
        /// 物料包装规则
        /// </summary>
        public ItemPackageRule ItemPackageRule
        {
            get { return this.GetRefEntity(ItemPackageRuleProperty); }
            set { this.SetRefEntity(ItemPackageRuleProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty =
            P<ScanLabelLog>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料Id
        /// </summary>
        public double? ItemId
        {
            get { return (double?)this.GetRefNullableId(ItemIdProperty); }
            set { this.SetRefNullableId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty =
            P<ScanLabelLog>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return this.GetRefEntity(ItemProperty); }
            set { this.SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 是否序列 IsSequence
        /// <summary>
        /// 是否序列
        /// </summary>
        [Label("是否序列")]
        public static readonly Property<bool> IsSequenceProperty = P<ScanLabelLog>.Register(e => e.IsSequence);

        /// <summary>
        /// 是否序列
        /// </summary>
        public bool IsSequence
        {
            get { return GetProperty(IsSequenceProperty); }
            set { SetProperty(IsSequenceProperty, value); }
        }
        #endregion

        #region 物料批次号 LotCode
        /// <summary>
        /// 物料批次号
        /// </summary>
        [Label("物料批次号")]
        public static readonly Property<string> LotCodeProperty = P<ScanLabelLog>.Register(e => e.LotCode);

        /// <summary>
        /// 物料批次号
        /// </summary>
        public string LotCode
        {
            get { return this.GetProperty(LotCodeProperty); }
            set { this.SetProperty(LotCodeProperty, value); }
        }
        #endregion

        #region 仓库 Warehouse
        /// <summary>
        /// 仓库Id
        /// </summary>
        [Label("仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty =
            P<ScanLabelLog>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 仓库Id
        /// </summary>
        public double? WarehouseId
        {
            get { return (double?)this.GetRefNullableId(WarehouseIdProperty); }
            set { this.SetRefNullableId(WarehouseIdProperty, value); }
        }

        /// <summary>
        /// 仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty =
            P<ScanLabelLog>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return this.GetRefEntity(WarehouseProperty); }
            set { this.SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 物料扩展属性 ItemExtProp
        /// <summary>
        /// 物料扩展属性
        /// </summary>
        [Label("物料扩展属性")]
        public static readonly Property<string> ItemExtPropProperty = P<ScanLabelLog>.Register(e => e.ItemExtProp);

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProp
        {
            get { return this.GetProperty(ItemExtPropProperty); }
            set { this.SetProperty(ItemExtPropProperty, value); }
        }
        #endregion

        #region 物料扩展属性 ItemExtPropName
        /// <summary>
        /// 物料扩展属性
        /// </summary>
        [Label("物料扩展属性")]
        public static readonly Property<string> ItemExtPropNameProperty = P<ScanLabelLog>.Register(e => e.ItemExtPropName);

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtPropName
        {
            get { return this.GetProperty(ItemExtPropNameProperty); }
            set { this.SetProperty(ItemExtPropNameProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<ScanLabelLog>.RegisterView(e => e.ItemCode, p => p.Item.Code);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
            set { this.SetProperty(ItemCodeProperty, value); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<ScanLabelLog>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
            set { this.SetProperty(ItemNameProperty, value); }
        }
        #endregion

        #region 包装单位 PackingTypeName
        /// <summary>
        /// 包装单位名称
        /// </summary>
        [Label("包装单位")]
        public static readonly Property<string> PackingTypeNameProperty = P<ScanLabelLog>.RegisterView(e => e.PackingTypeName, p => p.PackingType.Name);

        /// <summary>
        /// 包装单位名称
        /// </summary>
        public string PackingTypeName
        {
            get { return this.GetProperty(PackingTypeNameProperty); }
            set { this.SetProperty(PackingTypeNameProperty, value); }
        }
        #endregion

        #region 物料包装规则 ItemPackageRuleCode
        /// <summary>
        /// 物料包装规则
        /// </summary>
        [Label("物料包装规则")]
        public static readonly Property<string> ItemPackageRuleCodeProperty = P<ScanLabelLog>.RegisterView(e => e.ItemPackageRuleCode, p => p.ItemPackageRule.Code);

        /// <summary>
        /// 物料包装规则
        /// </summary>
        public string ItemPackageRuleCode
        {
            get { return this.GetProperty(ItemPackageRuleCodeProperty); }
            set { this.SetProperty(ItemPackageRuleCodeProperty, value); }
        }
        #endregion

        #region 物料包装规则名称 ItemPackageRuleName
        /// <summary>
        /// 物料包装规则名称
        /// </summary>
        [Label("物料包装规则名称")]
        public static readonly Property<string> ItemPackageRuleNameProperty = P<ScanLabelLog>.RegisterView(e => e.ItemPackageRuleName, p => p.ItemPackageRule.Name);

        /// <summary>
        /// 物料包装规则名称
        /// </summary>
        public string ItemPackageRuleName
        {
            get { return this.GetProperty(ItemPackageRuleNameProperty); }
            set { this.SetProperty(ItemPackageRuleNameProperty, value); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 标签条码 实体配置
    /// </summary>
    internal class ScanLabelLogConfig : EntityConfig<ScanLabelLog>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("SCAN_LABEL_LOG").MapAllProperties();
            Meta.Property(ScanLabelLog.ItemExtPropProperty).ColumnMeta.HasLength(480);
            Meta.Property(ScanLabelLog.ItemExtPropNameProperty).ColumnMeta.HasLength(2000);
            Meta.Property(ScanLabelLog.HighestNoProperty).MapColumn().HasIndex(IndexTypeMeta.Indexed);
            Meta.Property(ScanLabelLog.BillNoProperty).MapColumn().HasIndex(IndexTypeMeta.Indexed);
            Meta.Property(ScanLabelLog.ItemIdProperty).ColumnMeta.IgnoreFK();
            Meta.Property(ScanLabelLog.ItemPackageRuleIdProperty).ColumnMeta.IgnoreFK();
            Meta.EnablePhantoms();
            Meta.DisableDataSync();
        }
    }
}
