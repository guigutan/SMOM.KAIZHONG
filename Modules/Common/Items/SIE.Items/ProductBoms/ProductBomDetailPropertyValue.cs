using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Items
{
    /// <summary>
    /// 产品BOM明细属性值
    /// </summary>
    [ChildEntity, Serializable]
    [Label("产品BOM明细属性值")]
    [DisplayMember(nameof(Value))]
    public partial class ProductBomDetailPropertyValue : DataEntity
    {
        #region 属性值 Value
        /// <summary>
        /// 属性值
        /// </summary>
        [Required]
        [MaxLength(40)]
        [Label("属性值")]
        public static readonly Property<string> ValueProperty = P<ProductBomDetailPropertyValue>.Register(e => e.Value);

        /// <summary>
        /// 属性值
        /// </summary>
        public string Value
        {
            get { return GetProperty(ValueProperty); }
            set { SetProperty(ValueProperty, value); }
        }
        #endregion

        #region 物料属性定义 Definition
        /// <summary>
        /// 物料属性定义Id
        /// </summary>
        [Required]
        [Label("物料属性定义")]
        public static readonly IRefIdProperty DefinitionIdProperty = P<ProductBomDetailPropertyValue>.RegisterRefId(e => e.DefinitionId, ReferenceType.Normal);

        /// <summary>
        /// 物料属性定义Id
        /// </summary>
        public double DefinitionId
        {
            get { return (double)GetRefId(DefinitionIdProperty); }
            set { SetRefId(DefinitionIdProperty, value); }
        }

        /// <summary>
        /// 物料属性定义
        /// </summary>
        [Label("物料属性定义")]
        public static readonly RefEntityProperty<ItemPropertyDefinition> DefinitionProperty = P<ProductBomDetailPropertyValue>.RegisterRef(e => e.Definition, DefinitionIdProperty);

        /// <summary>
        /// 物料属性定义
        /// </summary>
        public ItemPropertyDefinition Definition
        {
            get { return GetRefEntity(DefinitionProperty); }
            set { SetRefEntity(DefinitionProperty, value); }
        }
        #endregion

        #region 属性值 DefinitionName
        /// <summary>
        /// 属性值
        /// </summary>
        [Label("属性值")]
        public static readonly Property<string> DefinitionNameProperty = P<ProductBomDetailPropertyValue>.RegisterView(e => e.DefinitionName, p => p.Definition.Name);

        /// <summary>
        /// 属性值
        /// </summary>
        public string DefinitionName
        {
            get { return this.GetProperty(DefinitionNameProperty); }
        }
        #endregion

        #region 属性组 PropertyGroup
        /// <summary>
        /// 属性组
        /// </summary>
        [Label("属性组")]
        public static readonly Property<string> PropertyGroupProperty = P<ProductBomDetailPropertyValue>.Register(e => e.PropertyGroup);

        /// <summary>
        /// 属性组
        /// </summary>
        public string PropertyGroup
        {
            get { return GetProperty(PropertyGroupProperty); }
            set { SetProperty(PropertyGroupProperty, value); }
        }
        #endregion

        #region 产品BOM明细 BomDetail
        /// <summary>
        /// 产品BOM明细Id
        /// </summary>
        [Required]
        [Label("产品BOM明细")]
        public static readonly IRefIdProperty DetailIdProperty =
            P<ProductBomDetailPropertyValue>.RegisterRefId(e => e.DetailId, ReferenceType.Parent);

        /// <summary>
        /// 产品BOM明细Id
        /// </summary>
        public double DetailId
        {
            get { return (double)this.GetRefId(DetailIdProperty); }
            set { this.SetRefId(DetailIdProperty, value); }
        }

        /// <summary>
        /// 产品BOM明细
        /// </summary>
        [Label("产品BOM明细")]
        public static readonly RefEntityProperty<ProductBomDetail> DetailProperty =
            P<ProductBomDetailPropertyValue>.RegisterRef(e => e.Detail, DetailIdProperty);

        /// <summary>
        /// 产品BOM明细
        /// </summary>
        public ProductBomDetail Detail
        {
            get { return this.GetRefEntity(DetailProperty); }
            set { this.SetRefEntity(DetailProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 产品BOM明细属性值 实体配置
    /// </summary>
    internal class ProductBomDetailPropertyValueConfig : EntityConfig<ProductBomDetailPropertyValue>
    {
        /// <summary>
        /// 对Meta属性的配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("ITEM_PROD_BOM_DTL_PROP_VAL").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}