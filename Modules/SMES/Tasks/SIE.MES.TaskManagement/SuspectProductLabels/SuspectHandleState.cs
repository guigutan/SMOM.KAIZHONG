using SIE.ObjectModel;

namespace SIE.MES.TaskManagement.SuspectProductLabels
{
    /// <summary>
    /// 可疑品处理状态
    /// </summary>
    public enum SuspectHandleState
    {
        /// <summary>
        /// 待处理
        /// </summary>
        [Label("待处理")]
        Pending,
        /// <summary>
        /// 已处理
        /// </summary>
        [Label("已处理")]
        Processed,
    }
}
