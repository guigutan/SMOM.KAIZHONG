using SIE.Domain;
using SIE.ManagedProperty;
using SIE.Resources.WipResources;
using SIE.Wpf.Editors;
using System.Collections.Generic;

namespace SIE.Wpf.MES.Editors
{
    /// <summary>
    /// 生产资源：企业模型、设备台账
    /// 排除自定义类型的生产资源
    /// </summary>
    public class EquipEnterpriseResourceEditor : PagingLookUpEditor
    {
        /// <summary>
        /// 编辑器名称
        /// </summary>
        public const string EditorName = "EnterpriseEquipmentResourceEditor";

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
            var typeList = new List<SyncSourceType>() { SyncSourceType.Enterprise, SyncSourceType.Equipment };
            return RT.Service.Resolve<WipResourceController>().GetWipResources(stateList, typeList, pagingInfo, keyword);
        }
    }
}
