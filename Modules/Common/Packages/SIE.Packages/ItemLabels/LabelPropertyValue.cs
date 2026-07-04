using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Packages.ItemLabels
{
    /// <summary>
    /// 箱号属性
    /// </summary>
    [RootEntity, Serializable]
    [DisplayMember(nameof(ItemLabelId))]
    [Label("标签属性")]
    public partial class LabelPropertyValue : DataEntity
    {
        #region 属性名 PropertyName
        /// <summary>
        /// 属性名
        /// </summary>
        [Label("属性名")]
        public static readonly Property<string> PropertyNameProperty = P<LabelPropertyValue>.Register(e => e.PropertyName);

        /// <summary>
        /// 属性名
        /// </summary>
        public string PropertyName
        {
            get { return GetProperty(PropertyNameProperty); }
            set { SetProperty(PropertyNameProperty, value); }
        }
        #endregion

        #region 属性值 PropertyValue
        /// <summary>
        /// 属性值
        /// </summary>
        [Label("属性值")]
        public static readonly Property<string> PropertyValueProperty = P<LabelPropertyValue>.Register(e => e.PropertyValue);

        /// <summary>
        /// 属性值
        /// </summary>
        public string PropertyValue
        {
            get { return GetProperty(PropertyValueProperty); }
            set { SetProperty(PropertyValueProperty, value); }
        }
        #endregion

        #region 箱号 ItemLabel
        /// <summary>
        /// 箱号Id
        /// </summary>
        [Label("箱号")]
        public static readonly IRefIdProperty ItemLabelIdProperty =
            P<LabelPropertyValue>.RegisterRefId(e => e.ItemLabelId, ReferenceType.Parent);

        /// <summary>
        /// 箱号Id
        /// </summary>
        public double ItemLabelId
        {
            get { return (double)this.GetRefId(ItemLabelIdProperty); }
            set { this.SetRefId(ItemLabelIdProperty, value); }
        }

        /// <summary>
        /// 箱号
        /// </summary>
        public static readonly RefEntityProperty<ItemLabel> ItemLabelProperty =
            P<LabelPropertyValue>.RegisterRef(e => e.ItemLabel, ItemLabelIdProperty);

        /// <summary>
        /// 箱号
        /// </summary>
        public ItemLabel ItemLabel
        {
            get { return this.GetRefEntity(ItemLabelProperty); }
            set { this.SetRefEntity(ItemLabelProperty, value); }
        }
        #endregion

        #region 属性定义 Definition
        /// <summary>
        /// 属性定义
        /// </summary>
        [Label("属性定义")]
        public static readonly IRefIdProperty DefinitionIdProperty =
            P<LabelPropertyValue>.RegisterRefId(e => e.DefinitionId, ReferenceType.Normal);

        /// <summary>
        /// 属性定义
        /// </summary>
        public double DefinitionId
        {
            get { return (double)this.GetRefId(DefinitionIdProperty); }
            set { this.SetRefId(DefinitionIdProperty, value); }
        }

        /// <summary>
        /// 属性定义
        /// </summary>
        public static readonly RefEntityProperty<ItemPropertyDefinition> DefinitionProperty =
            P<LabelPropertyValue>.RegisterRef(e => e.Definition, DefinitionIdProperty);

        /// <summary>
        /// 属性定义
        /// </summary>
        public ItemPropertyDefinition Definition
        {
            get { return this.GetRefEntity(DefinitionProperty); }
            set { this.SetRefEntity(DefinitionProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 箱号属性 实体配置
    /// </summary>
    internal class LabelPropertyValueConfig : EntityConfig<LabelPropertyValue>
    {
        /// <summary>
        /// Meta属性配置
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("Label_Property_Value").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}