using Newtonsoft.Json;
using SIE.Core.Logs;
using SIE.Domain;
using SIE.EventMessages.Release;
using SIE.MES.WorkOrders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.Interfaces.ApsTasks
{
    /// <summary>
    /// 计划任务拆分执行
    /// </summary>
    public class TaskSplitExecutor
    {
        /// <summary>
        /// 当前时间
        /// </summary>
        private readonly DateTime nowTime;

        /// <summary>
        ///  计划任务拆分数据列表
        /// </summary>
        private readonly IReadOnlyList<SplitedPlanData> splitedPlanDatas;

        /// <summary>
        /// 计划任务拆分执行构造函数
        /// </summary>
        public TaskSplitExecutor(IReadOnlyList<SplitedPlanData> _splitedPlanDatas)
        {
            nowTime = RF.Find<WorkOrder>().GetDbTime();

            this.splitedPlanDatas = _splitedPlanDatas;
        }

        /// <summary>
        /// APS拆分后将拆分结果回传给MES
        /// </summary>        
        /// <returns>拆分后结果</returns>
        public IReadOnlyList<SplitPlanResult> TaskSplited()
        {
            List<SplitPlanResult> splitPlanResults = new List<SplitPlanResult>();
            if (splitedPlanDatas == null)
            {
                return splitPlanResults;
            }

            SaveTaskSplitedLog(splitedPlanDatas);

            string successMsg = "拆分执行成功!".L10N();

            var planTaskIds = splitedPlanDatas.Select(x => x.PlanTaskId).Distinct().ToList();
            TaskSplitValidator taskSplitValidator = new TaskSplitValidator(planTaskIds);

            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                foreach (var curSplitedPlanData in splitedPlanDatas)
                {
                    var curSplitPlanResult = new SplitPlanResult(curSplitedPlanData.PlanTaskId);

                    try
                    {
                        taskSplitValidator.ValidataSplitedPlanData(curSplitedPlanData, curSplitPlanResult); //10.验证拆分数据合法性
                        if (!curSplitPlanResult.IsSuccess)
                        {
                            continue;
                        }

                        foreach (var curSplitedDetail in curSplitedPlanData.Details)
                        {
                            var curTaskDetail = taskSplitValidator.GetTaskUnionDetail(curSplitedDetail.DetailId);
                            WorkOrder curWorkOrder = taskSplitValidator.GetWorkOrder(curTaskDetail);

                            SplitUpdateWrorkOrder(curWorkOrder, curSplitPlanResult, curSplitedDetail);

                            if (!curSplitPlanResult.IsSuccess) //20. 修改被拆分工单的属性、生成工单日志
                            {
                                break;
                            }

                            curSplitPlanResult.Details.Add(TaskSplitHelper.CreateSplitDetailResult(curSplitedDetail.DetailId,
                            curSplitedDetail.ProcessTechOrderCode, 0, 0, successMsg)); //30. 创建成功的拆分结果明细
                        }

                        TaskSplitHelper.SetSplitPlanMainResult(curSplitPlanResult, true, successMsg);

                    }
                    catch (Exception exc)
                    {
                        var excMsg = "拆分执行异常: {0}".L10nFormat(exc.Message);
                        TaskSplitHelper.SetSplitPlanMainResult(curSplitPlanResult, false, excMsg);
                    }
                    finally
                    {
                        splitPlanResults.Add(curSplitPlanResult);
                    }

                }

                if (splitPlanResults.All(x => x.IsSuccess))
                {
                    tran.Complete();
                }
            }

            return splitPlanResults;
        }

        /// <summary>
        /// 保存APS拆分后将拆分结果回传给MES日志
        /// </summary>
        /// <param name="splitedPlanDatas">拆分后数据列表</param>
        private void SaveTaskSplitedLog(IReadOnlyList<SplitedPlanData> splitedPlanDatas)
        {
            using (var tran = DB.AutonomousTransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                var strValue = JsonConvert.SerializeObject(splitedPlanDatas);
                var inputValue = "拆分后数据列表:{0}".L10nFormat(strValue);
                var log = new InterfaceLog()
                {
                    Name = "IPlanTaskSplit",
                    Method = "TaskSplited",
                    ControllerName = "TaskSplitController",
                    InputValue = inputValue,
                };

                RF.Save(log);
                tran.Complete();
            }
        }

        /// <summary>
        /// 拆分时更新工单
        /// </summary>
        /// <param name="curWorkOrder">工单实体</param>
        /// <param name="splitPlanResult">拆分结果</param>
        /// <param name="splitedPlanDetail">拆分计划明细</param>        
        private void SplitUpdateWrorkOrder(WorkOrder curWorkOrder, SplitPlanResult splitPlanResult, SplitedPlanDetail splitedPlanDetail)
        {
            try
            {
                var planQty = (decimal)splitedPlanDetail.PlanAmount;

                if (planQty <= curWorkOrder.FinishQty + curWorkOrder.ScrapQty)
                {

                    DB.Update<WorkOrder>()
                        .Set(x => x.PlanQty, planQty)
                        .Set(x => x.State, SIE.Core.WorkOrders.WorkOrderState.Finish)
                        .Set(x => x.ActuFinishDate, nowTime)
                        .Where(x => x.Id == curWorkOrder.Id)
                        .Execute();

                    RT.Service.Resolve<WorkOrderController>().SaveWorkOrderLog(curWorkOrder.Id, WorkOrderLogType.Finish, "APS拆分完工", nowTime);
                    RT.EventBus.Publish(new WorkOrderFinishedEvent { WorkOrderId = curWorkOrder.Id });
                }
                else
                {
                    DB.Update<WorkOrder>()
                        .Set(x => x.PlanQty, planQty)
                        .Where(x => x.Id == curWorkOrder.Id)
                        .Execute();

                    RT.Service.Resolve<WorkOrderController>().SaveWorkOrderLog(curWorkOrder.Id, WorkOrderLogType.Split, "APS拆分", nowTime);
                }

                RT.EventBus.Publish(new WorkOrderEditEvent { WorkOrderId = curWorkOrder.Id });
            }
            catch (Exception exc)
            {
                var excMsg = "计划拆分保存工单失败: {0} ".L10nFormat(exc.Message);

                splitPlanResult.Details.Add(TaskSplitHelper.CreateSplitDetailResult(splitedPlanDetail.DetailId,
                    splitedPlanDetail.ProcessTechOrderCode, 0, 0, excMsg));
                TaskSplitHelper.SetSplitPlanMainResult(splitPlanResult, false, string.Empty);
            }
        }
    }
}
