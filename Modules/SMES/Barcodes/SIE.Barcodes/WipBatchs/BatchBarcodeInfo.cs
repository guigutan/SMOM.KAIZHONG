using System;

namespace SIE.Barcodes.WipBatchs
{
    /// <summary>
    /// 批次条码信息
    /// </summary>
    [Serializable]
    public class BatchBarcodeInfo
    {
        /// <summary>
        /// 工单Id
        /// </summary>
        public double WorkOrderId { get; set; }


        /// <summary>
        /// 批次编码规则ID
        /// </summary>
        public double NumberRuleId { get; set; }

        /// <summary>
        /// 批次数量
        /// </summary>
        public decimal BatchQty { get; set; }

        /// <summary>
        /// 生成数量
        /// </summary>
        public decimal GeneratingQty { get; set; }

        /// <summary>
        /// 是否生产子批次
        /// </summary>
        public bool GenerateChild { get; set; }

        /// <summary>
        /// 子批次编码规则Id
        /// </summary>
        public double ChildNumberRuleId { get; set; }

        /// <summary>
        /// 子批次数量
        /// </summary>
        public decimal ChildBatchQty { get; set; }
    }
}