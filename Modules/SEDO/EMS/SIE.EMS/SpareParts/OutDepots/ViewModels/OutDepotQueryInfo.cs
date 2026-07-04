using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.SpareParts.OutDepots.ViewModels
{
    /// <summary>
    /// 出库扫描返回信息
    /// </summary>
    [Serializable]
    public class OutDepotQueryInfo
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
        /// 出库信息
        /// </summary>
        public OutDepot OutDepotInfo { get; set; }
    }
}
