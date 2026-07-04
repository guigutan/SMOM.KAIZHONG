using SIE.Api;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.WIP;
using SIE.Resources.Employees;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using SIE.Tech.Stations;
using System;

namespace SIE.MES.Statistics.WIP
{
    /// <summary>
    /// 在制品统计控制器Api
    /// </summary>
    public partial class WipStatisticsController : DomainController
    {
        /// <summary>
        /// 获取当班采集数
        /// </summary>
        /// <param name="queryInfo">工位统计数据查询条件</param>
        /// <returns>当班采集数</returns>
        [ApiService("获取当班采集数")]
        [return: ApiReturn("获取当班采集数 GetQtyPass")]
        public virtual decimal GetQtyPass([ApiParameter("工位统计数据查询条件")] StatisticsQueryInfo queryInfo)
        {
            if (queryInfo.OperatorId <= 0)
                throw new ValidationException("员工Id不存在！".L10N());
            var employee = RF.GetById<Employee>(queryInfo.OperatorId);
            if (employee == null)
                throw new ValidationException("员工不存在！".L10N());
            if (queryInfo.ResourceId <= 0)
                throw new ValidationException("资源Id不存在！".L10N());
            var resource = RF.GetById<WipResource>(queryInfo.ResourceId);
            if (resource == null)
                throw new ValidationException("资源不存在！".L10N());
            if (queryInfo.ProcessId <= 0)
                throw new ValidationException("工序Id不存在！".L10N());
            var process = RF.GetById<Process>(queryInfo.ProcessId);
            if (process == null)
                throw new ValidationException("工序不存在！".L10N());
            if (queryInfo.StationId <= 0)
                throw new ValidationException("工位Id不存在！".L10N());
            var station = RF.GetById<Station>(queryInfo.StationId);
            if (station == null)
                throw new ValidationException("工位不存在！".L10N());

            var stationColEvent = RT.Service.Resolve<WipStatisticsController>().GetStationCollected(queryInfo);
            if (stationColEvent == null)
                return 0;
            return stationColEvent.QtyPass + stationColEvent.QtyFailed;
        }
        /// <summary>
        /// 获取工位采集数
        /// </summary>
        /// <param name="queryInfo">工位统计数据查询条件</param>
        /// <returns>当班采集数</returns>
        [ApiService("获取工位采集数")]
        [return: ApiReturn("工位采集数 GetStationCollectedQty")]
        public virtual decimal GetStationCollectQty([ApiParameter("工位统计数据查询条件")] StatisticsQueryInfo queryInfo)
        {
            if (queryInfo.OperatorId <= 0)
                throw new ValidationException("员工Id不存在！".L10N());
            var employee = RF.GetById<Employee>(queryInfo.OperatorId);
            if (employee == null)
                throw new ValidationException("员工不存在！".L10N());
            if (queryInfo.ResourceId <= 0)
                throw new ValidationException("资源Id不存在！".L10N());
            var resource = RF.GetById<WipResource>(queryInfo.ResourceId);
            if (resource == null)
                throw new ValidationException("资源不存在！".L10N());
            if (queryInfo.ProcessId <= 0)
                throw new ValidationException("工序Id不存在！".L10N());
            var process = RF.GetById<Process>(queryInfo.ProcessId);
            if (process == null)
                throw new ValidationException("工序不存在！".L10N());
            if (queryInfo.StationId <= 0)
                throw new ValidationException("工位Id不存在！".L10N());
            var station = RF.GetById<Station>(queryInfo.StationId);
            if (station == null)
                throw new ValidationException("工位不存在！".L10N());

            var stationColEvent = RT.Service.Resolve<WipStatisticsController>().GetStationCollectedQty(queryInfo);
            if (stationColEvent == null)
                return 0;
            return stationColEvent.QtyPass + stationColEvent.QtyFailed;
        }

        /// <summary>
        /// 获取当班采集数和当班不良数
        /// </summary>
        /// <param name="queryInfo">工位统计数据查询条件</param>
        /// <returns>当班采集数</returns>
        [ApiService("获取当班采集数和当班不良数")]
        [return: ApiReturn("获取当班采集数和当班不良数 GetQtyPassAndFailed")]
        public virtual  Tuple<decimal,decimal> GetQtyPassAndFailed([ApiParameter("工位统计数据查询条件")] StatisticsQueryInfo queryInfo)
        {
            if (queryInfo.OperatorId <= 0)
                throw new ValidationException("员工Id不存在！".L10N());
            var employee = RF.GetById<Employee>(queryInfo.OperatorId);
            if (employee == null)
                throw new ValidationException("员工不存在！".L10N());
            if (queryInfo.ResourceId <= 0)
                throw new ValidationException("资源Id不存在！".L10N());
            var resource = RF.GetById<WipResource>(queryInfo.ResourceId);
            if (resource == null)
                throw new ValidationException("资源不存在！".L10N());
            if (queryInfo.ProcessId <= 0)
                throw new ValidationException("工序Id不存在！".L10N());
            var process = RF.GetById<Process>(queryInfo.ProcessId);
            if (process == null)
                throw new ValidationException("工序不存在！".L10N());
            if (queryInfo.StationId <= 0)
                throw new ValidationException("工位Id不存在！".L10N());
            var station = RF.GetById<Station>(queryInfo.StationId);
            if (station == null)
                throw new ValidationException("工位不存在！".L10N());

            var stationColEvent = RT.Service.Resolve<WipStatisticsController>().GetStationCollected(queryInfo);
            if (stationColEvent == null)
                return new Tuple<decimal, decimal>(0,0);
            return  new Tuple<decimal, decimal>(stationColEvent.QtyPass + stationColEvent.QtyFailed, stationColEvent.QtyFailed);
        }
    }
}
