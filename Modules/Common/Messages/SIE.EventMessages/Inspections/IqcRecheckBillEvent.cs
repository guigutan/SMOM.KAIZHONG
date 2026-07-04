using System;
using System.Collections.Generic;

namespace SIE.EventMessages
{
    /// <summary>
    /// IQC复检
    /// </summary>
    [Serializable]
    public class IqcRecheckBillEvent
    {
        /// <summary>
        /// 供应商Id
        /// </summary>
        public double? SupplierId { get; set; }

        /// <summary>
        /// IQC复检记录ID
        /// </summary>
        public double IqcRecheckId { get; set; }

        /// <summary>
        /// 库存报检单Id
        /// </summary>
        public double InspectionId { get; set; }

        /// <summary>
        /// 库存报检单号
        /// </summary>
        public string InspectionNo { get; set; }

        /// <summary>
        /// 库存报检单明细单号（即IQC复检单号）
        /// </summary>
        public string IqcRecheckNo { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 来料批次号
        /// </summary>
        public string Lot { get; set; }

        /// <summary>
        /// 行号
        /// </summary>
        public string LineNo { get; set; }

        /// <summary>
        /// 是否紧急
        /// </summary>
        public bool IsUrgent { get; set; }

        /// <summary>
        /// 客户Id
        /// </summary>
        public double? CustomerId { get; set; }

        /// <summary>
        /// 仓库
        /// </summary>
        public double? WarehouseId { get; set; }

        /// <summary>
        /// 序列号标签
        /// </summary>
        public List<ReedData> ReedIds
        {
            get; set;
        }
    }

    /// <summary>
    /// 标签条码数据
    /// </summary>
    [Serializable]
    public class ReedData
    {
        /// <summary>
        /// ReedId标签条码
        /// </summary>
        public string ReedId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }
    }
}
