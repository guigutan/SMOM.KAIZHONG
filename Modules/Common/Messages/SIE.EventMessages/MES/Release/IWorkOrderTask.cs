using System;
using System.Collections.Generic;

namespace SIE.EventMessages.Release
{
    /// <summary>
    /// APS任务拆分MES任务单接口
    /// </summary>
    [Services.Service(FallbackType = typeof(EmptyPlanTaskSplit))]
    public interface IWorkOrderTask
    {
        /// <summary>
        /// APS拆分前获取MES.TaskManagement工单生成的任务单列表
        /// </summary>
        /// <param name="workOrderInfos">工单信息列表</param>
        /// <returns>任务单列表</returns>
        IReadOnlyList<DispatchTaskInfo> WorkOrderTask(IReadOnlyList<WorkOrderInfo> workOrderInfos);


        /// <summary>
        /// 获取工单派工信息
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <returns></returns>
        List<DispatchTaskInfo> GetWorkOrderDispatchTasks(double workOrderId);
    }

    class EmptyWorkOrderTask : IWorkOrderTask
    {
        public IReadOnlyList<DispatchTaskInfo> WorkOrderTask(IReadOnlyList<WorkOrderInfo> workOrderInfos)
        {
            return new List<DispatchTaskInfo>();
        }

        public List<DispatchTaskInfo> GetWorkOrderDispatchTasks(double workOrderId)
        {
            return new List<DispatchTaskInfo>();
        }
    }

    /// <summary>
    /// 拆分前已下达工单Id
    /// </summary>
    [Serializable]
    public class WorkOrderInfo
    {
        /// <summary>
        /// 工单Id
        /// </summary>
        public double WorkOrderId { get; set; }
    }

    /// <summary>
    /// 拆分前已下达工单对应任务单
    /// </summary>
    [Serializable]
    public class DispatchTaskInfo
    {
        /// <summary>
        /// 工单Id
        /// </summary>
        public double WorkOrderId { get; set; }

        /// <summary>
        /// 任务单Id
        /// </summary>
        public double DispatchTaskId { get; set; }

        /// <summary>
        /// 任务单编号
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// 任务单状态
        /// </summary>

        public int TaskStatus { get; set; }

        /// <summary>
        /// 合并状态
        /// </summary>
        public int MergedStatus { get; set; }
    }
}
