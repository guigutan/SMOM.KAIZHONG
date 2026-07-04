using System.Collections.Generic;

namespace SIE.Equipments.WorkFlows
{
    /// <summary>
    /// 工作流接口
    /// </summary>
    public interface IWorkFlow
    {
        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="workFlowBillIds"></param>
        void Submit(List<double> workFlowBillIds);

        /// <summary>
        /// 撤回
        /// </summary>
        /// <param name="workFlowBillIds"></param>
        void Recall(List<double> workFlowBillIds);

        /// <summary>
        /// 批准
        /// </summary>
        /// <param name="workFlowBillIds"></param>
        void Approve(List<double> workFlowBillIds);

        /// <summary>
        /// 驳回
        /// </summary>
        /// <param name="workFlowBillIds"></param>
        void Reject(List<double> workFlowBillIds);
    }
}
