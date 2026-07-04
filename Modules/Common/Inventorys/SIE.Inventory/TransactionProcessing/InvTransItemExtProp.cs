using SIE.Core.Common;
using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Inventory.Onhands
{
    /// <summary>
    /// 库存事务物料扩展属性
    /// </summary>
    [RootEntity, Serializable]
    [Label("库存事务物料扩展属性")]
    public partial class InvTransItemExtProp : BaseItemExtProp
    {
        #region 物料属性定义 Definition
        /// <summary>
        /// 物料属性定义Id
        /// </summary>
        [Label("物料属性")]
        public static readonly IRefIdProperty DefinitionIdProperty = P<InvTransItemExtProp>.RegisterRefId(e => e.DefinitionId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<ItemPropertyDefinition> DefinitionProperty = P<InvTransItemExtProp>.RegisterRef(e => e.Definition, DefinitionIdProperty);

        /// <summary>
        /// 物料属性定义
        /// </summary>
        public ItemPropertyDefinition Definition
        {
            get { return GetRefEntity(DefinitionProperty); }
            set { SetRefEntity(DefinitionProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 物料属性值 实体配置
    /// </summary>
    internal class InvTransItemExtPropConfig : EntityConfig<InvTransItemExtProp>
    {
        /// <summary>
        /// 属性元数据配置
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("INV_TRAN_ITEM_EXT_PROP").MapAllProperties();
            Meta.IndexGroupOnProperties(InvTransItemExtProp.FIdProperty, InvTransItemExtProp.ItemIdProperty, InvTransItemExtProp.DefinitionIdProperty);
            Meta.EnablePhantoms();
        }
    }
}