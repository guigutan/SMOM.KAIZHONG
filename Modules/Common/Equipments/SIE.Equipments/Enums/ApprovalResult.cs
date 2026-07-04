using SIE.ObjectModel;

namespace SIE.Equipments.Enums
{
    /// <summary>
    /// 审核结果
    /// </summary>
    public enum ApprovalResult
    {
        /// <summary>
        /// 提交
        /// </summary>
        [Label("提交")]
        Submit = 10,
        /// <summary>
        /// 撤回
        /// </summary>
        [Label("撤回")]
        Retract = 20,
        /// <summary>
        /// 通过
        /// </summary>
        [Label("通过")]
        Pass = 30,
        /// <summary>
        /// 驳回
        /// </summary>
        [Label("驳回")]
        Reject = 40,
        /// <summary>
        /// 转办
        /// </summary>
        [Label("转办")]
        Transfer = 50,
        /// <summary>
        /// 自动跳过
        /// </summary>
        [Label("自动跳过")]
        Skip = 60,
    }
}
