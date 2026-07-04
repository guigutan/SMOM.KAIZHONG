using SIE.ObjectModel;

namespace SIE.FMS
{
    /// <summary>
    /// 文件流审核状态
    /// </summary>
    public enum FileAuditState
    {
        /// <summary>
        /// 待审核
        /// </summary>
        [Label("待审核")]
        ToAuidt,
        /// <summary>
        /// 驳回
        /// </summary>
        [Label("驳回")]
        Reject,
        /// <summary>
        /// 通过
        /// </summary>
        [Label("通过")]
        Pass,
        /// <summary>
        /// 撤回
        /// </summary>
        [Label("撤回")]
        Cancel,
    }

    /// <summary>
    /// 文件流审核操作类型
    /// </summary>
    public enum OperationType
    {
        /// <summary>
        /// 待发布审核
        /// </summary>
        [Label("待发布审核")]
        ToPublish = 0,

        /// <summary>
        /// 待作废审核
        /// </summary>
        [Label("待作废审核")]
        ToScrap = 1
    }
}