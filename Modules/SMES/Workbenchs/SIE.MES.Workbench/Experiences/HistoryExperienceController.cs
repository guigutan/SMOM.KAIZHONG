using SIE.Domain;
using System.Linq;

namespace SIE.MES.Workbench.Experiences
{
    /// <summary>
    /// 历史经验库控制器
    /// </summary>
    public class HistoryExperienceController : DomainController
    {
        /// <summary>
        /// 获取物料的历史经验库列表
        /// </summary> 
        /// <param name="itemId">物料ID</param>  
        /// <param name="pagingInfo">分页信息</param> 
        /// <returns>物料的历史经验库列表</returns>
        public virtual EntityList<ExperienceDetail> GetHistoryExperienceList(double itemId, PagingInfo pagingInfo)
        {
            return Query<HistoryExperience>().Where(p => p.ItemId == itemId).FirstOrDefault()?.ExperienceDetailList;
        }
    }
}