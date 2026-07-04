using Newtonsoft.Json;

namespace SIE.EventMessages.EAP.Infs.Datas
{
    public class ReturnTaskParam
    {
        /// <summary>
        /// 任务编号
        /// </summary>
        [JsonProperty("jobGroupId")]
        public string JobGroupId { get; set; }

        /// <summary>
        /// 载具编码
        /// </summary>
        [JsonProperty("carriedCode")]
        public string CarriedCode { get; set; }

        /// <summary>
        /// 库位编码
        /// </summary>
        [JsonProperty("locationCode")]
        public string LocationCode { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [JsonProperty("StartTime")]
        public string StartTime { get; set; }

        /// <summary>
        /// 到位时间
        /// </summary>
        [JsonProperty("Endtime")]
        public string Endtime { get; set; }

        /// <summary>
        /// 任务执行结果 1 离开、2 故障、3 暂停、4 到达
        /// </summary>
        [JsonProperty("res")]
        public int Res { get; set; }
    }
}
