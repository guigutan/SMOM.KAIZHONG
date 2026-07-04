using System.Collections.Generic;

namespace SIE.EventMessages.PatrolInspBills
{
    /// <summary>
    /// 巡检接口
    /// </summary>
    [Services.Service(FallbackType = typeof(DefaultPatrolInspBillEvent))]
    public interface IPatrolInspBillEvent
    {
        /// <summary>
        /// 返回工单的工序Id列表
        /// </summary>
        /// <param name="woIds">工单Id集合</param>
        List<double> GetProcessList(List<double> woIds);
    }

    /// <summary>
    /// 巡检接口默认实现
    /// </summary>
    public class DefaultPatrolInspBillEvent : IPatrolInspBillEvent
    {
        /// <summary>
        /// 返回工单的工序Id列表
        /// </summary>
        /// <param name="woIds">工单Id集合</param>
        public List<double> GetProcessList(List<double> woIds)
        {
            return new List<double>();
        }
    }
}