using SIE.Domain;
using SIE.ESop.Displays;
using SIE.ManagedProperty;
using SIE.Wpf.Editors;

namespace SIE.Wpf.ESop.Editors
{
    /// <summary>
    /// 显示点编辑器
    /// </summary>
    public class DisplayPointLookUpEditor : PagingLookUpEditor
    {
        /// <summary>
        /// 编辑器名称
        /// </summary>
        public const string EditorName = "DisplayPointLookUpEditor";

        /// <summary>
        /// 获取物料分类小类
        /// </summary>
        /// <param name="source">上下文</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">关键字</param>
        /// <param name="titleProperty">显示属性</param>
        /// <returns>物料分类小类列表</returns>
        protected override EntityList GetDataSourceCore(Entity source, PagingInfo pagingInfo, string keyword, IManagedProperty titleProperty)
        {
            var workstation = source as ESopWorkstationSelector;
            
            if (workstation != null && workstation.Resource != null)
            {
                return RT.Service.Resolve<DisplayPointController>().GetDisplayPointList(workstation.ResourceId.Value, keyword, pagingInfo);
            }

            return new EntityList<DisplayPoint>();
        }
    }
}