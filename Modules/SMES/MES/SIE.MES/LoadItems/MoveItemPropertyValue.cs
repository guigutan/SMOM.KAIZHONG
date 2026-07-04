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
    public class MoveItemPropertyValue : DataEntity
    {
        #region 属性值 Value
        /// <summary>
        /// 属性值
        /// </summary>
        [Label("属性值")]
        public static readonly Property<string> ValueProperty = P<MoveItemPropertyValue>.Register(e => e.Value);

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
            P<MoveItemPropertyValue>.RegisterRefId(e => e.DefinitionId, ReferenceType.Normal);

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
            P<MoveItemPropertyValue>.RegisterRef(e => e.Definition, DefinitionIdProperty);

        /// <summary>
        /// 物料属性定义
        /// </summary>
        public ItemPropertyDefinition Definition
        {
            get { return this.GetRefEntity(DefinitionProperty); }
            set { this.SetRefEntity(DefinitionProperty, value); }
        }
        #endregion

        #region 挪料 MoveItem
        /// <summary>
        /// 挪料Id
        /// </summary>
        [Label("挪料Id")]
        public static readonly IRefIdProperty MoveItemIdProperty =
            P<MoveItemPropertyValue>.RegisterRefId(e => e.MoveItemId, ReferenceType.Parent);

        /// <summary>
        /// 挪料Id
        /// </summary>
        public double? MoveItemId
        {
            get { return (double?)this.GetRefNullableId(MoveItemIdProperty); }
            set { this.SetRefNullableId(MoveItemIdProperty, value); }
        }

        /// <summary>
        /// 挪料
        /// </summary>
        [Label("挪料")]
        public static readonly RefEntityProperty<MoveItem> MoveItemProperty =
            P<MoveItemPropertyValue>.RegisterRef(e => e.MoveItem, MoveItemIdProperty);

        /// <summary>
        /// 挪料
        /// </summary>
        public MoveItem MoveItem
        {
            get { return this.GetRefEntity(MoveItemProperty); }
            set { this.SetRefEntity(MoveItemProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 属性名称 PropertyName
        /// <summary>
        /// 属性名称
        /// </summary>
        [Label("属性名称")]
        public static readonly Property<string> PropertyNameProperty = P<MoveItemPropertyValue>.RegisterView(e => e.PropertyName, p => p.Definition.Name);

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
    internal class MoveItemPropertyValueConfig : EntityConfig<MoveItemPropertyValue>
    {
        /// <summary>
        /// 数据库表映射
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_PROP_VAL").MapAllProperties();
            Meta.EnableDiscriminator("MOVEITEM");
            Meta.DisablePhantoms();
        }
    }
}