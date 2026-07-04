using SIE.Domain;
using SIE.Items;
using SIE.ManagedProperty;
using SIE.Wpf.Editors;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Wpf.Items.Editors
{
    /// <summary>
    /// 物料质量分类小类编辑器
    /// </summary>
    public class ItemQualitySmallCategoryEditor : PagingLookUpEditor
    {
        /// <summary>
        /// 编辑器名称
        /// </summary>
        public const string EditorName = "ItemQualitySmallCategoryEditor";

        /// <summary>
        /// 物料类型列表参数名
        /// </summary>
        public const string TypeList = "TypeList";

        /// <summary>
        /// 获取质量分类小类
        /// </summary>
        /// <param name="source">上下文</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">关键字</param>
        /// <param name="titleProperty">显示属性</param>
        /// <returns>质量分类小类列表</returns>
        protected override EntityList GetDataSourceCore(Entity source, PagingInfo pagingInfo, string keyword, IManagedProperty titleProperty)
        {
            List<ItemType> itemTypeList = base.Config.GetPropertyOrDefault<List<ItemType>>(TypeList, new List<ItemType>());
            List<int> itemTypeValueList = itemTypeList.Select(p => (int)p).ToList();
            return RT.Service.Resolve<ItemController>().GetQualitySmallCategories(itemTypeValueList, pagingInfo, keyword);
        }
    }
}