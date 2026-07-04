using SIE.Common.NumberRules;
using SIE.Common.Prints;
using SIE.Common.Sort;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Packages.Packages;
using System;
using System.Linq;

namespace SIE.Packages
{
    /// <summary>
    /// 包装规则明细  
    /// </summary>
    [ChildEntity, Serializable]
    [Label("包装规则明细")]
    [DisplayMember(nameof(Id))]
    public partial class PackageRuleDetail : DataEntity
    {
        #region 长 Length
        /// <summary>
        /// 长
        /// </summary>
        [MinValue(0)]
        [Label("长(CM)")]
        public static readonly Property<decimal> LengthProperty = P<PackageRuleDetail>.Register(e => e.Length);

        /// <summary>
        /// 长
        /// </summary>
        public decimal Length
        {
            get { return GetProperty(LengthProperty); }
            set { SetProperty(LengthProperty, value); }
        }
        #endregion

        #region 宽 Width
        /// <summary>
        /// 宽
        /// </summary>
        [MinValue(0)]
        [Label("宽(CM)")]
        public static readonly Property<decimal> WidthProperty = P<PackageRuleDetail>.Register(e => e.Width);

        /// <summary>
        /// 宽
        /// </summary>
        public decimal Width
        {
            get { return GetProperty(WidthProperty); }
            set { SetProperty(WidthProperty, value); }
        }
        #endregion

        #region 主单位数量 Qty
        /// <summary>
        /// 主单位数量
        /// </summary>
        [MinValue(0)]
        [Label("产品数")]
        public static readonly Property<decimal> QtyProperty = P<PackageRuleDetail>.Register(e => e.Qty);

        /// <summary>
        /// 数量
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
        public static readonly Property<decimal> LevelQtyProperty = P<PackageRuleDetail>.Register(e => e.LevelQty);

        /// <summary>
        /// 包装数
        /// </summary>
        public decimal LevelQty
        {
            get { return this.GetProperty(LevelQtyProperty); }
            set { this.SetProperty(LevelQtyProperty, value); }
        }
        #endregion 

        #region 是否装箱 IsPackage
        /// <summary>
        /// 是否装箱
        /// </summary>
        [Label("是否装箱")]
        public static readonly Property<bool> IsPackageProperty = P<PackageRuleDetail>.Register(e => e.IsPackage);

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
        /// 是否出库
        /// </summary>
        [Label("是否出库")]
        public static readonly Property<bool> IsOutStockLabelProperty = P<PackageRuleDetail>.Register(e => e.IsOutStockLabel);

        /// <summary>
        /// 是否出库
        /// </summary>
        public bool IsOutStockLabel
        {
            get { return GetProperty(IsOutStockLabelProperty); }
            set { SetProperty(IsOutStockLabelProperty, value); }
        }
        #endregion

        #region 描述 Description
        /// <summary>
        /// 描述
        /// </summary>
        [Label("描述")]
        public static readonly Property<string> DescriptionProperty = P<PackageRuleDetail>.Register(e => e.Description);

        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get { return GetProperty(DescriptionProperty); }
            set { SetProperty(DescriptionProperty, value); }
        }
        #endregion

        #region 重量 Weight
        /// <summary>
        /// 重量
        /// </summary>
        [MinValue(0)]
        [Label("重量(KG)")]
        public static readonly Property<decimal> WeightProperty = P<PackageRuleDetail>.Register(e => e.Weight);

        /// <summary>
        /// 重量
        /// </summary>
        public decimal Weight
        {
            get { return GetProperty(WeightProperty); }
            set { SetProperty(WeightProperty, value); }
        }
        #endregion

        #region 是否入库 IsInStockLabel
        /// <summary>
        /// 是否入库
        /// </summary>
        [Label("是否入库")]
        public static readonly Property<bool> IsInStockLabelProperty = P<PackageRuleDetail>.Register(e => e.IsInStockLabel);

        /// <summary>
        /// 是否入库
        /// </summary>
        public bool IsInStockLabel
        {
            get { return GetProperty(IsInStockLabelProperty); }
            set { SetProperty(IsInStockLabelProperty, value); }
        }
        #endregion

        #region 高度 Height
        /// <summary>
        /// 高度
        /// </summary>
        [MinValue(0)]
        [Label("高度(CM)")]
        public static readonly Property<decimal> HeightProperty = P<PackageRuleDetail>.Register(e => e.Height);

        /// <summary>
        /// 高度
        /// </summary>
        public decimal Height
        {
            get { return GetProperty(HeightProperty); }
            set { SetProperty(HeightProperty, value); }
        }
        #endregion

        #region 体积 Volume
        /// <summary>
        /// 体积
        /// </summary>
        [MinValue(0)]
        [Label("体积(CM³)")]
        public static readonly Property<decimal> VolumeProperty = P<PackageRuleDetail>.Register(e => e.Volume);

        /// <summary>
        /// 体积
        /// </summary>
        public decimal Volume
        {
            get { return GetProperty(VolumeProperty); }
            set { SetProperty(VolumeProperty, value); }
        }
        #endregion

        #region 包装单位 PackageUnit
        /// <summary>
        /// 单位编码
        /// </summary>
        [Label("单位编码")]
        public static readonly IRefIdProperty PackageUnitIdProperty = P<PackageRuleDetail>.RegisterRefId(e => e.PackageUnitId, ReferenceType.Normal);

        /// <summary>
        /// 包装单位ID
        /// </summary>
        public double PackageUnitId
        {
            get { return (double)this.GetRefId(PackageUnitIdProperty); }
            set { this.SetRefId(PackageUnitIdProperty, value); }
        }

        /// <summary>
        /// 包装单位属性
        /// </summary>
        public static readonly RefEntityProperty<PackingUnit> PackageUnitProperty = P<PackageRuleDetail>.RegisterRef(e => e.PackageUnit, PackageUnitIdProperty);

        /// <summary>
        /// 包装单位
        /// </summary>
        public PackingUnit PackageUnit
        {
            get { return this.GetRefEntity(PackageUnitProperty); }
            set { this.SetRefEntity(PackageUnitProperty, value); }
        }
        #endregion

        #region 是否主单位 IsMasterUnit
        /// <summary>
        /// 是否主单位
        /// </summary>
        [Label("是否主单位")]
        public static readonly Property<bool> IsMasterUnitProperty = P<PackageRuleDetail>.RegisterView(e => e.IsMasterUnit, p => p.PackageUnit.IsMasterUnit);

        /// <summary>
        /// 单位名称
        /// </summary>
        public bool IsMasterUnit
        {
            get { return this.GetProperty(IsMasterUnitProperty); }
        }
        #endregion

        #region 包装规则 PackageRule
        /// <summary>
        /// 包装规则Id
        /// </summary>
        [Label("包装规则")]
        public static readonly IRefIdProperty PackageRuleIdProperty = P<PackageRuleDetail>.RegisterRefId(e => e.PackageRuleId, ReferenceType.Parent);

        /// <summary>
        /// 包装规则Id
        /// </summary>
        public double PackageRuleId
        {
            get { return (double)GetRefId(PackageRuleIdProperty); }
            set { SetRefId(PackageRuleIdProperty, value); }
        }

        /// <summary>
        /// 包装规则
        /// </summary>
        public static readonly RefEntityProperty<PackageRule> PackageRuleProperty = P<PackageRuleDetail>.RegisterRef(e => e.PackageRule, PackageRuleIdProperty);

        /// <summary>
        /// 包装规则
        /// </summary>
        public PackageRule PackageRule
        {
            get { return GetRefEntity(PackageRuleProperty); }
            set { SetRefEntity(PackageRuleProperty, value); }
        }
        #endregion

        #region 编码规则 NumberRule 
        /// <summary>
        /// 编码规则 IdProperty
        /// </summary>
        [Label("编码规则")]
        public static readonly IRefIdProperty NumberRuleIdProperty = P<PackageRuleDetail>.RegisterRefId(e => e.NumberRuleId, ReferenceType.Normal);

        /// <summary>
        /// Number规则
        /// </summary>
        public double? NumberRuleId
        {
            get { return (double?)this.GetRefNullableId(NumberRuleIdProperty); }
            set { this.SetRefNullableId(NumberRuleIdProperty, value); }
        }

        /// <summary>
        /// Number规则Property
        /// </summary>
        public static readonly RefEntityProperty<NumberRule> NumberRuleProperty = P<PackageRuleDetail>.RegisterRef(e => e.NumberRule, NumberRuleIdProperty);

        /// <summary>
        /// Number规则
        /// </summary>
        public NumberRule NumberRule
        {
            get { return this.GetRefEntity(NumberRuleProperty); }
            set { this.SetRefEntity(NumberRuleProperty, value); }
        }
        #endregion

        #region 是否打印 IsPrint
        /// <summary>
        /// 是否打印
        /// </summary>
        [Label("是否打印")]
        public static readonly Property<bool> IsPrintProperty = P<PackageRuleDetail>.Register(e => e.IsPrint);

        /// <summary>
        /// 是否打印
        /// </summary>
        public bool IsPrint
        {
            get { return this.GetProperty(IsPrintProperty); }
            set { this.SetProperty(IsPrintProperty, value); }
        }
        #endregion

        #region 打印模板 PrintTemplate
        /// <summary>
        /// 打印模板
        /// </summary>
        [Label("打印模板")]

        public static readonly IRefIdProperty PrintTemplateIdProperty = P<PackageRuleDetail>.RegisterRefId(e => e.PrintTemplateId, ReferenceType.Normal);

        /// <summary>
        /// 打印模板
        /// </summary>
        public double? PrintTemplateId
        {
            get { return (double?)this.GetRefNullableId(PrintTemplateIdProperty); }
            set { this.SetRefNullableId(PrintTemplateIdProperty, value); }
        }

        /// <summary>
        /// 打印模板
        /// </summary>
        public static readonly RefEntityProperty<PrintTemplate> PrintTemplateProperty = P<PackageRuleDetail>.RegisterRef(e => e.PrintTemplate, PrintTemplateIdProperty);

        /// <summary>
        /// 打印模板
        /// </summary>
        public PrintTemplate PrintTemplate
        {
            get { return this.GetRefEntity(PrintTemplateProperty); }
            set { this.SetRefEntity(PrintTemplateProperty, value); }
        }
        #endregion

        #region 是否序列 IsSequence
        /// <summary>
        /// 是否序列
        /// </summary>
        [Label("是否序列")]
        public static readonly Property<bool> IsSequenceProperty = P<PackageRuleDetail>.Register(e => e.IsSequence);

        /// <summary>
        /// 是否序列
        /// </summary>
        public bool IsSequence
        {
            get { return this.GetProperty(IsSequenceProperty); }
            set { this.SetProperty(IsSequenceProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 单位名称 PackageUnitName
        /// <summary>
        /// 单位名称
        /// </summary>
        [Label("单位名称")]
        public static readonly Property<string> PackageUnitNameProperty = P<PackageRuleDetail>.RegisterView(e => e.PackageUnitName, p => p.PackageUnit.Name);

        /// <summary>
        /// 单位名称
        /// </summary>
        public string PackageUnitName
        {
            get { return this.GetProperty(PackageUnitNameProperty); }
        }
        #endregion  

        #region 包装规则名称 PackageRuleName
        /// <summary>
        /// 包装规则名称
        /// </summary>
        [Label("包装规则名称")]
        public static readonly Property<string> PackageRuleNameProperty = P<PackageRuleDetail>.RegisterView(e => e.PackageRuleName, p => p.PackageRule.Name);

        /// <summary>
        /// 包装规则名称
        /// </summary>
        public string PackageRuleName
        {
            get { return this.GetProperty(PackageRuleNameProperty); }
        }
        #endregion

        #region 包装类型 PackageUnitType
        /// <summary>
        /// 包装类型
        /// </summary>
        [Label("包装类型")]
        public static readonly Property<PackageUnitType?> PackageUnitTypeProperty = P<PackageRuleDetail>.RegisterView(e => e.PackageUnitType, p => p.PackageUnit.PackageUnitType);

        /// <summary>
        /// 包装类型
        /// </summary>
        public PackageUnitType? PackageUnitType
        {
            get { return this.GetProperty(PackageUnitTypeProperty); }
        }
        #endregion

        #endregion

        /// <summary>
        /// 属性变更事件
        /// </summary>
        /// <param name="propertyName">属性名</param>
        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == nameof(LevelQty) && PackageRule != null)
            {
                //带重新运算的规则明细 
                var details = PackageRule.PackageRuleDetailList.Where(p => SortExtension.GetIndex(p) >= SortExtension.GetIndex(this)).OrderBy(f => SortExtension.GetIndex(f));
                foreach (PackageRuleDetail detail in details)
                {
                    var topLevel = PackageRule.PackageRuleDetailList.Where(p => SortExtension.GetIndex(p) < SortExtension.GetIndex(detail)).OrderByDescending(f => SortExtension.GetIndex(f)).FirstOrDefault();
                    if (topLevel == null) continue;
                    detail.Qty = detail.LevelQty * topLevel.Qty;
                }
            }

            if (propertyName == SortExtension.INDEX_Property.Name && PackageRule != null)
            {
                ValidationPropertyChanged();
                var details = PackageRule.PackageRuleDetailList.OrderBy(f => SortExtension.GetIndex(f)).ToList();
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
            var firstDetail = PackageRule.PackageRuleDetailList.OrderBy(f => SortExtension.GetIndex(f)).FirstOrDefault();
            if (firstDetail != null && firstDetail.PackageUnit != null && firstDetail.PackageUnit?.IsMasterUnit != true)
            {
                throw new ValidationException("主单位必须是第一个包装单位!".L10nFormat());
            }
        }
    }

    /// <summary>
    /// 包装规则明细 实体配置
    /// </summary>
    internal class PackageRuleDetailConfig : EntityConfig<PackageRuleDetail>
    {
        /// <summary>
        /// 实体配置
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("ITEM_PKG_RULE_DTL").MapAllProperties();
            Meta.Property(PackageRuleDetail.PackageRuleIdProperty).ColumnMeta.HasIndex();
            Meta.EnableSort();
            Meta.EnablePhantoms();
        }
    }
}