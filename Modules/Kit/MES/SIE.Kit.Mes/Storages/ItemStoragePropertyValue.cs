using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Kit.MES.Storages
{
    /// <summary>
    /// 产线物料货位属性值
    /// </summary>
    [ChildEntity, Serializable]
    [Label("产线物料货位属性值")]
    [DisplayMember(nameof(Value))]
    public partial class ItemStoragePropertyValue : DataEntity
    {
        #region 属性值 Value
        /// <summary>
        /// 属性值
        /// </summary>
        [Required]
        [MaxLength(40)]
        [Label("属性值")]
        public static readonly Property<string> ValueProperty = P<ItemStoragePropertyValue>.Register(e => e.Value);

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
        public static readonly IRefIdProperty DefinitionIdProperty = P<ItemStoragePropertyValue>.RegisterRefId(e => e.DefinitionId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<ItemPropertyDefinition> DefinitionProperty = P<ItemStoragePropertyValue>.RegisterRef(e => e.Definition, DefinitionIdProperty);

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
        public static readonly Property<string> DefinitionNameProperty = P<ItemStoragePropertyValue>.RegisterView(e => e.DefinitionName, p => p.Definition.Name);

        /// <summary>
        /// 属性值
        /// </summary>
        public string DefinitionName
        {
            get { return this.GetProperty(DefinitionNameProperty); }
        }
        #endregion

        #region 产线物料货位 ItemStorage
        /// <summary>
        /// 产线物料货位Id
        /// </summary>
        [Label("产线物料货位")]
        public static readonly IRefIdProperty ItemStorageIdProperty =
            P<ItemStoragePropertyValue>.RegisterRefId(e => e.ItemStorageId, ReferenceType.Parent);

        /// <summary>
        /// 产线物料货位Id
        /// </summary>
        public double ItemStorageId
        {
            get { return (double)this.GetRefId(ItemStorageIdProperty); }
            set { this.SetRefId(ItemStorageIdProperty, value); }
        }

        /// <summary>
        /// 产线物料货位
        /// </summary>
        public static readonly RefEntityProperty<ItemStorage> ItemStorageProperty =
            P<ItemStoragePropertyValue>.RegisterRef(e => e.ItemStorage, ItemStorageIdProperty);

        /// <summary>
        /// 产线物料货位
        /// </summary>
        public ItemStorage ItemStorage
        {
            get { return this.GetRefEntity(ItemStorageProperty); }
            set { this.SetRefEntity(ItemStorageProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 产线物料货位属性值 实体配置
    /// </summary>
    internal class ItemStoragePropertyValueConfig : EntityConfig<ItemStoragePropertyValue>
    {
        /// <summary>
        /// 对Meta属性的配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("ITEM_STO_PRO_VAL").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
