using System;

namespace SIE.EventMessages.WMS.StereoWarhouses
{
    /// <summary>
    /// 调用立库通用接口
    /// </summary>
    [Services.Service(FallbackType = typeof(DefaultWCSController))]
    public interface IWcsController
    {
        /// <summary>
        /// 推送立库 Common 方法（使用通用响应处理）
        /// </summary>
        /// <typeparam name="TRequest">请求参数类型</typeparam>
        /// <param name="apiMethod">调用的api方法</param>
        /// <param name="request">上传数据</param>
        /// <param name="direction">任务方向</param>
        /// <param name="mode">任务模式</param>
        /// <param name="operationType">操作类型</param>
        /// <param name="desc">日志描述</param>
        /// <param name="isSuccessful">是否成功</param>
        string RequestWcsCommon<TRequest>(string apiMethod, TRequest request
            , int direction, out bool isSuccessful, string desc = null);

        /// <summary>
        /// 推送立库 Common 方法
        /// </summary>
        /// <typeparam name="TRequest">请求参数类型</typeparam>
        /// <typeparam name="TResponse">返回结果类型</typeparam>
        /// <param name="apiMethod">调用的api方法</param>
        /// <param name="request">上传数据</param>
        /// <param name="direction">任务方向</param>
        /// <param name="mode">任务模式</param>
        /// <param name="operationType">操作类型</param>
        /// <param name="desc">日志描述</param>
        /// <param name="isSuccessful">是否成功</param>
        /// <param name="responseHandle">响应结果处理</param>
        string RequestWcsCommon<TRequest, TResponse>(string apiMethod, TRequest request, Action<TRequest, TResponse> responseHandle
            , int direction, out bool isSuccessful, string desc = null);
    }

    /// <summary>
    /// 接口默认实现类
    /// </summary>
    public class DefaultWCSController : IWcsController
    {
        /// <summary>
        /// 推送立库 Common 方法（使用通用响应处理）
        /// </summary>
        /// <typeparam name="TRequest">请求参数类型</typeparam>
        /// <param name="apiMethod">调用的api方法</param>
        /// <param name="request">上传数据</param>
        /// <param name="direction">任务方向</param>
        /// <param name="isSuccessful">是否成功</param>
        /// <param name="desc">日志描述</param>
        public string RequestWcsCommon<TRequest>(string apiMethod, TRequest request
            , int direction, out bool isSuccessful, string desc = null)
        {
            isSuccessful = false;
            return string.Empty;
        }

        /// <summary>
        /// 推送立库 Common 方法
        /// </summary>
        /// <typeparam name="TRequest">请求参数类型</typeparam>
        /// <typeparam name="TResponse">返回结果类型</typeparam>
        /// <param name="apiMethod">调用的api方法</param>
        /// <param name="request">上传数据</param>
        /// <param name="direction">任务方向</param>
        /// <param name="isSuccessful">是否成功</param>
        /// <param name="desc">日志描述</param>
        /// <param name="responseHandle">响应结果处理</param>
        public string RequestWcsCommon<TRequest, TResponse>(string apiMethod, TRequest request, Action<TRequest, TResponse> responseHandle
            , int direction, out bool isSuccessful, string desc = null)
        {
            isSuccessful = false;
            return string.Empty;
        }
    }
}
