using SIE.ObjectModel;

namespace SIE.ERPInterface.Common.Enums
{
    /// <summary>
    /// 操作类型
    /// </summary>
    public enum OperationType
    {
        /// <summary>
        /// 调度
        /// </summary>
        [Label("调度")]
        Scheduling = 0,

        /// <summary>
        /// 手动
        /// </summary>
        [Label("手动")]
        Manual = 1,

        /// <summary>
        /// API
        /// </summary>
        [Label("API")]
        API = 2,
    }
}