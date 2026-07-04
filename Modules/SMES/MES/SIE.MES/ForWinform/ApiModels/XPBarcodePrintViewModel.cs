using SIE.Barcodes;
using SIE.Common.NumberRules;
using SIE.Common.Prints;
using SIE.Domain;
using SIE.MES.WorkOrders;
using System;

namespace SIE.MES.ForWinform.ApiModels
{
    [Serializable]
    public class XPBarcodePrintViewModel
    {
        public double Id { get; set; }

        /// <summary>
        /// 工单
        /// </summary>
        public double WorkOrderId
        {
            get; set;
        }

        /// <summary>
        /// 条码规则ID
        /// </summary>
        public double? NumberRuleId
        {
            get; set;
        }


        /// <summary>
        /// 条码规则
        /// </summary>
        public NumberRule NumberRule
        {
            get; set;
        }

        /// <summary>
        /// 打印机
        /// </summary>
        public string Printer
        {
            get; set;
        }

        /// <summary>
        /// 模板Id
        /// </summary>
        public double? TemplateId
        {
            get; set;
        }


        /// <summary>
        /// 模板
        /// </summary>
        public PrintTemplate Template
        {
            get; set;
        }


        /// <summary>
        /// 开始条码
        /// </summary>
        public string BeginSn
        {
            get; set;
        }


        /// <summary>
        /// 结束条码
        /// </summary>
        public string EndSn
        {
            get; set;
        }


        /// <summary>
        /// 剩余数量
        /// </summary>
        public int ResidualQty
        {
            get; set;
        }


        /// <summary>
        /// 打印数量
        /// </summary>
        public int PrintQty
        {
            get; set;
        }


        /// <summary>
        /// 已打印数量
        /// </summary>
        public int PrintedQty
        {
            get; set;
        }


        /// <summary>
        /// 打印份数
        /// </summary>
        public int PageCount
        {
            get; set;
        }


        /// <summary>
        /// 单张数量
        /// </summary>
        public int SingleQty
        {
            get; set;
        }

        /// <summary>
        /// 报废数量
        /// </summary>
        public int DumpingQty
        {
            get; set;
        }


        /// <summary>
        /// 条码规则明细
        /// </summary>
        public string BarcodeRuleDtl
        {
            get; set;
        }


        /// <summary>
        /// 打印控制
        /// </summary>
        public bool PrintControl
        {
            get; set;
        }

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get; set;
        }


        /// <summary>
        /// 工单计划数量
        /// </summary>
        public string WorkOrderPlanQty
        {
            get; set;
        }


        /// <summary>
        /// 工单计划开始时间
        /// </summary>
        public DateTime WorkOrderPlanBeginDate
        {
            get; set;
        }


        /// <summary>
        /// 模板实体类型
        /// </summary>
        public string TemplateEntityType
        {
            get; set;
        }
    }
}
