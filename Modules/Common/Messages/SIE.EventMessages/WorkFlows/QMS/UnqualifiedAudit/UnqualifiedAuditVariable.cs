using SIE.Core.Inspections;
using System;
using SIE.Common.WorkFlow.Models;

namespace SIE.EventMessages.WorkFlows.QMS.UnqualifiedAudit
{
    /// <summary>
    /// 不合格审核流程变量类
    /// </summary>
    [Serializable]
    public class UnqualifiedAuditVariable : VariableBase
    {
        /// <summary>
        /// 单据Id
        /// </summary>
        public double BillId { get; set; }
        /// <summary>
        /// 检验类型
        /// </summary>
        public InspectionType InspectionType { get; set; }
        /// <summary>
        /// 不合格审核Id
        /// </summary>
        public double? FailedListAuditId { get; set; }
    }

    /// <summary>
    /// 不合格审核流程变量字段名称
    /// </summary>
    public static class UnqualifiedAuditPropertys
    {
        /// <summary>
        /// 单据ID
        /// </summary>
        public const string BillId = "BillId";

        /// <summary>
        /// 检验类型
        /// </summary>
        public const string InspectionType = "InspectionType";

        /// <summary>
        /// 不合格审核Id
        /// </summary>
        public const string FailedListAuditId = "FailedListAuditId";
    }
}
