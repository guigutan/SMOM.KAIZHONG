using SIE.Common.Catalogs;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Items
{
    /// <summary>
    /// 物料属性定义
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(ItemPropertyDefinitionCriteria))]
    [Label("物料属性定义")]
    [DisplayMember(nameof(Name))]
    public partial class ItemPropertyDefinition : DataEntity
    {
        #region 名称 Name
        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [NotDuplicate]
        [MaxLength(40)]
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<ItemPropertyDefinition>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 快码组 CatalogType
        /// <summary>
        /// 快码组Id
        /// </summary>
        [Label("快码组")]
        public static readonly IRefIdProperty CatalogTypeIdProperty = P<ItemPropertyDefinition>.RegisterRefId(e => e.CatalogTypeId, ReferenceType.Normal);

        /// <summary>
        /// 快码组Id
        /// </summary>
        public double? CatalogTypeId
        {
            get { return (double?)GetRefNullableId(CatalogTypeIdProperty); }
            set { SetRefNullableId(CatalogTypeIdProperty, value); }
        }

        /// <summary>
        /// 快码组
        /// </summary>
        public static readonly RefEntityProperty<CatalogType> CatalogTypeProperty = P<ItemPropertyDefinition>.RegisterRef(e => e.CatalogType, CatalogTypeIdProperty);

        /// <summary>
        /// 快码组
        /// </summary>
        [Label("快码组")]
        public CatalogType CatalogType
        {
            get { return GetRefEntity(CatalogTypeProperty); }
            set { SetRefEntity(CatalogTypeProperty, value); }
        }
        #endregion

        #region 类型 PropertyType
        /// <summary>
        /// 类型
        /// </summary>
        [Required]
        [Label("类型")]
        public static readonly Property<ItemPropertyType> PropertyTypeProperty = P<ItemPropertyDefinition>.Register(e => e.PropertyType);

        /// <summary>
        /// 类型
        /// </summary>
        public ItemPropertyType PropertyType
        {
            get { return GetProperty(PropertyTypeProperty); }
            set { SetProperty(PropertyTypeProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 物料属性定义 实体配置
    /// </summary>
    internal class ItemPropertyDefinitionConfig : EntityConfig<ItemPropertyDefinition>
    {
        /// <summary>
        /// 元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("ITEM_PROP_DEFINE").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}