using System;
using System.Collections.Generic;

namespace SIE.EventMessages.Release
{
    /// <summary>
    /// APS任务下达接口
    /// </summary>
    [Services.Service(FallbackType = typeof(EmptyPlanTaskSplit))]
    public interface IPlanTaskSplit
    {
        /// <summary>
        /// APS拆分前获取MES拆分在制数
        /// </summary>
        /// <param name="splitPlanDatas">拆分前数据列表</param>
        /// <returns>拆分前结果</returns>
        IReadOnlyList<SplitPlanResult> TaskSplit(IReadOnlyList<SplitPlanData> splitPlanDatas);

        /// <summary>
        /// APS拆分后将拆分结果回传给MES
        /// </summary>
        /// <param name="splitedPlanDatas">拆分后数据列表</param>
        /// <returns>拆分后结果</returns>
        IReadOnlyList<SplitPlanResult> TaskSplited(IReadOnlyList<SplitedPlanData> splitedPlanDatas);
    }
    class EmptyPlanTaskSplit : IPlanTaskSplit
    {
        public IReadOnlyList<SplitPlanResult> TaskSplited(IReadOnlyList<SplitedPlanData> splitedPlanDatas)
        {
            return new List<SplitPlanResult>();
        }

        public IReadOnlyList<SplitPlanResult> TaskSplit(IReadOnlyList<SplitPlanData> splitPlanDatas)
        {
            return new List<SplitPlanResult>();
        }
    }

    /// <summary>
    /// 拆分计划数据
    /// </summary>
    [Serializable]
    public class SplitPlanData
    {
        /// <summary>
        /// 计划任务ID
        /// </summary>
        public string PlanTaskId { get; set; }

        /// <summary>
        /// 明细
        /// </summary>
        public List<SplitPlanDetail> Details { get; } = new List<SplitPlanDetail>();
    }

    /// <summary>
    /// 拆分计划明细数据
    /// </summary>
    [Serializable]
    public class SplitPlanDetail
    {
        /// <summary>
        /// 明细ID
        /// </summary>
        public string DetailId { get; set; }

        /// <summary>
        /// 工艺单编号
        /// </summary>
        public string ProcessTechOrderCode { get; set; }
    }

    /// <summary>
    /// 拆分后计划数据
    /// </summary>
    [Serializable]
    public class SplitedPlanData
    {
        /// <summary>
        /// 计划任务ID
        /// </summary>
        public string PlanTaskId { get; set; }

        /// <summary>
        /// 拆分得到的新计划id列表
        /// </summary>
        public List<string> SplitPlanTaskIds { get; set; } = new List<string>();

        /// <summary>
        /// 明细
        /// </summary>
        public List<SplitedPlanDetail> Details { get; } = new List<SplitedPlanDetail>();
    }

    /// <summary>
    /// 拆分计划明细数据
    /// </summary>
    [Serializable]
    public class SplitedPlanDetail
    {
        /// <summary>
        /// 明细ID
        /// </summary>
        public string DetailId { get; set; }

        /// <summary>
        /// 工艺单编号
        /// </summary>
        public string ProcessTechOrderCode { get; set; }

        /// <summary>
        /// 拆分后的计划数量
        /// </summary>
        public double PlanAmount { get; set; }
    }

    /// <summary>
    /// 拆分结果
    /// </summary>
    [Serializable]
    public class SplitPlanResult
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SplitPlanResult(string planTaskId)
        {
            PlanTaskId = planTaskId;
            IsSuccess = true;
            Details = new List<SplitDetailResult>();
        }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 计划任务ID
        /// </summary>
        public string PlanTaskId { get; set; }

        /// <summary>
        /// 每个计划任务的拆分信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 明细
        /// </summary>
        public List<SplitDetailResult> Details { get; }
    }

    /// <summary>
    /// 拆分结果明细
    /// </summary>
    [Serializable]
    public class SplitDetailResult
    {
        /// <summary>
        /// 明细ID
        /// </summary>
        public string DetailId { get; set; }

        /// <summary>
        /// 工艺单编号
        /// </summary>
        public string ProcessTechOrderCode { get; set; }

        /// <summary>
        /// 每个计划任务的结果信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 在制数量
        /// </summary>
        public decimal OnlineAmount { get; set; }

        /// <summary>
        /// 完工数量
        /// </summary>
        public decimal FinishedAmount { get; set; }
    }
}
