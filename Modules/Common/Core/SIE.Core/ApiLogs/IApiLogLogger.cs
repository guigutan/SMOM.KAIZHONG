namespace SIE.Core.ApiLogs
{
    /// <summary>
    /// API日志记录器接口
    /// </summary>
    public interface IApiLogLogger
    {
        /// <summary>
        /// 释放时记录
        /// </summary>
        /// <param name="logReq"></param>
        void LogDispose(ApiRequestLog logReq);
    }
}
