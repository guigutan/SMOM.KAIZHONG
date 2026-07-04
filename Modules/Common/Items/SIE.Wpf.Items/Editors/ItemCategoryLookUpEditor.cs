using SIE.Domain;
using SIE.Items;
using SIE.ManagedProperty;
using SIE.Wpf.Editors;

namespace SIE.Wpf.Items.Editors
{
    /// <summary>
    /// 分类编辑器
    /// </summary>
    public class ItemCategoryLookUpEditor : PagingLookUpEditor
    {
        /// <summary>
        /// 编辑器名称
        /// </summary>
        public const string EditorName = "ItemCategoryLookUpEditor";

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
            var itemCR = source as ItemCategoryRelation;
            if (itemCR == null) return new EntityList<ItemCategory>();
            return RT.Service.Resolve<ItemController>().GetItemSmallCategory(itemCR.Type, itemCR.Item.Type, keyword, pagingInfo);
        }
    }
}