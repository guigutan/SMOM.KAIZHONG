using SIE.Core.Enums;
using SIE.Core.WorkOrders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EventMessages.MES.WorkOrders.Models
{
    /// <summary>
    /// 工单及分页信息
    /// </summary>
    [Serializable]
    public class WorkOrderLesInfoWithCount
    {
        /// <summary>
        /// 总数
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 工单信息
        /// </summary>
        public List<WorkOrderLesInfo> WorkOrderInfos { get; set; } = new List<WorkOrderLesInfo>();
    }

    /// <summary>
    /// 工单信息
    /// </summary>
    [Serializable]
    public class WorkOrderLesInfo
    {
        /// <summary>
        /// 工单ID
        /// </summary>
        public double WorkOrderId { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo { get; set; }

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
        /// 计划开始时间
        /// </summary>
        public DateTime PlanBeginDate { get; set; }

        /// <summary>
        /// 计划完成时间
        /// </summary>
        public DateTime PlanEndDate { get; set; }

        /// <summary>
        /// 计划数量
        /// </summary>
        public decimal PlanQty { get; set; }

        /// <summary>
        /// 完工数
        /// </summary>
        public decimal FinishQty { get; set; }

        /// <summary>
        /// 实际开始时间
        /// </summary>
        public DateTime? ActuStartDate { get; set; }

        /// <summary>
        /// 实际完成时间
        /// </summary>
        public DateTime? ActuFinishDate { get; set; }

        /// <summary>
        /// 车间ID
        /// </summary>
        public double? WorkShopId { get; set; }

        /// <summary>
        /// 车间编码
        /// </summary>
        public string WorkShopCode { get; set; }

        /// <summary>
        /// 产线ID
        /// </summary>
        public double? ResourceId { get; set; }

        /// <summary>
        /// 资源编码
        /// </summary>
        public string ResourceCode { get; set; }

        /// <summary>
        /// 工单状态
        /// </summary>
        public WorkOrderState State { get; set; }

        /// <summary>
        /// 工单类型
        /// </summary>
        public WorkOrderType Type { get; set; }

        /// <summary>
        /// 工艺面(电子使用)
        /// </summary>
        public Deck? ProcessSurface { get; set; }

        /// <summary>
        /// 工段
        /// </summary>
        public double ProcessSegmentId { get; set; }

        /// <summary>
        /// 消息类型(用于发送数据到边端)
        /// </summary>
        public string MsgType { get; set; }

        /// <summary>
        /// 工厂Id
        /// </summary>
        public double FactoryId { get; set; }

        /// <summary>
        /// 工厂编码
        /// </summary>
        public string FactoryCode { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 销售订单
        /// </summary>
        public string SaleOrderNo { get; set; }

        /// <summary>
        /// 客户订单
        /// </summary>
        public string CustomerOrderNo { get; set; }

        /// <summary>
        /// 项目号Id
        /// </summary>
        public double? ProjectMaintainId { get; set; }

        /// <summary>
        /// 项目号
        /// </summary>
        public string ProjectMaintainCode { get; set; }
    }
}
