using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.Reports.Datas
{
    /// <summary>
    /// 返回派工信息
    /// </summary>
    [Serializable]
    public class PdaPgReturnInfo
    {
        /// <summary>
        /// 工单Id
        /// </summary>
        public double WorkOrderId { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo { get; set; }

        /// <summary>
        /// 派工单ID
        /// </summary>
        public double DispatchTaskId { get; set; }

        /// <summary>
        /// 派工单号
        /// </summary>
        public string DispatchTaskNo { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>
        public double ProductId { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; } = string.Empty;

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 标签数量
        /// </summary>
        public decimal LabelQty { get; set; }

        /// <summary>
        /// 计划数
        /// </summary>
        public decimal PlanQty { get; set; }

        /// <summary>
        /// 分单数量
        /// </summary>
        public decimal Zcode { get; set; }

        /// <summary>
        /// 报工校验
        /// </summary>
        public bool IsReportValid { get; set; }

    }
}
