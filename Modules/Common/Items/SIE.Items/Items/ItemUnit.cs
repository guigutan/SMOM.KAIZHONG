using SIE.Domain;
using SIE.Items.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Items
{
    /// <summary>
    /// 转换单位
    /// </summary>
    [RootEntity, Serializable]
    [Label("转换单位")]
    [DisplayMember(nameof(Id))]
    public partial class ItemUnit : DataEntity
    {
        #region 分子 Numerator
        /// <summary>
        /// 分子
        /// </summary>
        [Label("分子")]
        [MinValue(1)]
        [Required]
        public static readonly Property<int> NumeratorProperty = P<ItemUnit>.Register(e => e.Numerator);

        /// <summary>
        /// 分子
        /// </summary>
        public int Numerator
        {
            get { return GetProperty(NumeratorProperty); }
            set { SetProperty(NumeratorProperty, value); }
        }
        #endregion

        #region 分母 Denominator
        /// <summary>
        /// 分母
        /// </summary>
        [Label("分母")]
        [MinValue(1)]
        [Required]
        public static readonly Property<int> DenominatorProperty = P<ItemUnit>.Register(e => e.Denominator);

        /// <summary>
        /// 分母
        /// </summary>
        public int Denominator
        {
            get { return GetProperty(DenominatorProperty); }
            set { SetProperty(DenominatorProperty, value); }
        }
        #endregion 

        #region 辅助单位 Unit
        /// <summary>
        /// 单位Id
        /// </summary>
        [Label("辅助单位")]
        public static readonly IRefIdProperty UnitIdProperty = P<ItemUnit>.RegisterRefId(e => e.UnitId, ReferenceType.Normal);

        /// <summary>
        /// 单位Id
        /// </summary>
        public double UnitId
        {
            get { return (double)GetRefId(UnitIdProperty); }
            set { SetRefId(UnitIdProperty, value); }
        }

        /// <summary>
        /// 辅助单位
        /// </summary>
        [Label("辅助单位")]
        public static readonly RefEntityProperty<Unit> UnitProperty = P<ItemUnit>.RegisterRef(e => e.Unit, UnitIdProperty);

        /// <summary>
        /// 辅助单位
        /// </summary>
        public Unit Unit
        {
            get { return GetRefEntity(UnitProperty); }
            set { SetRefEntity(UnitProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料编码")]
        public static readonly IRefIdProperty ItemIdProperty = P<ItemUnit>.RegisterRefId(e => e.ItemId, ReferenceType.Parent);

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
            P<ItemUnit>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return this.GetRefEntity(ItemProperty); }
            set { this.SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 默认辅助单位 IsDefault
        /// <summary>
        /// 默认辅助单位
        /// </summary>
        [Label("默认辅助单位")]
        public static readonly Property<bool> IsDefaultProperty = P<ItemUnit>.Register(e => e.IsDefault);

        /// <summary>
        /// 默认辅助单位
        /// </summary>
        public bool IsDefault
        {
            get { return GetProperty(IsDefaultProperty); }
            set { SetProperty(IsDefaultProperty, value); }
        }
        #endregion

        #region 单位来源 UnitSource
        /// <summary>
        /// 单位来源
        /// </summary>
        [Label("单位来源")]
        public static readonly Property<UnitSource> UnitSourceProperty = P<ItemUnit>.Register(e => e.UnitSource);

        /// <summary>
        /// 单位来源
        /// </summary>
        public UnitSource UnitSource
        {
            get { return GetProperty(UnitSourceProperty); }
            set { SetProperty(UnitSourceProperty, value); }
        }
        #endregion

        #region 主单位 MainUnit
        /// <summary>
        /// 主单位Id
        /// </summary>
        [Label("主单位")]
        public static readonly IRefIdProperty MainUnitIdProperty =
            P<ItemUnit>.RegisterRefId(e => e.MainUnitId, ReferenceType.Normal);

        /// <summary>
        /// 主单位Id
        /// </summary>
        public double MainUnitId
        {
            get { return (double)this.GetRefId(MainUnitIdProperty); }
            set { this.SetRefId(MainUnitIdProperty, value); }
        }

        /// <summary>
        /// 主单位
        /// </summary>
        public static readonly RefEntityProperty<Unit> MainUnitProperty =
            P<ItemUnit>.RegisterRef(e => e.MainUnit, MainUnitIdProperty);

        /// <summary>
        /// 主单位
        /// </summary>
        public Unit MainUnit
        {
            get { return this.GetRefEntity(MainUnitProperty); }
            set { this.SetRefEntity(MainUnitProperty, value); }
        }
        #endregion

        #region 基准单位 IsBaseUnit
        /// <summary>
        /// 基准单位
        /// </summary>
        [Label("基准单位")]
        public static readonly Property<bool> IsBaseUnitProperty = P<ItemUnit>.Register(e => e.IsBaseUnit);

        /// <summary>
        /// 基准单位
        /// </summary>
        public bool IsBaseUnit
        {
            get { return this.GetProperty(IsBaseUnitProperty); }
            set { this.SetProperty(IsBaseUnitProperty, value); }
        }
        #endregion

        #region 是否初始化 IsInit
        /// <summary>
        /// 是否初始化
        /// </summary>
        [Label("是否初始化")]
        public static readonly Property<bool> IsInitProperty = P<ItemUnit>.Register(e => e.IsInit);

        /// <summary>
        /// 是否初始化
        /// </summary>
        public bool IsInit
        {
            get { return GetProperty(IsInitProperty); }
            set { SetProperty(IsInitProperty, value); }
        }
        #endregion

        #region ERP分子 ErpNumerator
        /// <summary>
        /// ERP分子
        /// </summary>
        [Label("ERP分子")]
        public static readonly Property<decimal?> ErpNumeratorProperty = P<ItemUnit>.Register(e => e.ErpNumerator);

        /// <summary>
        /// ERP分子
        /// </summary>
        public decimal? ErpNumerator
        {
            get { return this.GetProperty(ErpNumeratorProperty); }
            set { this.SetProperty(ErpNumeratorProperty, value); }
        }
        #endregion

        #region ERP分母 ErpDenominator
        /// <summary>
        /// ERP分母
        /// </summary>
        [Label("ERP分母")]
        public static readonly Property<decimal?> ErpDenominatorProperty = P<ItemUnit>.Register(e => e.ErpDenominator);

        /// <summary>
        /// ERP分母
        /// </summary>
        public decimal? ErpDenominator
        {
            get { return this.GetProperty(ErpDenominatorProperty); }
            set { this.SetProperty(ErpDenominatorProperty, value); }
        }
        #endregion

        #region RegisterView注册视图属性(关联实体属性平铺显示)

        #region 类型 UnitType
        /// <summary>
        /// 类型
        /// </summary>
        [Label("类型")]
        public static readonly Property<string> UnitTypeProperty = P<ItemUnit>.RegisterView(e => e.UnitType, p => p.Unit.Type);

        /// <summary>
        /// 类型
        /// </summary>
        public string UnitType
        {
            get { return this.GetProperty(UnitTypeProperty); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<ItemUnit>.RegisterView(e => e.ItemCode, p => p.Item.Code);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<ItemUnit>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }
        #endregion

        #region 物料单位编码 ItemUnitCode
        /// <summary>
        /// 物料单位编码
        /// </summary>
        [Label("物料单位编码")]
        public static readonly Property<string> ItemUnitCodeProperty = P<ItemUnit>.RegisterView(e => e.ItemUnitCode, p => p.MainUnit.Code);

        /// <summary>
        /// 物料单位编码
        /// </summary>
        public string ItemUnitCode
        {
            get { return this.GetProperty(ItemUnitCodeProperty); }
        }
        #endregion

        #region 物料单位 ItemUnitName
        /// <summary>
        /// 物料单位
        /// </summary>
        [Label("主单位")]
        public static readonly Property<string> ItemUnitNameProperty = P<ItemUnit>.RegisterView(e => e.ItemUnitName, p => p.MainUnit.Name);

        /// <summary>
        /// 物料单位
        /// </summary>
        public string ItemUnitName
        {
            get { return this.GetProperty(ItemUnitNameProperty); }
            set { this.SetProperty(ItemUnitNameProperty, value); }
        }
        #endregion

        #region 辅助单位编码 UnitCode
        /// <summary>
        /// 辅助单位编码
        /// </summary>
        [Label("辅助单位编码")]
        public static readonly Property<string> UnitCodeProperty = P<ItemUnit>.RegisterView(e => e.UnitCode, p => p.Unit.Code);

        /// <summary>
        /// 辅助单位名称
        /// </summary>
        public string UnitCode
        {
            get { return this.GetProperty(UnitCodeProperty); }
            set { SetProperty(UnitCodeProperty, value); }
        }
        #endregion

        #region 辅助单位名称 UnitName
        /// <summary>
        /// 辅助单位名称
        /// </summary>
        [Label("辅助单位名称")]
        public static readonly Property<string> UnitNameProperty = P<ItemUnit>.RegisterView(e => e.UnitName, p => p.Unit.Name);

        /// <summary>
        /// 辅助单位名称
        /// </summary>
        public string UnitName
        {
            get { return this.GetProperty(UnitNameProperty); }
            set { SetProperty(UnitNameProperty, value); }
        }
        #endregion

        #region 主单位精度 MainUnitPrecision
        /// <summary>
        /// 主单位精度
        /// </summary>
        [Label("主单位精度")]
        public static readonly Property<int?> MainUnitPrecisionProperty = P<ItemUnit>.RegisterView(e => e.MainUnitPrecision, p => p.MainUnit.Precision);

        /// <summary>
        /// 主单位精度
        /// </summary>
        public int? MainUnitPrecision
        {
            get { return this.GetProperty(MainUnitPrecisionProperty); }
        }
        #endregion

        #region 辅单位精度 SecondUnitPrecision
        /// <summary>
        /// 辅单位精度
        /// </summary>
        [Label("辅单位精度")]
        public static readonly Property<int?> SecondUnitPrecisionProperty = P<ItemUnit>.RegisterView(e => e.SecondUnitPrecision, p => p.Unit.Precision);

        /// <summary>
        /// 辅单位精度
        /// </summary>
        public int? SecondUnitPrecision
        {
            get { return this.GetProperty(SecondUnitPrecisionProperty); }
        }
        #endregion

        #region 主单位取舍类型 MainTrade
        /// <summary>
        /// 主单位取舍类型
        /// </summary>
        [Label("主单位取舍类型")]
        public static readonly Property<TradeType> MainTradeProperty = P<ItemUnit>.RegisterView(e => e.MainTrade, p => p.MainUnit.TradeType);

        /// <summary>
        /// 主单位取舍类型
        /// </summary>
        public TradeType MainTrade
        {
            get { return this.GetProperty(MainTradeProperty); }
        }
        #endregion

        #region 辅单位取舍类型 SecondTrade
        /// <summary>
        /// 辅单位取舍类型
        /// </summary>
        [Label("辅单位取舍类型")]
        public static readonly Property<TradeType> SecondTradeProperty = P<ItemUnit>.RegisterView(e => e.SecondTrade, p => p.Unit.TradeType);

        /// <summary>
        /// 辅单位取舍类型
        /// </summary>
        public TradeType SecondTrade
        {
            get { return this.GetProperty(SecondTradeProperty); }
        }
        #endregion

        #region 转换说明 ChangeDesc
        /// <summary>
        /// 转换说明
        /// </summary>
        [Label("转换说明")]
        public static readonly Property<string> ChangeDescProperty = P<ItemUnit>.Register(e => e.ChangeDesc);

        /// <summary>
        /// 转换说明
        /// </summary>
        public string ChangeDesc
        {
            get { return this.GetProperty(ChangeDescProperty); }
            set { this.SetProperty(ChangeDescProperty, value); }
        }
        #endregion

        #endregion

        /// <summary>
        /// 获取转换率
        /// </summary>
        /// <returns></returns>
        public decimal GetConvertFigre()
        {
            if (Denominator <= 0)
                return 1;
            if (MainUnitPrecision > 0)
            {
                int per = MainUnitPrecision.Value;
                if (SecondUnitPrecision > per)
                    per = SecondUnitPrecision.Value;

                return Math.Round((decimal)Numerator / Denominator, per);
            }
            else
                return Math.Round((decimal)Numerator / Denominator, 3);
        }

    }

    /// <summary>
    /// 转换单位 实体配置
    /// </summary>
    internal class ItemUnitConfig : EntityConfig<ItemUnit>
    {
        /// <summary>
        /// 属性元数据配置
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("ITEM_UNIT").MapAllProperties();
            Meta.Property(ItemUnit.ChangeDescProperty).DontMapColumn();
            Meta.EnablePhantoms();
        }
    }
}