using System;


namespace SIE.Web.MES.PackingPrints.Commands
{
    /// <summary>
    /// 初始数据
    /// </summary>
    [Serializable]
    public class PackingPrintData
    {

        /// <summary>
        /// 条码规则Id
        /// </summary>
        public double? NumberRuleId { get; set; }

        /// <summary>
        /// 条码规则名称
        /// </summary>
        public string NumberRuleName { get; set; }

        /// <summary>
        /// 模板Id
        /// </summary>
        public double? TemplateId { get; set; }

        /// <summary>
        /// 模板名称
        /// </summary>
        public string TemplateName { get; set; }

        /// <summary>
        /// 模板实体类型
        /// </summary>
        public string TemplateEntityType { get; set; }

        /// <summary>
        /// 工单Id
        /// </summary>
        public double WorkOrderId { get; set; }

        /// <summary>
        /// 打印控制(反打)
        /// </summary>
        public bool PrintControl { get; set; }

        /// <summary>
        /// 产品数
        /// </summary>
        public int ProductQty { get; set; }

        /// <summary>
        /// 剩余数量
        /// </summary>
        public int ResidualQty { get; set; }

        /// <summary>
        /// 打印数量
        /// </summary>
        public int PrintQty { get; set; }

        /// <summary>
        /// 已打印数量
        /// </summary>
        public int PrintedQty { get; set; }

        /// <summary>
        /// 起始条码
        /// </summary>
        public string BeginSn { get; set; }

        /// <summary>
        /// 结束条码
        /// </summary>
        public string EndSn { get; set; }

        /// <summary>
        /// 条码规则明细
        /// </summary>
        public string BarcodeRuleDtl { get; set; }
    }
}
