using SIE.Defects.InspectionItems;
using SIE.Domain;
using SIE.ManagedProperty;
using SIE.Wpf.Editors;

namespace SIE.Wpf.MES.InspectionStandards.Editors
{
    /// <summary>
    /// 机型检验项目编辑器
    /// </summary>
    class LoadInspectionModeEditor : PagingLookUpEditor
    {
        /// <summary>
        /// 机型检验项目的检验方式添加编辑器名称
        /// </summary>
        public const string EditorName = "LoadInspectionModeEditor";

        /// <summary>
        /// 重新加载数据源
        /// </summary>
        /// <param name="source">数据源</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">关键字</param>
        /// <param name="titleProperty">标题属性</param>
        /// <returns>机型检验项目列表</returns>
        protected override EntityList GetDataSourceCore(Entity source, PagingInfo pagingInfo, string keyword, IManagedProperty titleProperty)
        {
            return RT.Service.Resolve<InspectionItemController>().GetInspectionModes(InspectionType.OnlineInsp, pagingInfo, keyword);
        }
    }
}
