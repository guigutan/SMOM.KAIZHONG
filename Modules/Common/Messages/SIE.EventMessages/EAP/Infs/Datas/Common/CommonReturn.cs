using Newtonsoft.Json;
using System;

namespace SIE.EventMessages.EAP.Infs.Datas.Common
{
    /// <summary>
    /// 通用响应结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CommonResult<T>
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public EquipEapError Error { get; set; } = new EquipEapError();

        /// <summary>
        /// 请求ID
        /// </summary>
        public double? RequestId { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public string Timestamp { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public T Data { get; set; } 
    }

    /// <summary>
    /// 通用返回
    /// </summary>
    public class CommonReturn
    {
        /// <summary>
        /// job任务Id
        /// </summary>
        [JsonProperty("jobGroupId")]
        public string JobGroupId { get; set; }

        /// <summary>
        /// 返回码具体描述
        /// </summary>
        [JsonProperty("msg")]
        public string Msg { get; set; }

        /// <summary>
        /// 返回码
        /// </summary>
        [JsonProperty("resultCode")]
        public string ResultCode { get; set; }
    }

    /// <summary>
    /// 数据对象
    /// </summary>
    [Serializable]
    public class EquipEapError
    {
        /// <summary>
        /// 信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 内部错误
        /// </summary>
        public object InnerError { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public int Code { get; set; }
    }
}
