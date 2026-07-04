using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Items
{
    /// <summary>
    /// 物料属性值
    /// </summary>
    [ChildEntity, Serializable]
    [Label("物料属性值")]
    [DisplayMember(nameof(Value))]
    public partial class ItemPropertyValue : DataEntity
    {
        #region 名称 Name
        /// <summary>
        /// 名称
        /// </summary>
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<ItemPropertyValue>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return this.GetProperty(NameProperty); }
            set { this.SetProperty(NameProperty, value); }
        }
        #endregion

        #region 值 Value
        /// <summary>
        /// 值
        /// </summary>
        [Required]
        [MaxLength(40)]
        [Label("值")]
        public static readonly Property<string> ValueProperty = P<ItemPropertyValue>.Register(e => e.Value);

        /// <summary>
        /// 值
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
        [Label("物料属性")]
        public static readonly IRefIdProperty DefinitionIdProperty = P<ItemPropertyValue>.RegisterRefId(e => e.DefinitionId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<ItemPropertyDefinition> DefinitionProperty = P<ItemPropertyValue>.RegisterRef(e => e.Definition, DefinitionIdProperty);

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
        public static readonly Property<string> DefinitionNameProperty = P<ItemPropertyValue>.RegisterView(e => e.DefinitionName, p => p.Definition.Name);

        /// <summary>
        /// 属性值
        /// </summary>
        public string DefinitionName
        {
            get { return this.GetProperty(DefinitionNameProperty); }
        }
        #endregion

        #region 快码组ID DefinitionName
        /// <summary>
        /// 快码组ID
        /// </summary>
        [Label("快码组ID")]
        public static readonly Property<string> CatalogTypeIdProperty = P<ItemPropertyValue>.RegisterView(e => e.CatalogTypeId, p => p.Definition.CatalogTypeId);

        /// <summary>
        /// 快码组ID
        /// </summary>
        public string CatalogTypeId
        {
            get { return this.GetProperty(CatalogTypeIdProperty); }
        }
        #endregion

        #region 快码值 CatalogCode
        /// <summary>
        /// 快码值
        /// </summary>
        public static readonly Property<string> CatalogCodeProperty = P<ItemPropertyValue>.RegisterView(e => e.CatalogCode, p => p.Value);

        /// <summary>
        /// 快码值
        /// </summary>
        public string CatalogCode
        {
            get { return GetProperty(CatalogCodeProperty); }
        }
        #endregion

        #region 属性组 PropertyGroup
        /// <summary>
        /// 属性组
        /// </summary>
        [Label("属性组")]
        public static readonly Property<string> PropertyGroupProperty = P<ItemPropertyValue>.Register(e => e.PropertyGroup);

        /// <summary>
        /// 属性组
        /// </summary>
        public string PropertyGroup
        {
            get { return GetProperty(PropertyGroupProperty); }
            set { SetProperty(PropertyGroupProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料ID
        /// </summary>
        [Required]
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty = P<ItemPropertyValue>.RegisterRefId(e => e.ItemId, ReferenceType.Parent);

        /// <summary>
        /// 物料ID
        /// </summary>
        public double ItemId
        {
            get { return (double)this.GetRefId(ItemIdProperty); }
            set { this.SetRefId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        [Label("物料")]
        public static readonly RefEntityProperty<Item> ItemProperty = P<ItemPropertyValue>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return this.GetRefEntity(ItemProperty); }
            set { this.SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<ItemPropertyValue>.RegisterView(e => e.ItemCode, p => p.Item.Code);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
        }
        #endregion
    }

    /// <summary>
    /// 物料属性值 实体配置
    /// </summary>
    internal class ItemPropertyValueConfig : EntityConfig<ItemPropertyValue>
    {
        /// <summary>
        /// 属性元数据配置
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("ITEM_PROP_VALUE").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}