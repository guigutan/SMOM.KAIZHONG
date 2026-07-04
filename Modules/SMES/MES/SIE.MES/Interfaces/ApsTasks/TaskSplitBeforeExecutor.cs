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
    /// 任务拆分前获取信息
    /// </summary>
    public class TaskSplitBeforeExecutor
    {
        /// <summary>
        /// 任务拆分接口信息
        /// </summary>
        private readonly IReadOnlyList<SplitPlanData> splitPlanDatas;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_splitPlanDatas"></param>
        public TaskSplitBeforeExecutor(IReadOnlyList<SplitPlanData> _splitPlanDatas)
        {
            splitPlanDatas = _splitPlanDatas;
        }

        /// <summary>
        /// APS拆分前获取MES拆分在制数
        /// </summary>        
        /// <returns>拆分前结果</returns>
        public IReadOnlyList<SplitPlanResult> TaskSplit()
        {
            List<SplitPlanResult> splitPlanResults = new List<SplitPlanResult>();
            if (splitPlanDatas == null)
            {
                return splitPlanResults;
            }

            SaveTaskSplitLog(splitPlanDatas);

            string successMsg = "拆分前执行成功!".L10N();

            var planTaskIds = splitPlanDatas.Select(x => x.PlanTaskId).Distinct().ToList();

            TaskSplitBeforeValidator taskSplitValidator = new TaskSplitBeforeValidator(planTaskIds);

            foreach (var curSplitPlan in splitPlanDatas)
            {
                var curSplitPlanResult = new SplitPlanResult(curSplitPlan.PlanTaskId);
                try
                {
                    taskSplitValidator.ValidataSplitPlanData(curSplitPlan, curSplitPlanResult); //10. 验证拆分数据合法性
                    if (!curSplitPlanResult.IsSuccess)
                    {
                        continue;
                    }

                    foreach (var detail in curSplitPlan.Details)
                    {
                        var curTaskDetail = taskSplitValidator.GetTaskUnionDetail(detail.DetailId);
                        WorkOrder curWorkOrder = taskSplitValidator.GetWorkOrder(curTaskDetail);

                        var onlineAmount = curWorkOrder.OnlineQty - curWorkOrder.FinishQty; //在制数包含报废数
                        var finishedAmount = curWorkOrder.FinishQty;

                        curSplitPlanResult.Details.Add(TaskSplitHelper.CreateSplitDetailResult(detail.DetailId, detail.ProcessTechOrderCode,
                            onlineAmount, finishedAmount, successMsg)); //20.创建成功的拆分结果明细信息                        
                    }

                    if (curSplitPlanResult.IsSuccess)
                    {
                        TaskSplitHelper.SetSplitPlanMainResult(curSplitPlanResult, true, successMsg);
                    }
                }
                catch (Exception exc)
                {
                    var excMsg = "拆分前执行异常: {0}".L10nFormat(exc.Message); //30.创建失败的结果信息
                    TaskSplitHelper.SetSplitPlanMainResult(curSplitPlanResult, false, excMsg);
                }
                finally
                {
                    splitPlanResults.Add(curSplitPlanResult);
                }
            }

            return splitPlanResults;
        }

        /// <summary>
        /// 保存APS拆分前获取MES拆分在制数日志
        /// </summary>
        /// <param name="splitPlanDatas">拆分前数据列表</param>
        private void SaveTaskSplitLog(IReadOnlyList<SplitPlanData> splitPlanDatas)
        {
            using (var tran = DB.AutonomousTransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                var strValue = JsonConvert.SerializeObject(splitPlanDatas);
                var inputValue = "拆分前数据列表:{0}".L10nFormat(strValue);
                var log = new InterfaceLog()
                {
                    Name = "IPlanTaskSplit",
                    Method = "TaskSplit",
                    ControllerName = "TaskSplitController",
                    InputValue = inputValue,
                };

                RF.Save(log);
                tran.Complete();
            }
        }
    }
}
