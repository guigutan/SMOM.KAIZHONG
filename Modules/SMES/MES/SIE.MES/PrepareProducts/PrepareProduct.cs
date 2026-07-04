using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.PrepareProducts
{
    /// <summary>
    /// 产品产前准备设置
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(PrepareProductCriteria))]
    [Label("产品产前准备设置")]
    public class PrepareProduct : DataEntity
    {
        #region 产品 Product
        /// <summary>
        /// 产品Id
        /// </summary>
        [Label("产品")]
        public static readonly IRefIdProperty ProductIdProperty =
            P<PrepareProduct>.RegisterRefId(e => e.ProductId, ReferenceType.Normal);

        /// <summary>
        /// 产品Id
        /// </summary>
        public double? ProductId
        {
            get { return (double?)this.GetRefNullableId(ProductIdProperty); }
            set { this.SetRefNullableId(ProductIdProperty, value); }
        }

        /// <summary>
        /// 产品
        /// </summary>
        public static readonly RefEntityProperty<Item> ProductProperty =
            P<PrepareProduct>.RegisterRef(e => e.Product, ProductIdProperty);

        /// <summary>
        /// 产品
        /// </summary>
        public Item Product
        {
            get { return this.GetRefEntity(ProductProperty); }
            set { this.SetRefEntity(ProductProperty, value); }
        }
        #endregion

        #region 产品族 ProductFamily
        /// <summary>
        /// 产品族Id
        /// </summary>
        [Label("产品族")]
        public static readonly IRefIdProperty ProductFamilyIdProperty =
            P<PrepareProduct>.RegisterRefId(e => e.ProductFamilyId, ReferenceType.Normal);

        /// <summary>
        /// 产品族Id
        /// </summary>
        public double? ProductFamilyId
        {
            get { return (double?)this.GetRefNullableId(ProductFamilyIdProperty); }
            set { this.SetRefNullableId(ProductFamilyIdProperty, value); }
        }

        /// <summary>
        /// 产品族
        /// </summary>
        public static readonly RefEntityProperty<ProductFamily> ProductFamilyProperty =
            P<PrepareProduct>.RegisterRef(e => e.ProductFamily, ProductFamilyIdProperty);

        /// <summary>
        /// 产品族
        /// </summary>
        public ProductFamily ProductFamily
        {
            get { return this.GetRefEntity(ProductFamilyProperty); }
            set { this.SetRefEntity(ProductFamilyProperty, value); }
        }
        #endregion

        #region 产前项目准备 PrepareProjectDetailList
        /// <summary>
        /// 产前项目准备
        /// </summary>
        [Label("产前项目准备")]
        public static readonly ListProperty<EntityList<PrepareProductDetail>> PrepareProjectDetailListProperty = P<PrepareProduct>.RegisterList(e => e.PrepareProjectDetailList);

        /// <summary>
        /// 产前项目准备
        /// </summary>
        public EntityList<PrepareProductDetail> PrepareProjectDetailList
        {
            get { return this.GetLazyList(PrepareProjectDetailListProperty); }
        }
        #endregion

        #region 视图属性
        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<PrepareProduct>.RegisterView(e => e.ProductCode, p => p.Product.Code);

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
        public static readonly Property<string> ProductNameProperty = P<PrepareProduct>.RegisterView(e => e.ProductName, p => p.Product.Name);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return this.GetProperty(ProductNameProperty); }
        }
        #endregion

        #region 产品族编码 ProductFamilyCode
        /// <summary>
        /// 产品族编码
        /// </summary>
        [Label("产品族编码")]
        public static readonly Property<string> ProductFamilyCodeProperty = P<PrepareProduct>.RegisterView(e => e.ProductFamilyCode, p => p.ProductFamily.Code);

        /// <summary>
        /// 产品族编码
        /// </summary>
        public string ProductFamilyCode
        {
            get { return this.GetProperty(ProductFamilyCodeProperty); }
        }
        #endregion

        #region 产品族名称 ProductFamilyName
        /// <summary>
        /// 产品族名称
        /// </summary>
        [Label("产品族名称")]
        public static readonly Property<string> ProductFamilyNameProperty = P<PrepareProduct>.RegisterView(e => e.ProductFamilyName, p => p.ProductFamily.Name);

        /// <summary>
        /// 产品族名称
        /// </summary>
        public string ProductFamilyName
        {
            get { return this.GetProperty(ProductFamilyNameProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 产品产前准备设置配置
    /// </summary>
    public class PrepareProductConfig : EntityConfig<PrepareProduct>
    {
        /// <summary>
        /// 数据库配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("MES_PREPRODUCT").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
