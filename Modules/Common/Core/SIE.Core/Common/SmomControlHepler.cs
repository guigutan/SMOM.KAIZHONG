using Newtonsoft.Json;
using SIE.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Core.Common
{
    public static class SmomControlHepler
    {
        /// <summary>
        /// 调用SMOM
        /// </summary>
        /// <param name="apiType"></param>
        /// <param name="method"></param>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public static T SmomPost<T>(string apiType, string method, string url, params SmomParam[] parameters)
        {
            try
            {
                SmomRequestData requestData = new SmomRequestData();
                requestData.ApiType = apiType;
                requestData.Method = method;
                requestData.Context = new SmomContext();
                requestData.Parameters = parameters;
                var requestJson = JsonConvert.SerializeObject(requestData);
                var responseJson = HttpClientHelper.HttpPost(url, requestJson);
                var segpResponseData = JsonConvert.DeserializeObject<SegpResponseData<T>>(responseJson);
                if (!segpResponseData.Success)
                    throw new ValidationException(segpResponseData.Message);
                return segpResponseData.Result;
            }
            catch (Exception ex)
            {
                throw new ValidationException("工厂API调用异常，异常原因：{0}".L10nFormat(ex.Message));
            }

        }
    }

    /// <summary>
    /// SMOM请求实体
    /// </summary>
    public class SmomRequestData
    {
        /// <summary>
        /// 控制器名称
        /// </summary>
        public string ApiType { get; set; }

        /// <summary>
        /// 参数
        /// </summary>
        public object Parameters { get; set; }

        /// <summary>
        /// 方法名称
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// 上下文参数
        /// </summary>
        public SmomContext Context { get; set; }
    }

    [Serializable]
    public class SmomParam
    {
        /// <summary>
        /// 值
        /// </summary>
        public object Value { get; set; }
    }

    /// <summary>
    /// 上下文参数
    /// </summary>
    [Serializable]
    public class SmomContext
    {
        /// <summary>
        /// 票据
        /// </summary>
        public string Ticket { get; set; }

        /// <summary>
        /// 库存组织
        /// </summary>
        public int? InvOrgId { get; set; } = 1;
    }

    /// <summary>
    /// SMOM请求返回数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class SegpResponseData<T>
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 异常消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 返回结果
        /// </summary>
        public T Result { get; set; }

        public SmomContext Context { get; set; }
    }
}
