using SIE.Domain;
using SIE.Items;
using SIE.ManagedProperty;
using SIE.Resources.Employees;
using SIE.Wpf.Editors;

namespace SIE.Wpf.Items.Editors
{
    /// <summary>
    /// 采购员编辑器
    /// </summary>
    public class WorkGroupEmployeeLookUpEditor : PagingLookUpEditor
    {
        /// <summary>
        /// 编辑器名称
        /// </summary>
        public const string EditorName = "WorkGroupEmployeeLookUpEditor";

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
            var item = source as Item;
            if (item == null || item.PurchasingGroup == null) return new EntityList<Employee>();
            var result = RT.Service.Resolve<EmployeeController>().GetEmployeeByWorkGroupId(item.PurchasingGroupId.Value, keyword, pagingInfo);
            return result;
        }
    }
}
