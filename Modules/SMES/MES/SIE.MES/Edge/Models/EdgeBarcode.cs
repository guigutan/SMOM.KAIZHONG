using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.Edge.Models
{
    /// <summary>
    /// 边缘采集条码信息
    /// </summary>
    [Serializable]
    public class EdgeBarcode
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// 是否报废
        /// </summary>
        public bool IsScraped { get; set; }

        /// <summary>
        /// 数量 Qty
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 是否挂起
        /// </summary>
        public bool IsPending { get; set; }

        /// <summary>
        /// 满箱数量
        /// </summary>
        public decimal BoxesQty { get; set; }

    }
}
