using SIE.Domain;
using SIE.EventMessages.Release;
using SIE.MES.WorkOrders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.MES.Interfaces.ApsTasks
{
    /// <summary>
    /// 计划任务拆分数据验证逻辑
    /// </summary>
    public class TaskSplitBeforeValidator
    {
        /// <summary>
        /// 计划任务关联工单
        /// </summary>
        private readonly EntityList<TaskUnion> taskUnions;

        /// <summary>
        /// 计划任务关联明细
        /// </summary>
        private readonly EntityList<TaskUnionDetail> taskUnionDetails;

        /// <summary>
        /// 派工任务信息
        /// </summary>
        private readonly Dictionary<double, List<DispatchTaskInfo>> dicTaskInfos;

        /// <summary>
        /// 工单信息
        /// </summary>
        private readonly EntityList<WorkOrder> workOrders;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="planTaskIds">计划任务ID列表</param>
        public TaskSplitBeforeValidator(List<string> planTaskIds)
        {
            //获取计划任务关联工单          
            taskUnions = RT.Service.Resolve<ApsMesTaskController>().GetTaskUnions(planTaskIds);

            //获取计划任务关联明细
            var taskUnionIds = taskUnions.Select(x => x.Id).Distinct().ToList();
            taskUnionDetails = RT.Service.Resolve<ApsMesTaskController>().GetTaskUnionDetails(taskUnionIds);

            //获取派工任务信息
            var workOrderIds = taskUnionDetails.Where(p => p.WorkOrderId != 0).Select(p => p.WorkOrderId).Distinct().ToList();
            var workOrderInfos = workOrderIds.Select(p => new WorkOrderInfo() { WorkOrderId = p }).ToList();
            var taskInfos = RT.Service.Resolve<IWorkOrderTask>().WorkOrderTask(workOrderInfos);
            dicTaskInfos = taskInfos.GroupBy(p => p.WorkOrderId).ToDictionary(p => p.Key, p => p.ToList());

            //获取工单
            workOrders = RT.Service.Resolve<WorkOrderController>().GetWorkOrdersByWoIds(workOrderIds);
        }

        /// <summary>
        /// 拆分前接口验证拆分计划数据
        /// </summary>
        /// <param name="splitPlanData">拆分计划数据</param>
        /// <param name="splitPlanResult">拆分结果</param>
        /// <returns>计划任务明细集合</returns>
        public void ValidataSplitPlanData(SplitPlanData splitPlanData, SplitPlanResult splitPlanResult)
        {
            if (splitPlanData == null)
            {
                throw new ArgumentNullException(nameof(splitPlanData));
            }

            if (splitPlanResult == null)
            {
                throw new ArgumentNullException(nameof(splitPlanResult));
            }

            ValidataSplitPlanDataMain(splitPlanData.PlanTaskId, splitPlanResult);
            if (!splitPlanResult.IsSuccess)
            {
                return;
            }

            ValidataSplitPlanDataDetails(splitPlanData.Details, splitPlanResult);
        }



        /// <summary>
        /// 验证拆分计划数据
        /// </summary>
        /// <param name="planTaskId">计划任务Id</param>
        /// <param name="splitPlanResult">拆分结果</param>
        /// <returns>计划任务明细集合</returns>
        protected void ValidataSplitPlanDataMain(string planTaskId, SplitPlanResult splitPlanResult)
        {
            var cursbMsg = new StringBuilder();

            if (planTaskId.IsNullOrWhiteSpace())
            {
                cursbMsg.Append("计划任务ID不能为空!".L10N());
            }

            var curTaskUnion = taskUnions.FirstOrDefault(x => x.PlanTaskId == planTaskId);

            if (curTaskUnion == null)
            {
                cursbMsg.Append("计划任务ID[{0}]未找到计划任务关联!".L10nFormat(planTaskId));
            }
            else
            {
                if (taskUnionDetails == null || !taskUnionDetails.Any(x => x.TaskUnionId == curTaskUnion.Id))
                {
                    cursbMsg.Append("计划任务ID[{0}]未找到计划任务明细!".L10nFormat(planTaskId));
                }
            }

            if (cursbMsg.Length > 0)
            {
                TaskSplitHelper.SetSplitPlanMainResult(splitPlanResult, false, cursbMsg.ToString());
            }
        }

        /// <summary>
        /// 验证拆分计划明细集合
        /// </summary>
        /// <param name="splitDetails">拆分计划明细集合</param>
        /// <param name="splitPlanResult">拆分结果</param>        
        private void ValidataSplitPlanDataDetails(List<SplitPlanDetail> splitDetails, SplitPlanResult splitPlanResult)
        {
            foreach (var detail in splitDetails)
            {
                var detailId = detail.DetailId;
                var processTechOrderCode = detail.ProcessTechOrderCode;

                ValidataSplitPlanDataDetail(splitPlanResult, detailId, processTechOrderCode);
                if (!splitPlanResult.IsSuccess)
                {
                    return;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="splitPlanResult"></param>
        /// <param name="detailId"></param>
        /// <param name="processTechOrderCode"></param>
        protected WorkOrder ValidataSplitPlanDataDetail(SplitPlanResult splitPlanResult, string detailId, string processTechOrderCode)
        {
            if (detailId.IsNullOrWhiteSpace())
            {
                SetResult(splitPlanResult, detailId, processTechOrderCode, "明细ID不能为空!".L10N());
                return null;
            }


            if (processTechOrderCode.IsNullOrWhiteSpace())
            {
                SetResult(splitPlanResult, detailId, processTechOrderCode, "工艺单编号不能为空!".L10N());
                return null;
            }

            if (taskUnionDetails == null || !taskUnionDetails.Any())
            {
                SetResult(splitPlanResult, detailId, processTechOrderCode, "明细ID[{0}]未找到计划任务明细实体!".L10nFormat(detailId));
                return null;
            }

            var curTaskDetail = taskUnionDetails.FirstOrDefault(x => x.DetailId == detailId
                          && x.ProcessTechOrderCode == processTechOrderCode);
            if (curTaskDetail == null)
            {
                SetResult(splitPlanResult, detailId, processTechOrderCode, "明细ID[{0}]未找到计划任务明细实体!".L10nFormat(detailId));
                return null;
            }

            var curWorkOrder = GetWorkOrder(curTaskDetail);

            if (curWorkOrder == null)
            {
                SetResult(splitPlanResult, detailId, processTechOrderCode, "明细ID[{0}]未找到关联的工单实体!".L10nFormat(detailId));
                return null;
            }

            if (curWorkOrder.State == Core.WorkOrders.WorkOrderState.CancelRelease
                || curWorkOrder.State == Core.WorkOrders.WorkOrderState.Finish
                || curWorkOrder.State == Core.WorkOrders.WorkOrderState.Close)
            {
                SetResult(splitPlanResult, detailId, processTechOrderCode, "明细ID[{0}]对应的工单[{1}]状态为[{2}],不能拆分!"
                    .L10nFormat(detailId, curWorkOrder.No, curWorkOrder.State));
                return curWorkOrder;
            }

            if (curWorkOrder.State == Core.WorkOrders.WorkOrderState.Producing)
            {
                List<DispatchTaskInfo> taskInfos = null;
                if (dicTaskInfos.TryGetValue(curWorkOrder.Id, out taskInfos) && taskInfos.Any())
                {
                    SetResult(splitPlanResult, detailId, processTechOrderCode, "明细ID[{0}]对应的工单[{1}]存在生产中的任务单,不能拆分!"
                        .L10nFormat(detailId, curWorkOrder.No));
                    return curWorkOrder;
                }
            }

            if (curWorkOrder.State == Core.WorkOrders.WorkOrderState.Release)
            {
                List<DispatchTaskInfo> taskInfos = null;
                if (dicTaskInfos.TryGetValue(curWorkOrder.Id, out taskInfos) && taskInfos.Any(p => p.MergedStatus == 2))
                {
                    var nos = taskInfos.Where(p => p.MergedStatus == 2).Select(p => p.No).Distinct().Concat(";");

                    SetResult(splitPlanResult, detailId, processTechOrderCode, "明细ID[{0}]对应的工单[{1}]关联的任务单:[{2}]存在合并关系，请先在派工管理解除合并关系再执行,不能拆分!"
                        .L10nFormat(detailId, curWorkOrder.No, nos));
                }
            }

            return curWorkOrder;
        }

        /// <summary>
        /// 获取计划任务关联明细
        /// </summary>
        /// <param name="detailId">拆分计划明细Id</param>
        /// <returns></returns>
        public TaskUnionDetail GetTaskUnionDetail(string detailId)
        {
            return taskUnionDetails.FirstOrDefault(x => x.DetailId == detailId);
        }

        /// <summary>
        /// 获取工单
        /// </summary>
        /// <param name="curTaskDetail"></param>
        /// <returns></returns>
        public WorkOrder GetWorkOrder(TaskUnionDetail curTaskDetail)
        {
            return workOrders.FirstOrDefault(x => x.Id == curTaskDetail.WorkOrderId);
        }

        /// <summary>
        /// 设置结果
        /// </summary>
        /// <param name="splitPlanResult">拆分结果</param>
        /// <param name="detailId">明细Id</param>
        /// <param name="processTechOrderCode">制程单号</param>        
        /// <param name="msg">消息</param>
        /// <exception cref="ArgumentNullException"></exception>
        protected static void SetResult(SplitPlanResult splitPlanResult, string detailId, string processTechOrderCode, string msg)
        {
            if (splitPlanResult == null)
            {
                throw new ArgumentNullException(nameof(splitPlanResult));
            }

            splitPlanResult.Details.Add(TaskSplitHelper.CreateSplitDetailResult(detailId,
                processTechOrderCode, 0, 0, msg));

            TaskSplitHelper.SetSplitPlanMainResult(splitPlanResult, false, string.Empty);

        }
    }
}
