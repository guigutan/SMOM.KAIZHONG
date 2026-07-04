using SIE.Domain;
using SIE.ManagedProperty;
using SIE.Resources.WipResources;
using SIE.Wpf.Editors;
using System.Collections.Generic;

namespace SIE.Wpf.Resources.Editors
{
    /// <summary>
    /// 客制化资源编辑器
    /// </summary>
    public class CustomResourceEditor : PagingLookUpEditor
    {
        /// <summary>
        /// 编辑器名称
        /// </summary>
        public const string EditorName = "CustomResourceEditor";

        /// <summary>
        /// 重新数据源
        /// </summary>
        /// <param name="source">实体</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">搜索关键词</param>
        /// <param name="titleProperty">IManagedProperty</param>
        /// <returns>资源列表</returns>
        protected override EntityList GetDataSourceCore(Entity source, PagingInfo pagingInfo, string keyword, IManagedProperty titleProperty)
        {
            var stateList = new List<ResourceState>() { ResourceState.Actived, ResourceState.Stop, ResourceState.Unused };
            return RT.Service.Resolve<WipResourceController>().GetWipResources(stateList, new List<SyncSourceType>() { SyncSourceType.Enterprise, SyncSourceType.Equipment }, pagingInfo, keyword);
        }
    }
}