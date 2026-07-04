using System;
using System.Collections.Generic;

namespace SIE.EventMessages.Release
{
    /// <summary>
    /// MES回传生产日进度信息给APS
    /// </summary>
    [Services.Service(FallbackType = typeof(EmptyPlanTaskDailySchedule))]
    public interface IPlanTaskDailySchedule
    {
        /// <summary>
        /// 更新生产日进度
        /// </summary>
        /// <param name="dailySchedulePlanDatas">进度计划数据列表</param>
        void UpdateDailyScheduleList(IReadOnlyList<DailySchedulePlanData> dailySchedulePlanDatas);
    }

    /// <summary>
    /// 默认实现
    /// </summary>
    class EmptyPlanTaskDailySchedule : IPlanTaskDailySchedule
    {
        public void UpdateDailyScheduleList(IReadOnlyList<DailySchedulePlanData> dailySchedulePlanDatas)
        {
            // 留空
        }
    }

    /// <summary>
    /// 日进度计划数据
    /// </summary>
    [Serializable]
    public class DailySchedulePlanData
    {
        /// <summary>
        /// 计划任务ID
        /// </summary>
        public string PlanTaskId { get; set; }

        /// <summary>
        /// 计划任务明细Id
        /// </summary>
        public string DetailId { get; set; }

        /// <summary>
        /// 班次ID
        /// </summary>
        public double ShiftId { get; set; }

        /// <summary>
        /// 生产资源ID
        /// </summary>
        public double WipResourceId { get; set; }

        /// <summary>
        /// 工艺单编号
        /// </summary>
        public string ProcessTechOrderCode { get; set; }

        /// <summary>
        /// 已完工数量
        /// </summary>
        public decimal FinishedAmount { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public DateTime FinishedDate { get; set; }

        /// <summary>
        /// 消耗工时
        /// </summary>
        public decimal CostHour { get; set; }
    }

    /// <summary>
    /// Mes的生产日进度信息
    /// </summary>
    [Serializable]
    public class MesDailySchedulePlanData : DailySchedulePlanData
    {
        /// <summary>
        /// 工单ID
        /// </summary>
        public double WorkOrderId { get; set; }

        /// <summary>
        /// 工单采集记录ID
        /// </summary>
        public double StatisticId { get; set; }

        /// <summary>
        /// 工单计划数量
        /// </summary>
        public decimal PlanQty { get; set; }
    }
}
