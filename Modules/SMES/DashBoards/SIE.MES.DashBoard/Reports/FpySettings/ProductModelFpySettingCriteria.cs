using SIE.Domain;
using SIE.Items;
using System;

namespace SIE.MES.DashBoard.Reports.FpySettings
{
    /// <summary>
    /// 产品机型直通率查询实体
    /// </summary>
    [QueryEntity, Serializable]
    public partial class ProductModelFpySettingCriteria : Criteria
    {
        #region 产品机型 Model
        /// <summary>
        /// 产品机型Id
        /// </summary>
        public static readonly IRefIdProperty ModelIdProperty = P<ProductModelFpySettingCriteria>.RegisterRefId(e => e.ModelId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<ProductModel> ModelProperty = P<ProductModelFpySettingCriteria>.RegisterRef(e => e.Model, ModelIdProperty);

        /// <summary>
        /// 产品机型
        /// </summary>
        public ProductModel Model
        {
            get { return GetRefEntity(ModelProperty); }
            set { SetRefEntity(ModelProperty, value); }
        }
        #endregion

        #region 产品 Product
        /// <summary>
        /// 产品Id
        /// </summary>
        public static readonly IRefIdProperty ProductIdProperty = P<ProductModelFpySettingCriteria>.RegisterRefId(e => e.ProductId, ReferenceType.Normal);

        /// <summary>
        /// 产品Id
        /// </summary>
        public double? ProductId
        {
            get { return (double?)GetRefNullableId(ProductIdProperty); }
            set { SetRefNullableId(ProductIdProperty, value); }
        }

        /// <summary>
        /// 产品
        /// </summary>
        public static readonly RefEntityProperty<Item> ProductProperty = P<ProductModelFpySettingCriteria>.RegisterRef(e => e.Product, ProductIdProperty);

        /// <summary>
        /// 产品
        /// </summary>
        public Item Product
        {
            get { return GetRefEntity(ProductProperty); }
            set { SetRefEntity(ProductProperty, value); }
        }
        #endregion

        /// <summary>
        /// 重写此方法实现查询
        /// </summary>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<FpySettingController>().GetProductModelFpySettings(this);
        }
    }
}