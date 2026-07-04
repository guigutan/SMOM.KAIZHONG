using SIE.Core.Inspections;
using System;

namespace SIE.EventMessages.WorkFlows.QMS.UnqualifiedAudit
{
    /// <summary>
    /// 审核流程完成事件
    /// </summary>
    [Serializable]
    public class FinishAuditDoneEvent
    {
        /// <summary>
        /// 单据ID
        /// </summary>
        public double BillId { get; set; }
        /// <summary>
        /// 检验类型
        /// </summary>
        public InspectionType? InspectionType { get; set; }

        /// <summary>
        /// 审核状态
        /// </summary>
        public AuditStatus AuditStatus { get; set; }

        /// <summary>
        /// 流程实例单号
        /// </summary>
        public string FlowInstanceCode { get; set; }
    }
}
