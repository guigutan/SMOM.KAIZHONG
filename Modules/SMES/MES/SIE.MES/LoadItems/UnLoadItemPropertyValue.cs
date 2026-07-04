using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.LoadItems
{
    /// <summary>
    /// 下料物料属性值
    /// </summary>
    [ChildEntity, Serializable]
    [Label("下料物料属性值")]
    public partial class UnLoadItemPropertyValue : DataEntity
    {
        #region 属性值 Value
        /// <summary>
        /// 属性值
        /// </summary>
        [Label("属性值")]
        public static readonly Property<string> ValueProperty = P<UnLoadItemPropertyValue>.Register(e => e.Value);

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
        [Label("物料属性定义Id")]
        public static readonly IRefIdProperty DefinitionIdProperty =
            P<UnLoadItemPropertyValue>.RegisterRefId(e => e.DefinitionId, ReferenceType.Normal);

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
        [Label("物料属性定义")]
        public static readonly RefEntityProperty<ItemPropertyDefinition> DefinitionProperty =
            P<UnLoadItemPropertyValue>.RegisterRef(e => e.Definition, DefinitionIdProperty);

        /// <summary>
        /// 物料属性定义
        /// </summary>
        public ItemPropertyDefinition Definition
        {
            get { return this.GetRefEntity(DefinitionProperty); }
            set { this.SetRefEntity(DefinitionProperty, value); }
        }
        #endregion

        #region 下料 UnLoadItem
        /// <summary>
        /// 下料Id
        /// </summary>
        [Label("下料Id")]
        public static readonly IRefIdProperty UnLoadItemIdProperty =
            P<UnLoadItemPropertyValue>.RegisterRefId(e => e.UnLoadItemId, ReferenceType.Parent);

        /// <summary>
        /// 下料Id
        /// </summary>
        public double? UnLoadItemId
        {
            get { return (double?)this.GetRefNullableId(UnLoadItemIdProperty); }
            set { this.SetRefNullableId(UnLoadItemIdProperty, value); }
        }

        /// <summary>
        /// 下料
        /// </summary>
        [Label("下料")]
        public static readonly RefEntityProperty<UnloadItem> UnLoadItemProperty =
            P<UnLoadItemPropertyValue>.RegisterRef(e => e.UnLoadItem, UnLoadItemIdProperty);

        /// <summary>
        /// 下料
        /// </summary>
        public UnloadItem UnLoadItem
        {
            get { return this.GetRefEntity(UnLoadItemProperty); }
            set { this.SetRefEntity(UnLoadItemProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 属性名称 PropertyName
        /// <summary>
        /// 属性名称
        /// </summary>
        [Label("属性名称")]
        public static readonly Property<string> PropertyNameProperty = P<UnLoadItemPropertyValue>.RegisterView(e => e.PropertyName, p => p.Definition.Name);

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
    /// 上料物料属性值 实体配置
    /// </summary>
    internal class UnLoadItemPropertyValueConfig : EntityConfig<UnLoadItemPropertyValue>
    {
        /// <summary>
        /// 实体数据库表映射
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_PROP_VAL").MapAllProperties();
            Meta.EnableDiscriminator("UNLOADITEM");
            Meta.DisablePhantoms();
        }
    }
}