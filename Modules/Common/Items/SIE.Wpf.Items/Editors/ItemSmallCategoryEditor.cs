using SIE.Domain;
using SIE.Items;
using SIE.ManagedProperty;
using SIE.Wpf.Editors;

namespace SIE.Wpf.Items.Editors
{
    /// <summary>
    /// 物料分类小类编辑器
    /// </summary>
    public class ItemSmallCategoryEditor : PagingLookUpEditor
    {
        /// <summary>
        /// 编辑器名称
        /// </summary>
        public const string EditorName = "ItemSmallCategoryEditor";

        /// <summary>
        /// 获取物料分类小类
        /// </summary>
        /// <param name="source">上下文</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">关键字</param>
        /// <param name="titleProperty">显示属性</param>
        /// <returns>物料分类小类列表</returns>
        protected override EntityList GetDataSourceCore(Entity source, PagingInfo pagingInfo, string keyword, IManagedProperty titleProperty)
        {
            return RT.Service.Resolve<ItemController>().GetItemSmallCategories(pagingInfo, keyword);
        }
    }
}
