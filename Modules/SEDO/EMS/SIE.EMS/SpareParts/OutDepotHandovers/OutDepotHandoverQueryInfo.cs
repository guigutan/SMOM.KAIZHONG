using SIE.Domain;
using System;

namespace SIE.EMS.SpareParts.OutDepotHandovers
{
    /// <summary>
    /// 交接扫描返回信息
    /// </summary>
    [Serializable]
    public class OutDepotHandoverQueryInfo
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 返回信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 交接单信息
        /// </summary>
        public EntityList<OutDepotHandover> OutDepotHandoverInfoList { get; set; }
    }
}
