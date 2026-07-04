using SIE.EventMessages.Release;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.Interfaces.ApsTasks
{
    /// <summary>
    /// 计划任务拆分接口工具类
    /// </summary>
    public static class TaskSplitHelper
    {
        /// <summary>
        /// 创建拆分结果明细实体
        /// </summary>
        /// <param name="detailId">明细Id</param>
        /// <param name="processTechOrderCode">工艺单编号</param>
        /// <param name="onlineAmount">工单在制数量</param>
        /// <param name="finishedAmount">工单完工数量</param>
        /// <param name="message">结果消息</param>
        /// <returns>拆分结果明细实体</returns>
        public static SplitDetailResult CreateSplitDetailResult(string detailId, string processTechOrderCode,
            decimal onlineAmount, decimal finishedAmount, string message)
        {
            var curSplitDetailResult = new SplitDetailResult();
            curSplitDetailResult.DetailId = detailId;
            curSplitDetailResult.ProcessTechOrderCode = processTechOrderCode;
            curSplitDetailResult.Message = message;
            curSplitDetailResult.OnlineAmount = onlineAmount;
            curSplitDetailResult.FinishedAmount = finishedAmount;
            return curSplitDetailResult;
        }



        /// <summary>
        /// 设置拆分结果属性值
        /// </summary>
        /// <param name="splitPlanResult">拆分结果</param>
        /// <param name="resultFlg">拆分结果布尔值</param>
        /// <param name="message">拆分结果消息</param>
        /// <param name="planTaskId">计划任务ID</param>
        public static void SetSplitPlanMainResult(SplitPlanResult splitPlanResult, bool resultFlg, string message)
        {
            if (splitPlanResult == null)
            {
                throw new ArgumentNullException(nameof(splitPlanResult));
            }

            splitPlanResult.IsSuccess = resultFlg;
            if (!message.IsNullOrWhiteSpace())
            {
                splitPlanResult.Message += message;
            }
        }

    }
}
