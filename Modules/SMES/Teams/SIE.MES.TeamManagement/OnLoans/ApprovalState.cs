using SIE.ObjectModel;

namespace SIE.MES.TeamManagement.OnLoans
{
    /// <summary>
    /// 审批状态
    /// </summary>
    public enum ApprovalState
    {
        /// <summary>
        /// 发起
        /// </summary>
        [Label("发起")]
        Launch = 0,

        /// <summary>
        /// 同意
        /// </summary>
        [Label("同意")]
        Agree = 1,

        /// <summary>
        /// 拒绝
        /// </summary>
        [Label("拒绝")]
        Refuse = 2,

        /// <summary>
        /// 修改
        /// </summary>
        [Label("修改")]
        Update = 3,

        /// <summary>
        /// 撤销
        /// </summary>
        [Label("撤销")]
        Repeal = 4,

        /// <summary>
        /// 审核中
        /// </summary>
        [Label("审核中")]
        InReview = 5,

        /// <summary>
        /// 修改中
        /// </summary>
        [Label("修改中")]
        Updating = 6
    }
}
