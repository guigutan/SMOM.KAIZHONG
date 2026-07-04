using SIE.Common.Catalogs;
using SIE.Domain;
using SIE.Items.ViewModels;
using SIE.ManagedProperty;
using SIE.Wpf.Editors;
using System;
using System.Linq;

namespace SIE.Wpf.Items.Editors
{
    /// <summary>
    /// 快码下拉编辑器
    /// </summary>
    public class CatalogLookUpEditor : PagingLookUpEditor
    {
        /// <summary>
        /// 编辑器名称
        /// </summary>
        public const string EditorName = "CatalogLookUpEditor";

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
            var vm = source as ItemPropertyViewModel;
            var cataLogList = new EntityList<Catalog>();
            if (vm.Definition != null || vm.Definition.CatalogTypeId > 0)
            {
                cataLogList = RT.Service.Resolve<CatalogController>().GetCatalogList(vm.Definition.CatalogTypeId.Value);
                if (!keyword.IsNullOrEmpty())
                {
                    var filterCatalogList = cataLogList.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword) || p.CatalogType.Name.Contains(keyword));
                    cataLogList.Clear();
                    cataLogList.DeletedList.Clear();
                    cataLogList.AddRange(filterCatalogList);
                }
            }

            return cataLogList;
        }
    }
}
