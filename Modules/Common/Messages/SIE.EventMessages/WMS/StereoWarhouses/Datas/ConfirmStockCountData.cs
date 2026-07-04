using Newtonsoft.Json;
using SIE.EventMessages.WMS.StereoWarhouses.Datas.Common;
using System.Collections.Generic;

namespace SIE.EventMessages.WMS.StereoWarhouses.Datas
{
    /// <summary>
    /// 确认盘点任务数据
    /// </summary>
    public class ConfirmStockCountData : RequestCommonParam
    {
        /// <summary>
        /// 容器数据
        /// </summary>
        [JsonProperty("palletList")]
        public List<ConfirmStockPalletData> PalletList { get; set; }
    }

    /// <summary>
    /// 盘点容器数据
    /// </summary>
    public class ConfirmStockPalletData : PalletCommonData
    {
        /// <summary>
        /// 物料数据
        /// </summary>
        [JsonProperty("matList")]
        public List<ConfirmStockMatData> MatList { get; set; }
    }

    /// <summary>
    /// 盘点物料数据
    /// </summary>
    public class ConfirmStockMatData : MatCommonData
    {
        /// <summary>
        /// 物料条码
        /// </summary>
        [JsonProperty("matBarcode")]
        public string MatBarcode { get; set; }
    }
}
