using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.DashBoard.Reports.FpySettings
{
    /// <summary>
    /// 产品机型直通率设置
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(ProductModelFpySettingCriteria))]
    [Label("产品机型直通率设置")]
    public partial class ProductModelFpySetting : FpySetting
    {
        #region 产品机型 Model
        /// <summary>
        /// 产品机型Id
        /// </summary>
        public static readonly IRefIdProperty ModelIdProperty = P<ProductModelFpySetting>.RegisterRefId(e => e.ModelId, ReferenceType.Normal);

        /// <summary>
        /// 产品机型Id
        /// </summary>
        public double ModelId
        {
            get { return (double)GetRefId(ModelIdProperty); }
            set { SetRefId(ModelIdProperty, value); }
        }

        /// <summary>
        /// 产品机型
        /// </summary>
        public static readonly RefEntityProperty<ProductModel> ModelProperty = P<ProductModelFpySetting>.RegisterRef(e => e.Model, ModelIdProperty);

        /// <summary>
        /// 产品机型
        /// </summary>
        public ProductModel Model
        {
            get { return GetRefEntity(ModelProperty); }
            set { SetRefEntity(ModelProperty, value); }
        }
        #endregion

        #region 产品直通率设置 ProductFpyList
        /// <summary>
        /// 产品直通率设置
        /// </summary>
        public static readonly ListProperty<EntityList<ProductFpySetting>> ProductFpyListProperty = P<ProductModelFpySetting>.RegisterList(e => e.ProductFpyList);

        /// <summary>
        /// 产品直通率设置
        /// </summary>
        public EntityList<ProductFpySetting> ProductFpyList
        {
            get { return this.GetLazyList(ProductFpyListProperty); }
        }
        #endregion

        #region 注册视图属性
        #region 产品机型名称 ProductModelName
        /// <summary>
        /// 产品机型名称
        /// </summary>
        [Label("产品机型名称")]
        public static readonly Property<string> ProductModelNameProperty = P<ProductModelFpySetting>.RegisterView(e => e.ProductModelName, p => p.Model.Name);

        /// <summary>
        /// 产品机型名称
        /// </summary>
        public string ProductModelName
        {
            get { return this.GetProperty(ProductModelNameProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 产品机型直通率设置 实体配置
    /// </summary>
    internal class ProductModelFpySettingConfig : EntityConfig<ProductModelFpySetting>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_PROD_ML_FPY").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}