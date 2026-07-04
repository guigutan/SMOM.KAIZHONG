using System.Collections.Generic;

namespace SIE.EventMessages.WMS.TraceableItem
{
    /// <summary>
    /// 查询物料追溯[TraceableItem]信息接口
    /// </summary>
    [Services.Service(FallbackType = typeof(DefaultTraceableItemInterface))]
    public interface ITraceableItem
    {
        /// <summary>
        /// 查询红牌清单
        /// </summary>
        /// <param name="criteriaInfo">查询条件</param>
        /// <returns></returns>
        List<TraceableInfo> GetTraceableItemInfos(TraceableItemCriteriaInfo criteriaInfo);
    }

    /// <summary>
    /// 查询TraceableItem信息接口的默认实现
    /// </summary>
    class DefaultTraceableItemInterface : ITraceableItem
    {
        /// <summary>
        /// 查询TraceableItem信息
        /// </summary>
        /// <param name="criteriaInfo">查询条件</param>
        /// <returns></returns>
        public List<TraceableInfo> GetTraceableItemInfos(TraceableItemCriteriaInfo criteriaInfo)
        {
            return new List<TraceableInfo>();
        }
    }
}
