using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.ProcessSegments;
using System;

namespace SIE.Items
{
    /// <summary>
    /// 产品BOM明细
    /// </summary>
    [ChildEntity, Serializable]
    [Label("产品BOM明细")]
    [DisplayMember(nameof(Id))]
    public partial class ProductBomDetail : DataEntity
    {
        #region 是否反冲物料 IsRecoilItem
        /// <summary>
        /// 是否反冲物料
        /// </summary>
        [Label("反冲物料")]
        public static readonly Property<bool?> IsRecoilItemProperty = P<ProductBomDetail>.Register(e => e.IsRecoilItem);

        /// <summary>
        /// 是否反冲物料
        /// </summary>
        public bool? IsRecoilItem
        {
            get { return this.GetProperty(IsRecoilItemProperty); }
            set { this.SetProperty(IsRecoilItemProperty, value); }
        }
        #endregion



        #region 损耗率 LossRate
        /// <summary>
        /// 损耗率
        /// </summary>
        [Label("损耗率")]
        public static readonly Property<decimal> LossRateProperty = P<ProductBomDetail>.Register(e => e.LossRate);

        /// <summary>
        /// 损耗率
        /// </summary>
        public decimal LossRate
        {
            get { return GetProperty(LossRateProperty); }
            set { SetProperty(LossRateProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<ProductBomDetail>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 单位耗用量 UnitQty
        /// <summary>
        /// 单位耗用量
        /// </summary>
        [Label("单位耗用量")]
        public static readonly Property<decimal> UnitQtyProperty = P<ProductBomDetail>.Register(e => e.UnitQty);

        /// <summary>
        /// 单位耗用量
        /// </summary>
        public decimal UnitQty
        {
            get { return GetProperty(UnitQtyProperty); }
            set { SetProperty(UnitQtyProperty, value); }
        }
        #endregion

        #region 工段 ProcessSegment
        /// <summary>
        /// 工段Id
        /// </summary>
        [Label("工段")]
        public static readonly IRefIdProperty ProcessSegmentIdProperty = P<ProductBomDetail>.RegisterRefId(e => e.ProcessSegmentId, ReferenceType.Normal);

        /// <summary>
        /// 工段Id
        /// </summary>
        public double? ProcessSegmentId
        {
            get { return (double?)GetRefNullableId(ProcessSegmentIdProperty); }
            set { SetRefNullableId(ProcessSegmentIdProperty, value); }
        }

        /// <summary>
        /// 工段
        /// </summary>
        public static readonly RefEntityProperty<ProcessSegment> ProcessSegmentProperty = P<ProductBomDetail>.RegisterRef(e => e.ProcessSegment, ProcessSegmentIdProperty);

        /// <summary>
        /// 工段
        /// </summary>
        public ProcessSegment ProcessSegment
        {
            get { return GetRefEntity(ProcessSegmentProperty); }
            set { SetRefEntity(ProcessSegmentProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty = P<ProductBomDetail>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

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
        [Label("物料")]
        public static readonly RefEntityProperty<Item> ItemProperty = P<ProductBomDetail>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<ProductBomDetail>.RegisterView(e => e.ItemCode, p => p.Item.Code);

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
        public static readonly Property<string> ItemNameProperty = P<ProductBomDetail>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }
        #endregion

        #region 规格型号 ItemSpecificationModel
        /// <summary>
        /// 规格型号
        /// </summary>
        [Label("规格型号")]
        public static readonly Property<string> ItemSpecificationModelProperty = P<ProductBomDetail>.RegisterView(e => e.ItemSpecificationModel, p => p.Item.SpecificationModel);

        /// <summary>
        /// 规格型号
        /// </summary>
        public string ItemSpecificationModel
        {
            get { return this.GetProperty(ItemSpecificationModelProperty); }
        }
        #endregion

        #region 单位 ItemUnitCode
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> ItemUnitCodeProperty = P<ProductBomDetail>.RegisterView(e => e.ItemUnitCode, p => p.Item.Unit.Code);

        /// <summary>
        /// 单位
        /// </summary>
        public string ItemUnitCode
        {
            get { return this.GetProperty(ItemUnitCodeProperty); }
        }
        #endregion

        #region 替代料 AlternativeList
        /// <summary>
        /// 替代料
        /// </summary>
        [Label("替代料")]
        public static readonly ListProperty<EntityList<ProductBomDetailAlternative>> AlternativeListProperty = P<ProductBomDetail>.RegisterList(e => e.AlternativeList);

        /// <summary>
        /// 替代料
        /// </summary>
        public EntityList<ProductBomDetailAlternative> AlternativeList
        {
            get { return this.GetLazyList(AlternativeListProperty); }
        }
        #endregion

        #region 属性值字符串Json格式 PropertyValueJson
        /// <summary>
        /// 属性值字符串Json格式
        /// </summary>
        [MaxLength(2000)]
        [Label("属性值字符串Json格式")]
        public static readonly Property<string> PropertyValueJsonProperty = P<ProductBomDetail>.Register(e => e.PropertyValueJson);

        /// <summary>
        /// 属性值字符串Json格式
        /// </summary>
        public string PropertyValueJson
        {
            get { return GetProperty(PropertyValueJsonProperty); }
            set { SetProperty(PropertyValueJsonProperty, value); }
        }
        #endregion

        #region 物料属性值 PropertyValueStr
        /// <summary>
        /// 物料属性值
        /// </summary>
        [MaxLength(1000)]
        [Label("物料属性值")]
        public static readonly Property<string> PropertyValueStrProperty = P<ProductBomDetail>.Register(e => e.PropertyValueStr);

        /// <summary>
        /// 物料属性值
        /// </summary>
        public string PropertyValueStr
        {
            get { return GetProperty(PropertyValueStrProperty); }
            set { SetProperty(PropertyValueStrProperty, value); }
        }
        #endregion

        #region 属性值 PropertyValueList
        /// <summary>
        /// 属性值
        /// </summary>
        [Label("属性值")]
        public static readonly ListProperty<EntityList<ProductBomDetailPropertyValue>> PropertyValueListProperty = P<ProductBomDetail>.RegisterList(e => e.PropertyValueList);

        /// <summary>
        /// 属性值
        /// </summary>
        public EntityList<ProductBomDetailPropertyValue> PropertyValueList
        {
            get { return this.GetLazyList(PropertyValueListProperty); }
        }
        #endregion


        #region 扩展属性 ItemExtProp
        /// <summary>
        /// 扩展属性
        /// </summary>
        [Label("扩展属性")]
        public static readonly Property<string> ItemExtPropProperty = P<ProductBomDetail>.Register(e => e.ItemExtProp);

        /// <summary>
        /// 扩展属性
        /// </summary>
        public string ItemExtProp
        {
            get { return GetProperty(ItemExtPropProperty); }
            set { SetProperty(ItemExtPropProperty, value); }
        }
        #endregion

        #region 扩展属性值 ItemExtPropName
        /// <summary>
        /// 扩展属性值
        /// </summary>
        [Label("扩展属性值")]
        public static readonly Property<string> ItemExtPropNameProperty = P<ProductBomDetail>.Register(e => e.ItemExtPropName);

        /// <summary>
        /// 扩展属性值
        /// </summary>
        public string ItemExtPropName
        {
            get { return GetProperty(ItemExtPropNameProperty); }
            set { SetProperty(ItemExtPropNameProperty, value); }
        }
        #endregion


        #region 产品BOM ProductBom
        /// <summary>
        /// 产品BOMId
        /// </summary>
        [Label("产品BOM")]
        public static readonly IRefIdProperty ProductBomIdProperty = P<ProductBomDetail>.RegisterRefId(e => e.ProductBomId, ReferenceType.Parent);

        /// <summary>
        /// 产品BOMId
        /// </summary>
        public double ProductBomId
        {
            get { return (double)GetRefId(ProductBomIdProperty); }
            set { SetRefId(ProductBomIdProperty, value); }
        }

        /// <summary>
        /// 产品BOM
        /// </summary>
        public static readonly RefEntityProperty<ProductBom> ProductBomProperty = P<ProductBomDetail>.RegisterRef(e => e.ProductBom, ProductBomIdProperty);

        /// <summary>
        /// 产品BOM
        /// </summary>
        public ProductBom ProductBom
        {
            get { return GetRefEntity(ProductBomProperty); }
            set { SetRefEntity(ProductBomProperty, value); }
        }
        #endregion

        #region 产品BOM编码 ProductBomCode
        /// <summary>
        /// 产品BOM编码
        /// </summary>
        [Label("产品BOM编码")]
        public static readonly Property<string> ProductBomCodeProperty = P<ProductBomDetail>.RegisterView(e => e.ProductBomCode, p => p.ProductBom.Code);

        /// <summary>
        /// 产品BOM编码
        /// </summary>
        public string ProductBomCode
        {
            get { return this.GetProperty(ProductBomCodeProperty); }
        }
        #endregion

        #region 来源主键 SourceKey
        /// <summary>
        /// 来源主键, 一般用于接口平台事务上传
        /// </summary>
        [Label("来源主键")]
        public static readonly Property<string> SourceKeyProperty = P<ProductBomDetail>.Register(e => e.SourceKey);

        /// <summary>
        /// 来源主键, 一般用于接口平台事务上传
        /// </summary>
        public string SourceKey
        {
            get { return this.GetProperty(SourceKeyProperty); }
            set { this.SetProperty(SourceKeyProperty, value); }
        }
        #endregion

        #region RegisterView注册视图属性(关联实体属性平铺显示) 
        #region 单位名称 ItemUnitName
        /// <summary>
        /// 单位名称
        /// </summary>
        [Label("单位名称")]
        public static readonly Property<string> ItemUnitNameProperty = P<ProductBomDetail>.RegisterView(e => e.ItemUnitName, p => p.Item.Unit.Name);

        /// <summary>
        /// 单位名称
        /// </summary>
        public string ItemUnitName
        {
            get { return this.GetProperty(ItemUnitNameProperty); }
        }
        #endregion

        #region 单位精度 Precision
        /// <summary>
        /// 单位精度
        /// </summary>
        [Label("单位精度")]
        public static readonly Property<int?> PrecisionProperty = P<ProductBomDetail>.RegisterView(e => e.Precision, p => p.Item.Precision);

        /// <summary>
        /// 单位精度
        /// </summary>
        public int? Precision
        {
            get { return this.GetProperty(PrecisionProperty); }
        }
        #endregion

        #region 来源类型 ItemSourceType
        /// <summary>
        /// 来源类型
        /// </summary>
        [Label("来源类型")]
        public static readonly Property<ItemSourceType?> ItemSourceTypeProperty = P<ProductBomDetail>.RegisterView(e => e.ItemSourceType, p => p.Item.ItemSourceType);

        /// <summary>
        /// 来源类型
        /// </summary>
        public ItemSourceType? ItemSourceType
        {
            get { return this.GetProperty(ItemSourceTypeProperty); }
        }
        #endregion

        #region 虚拟键 ItemIsVirtualPart
        /// <summary>
        /// 虚拟键
        /// </summary>
        [Label("虚拟键")]
        public static readonly Property<bool> ItemIsVirtualPartProperty = P<ProductBomDetail>.RegisterView(e => e.ItemIsVirtualPart, p => p.Item.IsVirtualPart);

        /// <summary>
        /// 虚拟键
        /// </summary>
        public bool ItemIsVirtualPart
        {
            get { return this.GetProperty(ItemIsVirtualPartProperty); }
        }
        #endregion

        #region 工段名称 ProcessSegmentName
        /// <summary>
        /// 工段名称
        /// </summary>
        [Label("工段名称")]
        public static readonly Property<string> ProcessSegmentNameProperty = P<ProductBomDetail>.RegisterView(e => e.ProcessSegmentName, p => p.ProcessSegment.Name);

        /// <summary>
        /// 工段名称
        /// </summary>
        public string ProcessSegmentName
        {
            get { return this.GetProperty(ProcessSegmentNameProperty); }
        }
        #endregion

        #region 是否拆分 IsSplit
        /// <summary>
        /// 是否拆分
        /// </summary>
        [Label("是否拆分")]
        public static readonly Property<bool?> IsSplitProperty = P<ProductBomDetail>.RegisterView(e => e.IsSplit, p => p.ProcessSegment.IsSplit);

        /// <summary>
        /// 是否拆分
        /// </summary>
        public bool? IsSplit
        {
            get { return this.GetProperty(IsSplitProperty); }
        }
        #endregion

        #region 启用扩展属性 EnableExtendProperty
        /// <summary>
        /// 启用扩展属性
        /// </summary>
        [Label("启用扩展属性")]
        public static readonly Property<bool> EnableExtendPropertyProperty = P<ProductBomDetail>.RegisterView(e => e.EnableExtendProperty, p => p.Item.EnableExtendProperty);

        /// <summary>
        /// 启用扩展属性
        /// </summary>
        public bool EnableExtendProperty
        {
            get { return this.GetProperty(EnableExtendPropertyProperty); }
        }
        #endregion
        #endregion

        /// <summary>
        /// 属性变更
        /// </summary>
        /// <param name="propertyName"></param>
        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == nameof(ItemId))
                PropertyValueList.Clear();
        }
    }

    /// <summary>
    /// 产品BOM明细 实体配置
    /// </summary>
    internal class ProductBomDetailConfig : EntityConfig<ProductBomDetail>
    {
        /// <summary>
        /// 对 Meta 属性的配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("ITEM_PROD_BOM_DTL").MapAllProperties();
            Meta.Property(ProductBomDetail.PropertyValueStrProperty).ColumnMeta.HasLength(1000);
            Meta.Property(ProductBomDetail.PropertyValueJsonProperty).ColumnMeta.HasLength(2000);
            Meta.EnablePhantoms();
        }
    }
}