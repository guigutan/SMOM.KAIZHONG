using SIE.Core.WorkOrders;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EventMessages.MES.WorkOrders.Models
{
    /// <summary>
    /// 工单信息
    /// </summary>
    [Serializable]
    public class WorkOrderSimpleInfo
    {
        /// <summary>
        /// 工单Id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 工单状态
        /// </summary>
        public WorkOrderState State { get; set; }


        /// <summary>
        /// 车间ID
        /// </summary>
        public double? WorkShopId { get; set; }

        /// <summary>
        /// 资源ID
        /// </summary>
        public double? ResourceId { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// 合并工单号
        /// </summary>
        public string MergeWorkOrderNo { get; set; }

        /// <summary>
        /// 工段编码
        /// </summary>
        public string ProcessSegmentCode { get; set; }

        /// <summary>
        /// 项目号Id
        /// </summary>
        public double? ProjectId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ProjectNo { get; set; }
    }

    /// <summary>
    /// 工单数据
    /// </summary>
    public class WorkOrderData
    {
        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo { get; set; }

        /// <summary>
        /// 工单状态
        /// </summary>
        public WorkOrderState State { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>
        public double ProductId { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 产品明细
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 车间ID
        /// </summary>
        public double? WorkShopId { get; set; }

        /// <summary>
        /// 车间编码
        /// </summary>
        public string WorkShopCode { get; set; }

        /// <summary>
        /// 车间名称
        /// </summary>
        public string WorkShopName { get; set; }

        /// <summary>
        /// 资源ID
        /// </summary>
        public double? ResourceId { get; set; }

        /// <summary>
        /// 资源编码
        /// </summary>
        public string ResourceCode { get; set; }

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        /// 订单数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 计划数量
        /// </summary>
        public decimal PlanQty { get; set; }

        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateTime? PlanStartDate { get; set; }

        /// <summary>
        /// 计划完成时间
        /// </summary>
        public DateTime? PlanFinishDate { get; set; }

        /// <summary>
        /// 实际开始时间
        /// </summary>
        public DateTime? ActuStartDate { get; set; }

        /// <summary>
        /// 实际完成时间
        /// </summary>
        public DateTime? ActFinishDate { get; set; }
    }
}
