using SIE.Domain;
using SIE.ManagedProperty;
using SIE.Resources.Employees;

namespace SIE.Wpf.MES.Editors
{
    /// <summary>
    /// 人员编辑器，可以锁定信息
    /// </summary>
    public class EmployeeLookUpEditor : LockableLookUpEditor
    {
        /// <summary>
        /// 人员编辑器名称
        /// </summary>
        public new const string EditorName = "EmployeeLookUpEditor";

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
            return RT.Service.Resolve<EmployeeController>().GetLinkedEmployees(keyword, keyword, pagingInfo);
        }
    }
}
