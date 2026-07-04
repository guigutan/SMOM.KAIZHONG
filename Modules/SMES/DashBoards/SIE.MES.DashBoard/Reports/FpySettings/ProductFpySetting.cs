using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.DashBoard.Reports.FpySettings
{
    /// <summary>
    /// 产品直通率设置
    /// </summary>
    [ChildEntity, Serializable]
    [Label("产品直通率设置")]
    public partial class ProductFpySetting : FpySetting
    {
        #region 产品 Product
        /// <summary>
        /// 产品Id
        /// </summary>
        public static readonly IRefIdProperty ProductIdProperty = P<ProductFpySetting>.RegisterRefId(e => e.ProductId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Item> ProductProperty = P<ProductFpySetting>.RegisterRef(e => e.Product, ProductIdProperty);

        /// <summary>
        /// 产品
        /// </summary>
        public Item Product
        {
            get { return GetRefEntity(ProductProperty); }
            set { SetRefEntity(ProductProperty, value); }
        }
        #endregion

        #region 产品直通率设置 ProductModelFpySetting
        /// <summary>
        /// 产品直通率设置Id
        /// </summary>
        public static readonly IRefIdProperty ProductModelFpySettingIdProperty = P<ProductFpySetting>.RegisterRefId(e => e.ProductModelFpySettingId, ReferenceType.Parent);

        /// <summary>
        /// 产品直通率设置Id
        /// </summary>
        public double ProductModelFpySettingId
        {
            get { return (double)GetRefId(ProductModelFpySettingIdProperty); }
            set { SetRefId(ProductModelFpySettingIdProperty, value); }
        }

        /// <summary>
        /// 产品直通率设置
        /// </summary>
        public static readonly RefEntityProperty<ProductModelFpySetting> ProductModelFpySettingProperty = P<ProductFpySetting>.RegisterRef(e => e.ProductModelFpySetting, ProductModelFpySettingIdProperty);

        /// <summary>
        /// 产品直通率设置
        /// </summary>
        public ProductModelFpySetting ProductModelFpySetting
        {
            get { return GetRefEntity(ProductModelFpySettingProperty); }
            set { SetRefEntity(ProductModelFpySettingProperty, value); }
        }
        #endregion

        #region 注册视图属性
        #region 产品名称 ProductName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ProductNameProperty = P<ProductFpySetting>.RegisterView(e => e.ProductName, p => p.Product.Name);

        /// <summary>
        /// 产品机型名称
        /// </summary>
        public string ProductName
        {
            get { return this.GetProperty(ProductNameProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 产品直通率设置 实体配置
    /// </summary>
    internal class ProductFpySettingConfig : EntityConfig<ProductFpySetting>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_PROD_FPY").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}