using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Items
{
    /// <summary>
    /// 产品BOM物料属性值
    /// </summary>
    [ChildEntity, Serializable]
    [Label("产品BOM物料属性值")]
    [DisplayMember(nameof(Value))]
    public partial class ProductBomPropertyValue : DataEntity
    {
        #region 属性值 Value
        /// <summary>
        /// 属性值
        /// </summary>
        [Required]
        [MaxLength(40)]
        [Label("属性值")]
        public static readonly Property<string> ValueProperty = P<ProductBomPropertyValue>.Register(e => e.Value);

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
        [Required]
        [Label("属性定义")]
        public static readonly IRefIdProperty DefinitionIdProperty =
            P<ProductBomPropertyValue>.RegisterRefId(e => e.DefinitionId, ReferenceType.Normal);

        /// <summary>
        /// 属性定义Id
        /// </summary>
        public double DefinitionId
        {
            get { return (double)this.GetRefId(DefinitionIdProperty); }
            set { this.SetRefId(DefinitionIdProperty, value); }
        }

        /// <summary>
        /// 属性定义
        /// </summary>
        [Label("属性定义")]
        public static readonly RefEntityProperty<ItemPropertyDefinition> DefinitionProperty =
            P<ProductBomPropertyValue>.RegisterRef(e => e.Definition, DefinitionIdProperty);

        /// <summary>
        /// 属性定义
        /// </summary>
        public ItemPropertyDefinition Definition
        {
            get { return this.GetRefEntity(DefinitionProperty); }
            set { this.SetRefEntity(DefinitionProperty, value); }
        }
        #endregion

        #region 产品BOM ProductBom
        /// <summary>
        /// 产品BOMId
        /// </summary>
        [Required]
        [Label("产品BOM")]
        public static readonly IRefIdProperty ProductBomIdProperty = P<ProductBomPropertyValue>.RegisterRefId(e => e.ProductBomId, ReferenceType.Parent);

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
        [Label("产品BOM")]
        public static readonly RefEntityProperty<ProductBom> ProductBomProperty = P<ProductBomPropertyValue>.RegisterRef(e => e.ProductBom, ProductBomIdProperty);

        /// <summary>
        /// 产品BOM
        /// </summary>
        public ProductBom ProductBom
        {
            get { return GetRefEntity(ProductBomProperty); }
            set { SetRefEntity(ProductBomProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 产品BOM物料属性值 实体配置
    /// </summary>
    internal class ProductBomPropertyValueConfig : EntityConfig<ProductBomPropertyValue>
    {
        /// <summary>
        /// 对Meta属性的配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("ITEM_PROD_BOM_PROP_VAL").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}