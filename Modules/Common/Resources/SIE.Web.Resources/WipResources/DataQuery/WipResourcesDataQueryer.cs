using SIE.Resources.ProcessSegments;
using SIE.Resources.WipResources;
using SIE.Web.Data;

namespace SIE.Web.Resources.WipResources.DataQuery
{
    /// <summary>
    /// 资源查询器
    /// </summary>
    public class WipResourcesDataQueryer : DataQueryer
    {
        /// <summary>
        /// 根据制程工艺编码获取工段
        /// </summary>
        /// <param name="id">制程工艺编码</param>
        /// <returns></returns>
        public ProcessSegment GetProcessSegmentById(double id)
        {
            return RT.Service.Resolve<WipResourceController>().GetProcessSegmentByProcessTechTypeId(id);
        }

    }
}
