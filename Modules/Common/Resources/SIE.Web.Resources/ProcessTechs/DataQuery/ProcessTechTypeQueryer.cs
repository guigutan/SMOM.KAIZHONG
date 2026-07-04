using SIE.Domain;
using SIE.Resources.ProcessTechs;
using SIE.Resources.ProcessTechTypes;
using SIE.Web.Data;

namespace SIE.Web.Resources.ProcessTechs.DataQuery
{
    /// <summary>
    /// 制程工艺查询器
    /// </summary>
    public class ProcessTechTypeQueryer : DataQueryer
    {
        /// <summary>
        /// 根据制程Id获取制程工艺
        /// </summary>
        /// <param name="processTechId">制程Id</param>
        /// <returns>制程工艺</returns>
        public EntityList<ProcessTech> GetProcessTech(double processTechId)
        {
            return RT.Service.Resolve<ProcessTechBaseController>().GetProcessTechList(processTechId);
        }

        /// <summary>
        /// 根据制程工艺类型Id获取制程工艺类型
        /// </summary>
        /// <param name="proTechTypeId">制程工艺类型Id</param>
        /// <returns>制程工艺类型</returns>
        public ProcessTechType GetProcessTechType(double proTechTypeId)
        {
            return RT.Service.Resolve<ProcessTechTypeController>().GetProcessTechTypeById(proTechTypeId);
        }
    }
}
