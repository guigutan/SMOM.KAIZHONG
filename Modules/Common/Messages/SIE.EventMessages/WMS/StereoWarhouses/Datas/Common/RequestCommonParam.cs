using Newtonsoft.Json;

namespace SIE.EventMessages.WMS.StereoWarhouses.Datas
{
    /// <summary>
    /// 请求通用参数
    /// </summary>
    public class RequestCommonParam
    {
        /// <summary>
        /// 请求编号
        /// </summary>
        [JsonProperty("reqCode")]
        public string ReqCode { get; set; }

        /// <summary>
        /// 请求时间 （ yyyy-mm-dd hh:mm:ss）
        /// </summary>
        [JsonProperty("reqTime")]
        public string ReqTime { get; set; }

        /// <summary>
        /// 任务编号
        /// </summary>
        [JsonProperty("taskCode")]
        public string TaskCode { get; set; }
    }
}
