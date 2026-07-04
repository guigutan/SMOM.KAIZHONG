using SIE.Domain;
using SIE.ManagedProperty;
using SIE.Tech.Processs;
using SIE.Wpf.MES.Editors;
using SIE.Wpf.MES.WIP;
using System.Collections.Generic;

namespace SIE.Wpf.MES.Workbench.Editors
{
    public class InspProcessEditor : LockableLookUpEditor
    {
        /// <summary>
        /// 资源编辑器名称
        /// </summary>
        public new const string EditorName = "InspProcessEditor";

        /// <summary>
        /// 重新加载数据源
        /// </summary>
        /// <param name="source">实体</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">搜索关键字</param>
        /// <param name="titleProperty">托管属性标记</param>
        /// <returns>资源列表</returns>
        protected override EntityList GetDataSourceCore(Entity source, PagingInfo pagingInfo, string keyword, IManagedProperty titleProperty)
        {
            Workstation workstation = source as Workstation;
            if (!workstation.EmployeeId.HasValue)
                return new EntityList<Process>();
            return RT.Service.Resolve<ProcessController>().GetProcesssByUserId(workstation.EmployeeId.Value, keyword, new List<ProcessType>() { ProcessType.Pqc });
        }
    }
}