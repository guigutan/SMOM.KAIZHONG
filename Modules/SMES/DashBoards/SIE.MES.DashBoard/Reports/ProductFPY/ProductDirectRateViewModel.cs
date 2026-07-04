using SIE.Domain;
using SIE.MES.DashBoard.Reports.Commons;
using SIE.MES.DashBoard.Reports.FpySettings;
using SIE.ObjectModel;
using System;

namespace SIE.MES.DashBoard.Reports.ProductFPY
{
    /// <summary>
    /// 产品报表ViewModel
    /// </summary>
    [RootEntity, Serializable]
    public class ProductDirectRateViewModel : DirectRateBaseViewModel
    {
        #region 产品机型Id ProductModelId
        /// <summary>
        /// 产品机型Id
        /// </summary>
        [Label("产品机型Id")]
        public static readonly Property<double?> ProductModelIdProperty = P<ProductDirectRateViewModel>.Register(e => e.ProductModelId);

        /// <summary>
        /// 产品机型Id
        /// </summary>
        public double? ProductModelId
        {
            get { return this.GetProperty(ProductModelIdProperty); }
            set { this.SetProperty(ProductModelIdProperty, value); }
        }
        #endregion

        #region 产品机型 ProductModel
        /// <summary>
        /// 产品机型
        /// </summary>
        [Label("产品机型")]
        [FieldSettingAttribute("机型", FieldArea.RowArea, 0, SummaryType = SummaryType.Custom)]
        public static readonly Property<string> ProductModelProperty = P<ProductDirectRateViewModel>.Register(e => e.ProductModel);

        /// <summary>
        /// 产品机型
        /// </summary>
        public string ProductModel
        {
            get { return this.GetProperty(ProductModelProperty); }
            set { this.SetProperty(ProductModelProperty, value); }
        }
        #endregion

        #region 产品机型直通率设置 ModelDirectRate
        /// <summary>
        /// 产品机型直通率设置Id
        /// </summary>
        [Label("产品机型直通率设置")]
        public static readonly IRefIdProperty ModelDirectRateIdProperty =
            P<ProductDirectRateViewModel>.RegisterRefId(e => e.ModelDirectRateId, ReferenceType.Normal);

        /// <summary>
        /// 产品机型直通率设置Id
        /// </summary>
        public double? ModelDirectRateId
        {
            get { return (double?)this.GetRefNullableId(ModelDirectRateIdProperty); }
            set { this.SetRefNullableId(ModelDirectRateIdProperty, value); }
        }

        /// <summary>
        /// 产品机型直通率设置
        /// </summary>
        public static readonly RefEntityProperty<ProductModelFpySetting> ModelDirectRateProperty =
            P<ProductDirectRateViewModel>.RegisterRef(e => e.ModelDirectRate, ModelDirectRateIdProperty);

        /// <summary>
        /// 产品机型直通率设置
        /// </summary>
        public ProductModelFpySetting ModelDirectRate
        {
            get { return this.GetRefEntity(ModelDirectRateProperty); }
            set { this.SetRefEntity(ModelDirectRateProperty, value); }
        }
        #endregion

        #region 产品Id ProductId
        /// <summary>
        /// 产品Id
        /// </summary>
        [Label("产品Id")]
        public static readonly Property<double?> ProductIdProperty = P<ProductDirectRateViewModel>.Register(e => e.ProductId);

        /// <summary>
        /// 产品Id
        /// </summary>
        public double? ProductId
        {
            get { return this.GetProperty(ProductIdProperty); }
            set { this.SetProperty(ProductIdProperty, value); }
        }
        #endregion

        #region 产品 Product
        /// <summary>
        /// 产品
        /// </summary>
        [Label("产品")]
        [FieldSettingAttribute("产品", FieldArea.RowArea, 1, SummaryType = SummaryType.Custom)]
        public static readonly Property<string> ProductProperty = P<ProductDirectRateViewModel>.Register(e => e.Product);

        /// <summary>
        /// 产品
        /// </summary>
        public string Product
        {
            get { return this.GetProperty(ProductProperty); }
            set { this.SetProperty(ProductProperty, value); }
        }
        #endregion

        #region 产品直通率设置 ProductDirectRate
        /// <summary>
        /// 产品直通率设置Id
        /// </summary>
        [Label("产品直通率设置")]
        public static readonly IRefIdProperty ProductDirectRateIdProperty =
            P<ProductDirectRateViewModel>.RegisterRefId(e => e.ProductDirectRateId, ReferenceType.Normal);

        /// <summary>
        /// 产品直通率设置Id
        /// </summary>
        public double? ProductDirectRateId
        {
            get { return (double?)this.GetRefNullableId(ProductDirectRateIdProperty); }
            set { this.SetRefNullableId(ProductDirectRateIdProperty, value); }
        }

        /// <summary>
        /// 产品直通率设置
        /// </summary>
        public static readonly RefEntityProperty<ProductFpySetting> ProductDirectRateProperty =
            P<ProductDirectRateViewModel>.RegisterRef(e => e.ProductDirectRate, ProductDirectRateIdProperty);

        /// <summary>
        /// 产品直通率设置
        /// </summary>
        public ProductFpySetting ProductDirectRate
        {
            get { return this.GetRefEntity(ProductDirectRateProperty); }
            set { this.SetRefEntity(ProductDirectRateProperty, value); }
        }
        #endregion
    }
}
