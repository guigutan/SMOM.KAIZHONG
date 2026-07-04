using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.Edge.Models
{
    /// <summary>
    /// 边缘包装记录
    /// </summary>
    [Serializable]
    public class EdgePackRecord
    {
        /// <summary>
        /// TreeId
        /// </summary>
        public string TreeId { get; set; }

        /// <summary>
        /// 工单ID
        /// </summary>
        public double WorkOrderId { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo { get; set; }

        /// <summary>
        /// 包装单位名称
        /// </summary>
        public double PackUnitId { get; set; }
        /// <summary>
        /// 包装单位名称
        /// </summary>
        public string PackUnitName { get; set; }

        /// <summary>
        /// 包装号
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 父包装号
        /// </summary>
        public string PCode { get; set; }

        /// <summary>
        /// 工序ID
        /// </summary>
        public string ProcessId { get; set; }

        /// <summary>
        /// 工位ID
        /// </summary>
        public string StationId { get; set; }

        /// <summary>
        /// 包装时间
        /// </summary>
        public string PackedDate { get; set; }

        /// <summary>
        /// 层级
        /// </summary>
        public double Index { get; set; }

        /// <summary>
        /// 产品数
        /// </summary>
        public decimal ItemQty { get; set; }
    }
}
