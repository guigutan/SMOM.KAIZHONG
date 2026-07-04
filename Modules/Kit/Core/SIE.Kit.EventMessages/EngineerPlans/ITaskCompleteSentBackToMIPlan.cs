using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Kit.EventMessages.EngineerPlans
{
    /// <summary>
    /// MI任务完成事件接口
    /// </summary>
    [Services.Service(FallbackType = typeof(DefalutITaskCompleteSentBackToMIPlan))]
    public interface ITaskCompleteSentBackToMIPlan
    {
        /// <summary>
        /// 回写 工程计划MI 状态
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        void SentBackToMIPlan(List<double> SaleOrderDetailIds);
    }

    /// <summary>
    /// 接口实现
    /// </summary>
    public class DefalutITaskCompleteSentBackToMIPlan : ITaskCompleteSentBackToMIPlan
    {
        public void SentBackToMIPlan(List<double> SaleOrderDetailIds)
        {
            return;
        }
    }
}
