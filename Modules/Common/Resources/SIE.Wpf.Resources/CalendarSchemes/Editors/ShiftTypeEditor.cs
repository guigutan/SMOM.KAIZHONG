using SIE.Domain;
using SIE.ManagedProperty;
using SIE.Resources.ShiftTypes;
using SIE.Wpf.Editors;

namespace SIE.Wpf.Resources.CalendarSchemes.Editors
{
    /// <summary>
    /// 班制编辑器(筛选掉没有班次的班制)
    /// </summary>
    class ShiftTypeEditor : PagingLookUpEditor
    {
        /// <summary>
        /// 编辑器名称
        /// </summary>
        public const string EditorName = "ShiftTypeEditor";

        /// <summary>
        /// 获取数据源
        /// </summary>
        /// <param name="source">源</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">关键字</param>
        /// <param name="titleProperty">标题属性</param>
        /// <returns>实体列表</returns>
        protected override EntityList GetDataSourceCore(Entity source, PagingInfo pagingInfo, string keyword, IManagedProperty titleProperty)
        {
            return RT.Service.Resolve<ShiftTypeController>().GetShiftTypes(pagingInfo, keyword);
        }
    }
}
