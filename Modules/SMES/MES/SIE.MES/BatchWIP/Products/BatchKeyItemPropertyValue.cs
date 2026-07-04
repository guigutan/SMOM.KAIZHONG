using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.BatchWIP.Products
{
    /// <summary>
    /// 关键件属性值
    /// </summary>
    [ChildEntity, Serializable]
    [Label("关键件属性值")]
    public partial class BatchKeyItemPropertyValue : DataEntity
    {
        #region 属性值 Value
        /// <summary>
        /// 属性值
        /// </summary>
        [Label("属性值")]
        public static readonly Property<string> ValueProperty = P<BatchKeyItemPropertyValue>.Register(e => e.Value);

        /// <summary>
        /// 属性值
        /// </summary>
        public string Value
        {
            get { return GetProperty(ValueProperty); }
            set { SetProperty(ValueProperty, value); }
        }
        #endregion

        #region 属性定义 Definition
        /// <summary>
        /// 属性定义Id
        /// </summary>
        public static readonly IRefIdProperty DefinitionIdProperty = P<BatchKeyItemPropertyValue>.RegisterRefId(e => e.DefinitionId, ReferenceType.Normal);

        /// <summary>
        /// 属性定义Id
        /// </summary>
        public double DefinitionId
        {
            get { return (double)GetRefId(DefinitionIdProperty); }
            set { SetRefId(DefinitionIdProperty, value); }
        }

        /// <summary>
        /// 属性定义
        /// </summary>
        public static readonly RefEntityProperty<ItemPropertyDefinition> DefinitionProperty = P<BatchKeyItemPropertyValue>.RegisterRef(e => e.Definition, DefinitionIdProperty);

        /// <summary>
        /// 属性定义
        /// </summary>
        public ItemPropertyDefinition Definition
        {
            get { return GetRefEntity(DefinitionProperty); }
            set { SetRefEntity(DefinitionProperty, value); }
        }
        #endregion

        #region 属性值 KeyItem
        /// <summary>
        /// 属性值Id
        /// </summary>
        public static readonly IRefIdProperty KeyItemIdProperty = P<BatchKeyItemPropertyValue>.RegisterRefId(e => e.KeyItemId, ReferenceType.Parent);

        /// <summary>
        /// 属性值Id
        /// </summary>
        public double KeyItemId
        {
            get { return (double)GetRefId(KeyItemIdProperty); }
            set { SetRefId(KeyItemIdProperty, value); }
        }

        /// <summary>
        /// 属性值
        /// </summary>
        public static readonly RefEntityProperty<BatchWipProductProcessKeyItem> KeyItemProperty = P<BatchKeyItemPropertyValue>.RegisterRef(e => e.KeyItem, KeyItemIdProperty);

        /// <summary>
        /// 属性值
        /// </summary>
        public BatchWipProductProcessKeyItem KeyItem
        {
            get { return GetRefEntity(KeyItemProperty); }
            set { SetRefEntity(KeyItemProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 属性名称 PropertyName
        /// <summary>
        /// 属性名称
        /// </summary>
        [Label("属性名称")]
        public static readonly Property<string> PropertyNameProperty = P<BatchKeyItemPropertyValue>.RegisterView(e => e.PropertyName, p => p.Definition.Name);

        /// <summary>
        /// 属性名称
        /// </summary>
        public string PropertyName
        {
            get { return this.GetProperty(PropertyNameProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 关键件属性值 实体配置
    /// </summary>
    internal class BatchKeyItemPropertyValueConfig : EntityConfig<BatchKeyItemPropertyValue>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("BWIP_PROP_VAL").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}