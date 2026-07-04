using SIE.ObjectModel;

namespace SIE.ERPInterface.Common.Enums
{
    /// <summary>
    /// 处理状态
    /// </summary>
    public enum ProcessState
    {
        /// <summary>
        /// 未处理
        /// </summary>
        [Label("未处理")]
        Unprocessed = 0,
        /// <summary>
        /// 已处理
        /// </summary>
        [Label("已处理")]
        Processed = 1,
        /// <summary>
        /// 失败
        /// </summary>
        [Label("失败")]
        Failed = 2,
        /// <summary>
        /// 重试
        /// </summary>
        [Label("重试")]
        Retry = 3,
        /// <summary>
        /// 处理中
        /// </summary>
        [Label("处理中")]
        Processing = 4,
        /// <summary>
        /// 放弃
        /// </summary>
        [Label("放弃")]
        Abandon = 5,
    }
}