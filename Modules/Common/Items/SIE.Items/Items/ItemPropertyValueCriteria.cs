using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Items.Items
{
    /// <summary>
    /// 物料属性值查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("物料属性查询")]
    public class ItemPropertyValueCriteria : Criteria
    {
        #region 属性定义 Definition
        /// <summary>
        /// 属性定义Id
        /// </summary>
        [Label("属性定义")]
        public static readonly IRefIdProperty DefinitionIdProperty =
            P<ItemPropertyValueCriteria>.RegisterRefId(e => e.DefinitionId, ReferenceType.Normal);

        /// <summary>
        /// 属性定义Id
        /// </summary>
        public double? DefinitionId
        {
            get { return (double?)this.GetRefNullableId(DefinitionIdProperty); }
            set { this.SetRefNullableId(DefinitionIdProperty, value); }
        }

        /// <summary>
        /// 属性定义
        /// </summary>
        public static readonly RefEntityProperty<ItemPropertyDefinition> DefinitionProperty =
            P<ItemPropertyValueCriteria>.RegisterRef(e => e.Definition, DefinitionIdProperty);

        /// <summary>
        /// 属性定义
        /// </summary>
        public ItemPropertyDefinition Definition
        {
            get { return this.GetRefEntity(DefinitionProperty); }
            set { this.SetRefEntity(DefinitionProperty, value); }
        }
        #endregion

        #region 值 Value
        /// <summary>
        /// 值
        /// </summary>
        [Label("值")]
        public static readonly Property<string> ValueProperty = P<ItemPropertyValueCriteria>.Register(e => e.Value);

        /// <summary>
        /// 值
        /// </summary>
        public string Value
        {
            get { return this.GetProperty(ValueProperty); }
            set { this.SetProperty(ValueProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty =
            P<ItemPropertyValueCriteria>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料Id
        /// </summary>
        public double? ItemId
        {
            get { return (double?)this.GetRefNullableId(ItemIdProperty); }
            set { this.SetRefNullableId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty =
            P<ItemPropertyValueCriteria>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return this.GetRefEntity(ItemProperty); }
            set { this.SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 过滤属性值ID FilterId
        /// <summary>
        /// 过滤属性值ID
        /// </summary>
        [Label("过滤属性值")]
        public static readonly Property<double[]> FilterIdProperty = P<ItemPropertyValueCriteria>.Register(e => e.FilterId);

        /// <summary>
        /// 过滤属性值ID
        /// </summary>
        public double[] FilterId
        {
            get { return this.GetProperty(FilterIdProperty); }
            set { this.SetProperty(FilterIdProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询实体默认查询方法
        /// </summary>
        /// <returns>EntityList</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<ItemController>().GetItemPropertys(this, FilterId);
        }
    }
}
