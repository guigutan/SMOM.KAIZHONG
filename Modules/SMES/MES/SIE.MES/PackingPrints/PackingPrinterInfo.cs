using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.PackingPrints
{
    /// <summary>
    /// 打印信息
    /// </summary>
    [Serializable]
    public class PackingPrinterInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public PackingPrinterInfo()
        {
        }

        /// <summary>
        /// 打印信息
        /// </summary>
        /// <param name="workOrderId">工单Id</param>
        /// <param name="ruleId">编码规则Id</param>
        /// <param name="packageRuleDetailId">工单与包装关系ID</param>
        /// <param name="templateId">打印模板Id</param>
        /// <param name="printQty">打印数量</param>
        /// <param name="pageCount">打印份数</param>
        public PackingPrinterInfo(double workOrderId, double ruleId, double packageRuleDetailId, double templateId, int printQty, int pageCount) : this() 
        {
            WorkOrderId = workOrderId;
            NumberRuleId = ruleId;
            PackageRuleDetailId = packageRuleDetailId;
            TemplateId = templateId;
            PrintQty = printQty;
            PageCount = pageCount;
        }

        /// <summary>
        /// 工单Id
        /// </summary>
        public double WorkOrderId { get; set; }

        /// <summary>
        /// 工单包装规则Id
        /// </summary>
        public double PackageRuleDetailId { get; set; }

        /// <summary>
        /// 编码规则Id
        /// </summary>
        public double NumberRuleId { get; set; }

        /// <summary>
        /// 打印数量
        /// </summary>
        public int PrintQty { get; set; }

        /// <summary>
        /// 打印模板Id
        /// </summary>
        public double TemplateId { get; set; }

        /// <summary>
        /// 打印份数
        /// </summary>
        public int PageCount { get; set; }
    }
}
