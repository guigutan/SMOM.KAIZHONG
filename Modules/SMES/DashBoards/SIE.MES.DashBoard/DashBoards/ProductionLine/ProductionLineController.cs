using SIE.Domain;
using SIE.MES.Statistics.Entities;
using SIE.MES.WIP.Products;
using SIE.MES.WorkOrders;
using SIE.Resources.Enterprises;
using SIE.Resources.ShiftTypes;
using System;

namespace SIE.MES.DashBoard.DashBoards.ProductionLine
{
    /// <summary>
    /// 产线看板控制器
    /// </summary>
    public class ProductionLineController : DomainController
    {
        /// <summary>
        /// 直通率\达成率默认背景颜色
        /// </summary>
        public static readonly string DefaultBackgroundStr = "#333434";

        /// <summary>
        /// 获取当日工单
        /// </summary>
        /// <param name="lineId">产线id</param>
        /// <param name="currentDay">当前时间</param>
        /// <returns>当日工单列表</returns>
        public virtual EntityList<WorkOrder> GetCurrentDayWorkOrderList(double lineId, DateTime currentDay)
        {
            var query = Query<WorkOrder>();
            query.Where(p => p.PlanBeginDate >= currentDay && p.PlanBeginDate < currentDay.AddDays(1) && p.ResourceId == lineId);
            return query.ToList();
        }

        /// <summary>
        /// 获取未关闭工单
        /// </summary>
        /// <param name="lineId">产线id</param>
        /// <param name="currentDay">当前时间</param>
        /// <returns>未关闭工单</returns>
        public virtual EntityList<WorkOrder> GetUnCloseWorkOrderList(double lineId, DateTime currentDay)
        {
            var query = Query<WorkOrder>();
            query.Where(p => p.PlanEndDate >= currentDay.AddDays(-30) && p.PlanEndDate < DateTime.Now.Date.AddDays(1)
            && p.State != Core.WorkOrders.WorkOrderState.Close
            && p.State != Core.WorkOrders.WorkOrderState.Finish
            && p.ResourceId == lineId);
            return query.ToList();
        }

        /// <summary>
        /// 根据id获取资源(车间或产线)
        /// </summary>
        /// <param name="id">资源id</param>
        /// <returns>资源</returns>
        public virtual Enterprise GetEnterprisesById(double id)
        {
            var q = Query<Enterprise>()
             .Where(p => p.Id == id && (p.InvOrgId == 0 || p.InvOrgId == AppRuntime.InvOrg));
            return q.FirstOrDefault();
        }

        /// <summary>
        /// 获取班次
        /// </summary>
        /// <param name="currentTime">当前时间</param>
        ///  <param name="shiftTypeId">资源日历id</param>
        /// <returns>班次</returns>
        public virtual Shift GetRealShiftByShiftTypeId(DateTime currentTime, double shiftTypeId)
        {
            var q = Query<ShiftType>()
            .Where(p => p.Id == shiftTypeId);
            var shiftType = q.FirstOrDefault();
            if (shiftType != null)
            {
                var shiftLits = shiftType.ShiftList;
                if (shiftLits != null && shiftLits.Count > 0)
                {
                    DateTime beginTime; //当班开始时间
                    DateTime endTime;   //当班结束时间
                    foreach (var shift in shiftLits)
                    {
                        beginTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, shift.BeginTime.Hour, shift.BeginTime.Minute, shift.BeginTime.Second);
                        endTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, shift.EndTime.Hour, shift.EndTime.Minute, shift.EndTime.Second);
                        if (currentTime >= beginTime && currentTime <= endTime)
                        {
                            shift.BeginTime = beginTime;
                            shift.EndTime = endTime;
                            return shift;
                        }

                        if (shift.IsOverDay)
                        {
                            DateTime twentyFouroclock = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, 23, 59, 59);
                            if (currentTime >= beginTime && currentTime <= twentyFouroclock)
                            {
                                //当前时间是夜班上半夜，还未跨天
                                shift.BeginTime = beginTime;
                                DateTime tempEndTime = currentTime.AddDays(1);
                                shift.EndTime = new DateTime(tempEndTime.Year, tempEndTime.Month, tempEndTime.Day, shift.EndTime.Hour, shift.EndTime.Minute, shift.EndTime.Second);
                            }
                            else
                            {
                                //当前时间是夜班下半夜，已经跨天
                                shift.BeginTime = beginTime.AddDays(-1);
                                shift.EndTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, shift.EndTime.Hour, shift.EndTime.Minute, shift.EndTime.Second);
                            }

                            return shift;
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 获取产线当班缺陷不良记录
        /// </summary>
        /// <param name="resourceId">产线id</param>
        /// <param name="shiftBeginTime">班次开始时间</param>
        /// <param name="shiftEndTime">班次结束时间</param>
        /// <returns>产线当班缺陷不良记录</returns>
        public virtual EntityList<WipProductDefect> GetWipProductDefectList(double resourceId, DateTime shiftBeginTime, DateTime shiftEndTime)
        {
            return Query<WipProductDefect>()
               .Where(p => p.ResourceId == resourceId
                      && p.CreateDate >= shiftBeginTime
                      && p.CreateDate <= shiftEndTime)
               .ToList();
        }

        /// <summary>
        /// 获取时间段内资源产能采集统计
        /// </summary>
        /// <param name="resourceId">产线id</param>
        /// <param name="beforeSevenDay">当前日期前七天</param>
        /// <returns>时间段内资源产能采集统计</returns>
        public virtual EntityList<ResourceStatistics> GetResourceStatisticsListByShiftDate(double resourceId, DateTime beforeSevenDay)
        {
            return Query<ResourceStatistics>()
            .Where(p => p.ResourceId == resourceId && p.CollectDate >= beforeSevenDay)
            .ToList();
        }
    }
}
