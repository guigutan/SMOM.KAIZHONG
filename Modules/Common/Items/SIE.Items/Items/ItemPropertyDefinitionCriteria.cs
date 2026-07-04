using SIE.Common.Catalogs;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Items
{
    /// <summary>
    /// 物料属性定义查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("物料属性定义查询实体")]
    public class ItemPropertyDefinitionCriteria : Criteria
    {
        #region 名称 Name
        /// <summary>
        /// 名称
        /// </summary>
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<ItemPropertyDefinitionCriteria>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 快码组 CatalogType
        /// <summary>
        /// 快码组Id
        /// </summary>
        [Label("快码组")]
        public static readonly IRefIdProperty CatalogTypeIdProperty = P<ItemPropertyDefinitionCriteria>.RegisterRefId(e => e.CatalogTypeId, ReferenceType.Normal);

        /// <summary>
        /// 快码组Id
        /// </summary>
        public double? CatalogTypeId
        {
            get { return (double?)GetRefNullableId(CatalogTypeIdProperty); }
            set { SetRefNullableId(CatalogTypeIdProperty, value); }
        }

        /// <summary>
        /// 快码组
        /// </summary>
        public static readonly RefEntityProperty<CatalogType> CatalogTypeProperty = P<ItemPropertyDefinitionCriteria>.RegisterRef(e => e.CatalogType, CatalogTypeIdProperty);

        /// <summary>
        /// 快码组
        /// </summary>
        [Label("快码组")]
        public CatalogType CatalogType
        {
            get { return GetRefEntity(CatalogTypeProperty); }
            set { SetRefEntity(CatalogTypeProperty, value); }
        }
        #endregion

        #region 类型 PropertyType
        /// <summary>
        /// 类型
        /// </summary>
        [Required]
        [Label("类型")]
        public static readonly Property<ItemPropertyType?> PropertyTypeProperty = P<ItemPropertyDefinitionCriteria>.Register(e => e.PropertyType);

        /// <summary>
        /// 类型
        /// </summary>
        public ItemPropertyType? PropertyType
        {
            get { return GetProperty(PropertyTypeProperty); }
            set { SetProperty(PropertyTypeProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询实体 获取查询结果
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<ItemController>().GetItemPropertyDefinitions(this);
        }
    }
}