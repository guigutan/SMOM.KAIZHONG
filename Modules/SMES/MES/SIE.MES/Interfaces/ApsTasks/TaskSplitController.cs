using SIE.EventMessages.Release;
using System.Collections.Generic;

namespace SIE.MES.Interfaces.ApsTasks
{
    /// <summary>
    /// 拆分控制器类
    /// </summary>
    public class TaskSplitController : DomainController, IPlanTaskSplit
    {
        #region 拆分前接口        
        /// <summary>
        /// APS拆分前获取MES拆分在制数
        /// </summary>
        /// <param name="splitPlanDatas">拆分前数据列表</param>
        /// <returns>拆分前结果</returns>
        public virtual IReadOnlyList<SplitPlanResult> TaskSplit(IReadOnlyList<SplitPlanData> splitPlanDatas)
        {
            TaskSplitBeforeExecutor taskSplitExecutor = new TaskSplitBeforeExecutor(splitPlanDatas);

            return taskSplitExecutor.TaskSplit();
        }
        #endregion  拆分前接口

        #region 拆分接口
        /// <summary>
        /// APS拆分后将拆分结果回传给MES
        /// </summary>
        /// <param name="splitedPlanDatas">拆分后数据列表</param>
        /// <returns>拆分后结果</returns>
        public virtual IReadOnlyList<SplitPlanResult> TaskSplited(IReadOnlyList<SplitedPlanData> splitedPlanDatas)
        {
            TaskSplitExecutor taskSplitExecutor = new TaskSplitExecutor(splitedPlanDatas);
            return taskSplitExecutor.TaskSplited();
        }
        #endregion 拆分接口
    }
}
