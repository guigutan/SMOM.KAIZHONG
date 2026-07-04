using SIE.Common.NumberRules;
using SIE.Common.Prints;
using SIE.Common.Sort;
using SIE.Core.Items;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Packages.Packages;
using System;
using System.Linq;

namespace SIE.Packages
{
    /// <summary>
    /// 物料包装规则明细
    /// </summary>
    [ChildEntity, Serializable]
    [Label("物料包装规则明细")]
    public partial class ItemPackageRuleDetail : DataEntity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ItemPackageRuleDetail()
        {
            LevelQty = 1;
        }

        #region 产品数 Qty
        /// <summary>
        /// 产品数
        /// </summary>
        [Label("产品数")]
        [MinValue(0)]
        public static readonly Property<decimal> QtyProperty = P<ItemPackageRuleDetail>.Register(e => e.Qty);

        /// <summary>
        /// 产品数
        /// </summary>
        public decimal Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion 

        #region 包装数 LevelQty
        /// <summary>
        /// 包装数
        /// </summary>
        [Label("包装数")]
        public static readonly Property<decimal> LevelQtyProperty = P<ItemPackageRuleDetail>.Register(e => e.LevelQty);

        /// <summary>
        /// 包装数
        /// </summary>
        public decimal LevelQty
        {
            get { return this.GetProperty(LevelQtyProperty); }
            set { this.SetProperty(LevelQtyProperty, value); }
        }
        #endregion 

        #region 描述 Description
        /// <summary>
        /// 描述
        /// </summary>
        [Label("描述")]
        public static readonly Property<string> DescriptionProperty = P<ItemPackageRuleDetail>.Register(e => e.Description);

        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get { return GetProperty(DescriptionProperty); }
            set { SetProperty(DescriptionProperty, value); }
        }
        #endregion

        #region 是否装箱 IsPackage
        /// <summary>
        /// 是否装箱
        /// </summary>
        [Label("是否装箱")]
        public static readonly Property<bool> IsPackageProperty = P<ItemPackageRuleDetail>.Register(e => e.IsPackage);

        /// <summary>
        /// 是否装箱
        /// </summary>
        public bool IsPackage
        {
            get { return GetProperty(IsPackageProperty); }
            set { SetProperty(IsPackageProperty, value); }
        }
        #endregion

        #region 是否出库 IsOutStockLabel
        /// <summary>
        /// 是否出库标签
        /// </summary>
        [Label("是否出库")]
        public static readonly Property<bool> IsOutStockLabelProperty = P<ItemPackageRuleDetail>.Register(e => e.IsOutStockLabel);

        /// <summary>
        /// 是否出库
        /// </summary>
        public bool IsOutStockLabel
        {
            get { return GetProperty(IsOutStockLabelProperty); }
            set { SetProperty(IsOutStockLabelProperty, value); }
        }
        #endregion

        #region 是否入库 IsInStockLabel
        /// <summary>
        /// 是否入库
        /// </summary>
        [Label("是否入库")]
        public static readonly Property<bool> IsInStockLabelProperty = P<ItemPackageRuleDetail>.Register(e => e.IsInStockLabel);

        /// <summary>
        /// 是否入库
        /// </summary>
        public bool IsInStockLabel
        {
            get { return GetProperty(IsInStockLabelProperty); }
            set { SetProperty(IsInStockLabelProperty, value); }
        }
        #endregion

        #region 长(CM) Length
        /// <summary>
        /// 长(CM)
        /// </summary>
        [Label("长(CM)")]
        [MinValue(0)]
        public static readonly Property<decimal> LengthProperty = P<ItemPackageRuleDetail>.Register(e => e.Length);

        /// <summary>
        /// 长(CM)
        /// </summary>
        public decimal Length
        {
            get { return GetProperty(LengthProperty); }
            set { SetProperty(LengthProperty, value); }
        }
        #endregion

        #region 宽(CM) Width
        /// <summary>
        /// 宽(CM)
        /// </summary>
        [Label("宽(CM)")]
        [MinValue(0)]
        public static readonly Property<decimal> WidthProperty = P<ItemPackageRuleDetail>.Register(e => e.Width);

        /// <summary>
        /// 宽(CM)
        /// </summary>
        public decimal Width
        {
            get { return GetProperty(WidthProperty); }
            set { SetProperty(WidthProperty, value); }
        }
        #endregion

        #region 高(CM) Height
        /// <summary>
        /// 高(CM)
        /// </summary>
        [Label("高(CM)")]
        [MinValue(0)]
        public static readonly Property<decimal> HeightProperty = P<ItemPackageRuleDetail>.Register(e => e.Height);

        /// <summary>
        /// 高(CM)
        /// </summary>
        public decimal Height
        {
            get { return GetProperty(HeightProperty); }
            set { SetProperty(HeightProperty, value); }
        }
        #endregion

        #region 体积(CM³) Volume
        /// <summary>
        /// 体积(CM³)
        /// </summary>
        [Label("体积(CM³)")]
        [MinValue(0)]
        public static readonly Property<decimal> VolumeProperty = P<ItemPackageRuleDetail>.Register(e => e.Volume);

        /// <summary>
        /// 体积(CM³)
        /// </summary>
        public decimal Volume
        {
            get { return GetProperty(VolumeProperty); }
            set { SetProperty(VolumeProperty, value); }
        }
        #endregion

        #region 重量(KG) Weight
        /// <summary>
        /// 重量(KG)
        /// </summary>
        [Label("重量(KG)")]
        [MinValue(0)]
        public static readonly Property<decimal> WeightProperty = P<ItemPackageRuleDetail>.Register(e => e.Weight);

        /// <summary>
        /// 重量(KG)
        /// </summary>
        public decimal Weight
        {
            get { return GetProperty(WeightProperty); }
            set { SetProperty(WeightProperty, value); }
        }
        #endregion

        #region 是否打印 IsPrint
        /// <summary>
        /// 是否不打印
        /// </summary>
        [Label("是否打印")]
        public static readonly Property<bool> IsPrintProperty = P<ItemPackageRuleDetail>.Register(e => e.IsPrint);

        /// <summary>
        /// 是否打印
        /// </summary>
        public bool IsPrint
        {
            get { return this.GetProperty(IsPrintProperty); }
            set { this.SetProperty(IsPrintProperty, value); }
        }
        #endregion

        #region 是否最小包装 IsMinPacking
        /// <summary>
        /// 是否最小包装
        /// </summary>
        [Label("是否最小包装")]
        public static readonly Property<bool> IsMinPackingProperty = P<ItemPackageRuleDetail>.Register(e => e.IsMinPacking);

        /// <summary>
        /// 是否最小包装
        /// </summary>
        public bool IsMinPacking
        {
            get { return GetProperty(IsMinPackingProperty); }
            set { SetProperty(IsMinPackingProperty, value); }
        }
        #endregion

        #region 是否序列 IsSequence
        /// <summary>
        /// 是否序列
        /// </summary>
        [Label("是否序列")]
        public static readonly Property<bool> IsSequenceProperty = P<ItemPackageRuleDetail>.Register(e => e.IsSequence);

        /// <summary>
        /// 是否序列
        /// </summary>
        public bool IsSequence
        {
            get { return GetProperty(IsSequenceProperty); }
            set { SetProperty(IsSequenceProperty, value); }
        }
        #endregion

        #region 编码规则 NumberRule
        /// <summary>
        /// 编码规则Id
        /// </summary>
        [Label("编码规则")]
        public static readonly IRefIdProperty NumberRuleIdProperty = P<ItemPackageRuleDetail>.RegisterRefId(e => e.NumberRuleId, ReferenceType.Normal);

        /// <summary>
        /// 编码规则Id
        /// </summary>
        public double? NumberRuleId
        {
            get { return (double?)GetRefNullableId(NumberRuleIdProperty); }
            set { SetRefNullableId(NumberRuleIdProperty, value); }
        }

        /// <summary>
        /// 编码规则
        /// </summary>
        public static readonly RefEntityProperty<NumberRule> NumberRuleProperty = P<ItemPackageRuleDetail>.RegisterRef(e => e.NumberRule, NumberRuleIdProperty);

        /// <summary>
        /// 编码规则
        /// </summary>
        public NumberRule NumberRule
        {
            get { return GetRefEntity(NumberRuleProperty); }
            set { SetRefEntity(NumberRuleProperty, value); }
        }
        #endregion

        #region 打印模板 PrintTemplate
        /// <summary>
        /// 打印模板Id
        /// </summary>
        [Label("打印模板")]
        public static readonly IRefIdProperty PrintTemplateIdProperty = P<ItemPackageRuleDetail>.RegisterRefId(e => e.PrintTemplateId, ReferenceType.Normal);

        /// <summary>
        /// 打印模板Id
        /// </summary>
        public double? PrintTemplateId
        {
            get { return (double?)GetRefNullableId(PrintTemplateIdProperty); }
            set { SetRefNullableId(PrintTemplateIdProperty, value); }
        }

        /// <summary>
        /// 打印模板
        /// </summary>
        public static readonly RefEntityProperty<PrintTemplate> PrintTemplateProperty = P<ItemPackageRuleDetail>.RegisterRef(e => e.PrintTemplate, PrintTemplateIdProperty);

        /// <summary>
        /// 打印模板
        /// </summary>
        public PrintTemplate PrintTemplate
        {
            get { return GetRefEntity(PrintTemplateProperty); }
            set { SetRefEntity(PrintTemplateProperty, value); }
        }
        #endregion

        #region 包装单位 PackageUnit
        /// <summary>
        /// 包装单位Id
        /// </summary>
        [Label("单位代码")]
        public static readonly IRefIdProperty PackageUnitIdProperty = P<ItemPackageRuleDetail>.RegisterRefId(e => e.PackageUnitId, ReferenceType.Normal);

        /// <summary>
        /// 包装单位Id
        /// </summary>
        public double PackageUnitId
        {
            get { return (double)GetRefId(PackageUnitIdProperty); }
            set { SetRefId(PackageUnitIdProperty, value); }
        }

        /// <summary>
        /// 包装单位
        /// </summary>
        //[Required]
        public static readonly RefEntityProperty<PackingUnit> PackageUnitProperty = P<ItemPackageRuleDetail>.RegisterRef(e => e.PackageUnit, PackageUnitIdProperty);

        /// <summary>
        /// 包装单位
        /// </summary>
        public PackingUnit PackageUnit
        {
            get { return GetRefEntity(PackageUnitProperty); }
            set { SetRefEntity(PackageUnitProperty, value); }
        }
        #endregion

        #region 是否主单位 IsMasterUnit
        /// <summary>
        /// 是否主单位
        /// </summary>
        [Label("是否主单位")]
        public static readonly Property<bool> IsMasterUnitProperty = P<ItemPackageRuleDetail>.Register(e => e.IsMasterUnit);

        /// <summary>
        /// 是否主单位
        /// </summary>
        public bool IsMasterUnit
        {
            get { return this.GetProperty(IsMasterUnitProperty); }
            set { this.SetProperty(IsMasterUnitProperty, value); }
        }
        #endregion

        #region 单位名称 PackageUnitName
        /// <summary>
        /// 单位名称
        /// </summary>
        [Label("单位名称")]
        public static readonly Property<string> PackageUnitNameProperty = P<ItemPackageRuleDetail>.RegisterView(e => e.PackageUnitName, p => p.PackageUnit.Name);

        /// <summary>
        /// 单位名称
        /// </summary>
        public string PackageUnitName
        {
            get { return this.GetProperty(PackageUnitNameProperty); }
        }
        #endregion

        #region 物料包装规则 ItemPackageRule
        /// <summary>
        /// 物料包装规则Id
        /// </summary>
        [Label("物料包装规则")]
        public static readonly IRefIdProperty ItemPackageRuleIdProperty = P<ItemPackageRuleDetail>.RegisterRefId(e => e.ItemPackageRuleId, ReferenceType.Parent);

        /// <summary>
        /// 物料包装规则Id
        /// </summary>
        public double ItemPackageRuleId
        {
            get { return (double)GetRefId(ItemPackageRuleIdProperty); }
            set { SetRefId(ItemPackageRuleIdProperty, value); }
        }

        /// <summary>
        /// 物料包装规则
        /// </summary>
        public static readonly RefEntityProperty<ItemPackageRule> ItemPackageRuleProperty = P<ItemPackageRuleDetail>.RegisterRef(e => e.ItemPackageRule, ItemPackageRuleIdProperty);

        /// <summary>
        /// 物料包装规则
        /// </summary>
        public ItemPackageRule ItemPackageRule
        {
            get { return GetRefEntity(ItemPackageRuleProperty); }
            set { SetRefEntity(ItemPackageRuleProperty, value); }
        }
        #endregion

        #region 物料包装规则 ItemPackageRuleName
        /// <summary>
        /// 物料包装规则
        /// </summary>
        [Label("物料包装规则")]
        public static readonly Property<string> ItemPackageRuleNameProperty = P<ItemPackageRuleDetail>.RegisterView(e => e.ItemPackageRuleName, p => p.ItemPackageRule.Name);

        /// <summary>
        /// 物料包装规则
        /// </summary>
        public string ItemPackageRuleName
        {
            get { return this.GetProperty(ItemPackageRuleNameProperty); }
        }

        #endregion

        #region 物料类型 ItemPackageRuleItemType
        /// <summary>
        /// 物料类型
        /// </summary>
        [Label("物料类型")]
        public static readonly Property<ItemType> ItemPackageRuleItemTypeProperty = P<ItemPackageRuleDetail>.RegisterView(e => e.ItemPackageRuleItemType, p => p.ItemPackageRule.Item.Type);

        /// <summary>
        /// 注释
        /// </summary>
        public ItemType ItemPackageRuleItemType
        {
            get { return this.GetProperty(ItemPackageRuleItemTypeProperty); }
        }
        #endregion

        #region 打印模板名称 PrintTemplateName
        /// <summary>
        /// 打印模板名称
        /// </summary>
        [Label("打印模板名称")]
        public static readonly Property<string> PrintTemplateNameProperty = P<ItemPackageRuleDetail>.RegisterView(e => e.PrintTemplateName, p => p.PrintTemplate.FileName);

        /// <summary>
        /// 打印模板名称
        /// </summary>
        public string PrintTemplateName
        {
            get { return this.GetProperty(PrintTemplateNameProperty); }
        }
        #endregion

        #region 包装类型 PackageUnitType
        /// <summary>
        /// 包装类型
        /// </summary>
        [Label("包装类型")]
        public static readonly Property<PackageUnitType?> PackageUnitTypeProperty = P<ItemPackageRuleDetail>.RegisterView(e => e.PackageUnitType, p => p.PackageUnit.PackageUnitType);

        /// <summary>
        /// 包装类型
        /// </summary>
        public PackageUnitType? PackageUnitType
        {
            get { return this.GetProperty(PackageUnitTypeProperty); }
        }
        #endregion

        #region 物料Id ItemId
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料Id")]
        public static readonly Property<double> ItemIdProperty = P<ItemPackageRuleDetail>.RegisterView(e => e.ItemId, p => p.ItemPackageRule.ItemId);

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId
        {
            get { return this.GetProperty(ItemIdProperty); }
        }
        #endregion


        /// <summary>
        /// 属性变更事件
        /// </summary>
        /// <param name="propertyName">属性名</param>
        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            if ((propertyName == nameof(LevelQty) || (propertyName == nameof(Qty) && PackageUnit?.IsMasterUnit == true)) && ItemPackageRule != null)
            {
                //带重新运算的规则明细 
                var details = ItemPackageRule.ItemPackageRuleDetailList.Where(p => SortExtension.GetIndex(p) >= SortExtension.GetIndex(this)).OrderBy(f => SortExtension.GetIndex(f));
                foreach (var detail in details)
                {
                    var topLevel = ItemPackageRule.ItemPackageRuleDetailList.Where(p => SortExtension.GetIndex(p) < SortExtension.GetIndex(detail)).OrderByDescending(f => SortExtension.GetIndex(f)).FirstOrDefault();
                    if (topLevel == null) continue;
                    detail.Qty = detail.LevelQty * topLevel.Qty;
                }
            }
            if ((propertyName == SortExtension.INDEX_Property.Name) && ItemPackageRule != null)
            {
                ValidationPropertyChanged();
                var details = ItemPackageRule.ItemPackageRuleDetailList.OrderBy(f => SortExtension.GetIndex(f)).ToList();
                var index = 0;
                foreach (var detail in details)
                {
                    if (detail.PackageUnit?.IsMasterUnit != true && index > 0)
                    {
                        var topLevel = details[index - 1];
                        if (topLevel != null)
                            detail.Qty = detail.LevelQty * topLevel.Qty;
                    }
                    index++;
                }

            }
        }

        /// <summary>
        /// 属性变更验证方法
        /// </summary>
        private void ValidationPropertyChanged()
        {
            var firstDetail = ItemPackageRule.ItemPackageRuleDetailList.OrderBy(f => SortExtension.GetIndex(f)).FirstOrDefault();
            if (firstDetail != null && firstDetail.PackageUnit != null && firstDetail.PackageUnit?.IsMasterUnit != true)
            {
                throw new ValidationException("主单位必须是第一个包装单位!".L10nFormat());
            }
        }
    }

    /// <summary>
    /// 物料包装规则明细 实体配置
    /// </summary>
    internal class ItemPackageRuleDetailConfig : EntityConfig<ItemPackageRuleDetail>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("ITEM_IN_PKG_RULE_DTL").MapAllProperties();
            Meta.Property(ItemPackageRuleDetail.ItemPackageRuleIdProperty).ColumnMeta.HasIndex();
            Meta.Property(ItemPackageRuleDetail.PackageUnitIdProperty).ColumnMeta.HasIndex();
            Meta.EnableSort();
            Meta.EnablePhantoms();
        }
    }
}