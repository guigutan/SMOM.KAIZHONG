using SIE.ObjectModel;

namespace SIE.Equipments.Enums
{
    /// <summary>
    /// 审核状态
    /// </summary>
    public enum ApprovalStatus
    {
        /// <summary>
        /// 待提交
        /// </summary>
        [Label("待提交")]
        Draft = 10,
        /// <summary>
        /// 待审核
        /// </summary>
        [Label("待审核")]
        PendingReview = 20,
        /// <summary>
        /// 审核中
        /// </summary>
        [Label("审核中")]
        UnderReview = 30,
        /// <summary>
        /// 已审批
        /// </summary>
        [Label("已审批")]
        Audited = 40,
        /// <summary>
        /// 驳回
        /// </summary>
        [Label("驳回")]
        Reject = 50,
    }
}
