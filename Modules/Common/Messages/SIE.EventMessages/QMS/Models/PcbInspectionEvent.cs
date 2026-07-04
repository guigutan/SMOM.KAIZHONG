using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EventMessages.QMS.Models
{
    /// <summary>
    /// 报检参数
    /// </summary>
    [Serializable]
    public class PcbInspectionEvent
    {

        /// <summary>
        /// 批次号
        /// </summary>
        public string LotNo { get; set; }

        /// <summary>
        /// 产品
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 客户ID
        /// </summary>
        public double CustomerId { get; set; }

        /// <summary>
        /// 工步ID
        /// </summary>
        public double ProcessId { get; set; }

        /// <summary>
        /// 工序ID
        /// </summary>
        public double ProcessSegmentId { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
        public double EquipmentId { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// SET数量
        /// </summary>
        public int? SETQty{ get; set; }

        /// <summary>
        /// PCS数量
        /// </summary>
        public int? PCSQty { get; set; }

        /// <summary>
        /// PNL数量
        /// </summary>
        public int? PNLQty { get; set; }

        /// <summary>
        /// 检验类型
        /// </summary>
        public InspBillType InspBillType { get; set; }
    }
}
