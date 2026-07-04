using System;
using System.Collections.Generic;

namespace SIE.EventMessages.MES.ProcessStatistics
{
    /// <summary>
    ///获取工序采集信息接口
    /// </summary>
    [Services.Service(FallbackType = typeof(DefaultProcessStatisticsInterface))]
    public interface IProcessStatistics
    {
        /// <summary>
        /// 工序采集信息
        /// </summary>
        /// <param name="workOrderId"></param>
        List<ProcessStatisticsEventInfo> GetProcessStatisticsList(double workOrderId);

        /// <summary>
        /// 获取工序直通采集信息
        /// </summary>
        /// <param name="workOrderId"></param>
        List<ProcessStatisticsEventInfo> GetProcessFpyStatisticsList(double workOrderId);

        /// <summary>
        /// 获取工序直通采集信息
        /// </summary>
        /// <param name="workShopId">车间ID</param>
        /// <param name="startDateTime">开始时间</param>
        /// <param name="endDateTime">结束时间</param>
        List<ProcessStatisticsEventInfo> GetProcessFpyStatisticsList(double workShopId, DateTime startDateTime, DateTime endDateTime);

        /// <summary>
        /// 获取当班采集数
        /// </summary>
        /// <param name="recourceId"></param>
        /// <param name="processId"></param>
        /// <param name="stationId"></param>
        /// <param name="workOrderId"></param>
        /// <returns></returns>
        decimal GetCollectQty(double recourceId, double processId, double stationId, double workOrderId);

        /// <summary>
        /// 根据工单Id集合获取工序采集信息
        /// </summary>
        /// <param name="workOrderIds"></param>
        /// <returns></returns>
        List<ProcessStatisticsEventInfo> GetProcessStatisticsListByWorkOrderIds(List<double> workOrderIds);
    }

    /// <summary>
    /// 获取工序采集信息的接口的默认实现
    /// </summary>
    class DefaultProcessStatisticsInterface : IProcessStatistics
    {
        /// <summary>
        /// 获取工序采集信息
        /// </summary>
        /// <param name="workOrderId"></param>
        public List<ProcessStatisticsEventInfo> GetProcessStatisticsList(double workOrderId)
        {
            return new List<ProcessStatisticsEventInfo>();
        }

        /// <summary>
        /// 获取工序直通采集信息
        /// </summary>
        /// <param name="workOrderId"></param>
        public List<ProcessStatisticsEventInfo> GetProcessFpyStatisticsList(double workOrderId)
        {
            return new List<ProcessStatisticsEventInfo>();
        }


        /// <summary>
        /// 获取工序直通采集信息
        /// </summary>
        /// <param name="workShopId">车间ID</param>
        /// <param name="startDateTime">开始时间</param>
        /// <param name="endDateTime">结束时间</param>
        public List<ProcessStatisticsEventInfo> GetProcessFpyStatisticsList(double workShopId, DateTime startDateTime, DateTime endDateTime)
        {
            return new List<ProcessStatisticsEventInfo>();
        }

        public decimal GetCollectQty(double recourceId, double processId, double stationId, double workOrderId)
        {
            return 0;
        }

        /// <summary>
        /// 获取工序采集信息
        /// </summary>
        /// <param name="workOrderIds"></param>
        /// <returns></returns>
        public List<ProcessStatisticsEventInfo> GetProcessStatisticsListByWorkOrderIds(List<double> workOrderIds)
        {
            return new List<ProcessStatisticsEventInfo>();
        }
    }
}
