using SIE.Domain;
using SIE.ManagedProperty;
using SIE.Resources.CalendarSchemes;
using SIE.Wpf.Editors;

namespace SIE.Wpf.Resources.CalendarSchemes.Editors
{
    /// <summary>
    /// 可用日历方案编辑器
    /// </summary>
    public class SchemeLookUpEditor : PagingLookUpEditor
    {
        /// <summary>
        /// 日历方案编辑器名称
        /// </summary>
        public const string EditorName = "SchemeLookUpEditor";

        /// <summary>
        /// 重新加载数据源
        /// </summary>
        /// <param name="source">对象</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">关键字</param>
        /// <param name="titleProperty">托管属性标记</param>
        /// <returns>可用日历方案列表</returns>
        protected override EntityList GetDataSourceCore(Entity source, PagingInfo pagingInfo, string keyword, IManagedProperty titleProperty)
        {
            return RT.Service.Resolve<CalendarSchemeController>().GetEnableCalendarSchemeList(pagingInfo, keyword);
        }
    }
}
