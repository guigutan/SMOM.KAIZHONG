using System;

namespace SIE.EventMessages.MES.WipRecords
{
    /// <summary>
    /// 文档信息
    /// </summary>
    [Serializable]
    public class WipSopInfo
    {
        /// <summary>
        /// 工序
        /// </summary>
        public string Process { get; set; }

        /// <summary>
        /// ESOP名称
        /// </summary>
        public string EsopName { get; set; }
    }
}
