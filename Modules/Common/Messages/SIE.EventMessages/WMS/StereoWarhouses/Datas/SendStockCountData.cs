using Newtonsoft.Json;
using SIE.EventMessages.WMS.StereoWarhouses.Datas.Common;
using System.Collections.Generic;

namespace SIE.EventMessages.WMS.StereoWarhouses.Datas
{
    /// <summary>
    /// 发送库存盘点数据
    /// </summary>
    public class SendStockCountData : RequestCommonParam
    {
        /// <summary>
        /// 容器数据
        /// </summary>
        [JsonProperty("palletList")]
        public List<StockPalletData> PalletList { get; set; }
    }

    /// <summary>
    /// 盘点容器数据
    /// </summary>
    public class StockPalletData : PalletCommonData
    {
        /// <summary>
        /// 物料数据
        /// </summary>
        [JsonProperty("matList")]
        public List<StockMatData> MatList { get; set; }
    }

    /// <summary>
    /// 盘点物料数据
    /// </summary>
    public class StockMatData : MatCommonData
    {
        /// <summary>
        /// 物料条码
        /// </summary>
        [JsonProperty("matBarcode")]
        public string MatBarcode { get; set; }

        /// <summary>
        /// json忽略数量字段
        /// </summary>
        [JsonIgnore]
        public new decimal Quantity { get; set; }
    }

}
