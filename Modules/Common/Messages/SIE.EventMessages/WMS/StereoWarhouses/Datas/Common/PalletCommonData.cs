using Newtonsoft.Json;

namespace SIE.EventMessages.WMS.StereoWarhouses.Datas.Common
{
    /// <summary>
    /// 容器通用数据
    /// </summary>
    public class PalletCommonData
    {
        /// <summary>
        /// 容器编号
        /// </summary>
        [JsonProperty("palletId")]
        public string PalletId { get; set; }
    }
}
