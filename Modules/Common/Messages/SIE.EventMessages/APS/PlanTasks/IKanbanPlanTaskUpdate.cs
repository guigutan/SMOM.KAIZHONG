using System;

namespace SIE.EventMessages.APS.PlanTasks
{
    /// <summary>
    /// 电子看板转产后更新计划接口
    /// </summary>
    [Services.Service(FallbackType = typeof(EmptyKanbanPlanTaskUpdate))]
    public interface IKanbanPlanTaskUpdate
    {
        /// <summary>
        /// 根据工单转产后更新计划
        /// </summary>
        /// <param name="planTaskInfo">查询参数</param>
        /// <returns></returns>
        int KanbanPlanTaskUpdate(KanbanPlanTaskInfo planTaskInfo);
    }
    /// <summary>
    /// 工单转产后更新计划接口默认实现
    /// </summary>
    public class EmptyKanbanPlanTaskUpdate : IKanbanPlanTaskUpdate
    {
        /// <summary>
        /// 根据工单转产后更新计划
        /// </summary>
        /// <param name="planTaskInfo">查询参数</param>
        /// <returns></returns>
        public int KanbanPlanTaskUpdate(KanbanPlanTaskInfo planTaskInfo)
        {
            return 0;
        }
    }






    /// <summary>
    /// 查询参数
    /// </summary>
    [Serializable]
    public class KanbanPlanTaskInfo
    {
        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo { get; set; }
        /// <summary>
        /// 制程单号
        /// </summary>
        public string ProcessTechCode { get; set; }
        /// <summary>
        /// 计划明细ID
        /// </summary>
        public double PlanTaskDetailId { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime DateTime { get; set; }
        /// <summary>
        /// 类型（1开始 2结束 3停厂）
        /// </summary>
        public int type { get; set; }
    }
}
