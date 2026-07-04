using SIE.Common;
using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.TaskManagement.Specifications
{
    /// <summary>
	/// 产品规格件对照表
	/// </summary>
	[RootEntity, Serializable]
    [ConditionQueryType(typeof(ProductSpecificationCriteria))]
    [Label("产品规格件对照表")]
    public partial class ProductSpecification : DataEntity
    {
        #region 规格件清单 Details
        /// <summary>
        /// 规格件清单
        /// </summary>
        public static readonly ListProperty<EntityList<ProductSpecificationDetail>> DetailsProperty = P<ProductSpecification>.RegisterList(e => e.Details);
        /// <summary>
        /// 规格件清单
        /// </summary>
        public EntityList<ProductSpecificationDetail> Details
        {
            get { return this.GetLazyList(DetailsProperty); }
        }
        #endregion

        #region 产品 Product
        /// <summary>
        /// 产品Id
        /// </summary>
        public static readonly IRefIdProperty ProductIdProperty = P<ProductSpecification>.RegisterRefId(e => e.ProductId, ReferenceType.Normal);

        /// <summary>
        /// 产品Id
        /// </summary>
        public double ProductId
        {
            get { return (double)GetRefId(ProductIdProperty); }
            set { SetRefId(ProductIdProperty, value); }
        }

        /// <summary>
        /// 产品
        /// </summary>
        public static readonly RefEntityProperty<Item> ProductProperty = P<ProductSpecification>.RegisterRef(e => e.Product, ProductIdProperty);

        /// <summary>
        /// 产品
        /// </summary>
        public Item Product
        {
            get { return GetRefEntity(ProductProperty); }
            set { SetRefEntity(ProductProperty, value); }
        }
        #endregion

        #region bs界面显示
        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<ProductSpecification>.RegisterView(e => e.ProductCode, p => p.Product.Code);

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode
        {
            get { return this.GetProperty(ProductCodeProperty); }
        }
        #endregion

        #region 产品名称 ProductName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ProductNameProperty = P<ProductSpecification>.RegisterView(e => e.ProductName, p => p.Product.Name);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return this.GetProperty(ProductNameProperty); }
        }
        #endregion

        #region 规格型号 ProductSpecificationModel
        /// <summary>
        /// 规格型号
        /// </summary>
        [Label("规格型号")]
        public static readonly Property<string> ProductSpecificationModelProperty = P<ProductSpecification>.RegisterView(e => e.ProductSpecificationModel, p => p.Product.SpecificationModel);

        /// <summary>
        /// 规格型号
        /// </summary>
        public string ProductSpecificationModel
        {
            get { return this.GetProperty(ProductSpecificationModelProperty); }
        }
        #endregion

        #region 基本计量单位 ProductUnitName
        /// <summary>
        /// 基本计量单位
        /// </summary>
        [Label("基本计量单位")]
        public static readonly Property<string> ProductUnitNameProperty = P<ProductSpecification>.RegisterView(e => e.ProductUnitName, p => p.Product.Unit.Name);

        /// <summary>
        /// 基本计量单位
        /// </summary>
        public string ProductUnitName
        {
            get { return this.GetProperty(ProductUnitNameProperty); }
        }
        #endregion

        #region 基本分类 ProductType
        /// <summary>
        /// 基本分类
        /// </summary>
        [Label("基本分类")]
        public static readonly Property<ItemType> ProductTypeProperty = P<ProductSpecification>.RegisterView(e => e.ProductType, p => p.Product.Type);

        /// <summary>
        /// 基本分类
        /// </summary>
        public ItemType ProductType
        {
            get { return this.GetProperty(ProductTypeProperty); }
        }
        #endregion

        #region 来源类型 ProductItemSourceType
        /// <summary>
        /// 来源类型
        /// </summary>
        [Label("来源类型")]
        public static readonly Property<ItemSourceType?> ProductItemSourceTypeProperty = P<ProductSpecification>.RegisterView(e => e.ProductItemSourceType, p => p.Product.ItemSourceType);

        /// <summary>
        /// 来源类型
        /// </summary>
        public ItemSourceType? ProductItemSourceType
        {
            get { return this.GetProperty(ProductItemSourceTypeProperty); }
        }
        #endregion

        #region 状态 ProductState
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<State> ProductStateProperty = P<ProductSpecification>.RegisterView(e => e.ProductState, p => p.Product.State);

        /// <summary>
        /// 状态
        /// </summary>
        public State ProductState
        {
            get { return this.GetProperty(ProductStateProperty); }
        }
        #endregion

        #region 来源 ProductSourceType
        /// <summary>
        /// 来源
        /// </summary>
        [Label("来源")]
        public static readonly Property<SourceType> ProductSourceTypeProperty = P<ProductSpecification>.RegisterView(e => e.ProductSourceType, p => p.Product.SourceType);

        /// <summary>
        /// 来源
        /// </summary>
        public SourceType ProductSourceType
        {
            get { return this.GetProperty(ProductSourceTypeProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 产品规格件对照表 实体配置
    /// </summary>
    internal class ProductSpecificationConfig : EntityConfig<ProductSpecification>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("TM_PROD_SPEC").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}











































































































































































































































































































































































































































































































































































































































































































































































































































































