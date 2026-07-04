using SIE.Domain;
using SIE.Items.Items;
using SIE.ObjectModel;
using System;

namespace SIE.Items
{
    /// <summary>
    /// 物料小类查询实体
    /// </summary>
    [QueryEntity, Serializable]
    public partial class ItemSmallCategoryCriteria : Criteria
    {
        #region 编码 Code  
        /// <summary>
        /// 编码
        /// </summary>
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<ItemSmallCategoryCriteria>.Register(e => e.Code);

        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 名称 Name  
        /// <summary>
        /// 名称
        /// </summary>
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<ItemSmallCategoryCriteria>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion 

        #region 物料分类层级 Level
        /// <summary>
        /// 物料分类层级Id
        /// </summary>
        [Label("分类层级")]
        public static readonly IRefIdProperty LevelIdProperty = P<ItemSmallCategoryCriteria>.RegisterRefId(e => e.LevelId, ReferenceType.Normal);

        /// <summary>
        /// 物料分类层级Id
        /// </summary>
        public double? LevelId
        {
            get { return (double?)GetRefNullableId(LevelIdProperty); }
            set { SetRefNullableId(LevelIdProperty, value); }
        }

        /// <summary>
        /// 物料分类层级
        /// </summary> 
        public static readonly RefEntityProperty<ItemCategoryLevel> LevelProperty = P<ItemSmallCategoryCriteria>.RegisterRef(e => e.Level, LevelIdProperty);

        /// <summary>
        /// 物料分类层级
        /// </summary>
        public ItemCategoryLevel Level
        {
            get { return GetRefEntity(LevelProperty); }
            set { SetRefEntity(LevelProperty, value); }
        }
        #endregion

        #region 分类类型 Type
        /// <summary>
        /// 分类类型
        /// </summary>
        [Label("分类类型")]
        public static readonly Property<CategoryType?> TypeProperty = P<ItemSmallCategoryCriteria>.Register(e => e.Type);

        /// <summary>
        /// 分类类型
        /// </summary>
        public CategoryType? Type
        {
            get { return this.GetProperty(TypeProperty); }
            set { this.SetProperty(TypeProperty, value); }
        }
        #endregion

        #region 物料类型 ItemType
        /// <summary>
        /// 物料类型
        /// </summary>
        [Label("物料类型")]
        public static readonly Property<ItemType?> ItemTypeProperty = P<ItemSmallCategoryCriteria>.Register(e => e.ItemType);

        /// <summary>
        /// 物料类型
        /// </summary>
        public ItemType? ItemType
        {
            get { return GetProperty(ItemTypeProperty); }
            set { SetProperty(ItemTypeProperty, value); }
        }
        #endregion

        /// <summary>
        /// 获取物料分类最底层级
        /// </summary>
        /// <returns>物料分类集合</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<ItemController>().GetItemSmallCategorys(this);
        }
    }
}