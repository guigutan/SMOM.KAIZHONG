using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.WIP.Products
{
    /// <summary>
    /// 关键件属性值
    /// </summary>
    [ChildEntity, Serializable]
    [Label("关键件属性值")]
    public class KeyItemPropertyValue : DataEntity
    {
        #region 属性值 Value
        /// <summary>
        /// 属性值
        /// </summary>
        [Label("属性值")]
        public static readonly Property<string> ValueProperty = P<KeyItemPropertyValue>.Register(e => e.Value);

        /// <summary>
        /// 属性值
        /// </summary>
        public string Value
        {
            get { return this.GetProperty(ValueProperty); }
            set { this.SetProperty(ValueProperty, value); }
        }
        #endregion

        #region 物料属性定义 Definition
        /// <summary>
        /// 物料属性定义Id
        /// </summary>
        public static readonly IRefIdProperty DefinitionIdProperty =
            P<KeyItemPropertyValue>.RegisterRefId(e => e.DefinitionId, ReferenceType.Normal);

        /// <summary>
        /// 物料属性定义Id
        /// </summary>
        public double DefinitionId
        {
            get { return (double)this.GetRefId(DefinitionIdProperty); }
            set { this.SetRefId(DefinitionIdProperty, value); }
        }

        /// <summary>
        /// 物料属性定义
        /// </summary>
        public static readonly RefEntityProperty<ItemPropertyDefinition> DefinitionProperty =
            P<KeyItemPropertyValue>.RegisterRef(e => e.Definition, DefinitionIdProperty);

        /// <summary>
        /// 物料属性定义
        /// </summary>
        public ItemPropertyDefinition Definition
        {
            get { return this.GetRefEntity(DefinitionProperty); }
            set { this.SetRefEntity(DefinitionProperty, value); }
        }
        #endregion

        #region 关键件 KeyItem
        /// <summary>
        /// 关键件Id
        /// </summary>
        public static readonly IRefIdProperty KeyItemIdProperty =
            P<KeyItemPropertyValue>.RegisterRefId(e => e.KeyItemId, ReferenceType.Parent);

        /// <summary>
        /// 关键件Id
        /// </summary>
        public double? KeyItemId
        {
            get { return (double?)this.GetRefNullableId(KeyItemIdProperty); }
            set { this.SetRefNullableId(KeyItemIdProperty, value); }
        }

        /// <summary>
        /// 关键件
        /// </summary>
        public static readonly RefEntityProperty<WipProductProcessKeyItem> KeyItemProperty =
            P<KeyItemPropertyValue>.RegisterRef(e => e.KeyItem, KeyItemIdProperty);

        /// <summary>
        /// 关键件
        /// </summary>
        public WipProductProcessKeyItem KeyItem
        {
            get { return this.GetRefEntity(KeyItemProperty); }
            set { this.SetRefEntity(KeyItemProperty, value); }
        }
        #endregion

        #region 视图属性 
        #region 名称 DefinitionName
        /// <summary>
        /// 名称
        /// </summary>
        [Label("名称")]
        public static readonly Property<string> DefinitionNameProperty = P<KeyItemPropertyValue>.RegisterView(e => e.DefinitionName, p => p.Definition.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string DefinitionName
        {
            get { return this.GetProperty(DefinitionNameProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 关键件属性值 实体配置
    /// </summary>
    internal class KeyItemPropertyValueConfig : EntityConfig<KeyItemPropertyValue>
    {
        /// <summary>
        /// 配置数据库映射
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_PROP_VAL").MapAllProperties();
            Meta.EnableDiscriminator("KEYITEM");
            Meta.DisablePhantoms();
        }
    }
}
