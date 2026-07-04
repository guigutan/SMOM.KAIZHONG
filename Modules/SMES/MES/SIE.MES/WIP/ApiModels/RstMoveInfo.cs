using System;

namespace SIE.MES.WIP.ApiModels
{
    /// <summary>
    /// 过站采集返回信息
    /// </summary>
    [Serializable]
    public class RstMoveInfo : RstWipInfo
    {
        /// <summary>
        /// 采集时间
        /// </summary>
        public string CollectDate { get; set; }
    }
}
