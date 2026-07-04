using SIE.Domain;
using SIE.Items;
using SIE.ManagedProperty;
using SIE.Wpf.Editors;
using System;
using System.Linq;

namespace SIE.Wpf.Items.Editors
{
    /// <summary>
    /// 产品等级快码编辑器
    /// </summary>
    public class CatalogProductGradeLookupEditor : PagingLookUpEditor
    {
        /// <summary>
        /// 编辑器名称
        /// </summary>
        public const string EditorName = "CatalogProductGradeLookupEditor";

        /// <summary>
        /// 重新数据源
        /// </summary>
        /// <param name="source">实体</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">搜索关键词</param>
        /// <param name="titleProperty">IManagedProperty</param>
        /// <returns>EntityList</returns>
        protected override EntityList GetDataSourceCore(Entity source, PagingInfo pagingInfo, string keyword, IManagedProperty titleProperty)
        {
            var result = RT.Service.Resolve<ItemController>().GetProductGrades("PRODUCTGRADE_TYPE");
            if (keyword.IsNotEmpty())
            {
                var curQrys = result.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword) || p.Describe.Contains(keyword));
                result.Clear();
                result.DeletedList.Clear();
                result.AddRange(curQrys);
            }

            return result;
        }
    }
}
