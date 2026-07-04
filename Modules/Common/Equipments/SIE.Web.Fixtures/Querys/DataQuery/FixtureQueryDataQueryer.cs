using SIE.EventMessages.EMS.Fixtures;
using SIE.Web.Data;

namespace SIE.Web.Fixtures.Querys.DataQuery
{
    /// <summary>
    /// 工治具查询查询器
    /// </summary>
    public class FixtureQueryDataQueryer : DataQueryer
    {
        /// <summary>
        /// 根据工单Id获取工单的工艺面
        /// </summary>
        /// <param name="woId">工单Id</param>
        /// <returns>工单的工艺面</returns>
        public int? GetWorkOrderDeck(double woId)
        {
            return RT.Service.Resolve<IProcessSurface>().GetProcessSurface(woId);
        }
    }
}
