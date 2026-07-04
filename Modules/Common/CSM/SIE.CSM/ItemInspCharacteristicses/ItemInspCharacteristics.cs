using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.CSM.ItemInspCharacteristicses
{
    /// <summary>
    /// 物料检验特性维护
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(ItemInspCharacteristicsCriteria))]
    //[CriteriaQuery]
    [Label("物料检验特性维护")]
    //[DisplayMember(nameof())]
    public partial class ItemInspCharacteristics : DataEntity
    {
        #region 物料周期检 RecurringInspection
        /// <summary>
        /// 物料周期检
        /// </summary>
        [Label("物料周期检")]
        public static readonly Property<bool> RecurringInspectionProperty = P<ItemInspCharacteristics>.Register(e => e.RecurringInspection);

        /// <summary>
        /// 物料周期检
        /// </summary>
        public bool RecurringInspection
        {
            get { return GetProperty(RecurringInspectionProperty); }
            set { SetProperty(RecurringInspectionProperty, value); }
        }
        #endregion

        #region 间隔周期 IntervalPeriod
        /// <summary>
        /// 间隔周期
        /// </summary>
        [Label("间隔周期")]
        public static readonly Property<int?> IntervalPeriodProperty = P<ItemInspCharacteristics>.Register(e => e.IntervalPeriod);

        /// <summary>
        /// 间隔周期
        /// </summary>
        public int? IntervalPeriod
        {
            get { return GetProperty(IntervalPeriodProperty); }
            set { SetProperty(IntervalPeriodProperty, value); }
        }
        #endregion

        #region 驻厂检 FactoryInspection
        /// <summary>
        /// 驻厂检
        /// </summary>
        [Label("驻厂检")]
        public static readonly Property<bool> FactoryInspectionProperty = P<ItemInspCharacteristics>.Register(e => e.FactoryInspection);

        /// <summary>
        /// 驻厂检
        /// </summary>
        public bool FactoryInspection
        {
            get { return GetProperty(FactoryInspectionProperty); }
            set { SetProperty(FactoryInspectionProperty, value); }
        }
        #endregion

        #region 确认检 ConfirmInspection
        /// <summary>
        /// 确认检
        /// </summary>
        [Label("确认检")]
        public static readonly Property<bool> ConfirmInspectionProperty = P<ItemInspCharacteristics>.Register(e => e.ConfirmInspection);

        /// <summary>
        /// 确认检
        /// </summary>
        public bool ConfirmInspection
        {
            get { return GetProperty(ConfirmInspectionProperty); }
            set { SetProperty(ConfirmInspectionProperty, value); }
        }
        #endregion

        #region 强制供方出货 ForceSupplierShipBill
        /// <summary>
        /// 强制供方出货
        /// </summary>
        [Label("强制供方出货")]
        public static readonly Property<bool> ForceSupplierShipBillProperty = P<ItemInspCharacteristics>.Register(e => e.ForceSupplierShipBill);

        /// <summary>
        /// 强制供方出货
        /// </summary>
        public bool ForceSupplierShipBill
        {
            get { return GetProperty(ForceSupplierShipBillProperty); }
            set { SetProperty(ForceSupplierShipBillProperty, value); }
        }
        #endregion

        #region 供方状态 SupplierState
        /// <summary>
        /// 供方状态
        /// </summary>
        [Label("供方状态")]
        public static readonly Property<State?> SupplierStateProperty = P<ItemInspCharacteristics>.Register(e => e.SupplierState);

        /// <summary>
        /// 供方状态
        /// </summary>
        public State? SupplierState
        {
            get { return GetProperty(SupplierStateProperty); }
            set { SetProperty(SupplierStateProperty, value); }
        }
        #endregion

        #region 周期类型 PeriodType
        /// <summary>
        /// 周期类型
        /// </summary>
        [Label("周期类型")]
        public static readonly Property<PeriodType?> PeriodTypeProperty = P<ItemInspCharacteristics>.Register(e => e.PeriodType);

        /// <summary>
        /// 周期类型
        /// </summary>
        public PeriodType? PeriodType
        {
            get { return GetProperty(PeriodTypeProperty); }
            set { SetProperty(PeriodTypeProperty, value); }
        }
        #endregion

        #region 检验开始时间 InspectionStartTime
        /// <summary>
        /// 检验开始时间
        /// </summary>
        [Label("检验开始时间")]
        public static readonly Property<DateTime?> InspectDateBeginProperty = P<ItemInspCharacteristics>.Register(e => e.InspectDateBegin);

        /// <summary>
        /// 检验开始时间
        /// </summary>
        public DateTime? InspectDateBegin
        {
            get { return GetProperty(InspectDateBeginProperty); }
            set { SetProperty(InspectDateBeginProperty, value); }
        }
        #endregion

        #region 已跳过批次数 SkipBatches
        /// <summary>
        /// 已跳过批次数
        /// </summary>
        [Label("已跳过批次数")]
        public static readonly Property<int?> SkipBatchesProperty = P<ItemInspCharacteristics>.Register(e => e.SkipBatches);

        /// <summary>
        /// 已跳过批次数
        /// </summary>
        public int? SkipBatches
        {
            get { return GetProperty(SkipBatchesProperty); }
            set { SetProperty(SkipBatchesProperty, value); }
        }
        #endregion

        #region 供应商 Supplier
        /// <summary>
        /// 供应商Id
        /// </summary>
        [Label("供应商编码")]
        public static readonly IRefIdProperty SupplierIdProperty = P<ItemInspCharacteristics>.RegisterRefId(e => e.SupplierId, ReferenceType.Normal);

        /// <summary>
        /// 供应商Id
        /// </summary>
        public double SupplierId
        {
            get { return (double)GetRefId(SupplierIdProperty); }
            set { SetRefId(SupplierIdProperty, value); }
        }

        /// <summary>
        /// 供应商
        /// </summary>
        public static readonly RefEntityProperty<Supplier> SupplierProperty = P<ItemInspCharacteristics>.RegisterRef(e => e.Supplier, SupplierIdProperty);

        /// <summary>
        /// 供应商
        /// </summary>
        public Supplier Supplier
        {
            get { return GetRefEntity(SupplierProperty); }
            set { SetRefEntity(SupplierProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty = P<ItemInspCharacteristics>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId
        {
            get { return (double)GetRefId(ItemIdProperty); }
            set { SetRefId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty = P<ItemInspCharacteristics>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 免检 InspectionFree
        /// <summary>
        /// 免检
        /// </summary>
        [Label("免检")]
        public static readonly Property<bool> InspectionFreeProperty = P<ItemInspCharacteristics>.Register(e => e.InspectionFree);

        /// <summary>
        /// 免检
        /// </summary>
        public bool InspectionFree
        {
            get { return GetProperty(InspectionFreeProperty); }
            set { SetProperty(InspectionFreeProperty, value); }
        }
        #endregion

        #region 免检生效时间 EffectiveStartTime
        /// <summary>
        /// 免检生效时间
        /// </summary>
        [Label("免检生效时间")]
        public static readonly Property<DateTime?> EffectiveStartTimeProperty = P<ItemInspCharacteristics>.Register(e => e.EffectiveStartTime);

        /// <summary>
        /// 免检生效时间
        /// </summary>
        public DateTime? EffectiveStartTime
        {
            get { return GetProperty(EffectiveStartTimeProperty); }
            set { SetProperty(EffectiveStartTimeProperty, value); }
        }
        #endregion

        #region 免检失效时间 EffectiveEndTime
        /// <summary>
        /// 免检失效时间
        /// </summary>
        [Label("免检失效时间")]
        public static readonly Property<DateTime?> EffectiveEndTimeProperty = P<ItemInspCharacteristics>.Register(e => e.EffectiveEndTime);

        /// <summary>
        /// 免检失效时间
        /// </summary>
        public DateTime? EffectiveEndTime
        {
            get { return GetProperty(EffectiveEndTimeProperty); }
            set { SetProperty(EffectiveEndTimeProperty, value); }
        }
        #endregion

        #region 注册视图属性(关联实体属性平铺显示，一般用于Web)

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<ItemInspCharacteristics>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }
        #endregion

        #region 供应商名称 SupplierName
        /// <summary>
        /// 供应商名称
        /// </summary>
        [Label("供应商名称")]
        public static readonly Property<string> SupplierNameProperty = P<ItemInspCharacteristics>.RegisterView(e => e.SupplierName, p => p.Supplier.Name);

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName
        {
            get { return this.GetProperty(SupplierNameProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 物料检验特性维护 实体配置
    /// </summary>
    internal class ItemInspCharacteristicsConfig : EntityConfig<ItemInspCharacteristics>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("ITEM_INSP_CHARACTER").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}