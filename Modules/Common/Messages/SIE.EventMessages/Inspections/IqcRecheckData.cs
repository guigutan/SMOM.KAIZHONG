using SIE.Common;
using System;

namespace SIE.EventMessages
{
    /// <summary>
    /// IQC复检结果数据
    /// </summary>
    [Serializable]
    public class IqcRecheckData
    {
        /// <summary>
        /// IQC复检单Id
        /// </summary>
        public double IqcRecheckId { get; set; }

        /// <summary>
        /// IQC复检单号
        /// </summary>
        public string IqcRecheckNo { get; set; }

        /// <summary>
        /// 检验结果
        /// </summary>
        public InspectionResult InspectionResult { get; set; }

        /// <summary>
        /// 合格数量
        /// </summary>
        public decimal QualifiedQty { get; set; }

        /// <summary>
        /// 不合格数量
        /// </summary>
        public decimal UnQualifiedQty { get; set; }

        /// <summary>
        /// 质检处理
        /// </summary>
        public ProcessMode? QuaInspectionProcess { get; set; }

        /// <summary>
        /// 检验人
        /// </summary>
        public double AuditById { get; set; }

        /// <summary>
        /// 检验时间
        /// </summary>
        public DateTime? AuditDate { get; set; }
    }
}
