using SIE.Domain;
using SIE.Items;
using SIE.Items.Items;
using SIE.ManagedProperty;
using SIE.Wpf.Editors;

namespace SIE.Wpf.Items.Editors
{
    /// <summary>
    /// 物料分类层级
    /// </summary>
    public class ItemCateLevelEditor : PagingLookUpEditor
    {
        /// <summary>
        /// 编辑器名称
        /// </summary>
        public const string EditorName = "ItemCateLevelEditor";

        /// <summary>
        /// 设置数据源
        /// </summary>
        /// <param name="source">实体</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">关键词</param>
        /// <param name="titleProperty">托管属性</param>
        /// <returns>EntityList</returns>
        protected override EntityList GetDataSourceCore(Entity source, PagingInfo pagingInfo, string keyword, IManagedProperty titleProperty)
        {
            CategoryType categoryType = CategoryType.Item;
            var config = base.Config as ItemCateLevelEditorConfig;
            if (config != null)
            {
                categoryType = config.CategoryType;
            }

            return RT.Service.Resolve<ItemController>().GetItemCategoryLevel(categoryType, keyword, pagingInfo);
        }
    }

    /// <summary>
    /// 客户编辑器配置类
    /// </summary>
    public class ItemCateLevelEditorConfig : PagingLookUpEditorConfig
    {
        /// <summary>
        /// 客户类型
        /// </summary>
        public CategoryType CategoryType
        {
            get { return GetProperty<CategoryType>(); }
            set { SetProperty(value); }
        }
    }
}
