using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Items
{
    /// <summary>
    /// 组合替代
    /// </summary>
    [ChildEntity, Serializable]
    //[CriteriaQuery]
    [Label("组合替代")]
    public partial class CombinationReplate : DataEntity
    {
        #region 替代组 ReplateGroup
        /// <summary>
        /// 替代组
        /// </summary>
        [Label("替代组")]
        [Required]
        public static readonly Property<string> ReplateGroupProperty = P<CombinationReplate>.Register(e => e.ReplateGroup);

        /// <summary>
        /// 替代组
        /// </summary>
        public string ReplateGroup
        {
            get { return GetProperty(ReplateGroupProperty); }
            set { SetProperty(ReplateGroupProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料编号")]
        public static readonly IRefIdProperty ItemIdProperty = P<CombinationReplate>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Item> ItemProperty = P<CombinationReplate>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 主物料 MainMaterial
        /// <summary>
        /// 主物料Id
        /// </summary>
        [Label("主物料编号")]
        public static readonly IRefIdProperty MainMaterialIdProperty = P<CombinationReplate>.RegisterRefId(e => e.MainMaterialId, ReferenceType.Normal);

        /// <summary>
        /// 主物料Id
        /// </summary>
        public double MainMaterialId
        {
            get { return (double)GetRefId(MainMaterialIdProperty); }
            set { SetRefId(MainMaterialIdProperty, value); }
        }

        /// <summary>
        /// 主物料
        /// </summary>
        public static readonly RefEntityProperty<Item> MainMaterialProperty = P<CombinationReplate>.RegisterRef(e => e.MainMaterial, MainMaterialIdProperty);

        /// <summary>
        /// 主物料
        /// </summary>
        public Item MainMaterial
        {
            get { return GetRefEntity(MainMaterialProperty); }
            set { SetRefEntity(MainMaterialProperty, value); }
        }
        #endregion

        #region 属性值字符串Json格式 PropertyValueJson
        /// <summary>
        /// 属性值字符串Json格式
        /// </summary>
        [MaxLength(2000)]
        [Label("属性值字符串Json格式")]
        public static readonly Property<string> PropertyValueJsonProperty = P<CombinationReplate>.Register(e => e.PropertyValueJson);

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
        public static readonly Property<string> PropertyValueStrProperty = P<CombinationReplate>.Register(e => e.PropertyValueStr);

        /// <summary>
        /// 物料属性值
        /// </summary>
        public string PropertyValueStr
        {
            get { return GetProperty(PropertyValueStrProperty); }
            set { SetProperty(PropertyValueStrProperty, value); }
        }
        #endregion

        #region 组合替代属性值列表 PropertyValueList
        /// <summary>
        /// 组合替代属性值列表
        /// </summary>
        public static readonly ListProperty<EntityList<CombinationReplatePropertyValue>> PropertyValueListProperty = P<CombinationReplate>.RegisterList(e => e.PropertyValueList);
       
        /// <summary>
        /// 组合替代属性值列表
        /// </summary>
        public EntityList<CombinationReplatePropertyValue> PropertyValueList
        {
            get { return this.GetLazyList(PropertyValueListProperty); }
        }
        #endregion

        #region 产品BOM ProductBom
        /// <summary>
        /// 产品BOMId
        /// </summary>
        public static readonly IRefIdProperty ProductBomIdProperty = P<CombinationReplate>.RegisterRefId(e => e.ProductBomId, ReferenceType.Parent);

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
        public static readonly RefEntityProperty<ProductBom> ProductBomProperty = P<CombinationReplate>.RegisterRef(e => e.ProductBom, ProductBomIdProperty);

        /// <summary>
        /// 产品BOM
        /// </summary>
        public ProductBom ProductBom
        {
            get { return GetRefEntity(ProductBomProperty); }
            set { SetRefEntity(ProductBomProperty, value); }
        }
        #endregion

        #region 扩展视图
        #region 主物料名称 MainMaterialName
        /// <summary>
        /// 主物料名称
        /// </summary>
        [Label("主物料名称")]
        public static readonly Property<string> MainMaterialNameProperty = P<CombinationReplate>.RegisterView(e => e.MainMaterialName, p => p.MainMaterial.Name);

        /// <summary>
        /// 主物料名称
        /// </summary>
        public string MainMaterialName
        {
            get { return this.GetProperty(MainMaterialNameProperty); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<CombinationReplate>.RegisterView(e => e.ItemName, p => p.Item.Name);

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
        public static readonly Property<string> ItemSpecificationModelProperty = P<CombinationReplate>.RegisterView(e => e.ItemSpecificationModel, p => p.Item.SpecificationModel);

        /// <summary>
        /// 规格型号
        /// </summary>
        public string ItemSpecificationModel
        {
            get { return this.GetProperty(ItemSpecificationModelProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 组合替代 实体配置
    /// </summary>
    internal class CombinationReplateConfig : EntityConfig<CombinationReplate>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("ITEM_COM_REPLATE").MapAllProperties();
            Meta.Property(CombinationReplate.PropertyValueStrProperty).ColumnMeta.HasLength(1000);
            Meta.Property(CombinationReplate.PropertyValueJsonProperty).ColumnMeta.HasLength(2000);
            Meta.EnablePhantoms();
        }
    }
}