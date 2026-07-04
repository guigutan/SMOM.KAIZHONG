using SIE.Core.Common.Service;
using SIE.WorkFlow.Base.Common.Interfaces;

namespace SIE.EMS.WorkFlow
{
    /// <summary>
    /// 通用工作流服务
    /// </summary>
    public class WorkflowService : DomainService
    {

        /// <summary>
        /// 工作流接口
        /// </summary>
        private readonly ISieWorkflow _sieWorkflow;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sieWorkflow">工作流接口</param>
        public WorkflowService(ISieWorkflow sieWorkflow)
        {
            _sieWorkflow = sieWorkflow;
        }

        /// <summary>
        /// 撤销工作流
        /// </summary>
        /// <param name="flowInstanceId">实例ID</param>
        /// <param name="reason">撤销原因</param>
        public virtual void CancelWorkflow(double flowInstanceId, string reason)
        {
            _sieWorkflow.CancelWorkflow(flowInstanceId, reason);
        }
    }
}
