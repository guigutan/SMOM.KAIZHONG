using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.Edge.Models
{
    /// <summary>
    /// 边缘包装条码信息
    /// </summary>
    [Serializable]
    public class EdgePackingBarcode
    {
        /// <summary>
        /// 包装号
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 包装单位名称
        /// </summary>
        public string PackUnitName { get; set; }

        /// <summary>
        /// 是否使用
        /// </summary>
        public bool IsUse { get; set; }

        /// <summary>
        /// 工单ID
        /// </summary>
        public double WorkOrderId { get; set; }
    }
}
