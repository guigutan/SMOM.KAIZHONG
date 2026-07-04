using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SIE.ERPInterface.Common.Datas
{
    /// <summary>
    /// Ebs接口处理结果
    /// </summary>
    [Serializable]
    public class EbsResult<T>
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
        public string XV_RET_STATUS { get; set; }

        /// <summary>
        /// 返回分组Id
        /// </summary>
        public string XV_GROUP_ID { get; set; }

        /// <summary>
        /// 总数
        /// </summary>
        public int XV_RECORD_TOTAL { get; set; }

        /// <summary>
        /// 返回状态说明
        /// </summary>
        public string XV_RET_MESSAGE { get; set; }

        /// <summary>
        /// 响应数据(json)
        /// </summary>        

        public List<T> XV_RESULT { get; set; } = new List<T>();
    }

    public class OutputParametersBase
    {
        public string XV_RET_STATUS { get; set; }

        public object XV_RET_MESSAGE { get; set; }
    }

    public class OutputParameters : OutputParametersBase
    {
        [JsonProperty("@xmlns:xsi")]
        public string XsiNamespace { get; set; }
        [JsonProperty("@xmlns")]
        public string Namespace { get; set; }
        public string XV_GROUP_ID { get; set; }
        public int XV_RECORD_TOTAL { get; set; }
        public string XV_RESULT { get; set; }
    }

    public class EbsResponse
    {
        public OutputParameters OutputParameters { get; set; }
    }

    public class EbsResponseCheck
    {
        public OutputParametersBase OutputParameters { get; set; }
    }

    #region 上传结果回传

    [Serializable]
    public class EbsUploadResponse
    {
        public UploadOutputParameters OutputParameters { get; set; }
    }

    [Serializable]
    public class UploadOutputParameters
    {
        [JsonProperty("@xmlns:xsi")]
        public string XsiNamespace { get; set; }
        [JsonProperty("@xmlns")]
        public string Namespace { get; set; }
        public string RETURN_DATA { get; set; }
        public string X_MSG_DATA { get; set; }
        public string X_RETURN_STATUS { get; set; }
    }

    [Serializable]
    public class EbsReturnData
    {
        /// <summary>
        /// 返回行处理状态，COMPLETED成功ERROR失败
        /// </summary>
        public string PROCESS_STATUS { get; set; }
        /// <summary>
        /// 返回信息
        /// </summary>
        public string PROCESS_MESSAGE { get; set; }
        /// <summary>
        /// 来源单号
        /// </summary>
        public string SCUX_SOURCE_NUM { get; set; }
        /// <summary>
        /// 来源行号
        /// </summary>
        public string SCUX_SOURCE_LINE_NUM { get; set; }
        /// <summary>
        /// 事务交易Id
        /// </summary>
        public string SCUX_SOURCE_LOT_NUM { get; set; }
    }
    #endregion
}
