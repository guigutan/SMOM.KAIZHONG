using SIE.XPCJ.Models.Enums;
using System;

namespace SIE.XPCJ.Models.WIP
{
    [Serializable]
    public class WorkOrder
    {
        /// <summary>
        /// 数据ID
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// 产品型号
        /// </summary>
        public string ProductModelName { get; set; }

        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateTime PlanBeginDate { get; set; }

        /// <summary>
        /// 计划完成时间
        /// </summary>
        public DateTime PlanEndDate { get; set; }

        /// <summary>
        /// 工单状态
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// 扩展属性
        /// </summary>
        public string ItemExtPropName { get; set; }

    }
}
