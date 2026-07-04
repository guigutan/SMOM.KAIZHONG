using SIE.Core.Common;
using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Packages.ItemLabels
{
    /// <summary>
    /// 标签条码扩展属性
    /// </summary>
    [RootEntity, Serializable]
    [Label("标签条码扩展属性")]
    public partial class PackingLabelItemExtProp : BaseItemExtProp
    {
        #region 物料属性定义 Definition
        /// <summary>
        /// 物料属性定义Id
        /// </summary>
        [Label("物料属性")]
        public static readonly IRefIdProperty DefinitionIdProperty = P<PackingLabelItemExtProp>.RegisterRefId(e => e.DefinitionId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<ItemPropertyDefinition> DefinitionProperty = P<PackingLabelItemExtProp>.RegisterRef(e => e.Definition, DefinitionIdProperty);

        /// <summary>
        /// 物料属性定义
        /// </summary>
        public ItemPropertyDefinition Definition
        {
            get { return GetRefEntity(DefinitionProperty); }
            set { SetRefEntity(DefinitionProperty, value); }
        }
        #endregion

        #region 属性名称 DefinitionName
        /// <summary>
        /// 属性名称
        /// </summary>
        [Label("属性名称")]
        public static readonly Property<string> DefinitionNameProperty = P<PackingLabelItemExtProp>.RegisterView(e => e.DefinitionName, e => e.Definition.Name);

        /// <summary>
        /// 属性名称
        /// </summary>
        public string DefinitionName
        {
            get { return this.GetProperty(DefinitionNameProperty); }
        }
        #endregion

    }

    /// <summary>
    /// 物料属性值 实体配置
    /// </summary>
    internal class PackingLabelItemExtPropConfig : EntityConfig<PackingLabelItemExtProp>
    {
        /// <summary>
        /// 属性元数据配置
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("PACK_LABEL_ITEM_EXT_PROP").MapAllProperties();
            Meta.IndexGroupOnProperties(PackingLabelItemExtProp.FIdProperty, PackingLabelItemExtProp.ItemIdProperty, PackingLabelItemExtProp.DefinitionIdProperty);
            Meta.EnablePhantoms();
        }
    }
}