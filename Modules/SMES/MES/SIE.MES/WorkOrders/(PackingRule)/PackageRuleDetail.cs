using SIE.Common.NumberRules;
using SIE.Common.Prints;
using SIE.Common.Sort;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ManagedProperty;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Packages;
using SIE.Packages.Packages;
using System;
using System.Linq;

namespace SIE.MES.WorkOrders
{
    /// <summary>
    /// 工单包装规则
    /// </summary>
    [ChildEntity, Serializable]
    [Label("工单包装规则")]
    public partial class WorkOrderPackageRuleDetail : DataEntity
    {
        #region 长 Length
        /// <summary>
        /// 长
        /// </summary>
        [MinValue(0)]
        [Label("长")]
        public static readonly Property<decimal> LengthProperty = P<WorkOrderPackageRuleDetail>.Register(e => e.Length);

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
        [Label("宽")]
        public static readonly Property<decimal> WidthProperty = P<WorkOrderPackageRuleDetail>.Register(e => e.Width);

        /// <summary>
        /// 宽
        /// </summary>
        public decimal Width
        {
            get { return GetProperty(WidthProperty); }
            set { SetProperty(WidthProperty, value); }
        }
        #endregion

        #region 产品数 Qty
        /// <summary>
        /// 产品数
        /// </summary>
        [MinValue(0)]
        [Label("产品数")]
        public static readonly Property<decimal> QtyProperty = P<WorkOrderPackageRuleDetail>.Register(e => e.Qty);

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
        public static readonly Property<decimal> LevelQtyProperty = P<WorkOrderPackageRuleDetail>.Register(e => e.LevelQty);

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
        public static readonly Property<bool> IsPackageProperty = P<WorkOrderPackageRuleDetail>.Register(e => e.IsPackage);

        /// <summary>
        /// 是否装箱
        /// </summary>
        public bool IsPackage
        {
            get { return GetProperty(IsPackageProperty); }
            set { SetProperty(IsPackageProperty, value); }
        }
        #endregion

        #region 是否出库标签 IsOutStockLabel
        /// <summary>
        /// 是否出库标签
        /// </summary>
        [Label("是否出库标签")]
        public static readonly Property<bool> IsOutStockLabelProperty = P<WorkOrderPackageRuleDetail>.Register(e => e.IsOutStockLabel);

        /// <summary>
        /// 是否出库标签
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
        public static readonly Property<string> DescriptionProperty = P<WorkOrderPackageRuleDetail>.Register(e => e.Description);

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
        [Label("重量")]
        public static readonly Property<decimal> WeightProperty = P<WorkOrderPackageRuleDetail>.Register(e => e.Weight);

        /// <summary>
        /// 重量
        /// </summary>
        public decimal Weight
        {
            get { return GetProperty(WeightProperty); }
            set { SetProperty(WeightProperty, value); }
        }
        #endregion

        #region 是否入库标签 IsInStockLabel
        /// <summary>
        /// 是否入库标签
        /// </summary>
        [Label("是否入库标签")]
        public static readonly Property<bool> IsInStockLabelProperty = P<WorkOrderPackageRuleDetail>.Register(e => e.IsInStockLabel);

        /// <summary>
        /// 是否入库标签
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
        [Label("高度")]
        public static readonly Property<decimal> HeightProperty = P<WorkOrderPackageRuleDetail>.Register(e => e.Height);

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
        [Label("体积")]
        public static readonly Property<decimal> VolumeProperty = P<WorkOrderPackageRuleDetail>.Register(e => e.Volume);

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
        /// 包装单位ID
        /// </summary>
        [Label("包装单位")]
        public static readonly IRefIdProperty PackageUnitIdProperty =
                 P<WorkOrderPackageRuleDetail>.RegisterRefId(e => e.PackageUnitId, ReferenceType.Normal);

        /// <summary>
        /// 包装单位ID
        /// </summary>
        public double PackageUnitId
        {
            get { return (double)this.GetRefId(PackageUnitIdProperty); }
            set { this.SetRefId(PackageUnitIdProperty, value); }
        }

        /// <summary>
        /// 包装单位
        /// </summary>
        public static readonly RefEntityProperty<PackingUnit> PackageUnitProperty =
            P<WorkOrderPackageRuleDetail>.RegisterRef(e => e.PackageUnit, PackageUnitIdProperty);

        /// <summary>
        /// 包装单位
        /// </summary>
        public PackingUnit PackageUnit
        {
            get { return this.GetRefEntity(PackageUnitProperty); }
            set { this.SetRefEntity(PackageUnitProperty, value); }
        }
        #endregion

        #region 编码规则 NumberRule
        /// <summary>
        /// 编码规则ID
        /// </summary>
        [Label("编码规则")]
        public static readonly IRefIdProperty NumberRuleIdProperty =
    P<WorkOrderPackageRuleDetail>.RegisterRefId(e => e.NumberRuleId, ReferenceType.Normal);

        /// <summary>
        /// 编码规则ID
        /// </summary>
        public double? NumberRuleId
        {
            get { return (double?)this.GetRefNullableId(NumberRuleIdProperty); }
            set { this.SetRefNullableId(NumberRuleIdProperty, value); }
        }

        /// <summary>
        /// 编码规则
        /// </summary>
        public static readonly RefEntityProperty<NumberRule> NumberRuleProperty =
            P<WorkOrderPackageRuleDetail>.RegisterRef(e => e.NumberRule, NumberRuleIdProperty);

        /// <summary>
        /// 编码规则
        /// </summary>
        public NumberRule NumberRule
        {
            get { return this.GetRefEntity(NumberRuleProperty); }
            set { this.SetRefEntity(NumberRuleProperty, value); }
        }
        #endregion

        #region 所属工单 WorkerOrder
        /// <summary>
        /// 工单ID
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty =
                    P<WorkOrderPackageRuleDetail>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Parent);

        /// <summary>
        /// 工单ID
        /// </summary>
        public double WorkOrderId
        {
            get { return (double)this.GetRefId(WorkOrderIdProperty); }
            set { this.SetRefId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> WorkerOrderProperty =
            P<WorkOrderPackageRuleDetail>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return this.GetRefEntity(WorkerOrderProperty); }
            set { this.SetRefEntity(WorkerOrderProperty, value); }
        }
        #endregion

        #region 打印模板 PrintTemplate
        /// <summary>
        /// 打印模板ID
        /// </summary>
        [Label("打印模板")]
        public static readonly IRefIdProperty PrintTemplateIdProperty =
            P<WorkOrderPackageRuleDetail>.RegisterRefId(e => e.PrintTemplateId, ReferenceType.Normal);

        /// <summary>
        /// 打印模板ID
        /// </summary>
        public double? PrintTemplateId
        {
            get { return (double?)this.GetRefNullableId(PrintTemplateIdProperty); }
            set { this.SetRefNullableId(PrintTemplateIdProperty, value); }
        }

        /// <summary>
        /// 打印模板
        /// </summary>
        public static readonly RefEntityProperty<PrintTemplate> PrintTemplateProperty =
            P<WorkOrderPackageRuleDetail>.RegisterRef(e => e.PrintTemplate, PrintTemplateIdProperty);

        /// <summary>
        /// 打印模板
        /// </summary>
        public PrintTemplate PrintTemplate
        {
            get { return this.GetRefEntity(PrintTemplateProperty); }
            set { this.SetRefEntity(PrintTemplateProperty, value); }
        }
        #endregion

        #region 是否打印 IsPrint
        /// <summary>
        /// 是否打印
        /// </summary>
        [Label("是否打印")]
        public static readonly Property<bool> IsPrintProperty = P<WorkOrderPackageRuleDetail>.Register(e => e.IsPrint);

        /// <summary>
        /// 是否打印
        /// </summary>
        public bool IsPrint
        {
            get { return this.GetProperty(IsPrintProperty); }
            set { this.SetProperty(IsPrintProperty, value); }
        }
        #endregion

        #region 物料包装规则明细 ItemPackingRuleDetail
        /// <summary>
        /// 物料包装规则明细Id
        /// </summary>
        [Label("属性名")]
        public static readonly IRefIdProperty DetailIdProperty =
            P<WorkOrderPackageRuleDetail>.RegisterRefId(e => e.DetailId, ReferenceType.Normal);

        /// <summary>
        /// 物料包装规则明细Id
        /// </summary>
        public double DetailId
        {
            get { return (double)this.GetRefId(DetailIdProperty); }
            set { this.SetRefId(DetailIdProperty, value); }
        }

        /// <summary>
        /// 物料包装规则明细
        /// </summary>
        public static readonly RefEntityProperty<ItemPackageRuleDetail> DetailProperty =
            P<WorkOrderPackageRuleDetail>.RegisterRef(e => e.Detail, DetailIdProperty);

        /// <summary>
        /// 物料包装规则明细
        /// </summary>
        public ItemPackageRuleDetail Detail
        {
            get { return this.GetRefEntity(DetailProperty); }
            set { this.SetRefEntity(DetailProperty, value); }
        }
        #endregion 

        #region 工单包装单位工序关系 WorkOrderProcessPackingUnitList
        /// <summary>
        /// 工单工序对应包装单位
        /// </summary>
        [Label("工单包装单位工序关系")]
        public static readonly ListProperty<EntityList<WorkOrderProcessPackingUnit>> WorkOrderProcessPackingUnitListProperty = P<WorkOrderPackageRuleDetail>.RegisterList(e => e.WorkOrderProcessPackingUnitList);

        /// <summary>
        /// 工单工序对应包装单位
        /// </summary>
        public EntityList<WorkOrderProcessPackingUnit> WorkOrderProcessPackingUnitList
        {
            get { return this.GetLazyList(WorkOrderProcessPackingUnitListProperty); }
        }
        #endregion

        #region BS注册视图  
        #region 包装单位名称 PackageUnitName
        /// <summary>
        /// 包装单位名称
        /// </summary>
        [Label("包装单位名称")]
        public static readonly Property<string> PackageUnitNameProperty = P<WorkOrderPackageRuleDetail>.RegisterView(e => e.PackageUnitName, e => e.PackageUnit.Name);

        /// <summary>
        /// 包装单位名称
        /// </summary>
        public string PackageUnitName
        {
            get { return GetProperty(PackageUnitNameProperty); }
            set { SetProperty(PackageUnitNameProperty, value); }
        }
        #endregion

        #region 规则名称 NumberRuleName
        /// <summary>
        /// 规则名称
        /// </summary>
        [Label("规则名称")]
        public static readonly Property<string> NumberRuleNameProperty = P<WorkOrderPackageRuleDetail>.RegisterView(e => e.NumberRuleName, e => e.NumberRule.Name);

        /// <summary>
        /// 规则名称
        /// </summary>
        public string NumberRuleName
        {
            get { return GetProperty(NumberRuleNameProperty); }
            set { SetProperty(NumberRuleNameProperty, value); }
        }
        #endregion

        #region 模板名称 TemplateName
        /// <summary>
        /// 模板名称
        /// </summary>
        [Label("模板名称")]
        public static readonly Property<string> TemplateNameProperty = P<WorkOrderPackageRuleDetail>.RegisterView(e => e.TemplateName, e => e.PrintTemplate.FileName);

        /// <summary>
        /// 模板名称
        /// </summary>
        public string TemplateName
        {
            get { return GetProperty(TemplateNameProperty); }
            set { SetProperty(TemplateNameProperty, value); }
        }
        #endregion

        #region 是否主单位 IsMasterUnit
        /// <summary>
        /// 是否主单位
        /// </summary>
        [Label("是否主单位")]
        public static readonly Property<bool> IsMasterUnitProperty = P<WorkOrderPackageRuleDetail>.RegisterView(e => e.IsMasterUnit, p => p.PackageUnit.IsMasterUnit);

        /// <summary>
        /// 是否主单位
        /// </summary>
        public bool IsMasterUnit
        {
            get { return this.GetProperty(IsMasterUnitProperty); }
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
            if ((propertyName == nameof(LevelQty) || propertyName == nameof(Qty)) && WorkOrder != null)
            {
                //带重新运算的规则明细 
                var details = WorkOrder.PackageRuleDetailList.Where(p => SortExtension.GetIndex(p) >= SortExtension.GetIndex(this)).OrderBy(f => SortExtension.GetIndex(f));
                foreach (var detail in details)
                {
                    var topLevel = WorkOrder.PackageRuleDetailList.Where(p => SortExtension.GetIndex(p) < SortExtension.GetIndex(detail)).OrderByDescending(f => SortExtension.GetIndex(f)).FirstOrDefault();
                    if (topLevel == null) continue;
                    detail.Qty = detail.LevelQty * topLevel.Qty;
                }
            }
        }
    }

    /// <summary>
    /// 包装采集规则明细 实体配置
    /// </summary>
    internal class PackageRuleDetailConfig : EntityConfig<WorkOrderPackageRuleDetail>
    {
        /// <summary>
        /// 配置数据库映射
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WO_PKG_RULE_DTL").MapAllProperties();
            Meta.Property(WorkOrderPackageRuleDetail.WorkOrderIdProperty).ColumnMeta.HasIndex();
            Meta.EnablePhantoms();
            Meta.EnableSort();
        }

        /// <summary>
        /// 增加实体验证规则
        /// </summary>
        /// <param name="rules">实体验证规则集合</param>
        protected override void AddValidations(IValidationDeclarer rules)
        {
            base.AddValidations(rules);
        }
    }

    /// <summary>
    /// 包装单位 实体配置
    /// </summary>
    internal class PackingUnitConfig : EntityConfig<PackingUnit>
    {
        /// <summary>
        /// 对实体验证规则的配置
        /// </summary>
        /// <param name="rules">实体验证规则集合</param>
        protected override void AddValidations(IValidationDeclarer rules)
        {
            base.AddValidations(rules);

            rules.Add(new NotUsedByReferenceRule()
            {
                ReferenceProperty = new ConcreteProperty(WorkOrderPackageRuleDetail.PackageUnitIdProperty, typeof(WorkOrderPackageRuleDetail)),
                MessageBuilder = (e, i) =>
                {
                    var s = e as PackingUnit;
                    return "无法删除包装单位：包装名称:{0}，已经被{1}使用了{2}次".L10nFormat(s.Name, "包装规则明细".L10N(), i);
                }
            }, new RuleMeta() { Scope = EntityStatusScopes.Delete });
        }
    }
}