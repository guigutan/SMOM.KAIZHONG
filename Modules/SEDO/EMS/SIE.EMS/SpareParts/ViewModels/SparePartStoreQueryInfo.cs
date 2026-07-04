using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.SpareParts.ViewModels
{
    /// <summary>
    /// 入库扫描返回信息
    /// </summary>
    [Serializable]
    public class SparePartStoreQueryInfo
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
        /// 入库信息
        /// </summary>
        public SparePartStore SparePartStoreInfo { get; set; }
    }
}
