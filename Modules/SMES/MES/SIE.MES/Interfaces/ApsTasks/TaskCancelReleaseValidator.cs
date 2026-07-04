using Microsoft.Scripting.Utils;
using SIE.Domain;
using SIE.EventMessages.Release;
using SIE.MES.WorkOrders;
using SIE.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SIE.MES.Interfaces.ApsTasks
{
    /// <summary>
    /// APS计划取消下达数据验证逻辑
    /// </summary>
    public class TaskCancelReleaseValidator
    {
        /// <summary>
        /// 计划任务关联
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
        /// 工单列表
        /// </summary>
        private readonly EntityList<WorkOrder> workOrders;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="cancelReleasePlanDatas"></param>
        public TaskCancelReleaseValidator(IReadOnlyList<CancelReleasePlanData> cancelReleasePlanDatas)
        {
            //获取计划任务关联工单
            var planTaskIds = cancelReleasePlanDatas.Select(x => x.PlanTaskId).Distinct().ToList();
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
        /// 验证取消下达数据合法性
        /// </summary>
        /// <param name="cancelReleaseData">取消下达数据</param>
        /// <param name="releasePlanResult">取消下达结果</param>
        /// <returns>计划任务关联实体</returns>
        public TaskUnion ValidataCancelReleasePlanData(CancelReleasePlanData cancelReleaseData, ReleasePlanResult releasePlanResult)
        {
            if (cancelReleaseData == null)
            {
                throw new ArgumentNullException(nameof(cancelReleaseData));
            }

            StringBuilder cursbMsg = new StringBuilder();
            var planTaskId = cancelReleaseData.PlanTaskId;
            if (planTaskId.IsNullOrWhiteSpace())
            {
                cursbMsg.Append("计划任务ID不能为空".L10N());
            }

            if (cursbMsg.Length > 0)
            {
                TaskReleaseHelper.SetReleasePlanMainResult(releasePlanResult, false, cursbMsg.ToString());
                return null;
            }

            TaskUnion curTaskUnion = taskUnions.FirstOrDefault(x => x.PlanTaskId == planTaskId);
            if (curTaskUnion == null)
            {
                cursbMsg.Append("根据计划任务ID[{0}]未找到计划任务关联实体!".L10nFormat(planTaskId));
            }
            else
            {
                var curDetails = taskUnionDetails.Where(x => x.TaskUnionId == curTaskUnion.Id);
                if (!curDetails.Any())
                {
                    cursbMsg.Append("根据计划任务ID[{0}]未找到计划任务明细!".L10nFormat(planTaskId));
                }
                else
                {
                    ValidataCancelReleaseDetails(curDetails, releasePlanResult);
                }
            }

            if (cursbMsg.Length > 0)
            {
                TaskReleaseHelper.SetReleasePlanMainResult(releasePlanResult, false, cursbMsg.ToString());
            }

            return curTaskUnion;
        }


        /// <summary>
        /// 验证取消下达明细数据合法性
        /// </summary>
        /// <param name="taskUnionDetails">计划任务明细</param>
        /// <param name="releasePlanResult">下达结果</param>        
        private void ValidataCancelReleaseDetails(IEnumerable<TaskUnionDetail> taskUnionDetails,
            ReleasePlanResult releasePlanResult)
        {
            StringBuilder cursbMsg = new StringBuilder();

            foreach (var curDetail in taskUnionDetails)
            {
                cursbMsg.Clear();

                var workOrder = GetWorkOrder(curDetail);
                if (workOrder == null)
                {
                    cursbMsg.Append("明细ID[{0}]对应的工单实体不存在!".L10nFormat(curDetail.DetailId));
                }
                else
                {
                    if (workOrder.State != Core.WorkOrders.WorkOrderState.Release)
                    {
                        cursbMsg.Append("明细ID[{0}]对应的工单状态[{1}]不是发放, 不允许进行取消下达!"
                            .L10nFormat(curDetail.DetailId, EnumViewModel.EnumToLabel(workOrder.State).L10N()));
                    }
                    else
                    {
                        if (workOrder.OnlineQty > 0)
                        {
                            cursbMsg.Append("明细ID[{0}]对应的工单已在生产, 不允许进行取消下达!"
                                .L10nFormat(curDetail.DetailId));
                        }
                    }


                    //电子套件扩展，检查工单上线状态，后续可以改成套件直接继承这个类重写逻辑
                    string msg = RT.Service.Resolve<TaskReleaseExtensionController>()
                        .CheckWorkOrderUplineState(workOrder, curDetail.DetailId);

                    if (!string.IsNullOrEmpty(msg))
                    {
                        cursbMsg.Append(msg);
                    }

                    if (dicTaskInfos.TryGetValue(workOrder.Id, out List<DispatchTaskInfo> disTaskInfos) && disTaskInfos.Any())
                    {
                        if (disTaskInfos.Any(p => p.MergedStatus == 2))
                        {
                            var nos = disTaskInfos.Where(p => p.MergedStatus == 2).Select(p => p.No).Distinct().Concat(";");
                            cursbMsg.Append("明细ID[{0}]对应的工单[{1}]关联的任务单:[{2}]存在合并关系，请先在派工管理解除合并关系再执行,不允许进行取消下达!"
                                .L10nFormat(curDetail.DetailId, workOrder.No, nos));
                        }
                        else
                        {
                            //删除任务单
                        }
                    }
                }

                if (cursbMsg.Length > 0)
                {
                    var workOrderNo = string.Empty;
                    if (workOrder != null)
                    {
                        workOrderNo = workOrder.No;
                    }

                    releasePlanResult.Details.Add(TaskReleaseHelper.CreateReleaseDetailResult(curDetail.DetailId,
                        curDetail.ProcessTechOrderCode, cursbMsg.ToString(), workOrderNo));

                    TaskReleaseHelper.SetReleasePlanMainResult(releasePlanResult, false, string.Empty);
                }
            }
        }

        /// <summary>
        /// 获取工单
        /// </summary>
        /// <param name="curDetail"></param>
        /// <returns></returns>
        public WorkOrder GetWorkOrder(TaskUnionDetail curDetail)
        {
            return workOrders.FirstOrDefault(x => x.Id == curDetail.WorkOrderId);
        }

        /// <summary>
        /// 获取计划关联明细
        /// </summary>
        /// <param name="taksUnionId">计划关联ID</param>
        /// <returns></returns>
        public List<TaskUnionDetail> GetTaskUnionDetails(double taksUnionId)
        {
            return taskUnionDetails.Where(x => x.TaskUnionId == taksUnionId).ToList();
        }

    }
}
