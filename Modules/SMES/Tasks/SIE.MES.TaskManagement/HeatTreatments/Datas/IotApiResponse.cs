using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.HeatTreatments.Datas
{
    /// <summary>
    /// IOT返回输出结果
    /// </summary>
    [Serializable]
    public class IotApiResponse
    {
        /// <summary>
        /// 执行结果
        /// </summary>
        public bool? Data { get; set; }

        /// <summary>
        /// 请求ID
        /// </summary>
        public string RequestId { get; set; }

        /// <summary>
        /// 请求者
        /// </summary>
        public string Requester { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// 模块名称
        /// </summary>
        public string ModuleName { get; set; }

        /// <summary>
        /// 方法全名
        /// </summary>
        public string MethodFullName { get; set; }

        /// <summary>
        /// 方法名
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        public int? MsgType { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSucceed { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public DateTime? Timestamp { get; set; }

        /// <summary>
        /// 耗时（单位未明确，通常为毫秒）
        /// </summary>
        public decimal? Duration { get; set; }
    }
}
