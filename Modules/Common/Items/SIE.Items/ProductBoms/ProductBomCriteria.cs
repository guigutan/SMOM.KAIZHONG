using SIE.Domain;
using SIE.Items.ProductBoms;
using SIE.ObjectModel;
using System;

namespace SIE.Items
{
    /// <summary>
    /// 产品BOM
    /// </summary>
    [QueryEntity, Serializable]
    [Label("产品BOM")]
    [DisplayMember(nameof(Name))]
    public partial class ProductBomCriteria : Criteria
    {
        #region BOM编码 Code
        /// <summary>
        /// BOM编码
        /// </summary>
        [Label("BOM编码")]
        public static readonly Property<string> CodeProperty = P<ProductBomCriteria>.Register(e => e.Code);

        /// <summary>
        /// BOM编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region BOM名称 Name
        /// <summary>
        /// BOM名称
        /// </summary>
        [Label("BOM名称")]
        public static readonly Property<string> NameProperty = P<ProductBomCriteria>.Register(e => e.Name);

        /// <summary>
        /// BOM名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<ProductBomCriteria>.Register(e => e.ProductCode);

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode
        {
            get { return this.GetProperty(ProductCodeProperty); }
            set { this.SetProperty(ProductCodeProperty, value); }
        }
        #endregion

        #region 产品名称 ProductName
        /// <summary>
        ///  产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ProductNameProperty = P<ProductBomCriteria>.Register(e => e.ProductName);

        /// <summary>
        ///  产品名称
        /// </summary>
        public string ProductName
        {
            get { return this.GetProperty(ProductNameProperty); }
            set { this.SetProperty(ProductNameProperty, value); }
        }
        #endregion

        #region 产品 Product
        /// <summary>
        /// 产品Id
        /// </summary>
        [Label("产品")]
        public static readonly IRefIdProperty ProductIdProperty = P<ProductBomCriteria>.RegisterRefId(e => e.ProductId, ReferenceType.Normal);

        /// <summary>
        /// 产品Id
        /// </summary>
        public string ProductId
        {
            get { return (string)GetRefId(ProductIdProperty); }
            set { SetRefId(ProductIdProperty, value); }
        }

        /// <summary>
        /// 产品
        /// </summary>
        public static readonly RefEntityProperty<Item> ProductProperty = P<ProductBomCriteria>.RegisterRef(e => e.Product, ProductIdProperty);

        /// <summary>
        /// 产品
        /// </summary>
        public Item Product
        {
            get { return GetRefEntity(ProductProperty); }
            set { SetRefEntity(ProductProperty, value); }
        }
        #endregion

        #region 规格型号 SpecificationModel
        /// <summary>
        ///  规格型号
        /// </summary>
        [Label("规格型号")]
        public static readonly Property<string> SpecificationModelProperty = P<ProductBomCriteria>.Register(e => e.SpecificationModel);

        /// <summary>
        ///  规格型号
        /// </summary>
        public string SpecificationModel
        {
            get { return this.GetProperty(SpecificationModelProperty); }
            set { this.SetProperty(SpecificationModelProperty, value); }
        }
        #endregion

        /// <summary>
        /// 重写此方法实现查询
        /// </summary>
        /// <returns>实体列表</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<ProductBomController>().GetProductBoms(this);
        }
    }
}
