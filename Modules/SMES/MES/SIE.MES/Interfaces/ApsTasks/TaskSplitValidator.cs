using SIE.EventMessages.Release;
using System;
using System.Collections.Generic;

namespace SIE.MES.Interfaces.ApsTasks
{
    /// <summary>
    /// 拆分执行数据验证
    /// </summary>
    public class TaskSplitValidator : TaskSplitBeforeValidator
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="planTaskIds">计划任务ID列表</param>
        public TaskSplitValidator(List<string> planTaskIds) : base(planTaskIds) { }

        /// <summary>
        /// 验证拆分计划数据
        /// </summary>
        /// <param name="splitedPlanData">拆分计划数据</param>
        /// <param name="splitPlanResult">拆分结果</param>
        /// <returns>计划任务明细集合</returns>
        public void ValidataSplitedPlanData(SplitedPlanData splitedPlanData, SplitPlanResult splitPlanResult)
        {
            if (splitedPlanData == null)
            {
                throw new ArgumentNullException(nameof(splitedPlanData));
            }

            if (splitPlanResult == null)
            {
                throw new ArgumentNullException(nameof(splitPlanResult));
            }

            //验证拆分数据
            ValidataSplitPlanDataMain(splitedPlanData.PlanTaskId, splitPlanResult);
            if (!splitPlanResult.IsSuccess)
            {
                return;
            }

            //验证明细
            ValidataSplitedDataDetails(splitedPlanData.Details, splitPlanResult);
        }


        /// <summary>
        /// 验证拆分明细数据集合
        /// </summary>
        /// <param name="splitedDetails">拆分计划明细集合</param>
        /// <param name="splitPlanResult">拆分结果</param>        
        private void ValidataSplitedDataDetails(List<SplitedPlanDetail> splitedDetails, SplitPlanResult splitPlanResult)
        {
            foreach (var detail in splitedDetails)
            {
                var detailId = detail.DetailId;
                var processTechOrderCode = detail.ProcessTechOrderCode;

                var workOrder = ValidataSplitPlanDataDetail(splitPlanResult, detailId, processTechOrderCode);
                if (!splitPlanResult.IsSuccess)
                {
                    return;
                }

                if ((decimal)detail.PlanAmount > workOrder.PlanQty
                    || (decimal)detail.PlanAmount < workOrder.OnlineQty)
                {
                    SetResult(splitPlanResult, detailId, processTechOrderCode,
                        "明细ID[{0}]对应的工单[{1}]的新计划数量必须<=原计划数量, 且新计划必须>=原上线数量!"
                        .L10nFormat(detail.DetailId, workOrder.No));
                }
            }
        }
    }
}
