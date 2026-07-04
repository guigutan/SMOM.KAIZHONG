using SIE.ObjectModel;

namespace SIE.ERPInterface.Common.Enums
{
    /// <summary>
    /// 请求状态
    /// </summary>
    public enum RequestState
    {
        /// <summary>
        /// 成功
        /// </summary>
        [Label("成功")]
        Success = 0,
        /// <summary>
        /// 失败
        /// </summary>
        [Label("失败")]
        Failed = 1,
        /// <summary>
        /// 异常
        /// </summary>
        [Label("异常")]
        Abnormal = 2,
    }
}