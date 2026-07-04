using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.XPCJ.Models.WIP.Entity
{
    [Serializable]
    public class BarcodePrintInfo
    {

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

        /// <summary>
        /// 模板类型
        /// </summary>
        public string TemplateType { get; set; }

        /// <summary>
        /// 打印机
        /// </summary>
        public string Printer { get; set; }
    }
}
