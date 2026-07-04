using Newtonsoft.Json;

namespace SIE.EventMessages.WMS.StereoWarhouses.Datas.Common
{
    /// <summary>
    /// 物料通用数据
    /// </summary>
    public class MatCommonData
    {
        /// <summary>
        /// 物料编号
        /// </summary>
        [JsonProperty("matCode")]
        public string MatCode { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        [JsonProperty("Quantity")]
        public decimal Quantity { get; set; }
    }
}
