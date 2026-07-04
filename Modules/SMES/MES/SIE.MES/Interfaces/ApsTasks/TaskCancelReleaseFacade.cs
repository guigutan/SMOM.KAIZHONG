using SIE.Domain;
using SIE.EventMessages.Release;
using SIE.MES.WorkOrders;
using System;
using System.Collections.Generic;

namespace SIE.MES.Interfaces.ApsTasks
{
    /// <summary>
    /// APS工单取消下达逻辑
    /// </summary>
    public class TaskCancelReleaseFacade
    {
        /// <summary>
        /// 需取消下达的数据
        /// </summary>
        private readonly IReadOnlyList<CancelReleasePlanData> cancelReleasePlanDatas;

        /// <summary>
        /// 取消下达数据验证
        /// </summary>
        private readonly TaskCancelReleaseValidator taskCancelReleaseValidator;

        /// <summary>
        /// 当前时间
        /// </summary>
        private readonly DateTime nowTime;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_cancelReleasePlanDatas"></param>
        public TaskCancelReleaseFacade(IReadOnlyList<CancelReleasePlanData> _cancelReleasePlanDatas)
        {
            this.cancelReleasePlanDatas = _cancelReleasePlanDatas;
            taskCancelReleaseValidator = new TaskCancelReleaseValidator(_cancelReleasePlanDatas);

            nowTime = RF.Find<WorkOrder>().GetDbTime();
        }


        /// <summary>
        /// 计划任务取消下达
        /// </summary>
        /// <returns>取消下达结果集合</returns>
        public List<ReleasePlanResult> TaskCancelReleasePlanResult()
        {
            List<ReleasePlanResult> releasePlanResults = new List<ReleasePlanResult>();
            string successMsg = "取消下达成功!".L10N();

            foreach (var curCancelReleaseData in cancelReleasePlanDatas)
            {
                var curReleasePlanResult = new ReleasePlanResult(curCancelReleaseData.PlanTaskId);

                try
                {
                    var curTaskUnion = taskCancelReleaseValidator
                        .ValidataCancelReleasePlanData(curCancelReleaseData, curReleasePlanResult);

                    if (!curReleasePlanResult.IsSuccess)
                    {
                        continue;
                    }

                    curTaskUnion.PersistenceStatus = PersistenceStatus.Deleted;

                    if (!curReleasePlanResult.IsSuccess)
                    {
                        continue;
                    }

                    var curTaskUnionDetails = taskCancelReleaseValidator.GetTaskUnionDetails(curTaskUnion.Id);

                    foreach (var curDetail in curTaskUnionDetails)
                    {
                        var workOrder = taskCancelReleaseValidator.GetWorkOrder(curDetail);

                        curDetail.PersistenceStatus = PersistenceStatus.Deleted;
                        if (!curReleasePlanResult.IsSuccess)
                        {
                            break;
                        }

                        CancelReleaseWrorkOrder(curDetail, curReleasePlanResult, workOrder); //30. 工单属性修改--状态"计划取消"

                        if (!curReleasePlanResult.IsSuccess)
                        {
                            break;
                        }

                        curReleasePlanResult.Details.Add(TaskReleaseHelper.CreateReleaseDetailResult(curDetail.DetailId,
                            curDetail.ProcessTechOrderCode, successMsg, workOrder.No)); ////40. 创建成功的取消下达结果明细信息
                    }

                    if (curReleasePlanResult.IsSuccess)
                    {
                        //20. 计划任务明细实体--删除 
                        foreach (var curTaskUnionDetail in curTaskUnionDetails)
                        {
                            DB.Delete<TaskUnionDetail>()
                               .Where(x => x.Id == curTaskUnionDetail.Id)
                               .Execute();
                        }

                        //10. 计划任务关联实体--删除 
                        DB.Delete<TaskUnion>()
                            .Where(x => x.Id == curTaskUnion.Id)
                            .Execute();

                        TaskReleaseHelper.SetReleasePlanMainResult(curReleasePlanResult, true, successMsg);
                    }
                }
                catch (Exception exc)
                {
                    var excMsg = "取消下达异常: {0}".L10nFormat(exc.Message); //创建失败的取消下达结果
                    TaskReleaseHelper.SetReleasePlanMainResult(curReleasePlanResult, false, excMsg);
                }
                finally
                {
                    releasePlanResults.Add(curReleasePlanResult);
                }
            }

            return releasePlanResults;
        }

        /// <summary>
        /// 取消下达更新工单
        /// </summary>
        /// <param name="taskUnionDetail">计划任务明细</param>
        /// <param name="releasePlanResult">取消下达结果</param>
        /// <param name="curWorkOrder">工单</param>        
        private void CancelReleaseWrorkOrder(TaskUnionDetail taskUnionDetail, ReleasePlanResult releasePlanResult, WorkOrder curWorkOrder)
        {
            try
            {
                DB.Update<WorkOrder>()
                    .Set(x => x.State, Core.WorkOrders.WorkOrderState.CancelRelease)
                    .Where(x => x.Id == curWorkOrder.Id)
                    .Execute();

                RT.Service.Resolve<WorkOrderController>()
                    .SaveWorkOrderLog(curWorkOrder.Id, WorkOrderLogType.CancelRelease, "APS取消发放", nowTime);

                RT.EventBus.Publish(new DispatchTaskDeleteEvent() { WorkOrderId = curWorkOrder.Id });
            }
            catch (Exception exc)
            {
                var excMsg = "取消下达保存工单异常: {0} ".L10nFormat(exc.Message);

                releasePlanResult.Details.Add(TaskReleaseHelper.CreateReleaseDetailResult(taskUnionDetail.DetailId,
                    taskUnionDetail.ProcessTechOrderCode, excMsg, taskUnionDetail.WorkOrder.No));
                TaskReleaseHelper.SetReleasePlanMainResult(releasePlanResult, false, string.Empty);
            }
        }
    }
}
