using SIE.Domain;
using SIE.Tech.Stations;
using System.Collections.Generic;

namespace SIE.Kit.MES.Stations
{
    /// <summary>
    /// 工位扩展控制器
    /// </summary>
    public class StationExtensionController : DomainController
    {
        /// <summary>
        /// 获取工位物料明细
        /// </summary>
        /// <param name="id">工位ID</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="sortInfo">排序信息</param>
        /// <returns></returns>
        public virtual EntityList<StationItem> GetStationItems(double id, PagingInfo pagingInfo, List<OrderInfo> sortInfo)
        {
            return Query<StationItem>()
                 .Where(x => x.StationId == id)
                 .OrderBy(sortInfo)
                 .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }
    }
}
