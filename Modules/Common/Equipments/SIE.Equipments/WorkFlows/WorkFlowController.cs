using System.Collections.Generic;

namespace SIE.Equipments.WorkFlows
{
    /// <summary>
    /// 工作流控制器
    /// </summary>
    public partial class WorkFlowController : DomainController, IWorkFlow
    {
        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="workFlowBillIds">流程表单ID</param>
        public virtual void Submit(List<double> workFlowBillIds)
        {
            //TODO调用外部流程引擎的提交功能
        }

        /// <summary>
        /// 撤回
        /// </summary>
        /// <param name="workFlowBillIds">流程表单ID</param>
        public virtual void Recall(List<double> workFlowBillIds)
        {
            //TODO调用外部流程引擎的撤回功能
        }

        /// <summary>
        /// 批准
        /// </summary>
        /// <param name="workFlowBillIds">流程表单ID</param>
        public virtual void Approve(List<double> workFlowBillIds)
        {
            //TODO调用外部流程引擎的批准功能
        }

        /// <summary>
        /// 驳回
        /// </summary>
        /// <param name="workFlowBillIds">流程表单ID</param>
        public virtual void Reject(List<double> workFlowBillIds)
        {
            //TODO调用外部流程引擎的驳回功能
        }
    }
}
