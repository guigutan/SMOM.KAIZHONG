using SIE.Core.Inspections;
using System;

namespace SIE.EventMessages.WorkFlows.QMS.UnqualifiedAudit
{
    /// <summary>
    /// 执行不合格审核-工作流-Model
    /// </summary>
    [Serializable]
    public class ExecuteWorkflowModel
    {

        /// <summary>
        /// 检验类型
        /// </summary>

        public InspectionType InspectionType { get; set; }

        /// <summary>
        /// 单据Id
        /// </summary>
        public double BillId { get; set; }
        /// <summary>
        /// 单据号
        /// </summary>

        public string BillNo { get; set; }

        /// <summary>
        /// 发起人
        /// </summary>

        public double StarterId { get; set; }

        /// <summary>
        /// 不合格审核ID
        /// </summary>
        public double? FailedListAuditId { get; set; }

        /// <summary>
        /// 质量分类ID
        /// </summary>
        public double? QualityCategoryId { get; set; }

        /// <summary>
        /// 是否允许撤销
        /// </summary>
        public bool CanCancel { get; set; }  

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
