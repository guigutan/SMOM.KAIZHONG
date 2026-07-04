using Newtonsoft.Json;
using SIE.EventMessages.WMS.StereoWarhouses.Datas.Common;
using System.Collections.Generic;

namespace SIE.EventMessages.WMS.StereoWarhouses.Datas
{
    /// <summary>
    /// 任务计划出库托盘列表数据
    /// </summary>
    public class TaskPlanOutWhLpnListData : RequestCommonParam
    {
        /// <summary>
        /// 托盘列表数据
        /// </summary>
        [JsonProperty("palletList")]
        public List<LpnData> PalletList { get; set; }
    }

    /// <summary>
    /// 托盘数据
    /// </summary>
    public class LpnData : PalletCommonData
    {
        /// <summary>
        /// 物料数据
        /// </summary>
        [JsonProperty("matList")]
        public List<MatCommonData> MatList { get; set; }
    }
}
