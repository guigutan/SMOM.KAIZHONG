using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.WIP.Products
{
    /// <summary>
    /// WIP产品属性
    /// </summary>
    [ChildEntity, Serializable]
    [Label("产品属性")]
    public partial class WipProductProperty : DataEntity
    {
        #region 属性值 Value
        /// <summary>
        /// 属性值
        /// </summary>
        [Required]
        [MaxLength(40)]
        [Label("属性值")]
        public static readonly Property<string> ValueProperty = P<WipProductProperty>.Register(e => e.Value);

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
        /// 物料属性定义ID
        /// </summary>
        [Label("属性定义")]
        public static readonly IRefIdProperty DefinitionIdProperty =
           P<WipProductProperty>.RegisterRefId(e => e.DefinitionId, ReferenceType.Normal);

        /// <summary>
        /// 物料属性定义ID
        /// </summary>
        public double DefinitionId
        {
            get { return (double)this.GetRefId(DefinitionIdProperty); }
            set { this.SetRefId(DefinitionIdProperty, value); }
        }

        /// <summary>
        /// 物料属性定义
        /// </summary>
        public static readonly RefEntityProperty<ItemPropertyDefinition> DefinitionProperty =
            P<WipProductProperty>.RegisterRef(e => e.Definition, DefinitionIdProperty);

        /// <summary>
        /// 物料属性定义
        /// </summary>
        public ItemPropertyDefinition Definition
        {
            get { return this.GetRefEntity(DefinitionProperty); }
            set { this.SetRefEntity(DefinitionProperty, value); }
        }
        #endregion

        #region 属性名称 DefinitionName
        /// <summary>
        /// 属性名称
        /// </summary>
        [Label("属性名称")]
        public static readonly Property<string> DefinitionNameProperty = P<WipProductProperty>.RegisterView(e => e.DefinitionName, p => p.Definition.Name);

        /// <summary>
        /// 属性名称
        /// </summary>
        public string DefinitionName
        {
            get { return this.GetProperty(DefinitionNameProperty); }
        }
        #endregion

        #region 属性 Product
        /// <summary>
        /// 属性Id
        /// </summary>
        [Label("属性")]
        public static readonly IRefIdProperty ProductIdProperty = P<WipProductProperty>.RegisterRefId(e => e.ProductId, ReferenceType.Parent);

        /// <summary>
        /// 属性Id
        /// </summary>
        public double ProductId
        {
            get { return (double)GetRefId(ProductIdProperty); }
            set { SetRefId(ProductIdProperty, value); }
        }

        /// <summary>
        /// 属性
        /// </summary>
        public static readonly RefEntityProperty<WipProduct> ProductProperty = P<WipProductProperty>.RegisterRef(e => e.Product, ProductIdProperty);

        /// <summary>
        /// 属性
        /// </summary>
        public WipProduct Product
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
        public static readonly Property<string> PropertyNameProperty = P<WipProductProperty>.RegisterView(e => e.PropertyName, p => p.Definition.Name);

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
    ///  WIP产品属性 实体配置
    /// </summary>
    internal class WipProductPropertyConfig : EntityConfig<WipProductProperty>
    {
        /// <summary>
        /// 配置数据库映射
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_PROD_PROP").MapAllProperties();
            Meta.Property(WipProductProperty.ProductIdProperty).ColumnMeta.HasIndex();
            Meta.EnablePhantoms();
        }
    }
}