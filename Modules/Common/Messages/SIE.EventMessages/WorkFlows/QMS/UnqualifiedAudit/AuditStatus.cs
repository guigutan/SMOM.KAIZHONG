namespace SIE.EventMessages.WorkFlows.QMS.UnqualifiedAudit
{
    /// <summary>
    /// 审核状态
    /// </summary>
    public enum AuditStatus
    {
        /// <summary>
        /// 待发起
        /// </summary>
        ToGoUp = 0,

        /// <summary>
        /// 处理中
        /// </summary>
        Processing = 1,
        /// <summary>
        /// 审核中
        /// </summary>
        Checking = 2,

        /// <summary>
        /// 已结束
        /// </summary>
        Finish = 3
    }
}
