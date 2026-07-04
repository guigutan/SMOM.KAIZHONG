using System.Collections.Generic;

namespace SIE.EventMessages.WorkFlows.WMS
{
    /// <summary>
    /// 库存调整工作流接口
    /// </summary>
    [Services.Service(FallbackType = typeof(DefaultIInvAdjustWorkFlow))]
    public interface IInvAdjustWorkFlow
    {
        /// <summary>
        /// 库存调整工作流
        /// </summary>        
        /// <param name="billIds">单Id</param>
        /// <param name="whCode">仓库</param>
        void InvAdjustWorkFlowExecute(List<double> billIds, string whCode);
    }

    /// <summary>
    /// 接口实现
    /// </summary>
    public class DefaultIInvAdjustWorkFlow : IInvAdjustWorkFlow
    {
        /// <summary>
        /// 库存调整工作流
        /// </summary>        
        /// <param name="billIds">单Id</param>
        /// <param name="whCode">仓库</param>
        public void InvAdjustWorkFlowExecute(List<double> billIds, string whCode)
        {        
            //
        }
    }
}
