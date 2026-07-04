using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.LoadItems
{
    /// <summary>
    /// 上料物料属性值
    /// </summary>
    [ChildEntity, Serializable]
    [Label("上料物料属性值")]
    public partial class LoadItemPropertyValue : DataEntity
    {
        #region 属性值 Value
        /// <summary>
        /// 属性值
        /// </summary>
        [Label("属性值")]
        public static readonly Property<string> ValueProperty = P<LoadItemPropertyValue>.Register(e => e.Value);

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
            P<LoadItemPropertyValue>.RegisterRefId(e => e.DefinitionId, ReferenceType.Normal);

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
            P<LoadItemPropertyValue>.RegisterRef(e => e.Definition, DefinitionIdProperty);

        /// <summary>
        /// 物料属性定义
        /// </summary>
        public ItemPropertyDefinition Definition
        {
            get { return this.GetRefEntity(DefinitionProperty); }
            set { this.SetRefEntity(DefinitionProperty, value); }
        }
        #endregion

        #region 上料 LoadItem
        /// <summary>
        /// 上料Id
        /// </summary>
        [Label("上料Id")]
        public static readonly IRefIdProperty LoadItemIdProperty =
            P<LoadItemPropertyValue>.RegisterRefId(e => e.LoadItemId, ReferenceType.Parent);

        /// <summary>
        /// 上料Id
        /// </summary>
        public double? LoadItemId
        {
            get { return (double?)this.GetRefNullableId(LoadItemIdProperty); }
            set { this.SetRefNullableId(LoadItemIdProperty, value); }
        }

        /// <summary>
        /// 上料
        /// </summary>
        [Label("上料")]
        public static readonly RefEntityProperty<LoadItem> LoadItemProperty =
            P<LoadItemPropertyValue>.RegisterRef(e => e.LoadItem, LoadItemIdProperty);

        /// <summary>
        /// 上料
        /// </summary>
        public LoadItem LoadItem
        {
            get { return this.GetRefEntity(LoadItemProperty); }
            set { this.SetRefEntity(LoadItemProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 属性名称 PropertyName
        /// <summary>
        /// 属性名称
        /// </summary>
        [Label("属性名称")]
        public static readonly Property<string> PropertyNameProperty = P<LoadItemPropertyValue>.RegisterView(e => e.PropertyName, p => p.Definition.Name);

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
    internal class LoadItemPropertyValueConfig : EntityConfig<LoadItemPropertyValue>
    {
        /// <summary>
        /// 配置数据库映射
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_PROP_VAL").MapAllProperties();
            Meta.EnableDiscriminator("LOADITEM");
            //Meta.IndexGroupOnProperties(LoadItemPropertyValue.LoadItemIdProperty, LoadItemPropertyValue.);
            Meta.Property(LoadItemPropertyValue.LoadItemIdProperty).ColumnMeta.HasIndex();
            Meta.DisablePhantoms();
        }
    }
}