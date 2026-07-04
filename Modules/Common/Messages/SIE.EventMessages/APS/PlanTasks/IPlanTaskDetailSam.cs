using System;
using System.Collections.Generic;

namespace SIE.EventMessages.APS.PlanTasks
{
    /// <summary>
    /// 计划明细工艺定额接口
    /// </summary>
    [Services.Service(FallbackType = typeof(EmptyPlanTaskDetailSam))]
    public interface IPlanTaskDetailSam
    {
        /// <summary>
        /// 根据工单号获取计划明细工艺定额
        /// </summary>
        /// <param name="workOrderNo">工单号</param>
        /// <returns>计划明细工艺定额</returns>
        IReadOnlyList<PlanTaskDetailSamInfo> PlanTaskDetailSam(List<string> workOrderNo);
    }

    /// <summary>
    /// 计划明细工艺定额接口默认实现
    /// </summary>
    public class EmptyPlanTaskDetailSam : IPlanTaskDetailSam
    {
        /// <summary>
        /// 根据工单号获取计划明细工艺定额
        /// </summary>
        /// <param name="workOrderNo">工单号</param>
        /// <returns>计划明细工艺定额</returns>
        public IReadOnlyList<PlanTaskDetailSamInfo> PlanTaskDetailSam(List<string> workOrderNo)
        {
            return new List<PlanTaskDetailSamInfo>();
        }
    }

    /// <summary>
    /// 工艺定额列表
    /// </summary>
    [Serializable]
    public class PlanTaskDetailSamInfo
    {
        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo { get; set; }

        /// <summary>
        /// 工艺定额
        /// </summary>
        public double Sam { get; set; }
    }
}
