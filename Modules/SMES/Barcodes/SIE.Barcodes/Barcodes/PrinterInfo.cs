using System;

namespace SIE.Barcodes
{
    /// <summary>
    /// 打印信息
    /// </summary>
    [Serializable]
    public class PrinterInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public PrinterInfo()
        {
        }

        /// <summary>
        /// 打印信息
        /// </summary>
        /// <param name="workOrderId">工单Id</param>
        /// <param name="ruleId">编码规则Id</param>
        /// <param name="templateId">打印模板Id</param>
        /// <param name="printQty">打印数量</param>
        /// <param name="singleQty">单张数量</param>
        /// <param name="printedQty">已打印数据</param>
        public PrinterInfo(double workOrderId, double ruleId, double templateId, int printQty, int singleQty, int printedQty) : this()
        {
            WorkOrderId = workOrderId;
            NumberRuleId = ruleId;
            PrintTemplateId = templateId;
            PrintQty = printQty;
            SingleQty = singleQty;
            PrintedQty = printedQty;
        }

        /// <summary>
        /// 工单Id
        /// </summary>
        public double WorkOrderId { get; set; }

        /// <summary>
        /// 编码规则Id
        /// </summary>
        public double NumberRuleId { get; set; }

        /// <summary>
        /// 打印数量
        /// </summary>
        public int PrintQty { get; set; }

        /// <summary>
        /// 单张数量
        /// </summary>
        public int SingleQty { get; set; }

        /// <summary>
        /// 打印模板Id
        /// </summary>
        public double PrintTemplateId { get; set; }

        /// <summary>
        /// 已打印数量
        /// </summary>
        public int PrintedQty { get; set; }
    }
}