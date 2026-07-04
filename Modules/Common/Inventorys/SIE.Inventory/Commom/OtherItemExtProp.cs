using SIE.Core.Common;
using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Inventory.Commom
{
    /// <summary>
    /// 其他扩展属性
    /// </summary>
    [RootEntity, Serializable]
    [Label("其他扩展属性")]
    public partial class OtherItemExtProp : BaseItemExtProp
    {
        #region 物料属性定义 Definition
        /// <summary>
        /// 物料属性定义Id
        /// </summary>
        [Label("物料属性")]
        public static readonly IRefIdProperty DefinitionIdProperty = P<OtherItemExtProp>.RegisterRefId(e => e.DefinitionId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<ItemPropertyDefinition> DefinitionProperty = P<OtherItemExtProp>.RegisterRef(e => e.Definition, DefinitionIdProperty);

        /// <summary>
        /// 物料属性定义
        /// </summary>
        public ItemPropertyDefinition Definition
        {
            get { return GetRefEntity(DefinitionProperty); }
            set { SetRefEntity(DefinitionProperty, value); }
        }
        #endregion

        #region 物料属性名称 DefinitionName
        /// <summary>
        /// 物料属性名称
        /// </summary>
        [Label("物料属性名称")]
        public static readonly Property<string> DefinitionNameProperty = P<OtherItemExtProp>.RegisterView(e => e.DefinitionName, e => e.Definition.Name);

        /// <summary>
        /// 物料属性名称
        /// </summary>
        public string DefinitionName
        {
            get { return this.GetProperty(DefinitionNameProperty); }
        }
        #endregion

        #region 类型 Type
        /// <summary>
        /// 类型
        /// </summary>
        public static readonly Property<ItemExtPropFunctionType> TypeProperty = P<OtherItemExtProp>.Register(e => e.Type);

        /// <summary>
        /// 类型
        /// </summary>
        public ItemExtPropFunctionType Type
        {
            get { return GetProperty(TypeProperty); }
            set { SetProperty(TypeProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 物料属性值 实体配置
    /// </summary>
    internal class OtherItemExtPropConfig : EntityConfig<OtherItemExtProp>
    {
        /// <summary>
        /// 属性元数据配置
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("OTHER_ITEM_EXT_PROP").MapAllProperties();
            Meta.IndexGroupOnProperties(OtherItemExtProp.FIdProperty, OtherItemExtProp.ItemIdProperty, OtherItemExtProp.DefinitionIdProperty);
            Meta.EnablePhantoms();
        }
    }
}