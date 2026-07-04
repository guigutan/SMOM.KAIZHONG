using SIE.Common;
using SIE.Common.Configs;
using SIE.Core.Items;
using SIE.Domain;
using SIE.Items.Items;
using SIE.Items.Items.Configs;
using SIE.ManagedProperty;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;

namespace SIE.Items
{
    /// <summary>
    /// 物料
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(ItemCriteria))]
    [EntityWithConfig(typeof(ItemCodeNoConfig))]
    [EntityWithConfig(typeof(LengthUnitNoConfig))]
    [EntityWithConfig(typeof(VolumeUnitNoConfig))]
    [EntityWithConfig(typeof(WeightUnitNoConfig))]
    [DisplayMember(nameof(Code))]
    [Label("物料")]
    public partial class Item : SIE.Core.Items.Item
    {
        #region 图号 DrawingNo
        /// <summary>
        /// 图号
        /// </summary>
        [Label("图号")]
        [MaxLength(240)]
        public static readonly Property<string> DrawingNoProperty = P<Item>.Register(e => e.DrawingNo);

        /// <summary>
        /// 图号
        /// </summary>
        public string DrawingNo
        {
            get { return GetProperty(DrawingNoProperty); }
            set { SetProperty(DrawingNoProperty, value); }
        }
        #endregion

        #region 图号版本 Version
        /// <summary>
        /// 图号版本
        /// </summary>
        [Label("图号版本")]
        public static readonly Property<string> VersionProperty = P<Item>.Register(e => e.Version);

        /// <summary>
        /// 图号版本
        /// </summary>
        public string Version
        {
            get { return GetProperty(VersionProperty); }
            set { SetProperty(VersionProperty, value); }
        }
        #endregion

        #region 基准机型 BaseModel
        /// <summary>
        /// 基准机型
        /// </summary>
        [Label("基准机型")]
        public static readonly Property<string> BaseModelProperty = P<Item>.Register(e => e.BaseModel);

        /// <summary>
        /// 基准机型
        /// </summary>
        public string BaseModel
        {
            get { return GetProperty(BaseModelProperty); }
            set { SetProperty(BaseModelProperty, value); }
        }
        #endregion

        #region 责任人 Person
        /// <summary>
        /// 责任人
        /// </summary>
        [Label("责任人")]
        public static readonly Property<string> PersonProperty = P<Item>.Register(e => e.Person);

        /// <summary>
        /// 责任人
        /// </summary>
        public string Person
        {
            get { return GetProperty(PersonProperty); }
            set { SetProperty(PersonProperty, value); }
        }
        #endregion

        #region MRP控制者 MrpPerson
        /// <summary>
        /// MRP控制者
        /// </summary>
        [Label("MRP控制者")]
        public static readonly Property<string> MrpPersonProperty = P<Item>.Register(e => e.MrpPerson);

        /// <summary>
        /// MRP控制者
        /// </summary>
        public string MrpPerson
        {
            get { return GetProperty(MrpPersonProperty); }
            set { SetProperty(MrpPersonProperty, value); }
        }
        #endregion

        #region 采购员 PurchasingAgent
        /// <summary>
        /// 采购员Id
        /// </summary>
        [Label("采购员")]
        public static readonly IRefIdProperty PurchasingAgentIdProperty = P<Item>.RegisterRefId(e => e.PurchasingAgentId, ReferenceType.Normal);

        /// <summary>
        /// 采购员Id
        /// </summary>
        public double? PurchasingAgentId
        {
            get { return (double?)GetRefNullableId(PurchasingAgentIdProperty); }
            set { SetRefNullableId(PurchasingAgentIdProperty, value); }
        }

        /// <summary>
        /// 采购员
        /// </summary>
        public static readonly RefEntityProperty<Employee> PurchasingAgentProperty = P<Item>.RegisterRef(e => e.PurchasingAgent, PurchasingAgentIdProperty);

        /// <summary>
        /// 采购员
        /// </summary>
        public Employee PurchasingAgent
        {
            get { return GetRefEntity(PurchasingAgentProperty); }
            set { SetRefEntity(PurchasingAgentProperty, value); }
        }
        #endregion

        #region 物料属性列表 ItemPropertyList
        /// <summary>
        /// 物料属性列表
        /// </summary>
        [Label("物料属性列表")]
        public static readonly ListProperty<EntityList<ItemPropertyValue>> PropertyValueListProperty = P<Item>.RegisterList(e => e.PropertyValueList);

        /// <summary>
        /// 物料属性列表
        /// </summary>
        public EntityList<ItemPropertyValue> PropertyValueList
        {
            get { return this.GetLazyList(PropertyValueListProperty); }
        }
        #endregion

        #region 上偏差 UpperWeight
        /// <summary>
        /// 上偏差
        /// </summary>
        [Label("上偏差")]
        [MinValue(0)]
        public static readonly Property<decimal> UpperWeightProperty = P<Item>.Register(e => e.UpperWeight);

        /// <summary>
        /// 上偏差
        /// </summary>
        public decimal UpperWeight
        {
            get { return GetProperty(UpperWeightProperty); }
            set { SetProperty(UpperWeightProperty, value); }
        }
        #endregion

        #region 下偏差 LowerWeight 
        /// <summary>
        /// 下偏差
        /// </summary>
        [Label("下偏差")]
        [MinValue(0)]
        public static readonly Property<decimal> LowerWeightProperty = P<Item>.Register(e => e.LowerWeight);

        /// <summary>
        /// 下偏差
        /// </summary>
        public decimal LowerWeight
        {
            get { return GetProperty(LowerWeightProperty); }
            set { SetProperty(LowerWeightProperty, value); }
        }
        #endregion

        #region 最小包装数 MinPackingQty
        /// <summary>
        /// 最小包装数
        /// </summary>
        [Label("最小包装数")]
        [MinValue(0)]
        public static readonly Property<decimal?> MinPackingQtyProperty = P<Item>.Register(e => e.MinPackingQty);

        /// <summary>
        /// 最小包装数
        /// </summary>
        public decimal? MinPackingQty
        {
            get { return GetProperty(MinPackingQtyProperty); }
            set { SetProperty(MinPackingQtyProperty, value); }
        }
        #endregion

        #region 类型 Type
        /// <summary>
        /// 类型
        /// </summary>
        [Label("类型")]
        public static readonly Property<ItemType> TypeProperty = P<Item>.Register(e => e.Type);

        /// <summary>
        /// 类型
        /// </summary>
        public ItemType Type
        {
            get { return GetProperty(TypeProperty); }
            set { SetProperty(TypeProperty, value); }
        }
        #endregion

        #region 物料消耗类型 ConsumeMode
        /// <summary>
        /// 物料消耗类型
        /// </summary>
        [Required]
        [Label("物料消耗类型")]
        public static readonly Property<ConsumeMode> ConsumeModeProperty = P<Item>.Register(e => e.ConsumeMode);

        /// <summary>
        /// 物料消耗类型
        /// </summary>
        public ConsumeMode ConsumeMode
        {
            get { return this.GetProperty(ConsumeModeProperty); }
            set { this.SetProperty(ConsumeModeProperty, value); }
        }
        #endregion

        #region 来源类型 ItemSourceType
        /// <summary>
        /// 来源类型
        /// </summary>
        [Label("来源类型")]
        public static readonly Property<ItemSourceType?> ItemSourceTypeProperty = P<Item>.Register(e => e.ItemSourceType);

        /// <summary>
        /// 来源类型
        /// </summary>
        public ItemSourceType? ItemSourceType
        {
            get { return GetProperty(ItemSourceTypeProperty); }
            set { SetProperty(ItemSourceTypeProperty, value); }
        }
        #endregion

        #region 来源 SourceType
        /// <summary>
        /// 来源
        /// </summary>
        [Label("来源")]
        public static readonly Property<SourceType> SourceTypeProperty = P<Item>.Register(e => e.SourceType);

        /// <summary>
        /// 来源
        /// </summary>
        public SourceType SourceType
        {
            get { return GetProperty(SourceTypeProperty); }
            set { SetProperty(SourceTypeProperty, value); }
        }
        #endregion

        #region 物料与单位关系 UnitList
        /// <summary>
        /// 物料与单位关系
        /// </summary>
        [Label("转换单位")]
        public static readonly ListProperty<EntityList<ItemUnit>> UnitListProperty = P<Item>.RegisterList(e => e.UnitList);

        /// <summary>
        /// 物料与单位关系
        /// </summary>
        public EntityList<ItemUnit> UnitList
        {
            get { return this.GetLazyList(UnitListProperty); }
        }
        #endregion

        #region 物料标签类型 ItemLabelType
        /// <summary>
        /// 物料标签类型
        /// </summary>
        [Label("物料标签类型")]
        public static readonly Property<ItemLabelType> ItemLabelTypeProperty = P<Item>.Register(e => e.ItemLabelType);

        /// <summary>
        /// 物料标签类型
        /// </summary>
        public ItemLabelType ItemLabelType
        {
            get { return GetProperty(ItemLabelTypeProperty); }
            set { SetProperty(ItemLabelTypeProperty, value); }
        }
        #endregion

        #region 单位 Unit
        /// <summary>
        /// 单位Id
        /// </summary>
        [Label("单位")]
        public static readonly IRefIdProperty UnitIdProperty = P<Item>.RegisterRefId(e => e.UnitId, ReferenceType.Normal);

        /// <summary>
        /// 单位Id
        /// </summary>
        public double? UnitId
        {
            get { return (double?)this.GetRefNullableId(UnitIdProperty); }
            set { this.SetRefNullableId(UnitIdProperty, value); }
        }

        /// <summary>
        /// 单位
        /// </summary>
        public static readonly RefEntityProperty<Unit> UnitProperty = P<Item>.RegisterRef(e => e.Unit, UnitIdProperty);

        /// <summary>
        /// 单位
        /// </summary>
        public Unit Unit
        {
            get { return this.GetRefEntity(UnitProperty); }
            set { this.SetRefEntity(UnitProperty, value); }
        }
        #endregion

        #region 单位编码 UnitCode
        /// <summary>
        /// 单位编码
        /// </summary>
        [Label("单位编码")]
        public static readonly Property<string> UnitCodeProperty = P<Item>.RegisterView(e => e.UnitCode, p => p.Unit.Code);

        /// <summary>
        /// 单位编码
        /// </summary>
        public string UnitCode
        {
            get { return GetProperty(UnitCodeProperty); }
            set { SetProperty(UnitCodeProperty, value); }
        }
        #endregion

        #region 单位名称 UnitName
        /// <summary>
        /// 单位名称
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> UnitNameProperty = P<Item>.RegisterView(e => e.UnitName, p => p.Unit.Name);

        /// <summary>
        /// 单位名称
        /// </summary>
        public string UnitName
        {
            get { return GetProperty(UnitNameProperty); }
            set { SetProperty(UnitNameProperty, value); }
        }
        #endregion

        #region 单位精度 UnitPrecision
        /// <summary>
        /// 单位精度
        /// </summary>
        [Label("单位精度")]
        public static readonly Property<int?> UnitPrecisionProperty = P<Item>.RegisterView(e => e.UnitPrecision, p => p.Unit.Precision);

        /// <summary>
        /// 单位精度
        /// </summary>
        public int? UnitPrecision
        {
            get { return this.GetProperty(UnitPrecisionProperty); }
        }
        #endregion

        #region 取舍类型 UnitTradeType
        /// <summary>
        /// 取舍类型
        /// </summary>
        [Label("取舍类型")]
        public static readonly Property<TradeType> UnitTradeTypeProperty = P<Item>.RegisterView(e => e.UnitTradeType, p => p.Unit.TradeType);

        /// <summary>
        /// 取舍类型
        /// </summary>
        public TradeType UnitTradeType
        {
            get { return this.GetProperty(UnitTradeTypeProperty); }
        }
        #endregion

        #region 产品机型 Model
        /// <summary>
        /// 产品机型Id
        /// </summary>
        [Label("产品机型")]
        public static readonly IRefIdProperty ModelIdProperty = P<Item>.RegisterRefId(e => e.ModelId, ReferenceType.Normal);

        /// <summary>
        /// 产品机型Id
        /// </summary>
        public double? ModelId
        {
            get { return (double?)GetRefNullableId(ModelIdProperty); }
            set { SetRefNullableId(ModelIdProperty, value); }
        }

        /// <summary>
        /// 产品机型
        /// </summary>
        [Label("产品机型")]
        public static readonly RefEntityProperty<ProductModel> ModelProperty = P<Item>.RegisterRef(e => e.Model, ModelIdProperty);

        /// <summary>
        /// 产品机型
        /// </summary>
        public ProductModel Model
        {
            get { return GetRefEntity(ModelProperty); }
            set { SetRefEntity(ModelProperty, value); }
        }
        #endregion

        #region 产品族 ProductFamily
        /// <summary>
        /// 产品族Id
        /// </summary>
        [Label("产品族")]
        public static readonly IRefIdProperty ProductFamilyIdProperty = P<Item>.RegisterRefId(e => e.ProductFamilyId, ReferenceType.Normal);

        /// <summary>
        /// 产品族Id
        /// </summary>
        public double? ProductFamilyId
        {
            get { return (double?)GetRefNullableId(ProductFamilyIdProperty); }
            set { SetRefNullableId(ProductFamilyIdProperty, value); }
        }

        /// <summary>
        /// 产品族
        /// </summary>
        [Label("产品族")]
        public static readonly RefEntityProperty<ProductFamily> ProductFamilyProperty = P<Item>.RegisterRef(e => e.ProductFamily, ProductFamilyIdProperty);

        /// <summary>
        /// 产品族
        /// </summary>
        public ProductFamily ProductFamily
        {
            get { return GetRefEntity(ProductFamilyProperty); }
            set { SetRefEntity(ProductFamilyProperty, value); }
        }
        #endregion

        #region 基准编码 BaseCode
        /// <summary>
        /// 基准编码Id
        /// </summary>
        [Label("基准编码")]
        public static readonly IRefIdProperty BaseCodeIdProperty = P<Item>.RegisterRefId(e => e.BaseCodeId, ReferenceType.Normal);

        /// <summary>
        /// 基准编码Id
        /// </summary>
        public double? BaseCodeId
        {
            get { return (double?)GetRefNullableId(BaseCodeIdProperty); }
            set { SetRefNullableId(BaseCodeIdProperty, value); }
        }

        /// <summary>
        /// 基准编码
        /// </summary>
        public static readonly RefEntityProperty<Item> BaseCodeProperty = P<Item>.RegisterRef(e => e.BaseCode, BaseCodeIdProperty);

        /// <summary>
        /// 基准编码
        /// </summary>
        public Item BaseCode
        {
            get { return GetRefEntity(BaseCodeProperty); }
            set { SetRefEntity(BaseCodeProperty, value); }
        }
        #endregion

        #region 规格型号 SpecificationModel
        /// <summary>
        /// 规格型号
        /// </summary>
        [Label("规格型号")]
        //[MaxLength(240)]
        public static readonly Property<string> SpecificationModelProperty = P<Item>.Register(e => e.SpecificationModel);

        /// <summary>
        /// 规格型号
        /// </summary>
        public string SpecificationModel
        {
            get { return GetProperty(SpecificationModelProperty); }
            set { SetProperty(SpecificationModelProperty, value); }
        }
        #endregion

        #region 英文描述 EnglishDescription
        /// <summary>
        /// 英文描述
        /// </summary>
        [Label("英文描述")]
        [MaxLength(4000)]
        public static readonly Property<string> EnglishDescriptionProperty = P<Item>.Register(e => e.EnglishDescription);

        /// <summary>
        /// 英文描述
        /// </summary>
        public string EnglishDescription
        {
            get { return GetProperty(EnglishDescriptionProperty); }
            set { SetProperty(EnglishDescriptionProperty, value); }
        }
        #endregion

        #region 物料简称 ShortDescription
        /// <summary>
        /// 物料简称
        /// </summary>
        [Label("旧料号(凯中旧料号)")]
        public static readonly Property<string> ShortDescriptionProperty = P<Item>.Register(e => e.ShortDescription);

        /// <summary>
        /// 物料简称
        /// </summary>
        public string ShortDescription
        {
            get { return GetProperty(ShortDescriptionProperty); }
            set { SetProperty(ShortDescriptionProperty, value); }
        }
        #endregion

        #region AbcType ABC分类
        /// <summary>
        /// AbcType
        /// </summary>
        [Label("ABC分类")]
        public static readonly Property<AbcType?> ABCCategoryProperty = P<Item>.Register(e => e.ABCCategory);

        /// <summary>
        /// AbcType
        /// </summary>
        public AbcType? ABCCategory
        {
            get { return GetProperty(ABCCategoryProperty); }
            set { SetProperty(ABCCategoryProperty, value); }
        }
        #endregion

        #region 长 Length
        /// <summary>
        /// 长
        /// </summary>
        [Label("长(CM)")]
        public static readonly Property<decimal?> LengthProperty = P<Item>.Register(e => e.Length);

        /// <summary>
        /// 长
        /// </summary>
        public decimal? Length
        {
            get { return GetProperty(LengthProperty); }
            set { SetProperty(LengthProperty, value); }
        }
        #endregion

        #region 宽 Width
        /// <summary>
        /// 宽
        /// </summary>
        [Label("宽(CM)")]
        public static readonly Property<decimal?> WidthProperty = P<Item>.Register(e => e.Width);

        /// <summary>
        /// 宽
        /// </summary>
        public decimal? Width
        {
            get { return GetProperty(WidthProperty); }
            set { SetProperty(WidthProperty, value); }
        }
        #endregion

        #region 高 Height
        /// <summary>
        /// 高
        /// </summary>
        [Label("高(CM)")]
        public static readonly Property<decimal?> HeightProperty = P<Item>.Register(e => e.Height);

        /// <summary>
        /// 高
        /// </summary>
        public decimal? Height
        {
            get { return GetProperty(HeightProperty); }
            set { SetProperty(HeightProperty, value); }
        }
        #endregion

        #region 体积 Volume
        /// <summary>
        /// 体积
        /// </summary>
        [Label("体积(CM³)")]
        public static readonly Property<decimal?> VolumeProperty = P<Item>.Register(e => e.Volume);

        /// <summary>
        /// 体积
        /// </summary>
        public decimal? Volume
        {
            get { return GetProperty(VolumeProperty); }
            set { SetProperty(VolumeProperty, value); }
        }
        #endregion

        #region 重量 Weight
        /// <summary>
        /// 重量
        /// </summary>
        [Label("单位净重")]
        public static readonly Property<decimal?> WeightProperty = P<Item>.Register(e => e.Weight);

        /// <summary>
        /// 重量
        /// </summary>
        public decimal? Weight
        {
            get { return GetProperty(WeightProperty); }
            set { SetProperty(WeightProperty, value); }
        }
        #endregion

        #region 净重单位 WeightUnit
        /// <summary>
        /// 净重单位
        /// </summary>
        [Label("净重单位")]
        public static readonly Property<string> WeightUnitProperty = P<Item>.Register(e => e.WeightUnit);

        /// <summary>
        /// 净重单位
        /// </summary>
        public string WeightUnit
        {
            get { return this.GetProperty(WeightUnitProperty); }
            set { this.SetProperty(WeightUnitProperty, value); }
        }
        #endregion

        #region 图片 Photo
        /// <summary>
        /// 图片
        /// </summary>
        [Label("图片")]
        public static readonly Property<byte[]> PhotoProperty = P<Item>.Register(e => e.Photo);

        /// <summary>
        /// 图片
        /// </summary>
        public byte[] Photo
        {
            get { return GetProperty(PhotoProperty); }
            set { SetProperty(PhotoProperty, value); }
        }
        #endregion

        #region 采购组 PurchasingGroup
        /// <summary>
        /// 采购组Id
        /// </summary>
        [Label("采购组")]
        public static readonly IRefIdProperty PurchasingGroupIdProperty = P<Item>.RegisterRefId(e => e.PurchasingGroupId, ReferenceType.Normal);

        /// <summary>
        /// 采购组Id
        /// </summary>
        public double? PurchasingGroupId
        {
            get { return (double?)GetRefNullableId(PurchasingGroupIdProperty); }
            set { SetRefNullableId(PurchasingGroupIdProperty, value); }
        }

        /// <summary>
        /// 采购组
        /// </summary>
        public static readonly RefEntityProperty<WorkGroup> PurchasingGroupProperty = P<Item>.RegisterRef(e => e.PurchasingGroup, PurchasingGroupIdProperty);

        /// <summary>
        /// 采购组
        /// </summary>
        public WorkGroup PurchasingGroup
        {
            get { return GetRefEntity(PurchasingGroupProperty); }
            set { SetRefEntity(PurchasingGroupProperty, value); }
        }
        #endregion

        #region  采购提前期 PurchaseLeadTime
        /// <summary>
        /// 采购提前期
        /// </summary>
        [Label("采购提前期")]
        public static readonly Property<int?> PurchaseLeadTimeProperty = P<Item>.Register(e => e.PurchaseLeadTime);

        /// <summary>
        /// 采购提前期
        /// </summary>
        public int? PurchaseLeadTime
        {
            get { return GetProperty(PurchaseLeadTimeProperty); }
            set { SetProperty(PurchaseLeadTimeProperty, value); }
        }
        #endregion

        #region 单位精度  Precision 
        /// <summary>
        /// 单位精度
        /// </summary>
        [Label("单位精度")]
        public static readonly Property<int?> PrecisionProperty = P<Item>.Register(e => e.Precision);

        /// <summary>
        /// 单位精度
        /// </summary>
        public int? Precision
        {
            get { return GetProperty(PrecisionProperty); }
            set { SetProperty(PrecisionProperty, value); }
        }
        #endregion

        #region 商品条码  GoodsBarcode 
        /// <summary>
        /// 商品条码
        /// </summary>
        [Label("商品条码")]
        public static readonly Property<string> GoodsBarcodeProperty = P<Item>.Register(e => e.GoodsBarcode);

        /// <summary>
        /// 商品条码
        /// </summary>
        public string GoodsBarcode
        {
            get { return GetProperty(GoodsBarcodeProperty); }
            set { SetProperty(GoodsBarcodeProperty, value); }
        }
        #endregion

        #region 是否虚拟件 IsVirtualPart
        /// <summary>
        /// 是否虚拟件
        /// </summary>
        [Label("是否虚拟件")]
        public static readonly Property<bool> IsVirtualPartProperty = P<Item>.Register(e => e.IsVirtualPart);

        /// <summary>
        /// 是否虚拟件
        /// </summary>
        public bool IsVirtualPart
        {
            get { return GetProperty(IsVirtualPartProperty); }
            set { SetProperty(IsVirtualPartProperty, value); }
        }
        #endregion

        #region 启用扩展属性 EnableExtendProperty
        /// <summary>
        /// 启用扩展属性
        /// </summary>
        [Label("启用扩展属性")]
        public static readonly Property<bool> EnableExtendPropertyProperty = P<Item>.Register(e => e.EnableExtendProperty);

        /// <summary>
        /// 启用扩展属性
        /// </summary>
        public bool EnableExtendProperty
        {
            get { return this.GetProperty(EnableExtendPropertyProperty); }
            set { this.SetProperty(EnableExtendPropertyProperty, value); }
        }
        #endregion

        #region 来源主键 SourceKey
        /// <summary>
        /// 来源主键, 一般用于接口平台事务上传
        /// </summary>
        [Label("来源主键")]
        public static readonly Property<string> SourceKeyProperty = P<Item>.Register(e => e.SourceKey);

        /// <summary>
        /// 来源主键, 一般用于接口平台事务上传
        /// </summary>
        public string SourceKey
        {
            get { return this.GetProperty(SourceKeyProperty); }
            set { this.SetProperty(SourceKeyProperty, value); }
        }
        #endregion

        #region 生产提前期 ProductLeadDay
        /// <summary>
        /// 生产提前期
        /// </summary>
        [Label("生产提前期")]
        public static readonly Property<int?> ProductLeadDayProperty = P<Item>.Register(e => e.ProductLeadDay);

        /// <summary>
        /// 生产提前期
        /// </summary>
        public int? ProductLeadDay
        {
            get { return this.GetProperty(ProductLeadDayProperty); }
            set { this.SetProperty(ProductLeadDayProperty, value); }
        }
        #endregion

        #region 工艺参数分类 TechParamCategory
        /// <summary>
        /// 工艺参数分类
        /// </summary>
        [Label("工艺参数分类")]
        public static readonly Property<string> TechParamCategoryProperty = P<Item>.Register(e => e.TechParamCategory);

        /// <summary>
        /// 工艺参数分类
        /// </summary>
        public string TechParamCategory
        {
            get { return this.GetProperty(TechParamCategoryProperty); }
            set { this.SetProperty(TechParamCategoryProperty, value); }
        }
        #endregion

        #region 是否标签管理 IsLabel
        /// <summary>
        /// 是否标签管理
        /// </summary>
        [Label("标签管理")]
        public static readonly Property<bool?> IsLabelProperty = P<Item>.Register(e => e.IsLabel);

        /// <summary>
        /// 是否标签管理
        /// </summary>
        public bool? IsLabel
        {
            get { return this.GetProperty(IsLabelProperty); }
            set { this.SetProperty(IsLabelProperty, value); }
        }
        #endregion

        #region 是否标记 IsMarked
        /// <summary>
        /// 是否标记
        /// </summary>
        [Label("是否标记")]
        public static readonly Property<bool?> IsMarkedProperty = P<Item>.Register(e => e.IsMarked);

        /// <summary>
        /// 是否标记
        /// </summary>
        public bool? IsMarked
        {
            get { return this.GetProperty(IsMarkedProperty); }
            set { this.SetProperty(IsMarkedProperty, value); }
        }
        #endregion

        #region 默认辅助单位 SecondUnit
        /// <summary>
        /// 默认辅助单位Id
        /// </summary>
        [Label("默认辅助单位")]
        public static readonly IRefIdProperty SecondUnitIdProperty =
            P<Item>.RegisterRefId(e => e.SecondUnitId, ReferenceType.Normal);

        /// <summary>
        /// 默认辅助单位Id
        /// </summary>
        public double? SecondUnitId
        {
            get { return (double?)this.GetRefNullableId(SecondUnitIdProperty); }
            set { this.SetRefNullableId(SecondUnitIdProperty, value); }
        }

        /// <summary>   
        /// 默认辅助单位
        /// </summary>
        public static readonly RefEntityProperty<Unit> SecondUnitProperty =
            P<Item>.RegisterRef(e => e.SecondUnit, SecondUnitIdProperty);

        /// <summary>
        /// 默认辅助单位
        /// </summary>
        public Unit SecondUnit
        {
            get { return this.GetRefEntity(SecondUnitProperty); }
            set { this.SetRefEntity(SecondUnitProperty, value); }
        }
        #endregion

        #region 默认辅助单位名称 SecondUnitName
        /// <summary>
        /// 默认辅助单位名称
        /// </summary>
        [Label("默认辅助单位名称")]
        public static readonly Property<string> SecondUnitNameProperty = P<Item>.RegisterView(e => e.SecondUnitName, p => p.SecondUnit.Name);

        /// <summary>
        /// 默认辅助单位名称
        /// </summary>
        public string SecondUnitName
        {
            get { return this.GetProperty(SecondUnitNameProperty); }
        }
        #endregion

        #region 集团状态 GroupState
        /// <summary>
        /// 集团状态
        /// </summary>
        [Label("集团状态")]
        public static readonly Property<State> GroupStateProperty = P<Item>.Register(e => e.GroupState);

        /// <summary>
        /// 集团状态
        /// </summary>
        public State GroupState
        {
            get { return this.GetProperty(GroupStateProperty); }
            set { this.SetProperty(GroupStateProperty, value); }
        }
        #endregion

        #region 工厂状态 FactoryState
        /// <summary>
        /// 工厂状态
        /// </summary>
        [Label("工厂状态")]
        public static readonly Property<State> FactoryStateProperty = P<Item>.Register(e => e.FactoryState);

        /// <summary>
        /// 工厂状态
        /// </summary>
        public State FactoryState
        {
            get { return this.GetProperty(FactoryStateProperty); }
            set { this.SetProperty(FactoryStateProperty, value); }
        }
        #endregion

        #region Mrp控制者 MrpController
        /// <summary>
        /// Mrp控制者
        /// </summary>
        [Label("Mrp控制者")]
        public static readonly Property<string> MrpControllerProperty = P<Item>.Register(e => e.MrpController);

        /// <summary>
        /// Mrp控制者
        /// </summary>
        public string MrpController
        {
            get { return this.GetProperty(MrpControllerProperty); }
            set { this.SetProperty(MrpControllerProperty, value); }
        }
        #endregion

        #region 超报工比例(%) ExcessReportRatio
        /// <summary>
        /// 超报工比例(%)
        /// </summary>
        [Label("超报工比例(%)")]
        public static readonly Property<decimal?> ExcessReportRatioProperty = P<Item>.Register(e => e.ExcessReportRatio);

        /// <summary>
        /// 超报工比例(%)
        /// </summary>
        public decimal? ExcessReportRatio
        {
            get { return this.GetProperty(ExcessReportRatioProperty); }
            set { this.SetProperty(ExcessReportRatioProperty, value); }
        }
        #endregion

        #region 后继物料 SuccessorItem
        /// <summary>
        /// 后继物料
        /// </summary>
        [Label("后继物料")]
        public static readonly Property<string> SuccessorItemProperty = P<Item>.Register(e => e.SuccessorItem);

        /// <summary>
        /// 后继物料
        /// </summary>
        public string SuccessorItem
        {
            get { return this.GetProperty(SuccessorItemProperty); }
            set { this.SetProperty(SuccessorItemProperty, value); }
        }
        #endregion

        #region 后继生效时间 SuccessorEffeTime
        /// <summary>
        /// 后继生效时间
        /// </summary>
        [Label("后继生效时间")]
        public static readonly Property<DateTime?> SuccessorEffeTimeProperty = P<Item>.Register(e => e.SuccessorEffeTime);

        /// <summary>
        /// 后继生效时间
        /// </summary>
        public DateTime? SuccessorEffeTime
        {
            get { return this.GetProperty(SuccessorEffeTimeProperty); }
            set { this.SetProperty(SuccessorEffeTimeProperty, value); }
        }
        #endregion

        #region 物料类型 Mtart
        /// <summary>
        /// 物料类型
        /// </summary>
        [Label("物料类型")]
        public static readonly Property<string> MtartProperty = P<Item>.Register(e => e.Mtart);

        /// <summary>
        /// 物料类型
        /// </summary>
        public string Mtart
        {
            get { return this.GetProperty(MtartProperty); }
            set { this.SetProperty(MtartProperty, value); }
        }
        #endregion

        #region 客户料码数据 ItemCusotmerRelation
        /// <summary>
        /// 客户料码数据
        /// </summary>
        [Label("客户料码数据")]
        public static readonly ListProperty<EntityList<ItemCusotmerRelation>> ItemCusotmerRelationProperty = P<Item>.RegisterList(e => e.ItemCusotmerRelation);

        /// <summary>
        /// 客户料码数据
        /// </summary>
        public EntityList<ItemCusotmerRelation> ItemCusotmerRelation
        {
            get { return this.GetLazyList(ItemCusotmerRelationProperty); }
        }
        #endregion

        #region 父级物料信息 ParentItemList
        /// <summary>
        /// 父级物料信息
        /// </summary>
        [Label("父级物料信息")]
        public static readonly ListProperty<EntityList<ParentItem>> ParentItemListProperty = P<Item>.RegisterList(e => e.ParentItemList);

        /// <summary>
        /// 父级物料信息
        /// </summary>
        public EntityList<ParentItem> ParentItemList
        {
            get { return this.GetLazyList(ParentItemListProperty); }
        }
        #endregion

        #region 物料客户与特性关系 CustomFeatureRelList
        /// <summary>
        /// 物料客户与特性关系
        /// </summary>
        [Label("物料客户与特性关系")]
        public static readonly ListProperty<EntityList<CustomFeatureRel>> CustomFeatureRelListProperty = P<Item>.RegisterList(e => e.CustomFeatureRelList);

        /// <summary>
        /// 物料客户与特性关系
        /// </summary>
        public EntityList<CustomFeatureRel> CustomFeatureRelList
        {
            get { return this.GetLazyList(CustomFeatureRelListProperty); }
        }
        #endregion


        /// <summary>
        /// 属性变更事件
        /// </summary>
        /// <param name="e">e</param>
        protected override void OnPropertyChanged(ManagedPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Source == ManagedPropertyChangedSource.FromUIOperating && e.Property.Name == PurchasingGroupProperty.Name)
            {
                this.PurchasingAgent = null;
            }
        }
    }

    /// <summary>
    /// 物料 实体配置
    /// </summary>
    internal class ItemConfig : EntityConfig<Item>
    {
        /// <summary>
        /// 数据库表配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("ITEM").MapAllProperties();
            Meta.Property(Item.DrawingNoProperty).ColumnMeta.HasLength(240);
            Meta.Property(Item.SpecificationModelProperty).ColumnMeta.HasLength(4000);
            Meta.Property(Item.DescriptionProperty).ColumnMeta.HasLength("max");
            Meta.Property(Item.EnglishDescriptionProperty).ColumnMeta.HasLength("max");
            Meta.Property(Item.CodeProperty).ColumnMeta.HasIndex();
            Meta.Property(Item.NameProperty).ColumnMeta.HasIndex();
            Meta.EnablePhantoms();
        }
    }

    /// <summary>
    /// 物料打印设置扩展属性
    /// </summary>
    [CompiledPropertyDeclarer]
    public class LabelPrintDetailProperty
    {
        /// <summary>
        /// 扩展打印设置属性
        /// </summary>
        public static readonly Property<LabelPrintTemplate> LabelPrintTemProperty =
            P<Item>.RegisterExtension<LabelPrintTemplate>("LabelPrintTem", typeof(LabelPrintDetailProperty));

        /// <summary>
        /// 获取打印设置对象
        /// </summary>
        /// <param name="me">物料对象</param>
        /// <returns>返回打印设置对象</returns>
        public static LabelPrintTemplate GetLabelPrintTem(Item me)
        {
            return me.GetProperty(LabelPrintTemProperty);
        }

        /// <summary>
        /// 设置打印设置对象
        /// </summary>
        /// <param name="me">物料</param>
        /// <param name="value">需要设置的打印设置对象</param>
        public static void SetLabelPrintTem(Item me, LabelPrintTemplate value)
        {
            me.SetProperty(LabelPrintTemProperty, value);
        }

        /// <summary>
        /// 物料扩展属性 实体配置
        /// </summary>
        internal class LabelPrintDetailPropertyConfig : EntityConfig<Item>
        {
            /// <summary>
            /// 属性元数据配置
            /// </summary>
            protected override void ConfigMeta()
            {
                Meta.Property(LabelPrintDetailProperty.LabelPrintTemProperty)?.DontMapColumn();
            }
        }
    }
}