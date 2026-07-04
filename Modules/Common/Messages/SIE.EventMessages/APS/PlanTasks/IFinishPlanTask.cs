using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EventMessages.APS.PlanTasks
{
    /// <summary>
    /// 完成计划接口
    /// </summary>
    [Services.Service(FallbackType = typeof(EmptyFinishPlanTask))]
    public interface IFinishPlanTask
    {
        /// <summary>
        /// 完成计划
        /// </summary>
        /// <param name="finishTaskDatas">完成计划信息数据列表</param>
        /// <returns>完成计划结果列表</returns>
        IReadOnlyList<FinishTaskResult> FinishPlanTasks(IReadOnlyList<FinishTaskData> finishTaskDatas);
    }

    /// <summary>
    /// 完成计划接口默认实现
    /// </summary>
    public class EmptyFinishPlanTask : IFinishPlanTask
    {
        /// <summary>
        /// 完成计划
        /// </summary>
        /// <param name="finishTaskDatas">完成计划信息数据列表</param>
        /// <returns>完成计划结果列表</returns>
        public IReadOnlyList<FinishTaskResult> FinishPlanTasks(IReadOnlyList<FinishTaskData> finishTaskDatas)
        {
            List<FinishTaskResult> results = new List<FinishTaskResult>();

            finishTaskDatas.ForEach(data =>
            {
                FinishTaskResult finishTaskResult = new FinishTaskResult(data.PlanTaskId);
                data.FinishTaskDetailDatas.ForEach(detail =>
                {
                    FinishTaskDetailResult detailResult = new FinishTaskDetailResult(detail.PlanTaskDetailId, detail.WorkOrder);
                    finishTaskResult.FinishTaskDetailResults.Add(detailResult);
                });
                results.Add(finishTaskResult);
            });

            return results;
        }
    }

    /// <summary>
    /// 完成计划信息数据
    /// </summary>
    [Serializable]
    public class FinishTaskData
    {
        /// <summary>
        /// 计划任务ID
        /// </summary>
        public string PlanTaskId { get; set; }

        /// <summary>
        /// 完成计划信息明细列表
        /// </summary>
        public List<FinishTaskDetailData> FinishTaskDetailDatas { get; set; }
    }

    /// <summary>
    /// 完成计划信息明细数据
    /// </summary>
    [Serializable]
    public class FinishTaskDetailData
    {
        /// <summary>
        /// 计划任务明细ID
        /// </summary>
        public string PlanTaskDetailId { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrder { get; set; }
    }

    /// <summary>
    /// 完成计划结果
    /// </summary>
    [Serializable]
    public class FinishTaskResult
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="planTaskId">计划任务ID</param>
        /// <param name="isSuccess">是否成功</param>
        /// <param name="msg">信息</param>
        public FinishTaskResult(string planTaskId, bool isSuccess = true, string msg = "")
        {
            PlanTaskId = planTaskId;
            IsSuccess = isSuccess;
            Message = msg;
            FinishTaskDetailResults = new List<FinishTaskDetailResult>();
        }

        /// <summary>
        /// 计划任务ID
        /// </summary>
        public string PlanTaskId { get; set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 计划任务的结果信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 完成计划明细结果列表
        /// </summary>
        public List<FinishTaskDetailResult> FinishTaskDetailResults { get; set; }
    }

    /// <summary>
    /// 完成计划明细结果
    /// </summary>
    [Serializable]
    public class FinishTaskDetailResult
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="planTaskDetailId">计划任务明细ID</param>
        /// <param name="workOrder">工单号</param>
        /// <param name="isSuccess">是否成功</param>
        /// <param name="msg">信息</param>
        public FinishTaskDetailResult(string planTaskDetailId, string workOrder, bool isSuccess = true, string msg = "")
        {
            PlanTaskDetailId = planTaskDetailId;
            WorkOrder = workOrder;
            IsSuccess = isSuccess;
            Message = msg;
        }

        /// <summary>
        /// 计划任务明细ID
        /// </summary>
        public string PlanTaskDetailId { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrder { get; set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 工单（对应计划明细）结果信息
        /// </summary>
        public string Message { get; set; }
    }
}
