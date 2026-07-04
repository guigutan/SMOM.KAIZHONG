using SIE.Domain;
using SIE.ProductIntfc.FirstInsps;
using SIE.ProductIntfc.InspLogs;
using SIE.Web.Data;

namespace SIE.Web.ProductIntfc.FirstInsps.DataQuery
{
    /// <summary>
    /// 首件数据查询器
    /// </summary>  
    public class FirstInspsDataQuery : DataQueryer
    {
        /// <summary>
        /// 获取首件规则，没有则默认生成数据
        /// </summary>
        /// <returns>首件规则列表</returns>
        public EntityList<FirstInspRule> GetFirstInspRules()
        {
            return RT.Service.Resolve<InspLogController>().GetFirstInspRuleList();
        }
    }
}
