using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.Edge.Models
{
    /// <summary>
    /// 边缘包装规则
    /// </summary>
    [Serializable]
    public class EdgePackRule
    {
        /// <summary>
        /// Id
        /// </summary>
        public double Id { get; set; }

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
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 包装数
        /// </summary>
        public decimal PackQty { get; set; }

        /// <summary>
        /// 产品数
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 编码规则ID
        /// </summary>
        public double? NumberRuleId { get; set; }

        /// <summary>
        /// 编码规则名称
        /// </summary>
        public string NumberRuleName { get; set; }

        /// <summary>
        /// 是否打印
        /// </summary>
        public bool IsPrint { get; set; }

        /// <summary>
        /// 打印模板ID
        /// </summary>
        public double? PrintTemplateId { get; set; }

        /// <summary>
        /// 打印模板名称
        /// </summary>
        public string PrintTemplateName { get; set; }

        /// <summary>
        /// 是否装箱
        /// </summary>
        public bool IsPackage { get; set; }

        /// <summary>
        /// 层级
        /// </summary>
        public double Index { get; set; }

        /// <summary>
        /// 包装工序Id
        /// </summary>
        public List<double> ProcessIds { get; set; }
    }
}
