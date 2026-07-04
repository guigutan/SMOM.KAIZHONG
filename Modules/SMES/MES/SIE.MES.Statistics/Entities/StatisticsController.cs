using SIE.Domain;
using System;

namespace SIE.MES.Statistics.Entities
{
    /// <summary>
    /// 统计数据控制器
    /// </summary>
    public class StatisticsController : DomainController
    {
        /// <summary>
        /// 获取时间段内资源产能采集统计
        /// </summary>
        /// <param name="resourceId">产线id</param>
        /// <param name="beginTime">当班开始时间</param>
        /// <param name="endTime">当班结束时间</param>
        /// <returns>时间段内资源产能采集统计</returns>
        public virtual EntityList<ResourceStatistics> GetResourceStatisticsList(double resourceId, DateTime beginTime, DateTime endTime)
        {
            return Query<ResourceStatistics>()
            .Where(p => p.ResourceId == resourceId
                   && p.StartTime >= beginTime
                   && p.StartTime <= endTime)
            .ToList();
        }

        /// <summary>
        /// 根据车间ID/开始时间/结束时间获取产量
        /// </summary>
        /// <param name="workShopId">车间ID</param>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>产量列表</returns>
        public virtual EntityList<WorkOrderStatistics> GetWorkOrderStatisticsList(double workShopId, DateTime beginTime, DateTime endTime)
        {
            return Query<WorkOrderStatistics>()
            .Where(p => p.WorkShopId == workShopId
                   && p.BeginCollectTime >= beginTime
                   && p.BeginCollectTime < endTime)
            .ToList();
        }

        /// <summary>
        /// 根据工单ID获取结束时间
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <returns></returns>
        public virtual DateTime? GetWorkOrderStatisticses(double workOrderId)
        {
            return Query<WorkOrderStatistics>().Where(p => p.WorkOrderId == workOrderId).Select(p => p.EndCollectTime.MAX()).FirstOrDefault<DateTime?>();
        }
    }
}