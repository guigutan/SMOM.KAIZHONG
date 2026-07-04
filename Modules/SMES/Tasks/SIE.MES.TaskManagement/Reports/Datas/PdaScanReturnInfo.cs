using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.Reports.Datas
{
    /// <summary>
    /// 扫码返回信息
    /// </summary>
    [Serializable]
    public class PdaScanReturnInfo
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
        /// 派工数量
        /// </summary>
        public decimal DispatchQty { get; set; }
        /// <summary>
        /// 最大可报工数
        /// </summary>
        public decimal MaxReportQty { get; set; }
        /// <summary>
        /// 剩余可报工数量
        /// </summary>
        public decimal RemainQty { get; set; }

        /// <summary>
        /// 最大剩余可报工数
        /// </summary>
        public decimal MaxRemainQty { get; set; }

        /// <summary>
        /// 工序最大剩余可报工数
        /// </summary>
        public decimal ProcessMaxRemainQty { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>
        public double ProductId { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }


        /// <summary>
        /// 标签数量
        /// </summary>
        public decimal LabelQty { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        public string LabelNo { get; set; }

        /// <summary>
        /// 是否可疑品
        /// </summary>
        public bool IsSuspectProduct { get; set; }

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

        /// <summary>
        /// 状态
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// 提示信息
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcessCode { get; set; }
    }
}
