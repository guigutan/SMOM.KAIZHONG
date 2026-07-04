using SIE.Core.Inspections;
using SIE.Domain;
using SIE.EventMessages.WMS.Traces;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EventMessages.QMS.Traces
{
    /// <summary>
    /// 来料检验追溯
    /// </summary>
    [Serializable]
    public class TraceInfoForQms
    {
        /// <summary>
        /// 总条数
        /// </summary>
        public int TotalCount { get; set; } = 0;

        /// <summary>
        /// 数据
        /// </summary>
        public List<TraceItemInfoForQms> Data { get; set; } = new List<TraceItemInfoForQms>();
    }

    /// <summary>
    /// 来料检验追溯详细
    /// </summary>
    [Serializable]
    public class TraceItemInfoForQms
    {
        /// <summary>
        /// 报检的asn明细Id
        /// </summary>
        public double? AsnDetailId { get; set; }

        /// <summary>
        /// 检验类型
        /// </summary>
        public string InspectionType { get; set; }

        /// <summary>
        /// 检验单据Id
        /// </summary>
        public double BillId { get; set; }

        /// <summary>
        /// 检验单号
        /// </summary>
        public string InspectionNo { get; set; }

        /// <summary>
        /// 检验结果
        /// </summary>
        public string InspectionResult { get; set; }


        /// <summary>
        /// 不合格处理结果
        /// </summary>
        public string FailedAuditResult { get; set; }

        /// <summary>
        /// 缺陷记录
        /// </summary>
        public string DefectRecord { get; set; }

        /// <summary>
        /// 不合格流程编码
        /// </summary>
        public string FailedAuditWorkflowCode { get; set; }

        /// <summary>
        /// 质量改进流程编码
        /// </summary>
        public string QualityWorkflowCode { get; set; }

        /// <summary>
        /// 检验员名称
        /// </summary>
        public string InspectionBy { get; set; }

        /// <summary>
        /// 检验时间
        /// </summary>
        public DateTime? InspectionTime { get; set; }

    }
}
