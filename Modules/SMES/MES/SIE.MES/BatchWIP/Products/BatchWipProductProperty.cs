using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.BatchWIP.Products
{
    /// <summary>
    /// 产品属性
    /// </summary>
    [ChildEntity, Serializable]
    [Label("产品属性")]
    public partial class BatchWipProductProperty : DataEntity
    {
        #region 属性值 Value
        /// <summary>
        /// 属性值
        /// </summary>
        [Label("属性值")]
        public static readonly Property<string> ValueProperty = P<BatchWipProductProperty>.Register(e => e.Value);

        /// <summary>
        /// 属性值
        /// </summary>
        public string Value
        {
            get { return GetProperty(ValueProperty); }
            set { SetProperty(ValueProperty, value); }
        }
        #endregion

        #region 属性定义 Definition
        /// <summary>
        /// 属性定义Id
        /// </summary>
        [Label("属性定义")]
        public static readonly IRefIdProperty DefinitionIdProperty = P<BatchWipProductProperty>.RegisterRefId(e => e.DefinitionId, ReferenceType.Normal);

        /// <summary>
        /// 属性定义Id
        /// </summary>
        public double DefinitionId
        {
            get { return (double)GetRefId(DefinitionIdProperty); }
            set { SetRefId(DefinitionIdProperty, value); }
        }

        /// <summary>
        /// 属性定义
        /// </summary>
        public static readonly RefEntityProperty<ItemPropertyDefinition> DefinitionProperty = P<BatchWipProductProperty>.RegisterRef(e => e.Definition, DefinitionIdProperty);

        /// <summary>
        /// 属性定义
        /// </summary>
        public ItemPropertyDefinition Definition
        {
            get { return GetRefEntity(DefinitionProperty); }
            set { SetRefEntity(DefinitionProperty, value); }
        }
        #endregion

        #region 产品 Product
        /// <summary>
        /// 产品Id
        /// </summary>
        [Label("产品")]
        public static readonly IRefIdProperty ProductIdProperty = P<BatchWipProductProperty>.RegisterRefId(e => e.ProductId, ReferenceType.Parent);

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
        public static readonly RefEntityProperty<BatchWipProduct> ProductProperty = P<BatchWipProductProperty>.RegisterRef(e => e.Product, ProductIdProperty);

        /// <summary>
        /// 产品
        /// </summary>
        public BatchWipProduct Product
        {
            get { return GetRefEntity(ProductProperty); }
            set { SetRefEntity(ProductProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 属性名称 PropertyName
        /// <summary>
        /// 属性名称
        /// </summary>
        [Label("属性名称")]
        public static readonly Property<string> PropertyNameProperty = P<BatchWipProductProperty>.RegisterView(e => e.PropertyName, p => p.Definition.Name);

        /// <summary>
        /// 属性名称
        /// </summary>
        public string PropertyName
        {
            get { return this.GetProperty(PropertyNameProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 产品属性 实体配置
    /// </summary>
    internal class BatchWipProductPropertyConfig : EntityConfig<BatchWipProductProperty>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("BWIP_PROD_PROP").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}