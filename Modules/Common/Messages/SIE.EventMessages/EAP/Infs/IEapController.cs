using System;

namespace SIE.EventMessages.EAP.Infs
{
    /// <summary>
    /// 调用eap通用接口
    /// </summary>
    [Services.Service(FallbackType = typeof(DefaultEapController))]
    public interface IEapController
    {
        /// <summary>
        /// 推送EAP Common 方法（使用通用响应处理）
        /// </summary>
        /// <typeparam name="TRequest">请求参数类型</typeparam>
        /// <param name="apiMethod">调用的api方法</param>
        /// <param name="request">上传数据</param>
        /// <param name="direction">任务方向</param>
        /// <param name="desc">日志描述</param>
        /// <param name="isSuccessful">是否成功</param>
        string RequestEapCommon<TRequest>(string apiMethod, TRequest request
            , int direction, out bool isSuccessful, string desc = null);

        /// <summary>
        /// 推送EAP Common 方法
        /// </summary>
        /// <typeparam name="TRequest">请求参数类型</typeparam>
        /// <typeparam name="TResponse">返回结果类型</typeparam>
        /// <param name="apiMethod">调用的api方法</param>
        /// <param name="request">上传数据</param>
        /// <param name="direction">任务方向</param>
        /// <param name="desc">日志描述</param>
        /// <param name="isSuccessful">是否成功</param>
        /// <param name="responseHandle">响应结果处理</param>
        TResponse RequestEapCommon<TRequest, TResponse>(string apiMethod, TRequest request, Action<TRequest, TResponse> responseHandle,
            int direction, out bool isSuccessful, string desc);

        /// <summary>
        /// 保存EAP接口调用日志
        /// </summary>
        /// <param name="desc"></param>
        /// <param name="direction"></param>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="remark"></param>
        /// <param name="requestContent"></param>
        /// <param name="responseContent"></param>
        void SaveEAPInfLog(string desc, int direction, DateTime beginDate, DateTime endDate, string remark = null,
            string requestContent = null, string responseContent = null);

    }

    /// <summary>
    /// 接口默认实现类
    /// </summary>
    public class DefaultEapController : IEapController
    {
        /// <summary>
        /// 推送EAP Common 方法（使用通用响应处理）
        /// </summary>
        /// <typeparam name="TRequest">请求参数类型</typeparam>
        /// <param name="apiMethod">调用的api方法</param>
        /// <param name="request">上传数据</param>
        /// <param name="direction">任务方向</param>
        /// <param name="desc">日志描述</param>
        /// <param name="isSuccessful">是否成功</param>
        public string RequestEapCommon<TRequest>(string apiMethod, TRequest request, int direction,
            out bool isSuccessful, string desc = null)
        {
            isSuccessful = true;
            return string.Empty;
        }

        /// <summary>
        /// 推送EAP Common 方法
        /// </summary>
        /// <typeparam name="TRequest">请求参数类型</typeparam>
        /// <typeparam name="TResponse">返回结果类型</typeparam>
        /// <param name="apiMethod">调用的api方法</param>
        /// <param name="request">上传数据</param>
        /// <param name="direction">任务方向</param>
        /// <param name="desc">日志描述</param>
        /// <param name="isSuccessful">是否成功</param>
        /// <param name="responseHandle">响应结果处理</param>
        public TResponse RequestEapCommon<TRequest, TResponse>(string apiMethod, TRequest request,
            Action<TRequest, TResponse> responseHandle, int direction, out bool isSuccessful, string desc)
        {
            isSuccessful = true;
            return default(TResponse);
        }

        /// <summary>
        /// 保存EAP接口调用日志
        /// </summary>
        /// <param name="desc"></param>
        /// <param name="direction"></param>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="remark"></param>
        /// <param name="requestContent"></param>
        /// <param name="responseContent"></param>
        public void SaveEAPInfLog(string desc, int direction, DateTime beginDate, DateTime endDate, string remark = null, string requestContent = null, string responseContent = null)
        {
            // 保存EAP接口调用日志
        }
    }
}
