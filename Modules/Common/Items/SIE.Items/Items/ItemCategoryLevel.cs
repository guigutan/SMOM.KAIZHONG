using SIE.Domain;
using SIE.Items.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Items
{
    /// <summary>
    /// 分类层级
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("分类层级")]
    [DisplayMember(nameof(Name))]
    public partial class ItemCategoryLevel : DataEntity
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Required]
        [NotDuplicate]
        [MaxLength(40)]
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<ItemCategoryLevel>.Register(e => e.Code);

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
        [NotDuplicate]
        [MaxLength(40)]
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<ItemCategoryLevel>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 分类类型 Type
        /// <summary>
        /// 分类类型
        /// </summary>
        [Label("类型")]
        [Required]
        public static readonly Property<CategoryType> TypeProperty = P<ItemCategoryLevel>.Register(e => e.Type);

        /// <summary>
        /// 分类类型
        /// </summary>
        public CategoryType Type
        {
            get { return this.GetProperty(TypeProperty); }
            set { this.SetProperty(TypeProperty, value); }
        }
        #endregion

        #region 注册视图属性(关联实体属性平铺显示，一般用于Web)
        #region 层级数 LevelNumber
        /// <summary>
        /// 层级数
        /// </summary>
        [Label("层级数")]
        public static readonly Property<int> LevelNumberProperty = P<ItemCategoryLevel>.RegisterReadOnly(e => e.LevelNumber, p => p.GetLevelNumber(), ItemCategoryLevel.TreePIdProperty);

        /// <summary>
        /// 层级数
        /// </summary>
        public int LevelNumber
        {
            get { return this.GetProperty(LevelNumberProperty); }
        }

        /// <summary>
        /// 计算层级数
        /// </summary>
        /// <returns>string</returns>
        private int GetLevelNumber()
        {
            int level = 1;//最上级用1表示
            const int maxLevel = 10;//最大层级
            var entity = this;
            for (int i = 0; i < maxLevel; i++)
            {
                if (entity == null || !entity.TreePId.HasValue)
                    break;
                entity = RF.GetById<ItemCategoryLevel>(entity.TreePId);
                level++;
            }
            return level;
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 分类层级 实体配置
    /// </summary>
    internal class CategoryLevelConfig : EntityConfig<ItemCategoryLevel>
    {
        /// <summary>
        ///  元数据配置
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("ITEM_CATE_LEVEL").MapAllProperties();
            Meta.EnablePhantoms();
            Meta.SupportTree();
        }
    }
}