using Newtonsoft.Json;

namespace SIE.EventMessages.WMS.StereoWarhouses.Datas
{
    /// <summary>
    /// 返回结果
    /// </summary>
    public class ReturnData
    {
        /// <summary>
        /// 返回结果 0:成功 1~N:失败
        /// </summary>
        [JsonProperty("code")]
        public string Code { get; set; }

        /// <summary>
        /// 结果描述
        /// 结果=0 时 返回“成功”，其他失败，则描述失败原因。
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }

        /// <summary>
        /// 请求编号
        /// </summary>
        [JsonProperty("reqCode")]
        public string ReqCode { get; set; }
    }
}
