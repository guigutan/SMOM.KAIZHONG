using SIE.Equipments.Enums;

namespace SIE.Equipments.WorkFlows
{
    /// <summary>
    /// 流程表单
    /// </summary>
    public interface IWorkFlowBill
    {
        /// <summary>
        /// 流程表单ID
        /// </summary>
        string WorkFlowId { get; set; }

        /// <summary>
        /// 审核状态
        /// </summary>
        ApprovalStatus approvalStatus { get; set; }
    }
}
