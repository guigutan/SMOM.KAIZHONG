using SIE.Common.Configs;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.SpareParts.Configs;
using SIE.EMS.SpareParts.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.SpareParts
{
    /// <summary>
    /// 备件库存
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(StoreSummaryCriteria))]
    [EntityWithConfig(typeof(BatchNumberNoConfig))]
    [Label("备件库存")]
    public partial class StoreSummary : DataEntity
    {
        #region 备件基础数据 SparePart
        /// <summary>
        /// 备件基础数据Id
        /// </summary>
        [Label("备件编码")]
        public static readonly IRefIdProperty SparePartIdProperty = P<StoreSummary>.RegisterRefId(e => e.SparePartId, ReferenceType.Normal);

        /// <summary>
        /// 备件基础数据Id
        /// </summary>
        public double SparePartId
        {
            get { return (double)GetRefId(SparePartIdProperty); }
            set { SetRefId(SparePartIdProperty, value); }
        }

        /// <summary>
        /// 备件基础数据
        /// </summary>
        public static readonly RefEntityProperty<SparePart> SparePartProperty = P<StoreSummary>.RegisterRef(e => e.SparePart, SparePartIdProperty);

        /// <summary>
        /// 备件基础数据
        /// </summary>
        public SparePart SparePart
        {
            get { return GetRefEntity(SparePartProperty); }
            set { SetRefEntity(SparePartProperty, value); }
        }
        #endregion

        #region 不良品数 RotNumber
        /// <summary>
        /// 不良品数
        /// </summary>
        [Label("不良品数")]
        public static readonly Property<int> RotNumberProperty = P<StoreSummary>.Register(e => e.RotNumber);

        /// <summary>
        /// 不良品数
        /// </summary>
        public int RotNumber
        {
            get { return GetProperty(RotNumberProperty); }
            set { SetProperty(RotNumberProperty, value); }
        }
        #endregion

        #region 可用库存 GoodNumber
        /// <summary>
        /// 可用库存
        /// </summary>
        [Label("可用库存")]
        public static readonly Property<int> GoodNumberProperty = P<StoreSummary>.Register(e => e.GoodNumber);

        /// <summary>
        /// 可用库存
        /// </summary>
        public int GoodNumber
        {
            get { return GetProperty(GoodNumberProperty); }
            set { SetProperty(GoodNumberProperty, value); }
        }
        #endregion

        #region 总库存 SumNumber
        /// <summary>
        /// 总库存
        /// </summary>
        [Label("总库存")]
        public static readonly Property<int> SumNumberProperty = P<StoreSummary>.Register(e => e.SumNumber);

        /// <summary>
        /// 总库存
        /// </summary>
        public int SumNumber
        {
            get { return GetProperty(SumNumberProperty); }
            set { SetProperty(SumNumberProperty, value); }
        }
        #endregion

        #region 平均成本 AverageCost
        /// <summary>
        /// 平均成本
        /// </summary>
        [Label("平均成本")]
        public static readonly Property<decimal> AverageCostProperty = P<StoreSummary>.Register(e => e.AverageCost);

        /// <summary>
        /// 平均成本
        /// </summary>
        public decimal AverageCost
        {
            get { return this.GetProperty(AverageCostProperty); }
            set { this.SetProperty(AverageCostProperty, value); }
        }
        #endregion

        #region 仓库明细 StoreSummaryWarehouseList
        /// <summary>
        /// 仓库明细
        /// </summary>
        [Label("仓库明细")]
        public static readonly ListProperty<EntityList<StoreSummaryWarehouse>> StoreSummaryWarehouseListProperty = P<StoreSummary>.RegisterList(e => e.StoreSummaryWarehouseList);

        /// <summary>
        /// 仓库明细
        /// </summary>
        public EntityList<StoreSummaryWarehouse> StoreSummaryWarehouseList
        {
            get { return this.GetLazyList(StoreSummaryWarehouseListProperty); }
        }
        #endregion

        #region 库位明细 StoreSummaryStockList
        /// <summary>
        /// 库位明细
        /// </summary>
        [Label("库位明细")]
        public static readonly ListProperty<EntityList<StoreSummaryStock>> StoreSummaryStockListProperty = P<StoreSummary>.RegisterList(e => e.StoreSummaryStockList);

        /// <summary>
        /// 库位明细
        /// </summary>
        public EntityList<StoreSummaryStock> StoreSummaryStockList
        {
            get { return this.GetLazyList(StoreSummaryStockListProperty); }
        }
        #endregion

        #region 物料编码明细 StoreSummaryLocationList
        /// <summary>
        /// 物料编码明细
        /// </summary>
        [Label("物料编码明细")]
        public static readonly ListProperty<EntityList<StoreSummaryLocation>> StoreSummaryLocationListProperty = P<StoreSummary>.RegisterList(e => e.StoreSummaryLocationList);

        /// <summary>
        /// 物料编码明细
        /// </summary>
        public EntityList<StoreSummaryLocation> StoreSummaryLocationList
        {
            get { return this.GetLazyList(StoreSummaryLocationListProperty); }
        }
        #endregion

        #region 批次明细 StoreSummaryLotList
        /// <summary>
        /// 批次明细
        /// </summary>
        [Label("批次明细")]
        public static readonly ListProperty<EntityList<StoreSummaryLot>> StoreSummaryDepotListProperty = P<StoreSummary>.RegisterList(e => e.StoreSummaryDepotList);

        /// <summary>
        /// 批次明细
        /// </summary>
        public EntityList<StoreSummaryLot> StoreSummaryDepotList
        {
            get { return this.GetLazyList(StoreSummaryDepotListProperty); }
        }
        #endregion

        #region 序列号明细 StoreSummaryDetailList
        /// <summary>
        /// 序列号明细
        /// </summary>
        [Label("序列号明细")]
        public static readonly ListProperty<EntityList<StoreSummaryDetail>> StoreSummaryDetailListProperty = P<StoreSummary>.RegisterList(e => e.StoreSummaryDetailList);

        /// <summary>
        /// 序列号明细
        /// </summary>
        public EntityList<StoreSummaryDetail> StoreSummaryDetailList
        {
            get { return this.GetLazyList(StoreSummaryDetailListProperty); }
        }
        #endregion

        #region 视图属性
        #region 备件编码 SparePartCode
        /// <summary>
        /// 备件编码
        /// </summary>
        [Label("备件编码")]
        public static readonly Property<string> SparePartCodeProperty = P<StoreSummary>.RegisterView(e => e.SparePartCode, p => p.SparePart.SparePartCode);

        /// <summary>
        /// 备件编码
        /// </summary>
        public string SparePartCode
        {
            get { return GetProperty(SparePartCodeProperty); }
            set { SetProperty(SparePartCodeProperty, value); }
        }
        #endregion

        #region 备件名称 SparePartName
        /// <summary>
        /// 备件名称
        /// </summary>
        [Label("备件名称")]
        public static readonly Property<string> SparePartNameProperty = P<StoreSummary>.RegisterView(e => e.SparePartName, p => p.SparePart.SparePartName);

        /// <summary>
        /// 备件名称
        /// </summary>
        public string SparePartName
        {
            get { return GetProperty(SparePartNameProperty); }
            set { SetProperty(SparePartNameProperty, value); }
        }
        #endregion

        #region 安全库存 SafeStock
        /// <summary>
        /// 安全库存
        /// </summary>
        [Label("安全库存")]
        public static readonly Property<int?> SafeStockProperty = P<StoreSummary>.RegisterView(e => e.SafeStock,p=>p.SparePart.SafeStock);

        /// <summary>
        /// 安全库存
        /// </summary>
        public int? SafeStock
        {
            get { return GetProperty(SafeStockProperty); }
            set { SetProperty(SafeStockProperty, value); }
        }
        #endregion

        #region 规格型号 Specification
        /// <summary>
        /// 规格型号
        /// </summary>
        [Label("规格型号")]
        public static readonly Property<string> SpecificationProperty = P<StoreSummary>.RegisterView(e => e.Specification, p => p.SparePart.Specification);

        /// <summary>
        /// 规格型号
        /// </summary>
        public string Specification
        {
            get { return GetProperty(SpecificationProperty); }
            set { SetProperty(SpecificationProperty, value); }
        }
        #endregion

        #region 备件类型 SparePartType
        /// <summary>
        /// 备件类型
        /// </summary>
        [Label("备件类型")]
        public static readonly Property<string> SparePartTypeProperty = P<StoreSummary>.RegisterView(e => e.SparePartType, p => p.SparePart.ItemCategory.Name);

        /// <summary>
        /// 备件类型
        /// </summary>
        public string SparePartType
        {
            get { return GetProperty(SparePartTypeProperty); }
            set { SetProperty(SparePartTypeProperty, value); }
        }
        #endregion

        #region 单位 Unit
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> UnitProperty = P<StoreSummary>.RegisterView(e => e.Unit, p => p.SparePart.Unit.Name);
        /// <summary>
        /// 单位
        /// </summary>
        public string Unit
        {
            get { return GetProperty(UnitProperty); }
            set { SetProperty(UnitProperty, value); }
        }
        #endregion

        #region 分类层级 ItemCategory
        /// <summary>
        /// 分类层级
        /// </summary>
        [Label("分类层级")]
        public static readonly Property<string> ItemCategoryProperty = P<StoreSummary>.RegisterView(e => e.ItemCategory, p => p.SparePart.ItemCategory.Name);

        /// <summary>
        /// 分类层级
        /// </summary>
        public string ItemCategory
        {
            get { return this.GetProperty(ItemCategoryProperty); }
        }
        #endregion

        #region 类型 SpartType
        /// <summary>
        /// 类型
        /// </summary>
        [Label("类型")]
        public static readonly Property<SparePartType> SpartTypeProperty = P<StoreSummary>.RegisterView(e => e.SpartType, p => p.SparePart.SpartType);

        /// <summary>
        /// 类型
        /// </summary>
        public SparePartType SpartType
        {
            get { return this.GetProperty(SpartTypeProperty); }
        }
        #endregion

        #region 管控方式 ControlMethod
        /// <summary>
        /// 管控方式
        /// </summary>
        [Label("管控方式")]
        public static readonly Property<ControlMethod> ControlMethodProperty = P<StoreSummary>.RegisterView(e => e.ControlMethod, p => p.SparePart.ControlMethod);

        /// <summary>
        /// 管控方式
        /// </summary>
        public ControlMethod ControlMethod
        {
            get { return this.GetProperty(ControlMethodProperty); }
        }
        #endregion

        #region 以旧换新 IsReplacement
        /// <summary>
        /// 以旧换新
        /// </summary>
        [Label("以旧换新")]
        public static readonly Property<bool> IsReplacementProperty = P<StoreSummary>.RegisterView(e => e.IsReplacement, p => p.SparePart.IsReplacement);

        /// <summary>
        /// 以旧换新
        /// </summary>
        public bool IsReplacement
        {
            get { return this.GetProperty(IsReplacementProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 备件库存查询 实体配置
    /// </summary>
    internal class StoreSummaryConfig : EntityConfig<StoreSummary>
    {
        protected override void AddValidations(IValidationDeclarer rules)
        {
            base.AddValidations(rules);
            rules.AddRule(StoreSummary.GoodNumberProperty, new NumberRangeRule() { Min = 0 });
            rules.AddRule(StoreSummary.SumNumberProperty, new NumberRangeRule() { Min = 0 });
        }

        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_SPARE_PART_STR_SUMR").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
