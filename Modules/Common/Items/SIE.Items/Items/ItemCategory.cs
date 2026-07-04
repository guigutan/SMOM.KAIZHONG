using SIE.Domain;
using SIE.Items.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Items
{
    /// <summary>
    /// 分类
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("分类")]
    [DisplayMember(nameof(ItemCategory.Code))]
    public partial class ItemCategory : DataEntity
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Required]
        [NotDuplicate]
        [MaxLength(40)]
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<ItemCategory>.Register(e => e.Code);

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
        [Required]
        [MaxLength(40)]
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<ItemCategory>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        [Label("质量分类名称")]
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
        public static readonly IRefIdProperty LevelIdProperty = P<ItemCategory>.RegisterRefId(e => e.LevelId, ReferenceType.Normal);

        /// <summary>
        /// 物料分类层级Id
        /// </summary>
        public double LevelId
        {
            get { return (double)GetRefId(LevelIdProperty); }
            set { SetRefId(LevelIdProperty, value); }
        }

        /// <summary>
        /// 物料分类层级
        /// </summary> 
        public static readonly RefEntityProperty<ItemCategoryLevel> LevelProperty = P<ItemCategory>.RegisterRef(e => e.Level, LevelIdProperty);

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
        public static readonly Property<CategoryType> TypeProperty = P<ItemCategory>.Register(e => e.Type);

        /// <summary>
        /// 分类类型
        /// </summary>
        public CategoryType Type
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
        public static readonly Property<ItemType?> ItemTypeProperty = P<ItemCategory>.Register(e => e.ItemType);

        /// <summary>
        /// 物料类型
        /// </summary>
        public ItemType? ItemType
        {
            get { return GetProperty(ItemTypeProperty); }
            set { SetProperty(ItemTypeProperty, value); }
        }
        #endregion

        #region 来源主键 SourceKey
        /// <summary>
        /// 来源主键, 一般用于接口平台事务上传
        /// </summary>
        [Label("来源主键")]
        public static readonly Property<string> SourceKeyProperty = P<ItemCategory>.Register(e => e.SourceKey);

        /// <summary>
        /// 来源主键, 一般用于接口平台事务上传
        /// </summary>
        public string SourceKey
        {
            get { return this.GetProperty(SourceKeyProperty); }
            set { this.SetProperty(SourceKeyProperty, value); }
        }
        #endregion

        #region 分类层级名称 LevelName
        /// <summary>
        /// 分类层级名称
        /// </summary>  
        [Label("分类层级名称")]
        public static readonly Property<string> LevelNameProperty = P<ItemCategory>.RegisterView(e => e.LevelName, p => p.Level.Name);

        /// <summary>
        /// 分类层级名称
        /// </summary>
        public string LevelName
        {
            get { return this.GetProperty(LevelNameProperty); }
        }
        #endregion

        #region ERP物料分类 Id
        /// <summary>
        /// ERP物料分类Id
        /// </summary>
        [Label("ERP物料分类Id")]
        public static readonly Property<double?> ErpCategoryIdProperty = P<ItemCategory>.Register(e => e.ErpCategoryId);

        /// <summary>
        /// ERP物料分类Id
        /// </summary>
        public double? ErpCategoryId
        {
            get { return GetProperty(ErpCategoryIdProperty); }
            set { SetProperty(ErpCategoryIdProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 分类 实体配置
    /// </summary>
    internal class CategoryConfig : EntityConfig<ItemCategory>
    {
        /// <summary>
        /// 元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.SupportTree();
            Meta.MapTable("ITEM_CATE").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}