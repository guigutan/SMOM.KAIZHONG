using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EventMessages.APS.PlanTasks
{
    /// <summary>
    /// 取消完成计划接口
    /// </summary>
    [Services.Service(FallbackType = typeof(EmptyCancelFinishTask))]
    public interface ICancelFinishTask
    {
        /// <summary>
        /// 取消完成计划
        /// </summary>
        /// <param name="CancelFinishTaskDatas">取消完成计划信息数据列表</param>
        /// <returns>取消完成计划结果列表</returns>
        IReadOnlyList<CancelFinishTaskResult> CancelFinishPlanTasks(IReadOnlyList<CancelFinishTaskData> CancelFinishTaskDatas);
    }

    /// <summary>
    /// 取消完成计划接口默认实现
    /// </summary>
    public class EmptyCancelFinishTask : ICancelFinishTask
    {
        /// <summary>
        /// 取消完成计划
        /// </summary>
        /// <param name="CancelFinishTaskDatas">取消完成计划信息数据列表</param>
        /// <returns>取消完成计划结果列表</returns>
        public IReadOnlyList<CancelFinishTaskResult> CancelFinishPlanTasks(IReadOnlyList<CancelFinishTaskData> CancelFinishTaskDatas)
        {
            List<CancelFinishTaskResult> results = new List<CancelFinishTaskResult>();

            CancelFinishTaskDatas.ForEach(data =>
            {
                CancelFinishTaskResult cancelFinishTaskResult = new CancelFinishTaskResult(data.PlanTaskId);
                data.CancelFinishTaskDetailDatas.ForEach(detail =>
                {
                    CancelFinishTaskDetailResult detailResult = new CancelFinishTaskDetailResult(detail.PlanTaskDetailId, detail.WorkOrder);
                    cancelFinishTaskResult.CancelFinishTaskDetailResults.Add(detailResult);
                });
                results.Add(cancelFinishTaskResult);
            });

            return results;
        }
    }

    /// <summary>
    /// 取消完成计划信息数据
    /// </summary>
    [Serializable]
    public class CancelFinishTaskData
    {
        /// <summary>
        /// 计划任务ID
        /// </summary>
        public string PlanTaskId { get; set; }

        /// <summary>
        /// 取消后是否有明细为生产中
        /// </summary>
        public bool IsProducing { get; set; }

        /// <summary>
        /// 取消完成计划信息明细列表
        /// </summary>
        public List<CancelFinishTaskDetailData> CancelFinishTaskDetailDatas { get; set; }
    }

    /// <summary>
    /// 取消完成计划信息明细数据
    /// </summary>
    [Serializable]
    public class CancelFinishTaskDetailData
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
    /// 取消完成计划结果
    /// </summary>
    [Serializable]
    public class CancelFinishTaskResult
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="planTaskId">计划任务ID</param>
        /// <param name="isSuccess">是否成功</param>
        /// <param name="msg">信息</param>
        public CancelFinishTaskResult(string planTaskId, bool isSuccess = true, string msg = "")
        {
            PlanTaskId = planTaskId;
            IsSuccess = isSuccess;
            Message = msg;
            CancelFinishTaskDetailResults = new List<CancelFinishTaskDetailResult>();
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
        /// 取消完成计划明细结果列表
        /// </summary>
        public List<CancelFinishTaskDetailResult> CancelFinishTaskDetailResults { get; set; }
    }

    /// <summary>
    /// 取消完成计划明细结果
    /// </summary>
    [Serializable]
    public class CancelFinishTaskDetailResult
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="planTaskDetailId">计划任务明细ID</param>
        /// <param name="workOrder">工单号</param>
        /// <param name="isSuccess">是否成功</param>
        /// <param name="msg">信息</param>
        public CancelFinishTaskDetailResult(string planTaskDetailId, string workOrder, bool isSuccess = true, string msg = "")
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
