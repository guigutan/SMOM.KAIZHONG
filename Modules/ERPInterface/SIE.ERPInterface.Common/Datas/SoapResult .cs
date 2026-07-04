using System;

namespace SIE.ERPInterface.Common.Datas
{
    /// <summary>
    /// SOAP接口处理结果
    /// </summary>
    [Serializable]
    public class SoapResult
    {
        /// <summary>
        /// 请求报文
        /// </summary>
        public string RequestStr { get; set; }

        /// <summary>
        /// 接收报文
        /// </summary>
        public string ResponseStr { get; set; }

        /// <summary>
        /// 请求时间
        /// </summary>
        public DateTime RequestDate { get; set; }

        /// <summary>
        /// 接收时间
        /// </summary>
        public DateTime ResponseDate { get; set; }

        /// <summary>
        /// 返回状态(E 错误，S 成功)
        /// </summary>
        public string X_RESPONSE_STATUS { get; set; }

        /// <summary>
        /// 返回错误码
        /// </summary>
        public string X_RESPONSE_CODE { get; set; }

        /// <summary>
        /// 返回状态说明
        /// </summary>
        public string X_RESPONSE_MESSAGE { get; set; }

        /// <summary>
        /// 响应数据(XML)
        /// </summary>
        public string X_RESPONSE_DATA { get; set; }
    }
}
