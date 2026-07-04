using Newtonsoft.Json;
using System.Collections.Generic;

namespace SIE.EventMessages.WMS.StereoWarhouses.Datas
{
    /// <summary>
    /// 解冻物料数据
    /// </summary>
    public class UnFreezeItemData : RequestCommonParam
    {
        /// <summary>
        /// 容器编号
        /// </summary>
        [JsonProperty("palletIds")]
        public List<string> PalletIds { get; set; }

        /// <summary>
        /// 物料编号
        /// </summary>
        [JsonProperty("barcodes")]
        public List<string> Barcodes { get; set; }
    }
}
