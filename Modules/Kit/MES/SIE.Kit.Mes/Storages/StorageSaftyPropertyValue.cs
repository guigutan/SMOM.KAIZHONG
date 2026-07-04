using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Kit.MES.Storages
{
    /// <summary>
    /// 产线库存属性值
    /// </summary>
    [ChildEntity, Serializable]
    [Label("产线库存属性值")]
    [DisplayMember(nameof(Value))]
    public partial class StorageSaftyPropertyValue : DataEntity
    {
        #region 属性值 Value
        /// <summary>
        /// 属性值
        /// </summary>
        [Required]
        [MaxLength(40)]
        [Label("属性值")]
        public static readonly Property<string> ValueProperty = P<StorageSaftyPropertyValue>.Register(e => e.Value);

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
        public static readonly IRefIdProperty DefinitionIdProperty = P<StorageSaftyPropertyValue>.RegisterRefId(e => e.DefinitionId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<ItemPropertyDefinition> DefinitionProperty = P<StorageSaftyPropertyValue>.RegisterRef(e => e.Definition, DefinitionIdProperty);

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
        public static readonly Property<string> DefinitionNameProperty = P<StorageSaftyPropertyValue>.RegisterView(e => e.DefinitionName, p => p.Definition.Name);

        /// <summary>
        /// 属性值
        /// </summary>
        public string DefinitionName
        {
            get { return this.GetProperty(DefinitionNameProperty); }
        }
        #endregion

        #region 物料库存 StorageSafty
        /// <summary>
        /// 物料库存Id
        /// </summary>
        [Label("物料库存")]
        public static readonly IRefIdProperty StorageSaftyIdProperty =
            P<StorageSaftyPropertyValue>.RegisterRefId(e => e.StorageSaftyId, ReferenceType.Parent);

        /// <summary>
        /// 物料库存Id
        /// </summary>
        public double StorageSaftyId
        {
            get { return (double)this.GetRefId(StorageSaftyIdProperty); }
            set { this.SetRefId(StorageSaftyIdProperty, value); }
        }

        /// <summary>
        /// 物料库存
        /// </summary>
        public static readonly RefEntityProperty<StorageSafty> StorageSaftyProperty =
            P<StorageSaftyPropertyValue>.RegisterRef(e => e.StorageSafty, StorageSaftyIdProperty);

        /// <summary>
        /// 物料库存
        /// </summary>
        public StorageSafty StorageSafty
        {
            get { return this.GetRefEntity(StorageSaftyProperty); }
            set { this.SetRefEntity(StorageSaftyProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 产线库存属性值 实体配置
    /// </summary>
    internal class StorageSaftyPropertyValueConfig : EntityConfig<StorageSaftyPropertyValue>
    {
        /// <summary>
        /// 对Meta属性的配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("STO_SAF_PRO_VAL").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
