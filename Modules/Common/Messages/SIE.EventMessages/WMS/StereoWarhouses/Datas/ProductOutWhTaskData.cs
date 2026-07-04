using Newtonsoft.Json;

namespace SIE.EventMessages.WMS.StereoWarhouses.Datas
{
    /// <summary>
    /// 生产领料出库任务-传参
    /// </summary>
    public class ProductOutWhTaskParam : RequestCommonParam
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
