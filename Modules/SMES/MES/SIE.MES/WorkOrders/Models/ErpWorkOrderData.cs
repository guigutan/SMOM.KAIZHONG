using SIE.EventMessages;
using System;
using System.Collections.Generic;

namespace SIE.MES.WorkOrders.Models
{
    /// <summary>
    /// 工单数据
    /// </summary>
    [Serializable]
    public class ErpWorkOrderData : ErpInfoData
    {
        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo { get; set; }

        /// <summary>
        /// Erp工单号
        /// </summary>
        public string ErpWorkOrderNo { get; set; }

        /// <summary>
        /// 客户ID
        /// </summary>
        public double? CustomerId { get; set; }

        /// <summary>
        /// 客户编码
        /// </summary>
        public string CustomerCode { get; set; }

        /// <summary>
        /// 客户订单号
        /// </summary>
        public string CustomerOrderNo { get; set; }

        /// <summary>
        /// 销售订单号
        /// </summary>
        public string SaleOrderNo { get; set; }

        /// <summary>
        /// 工单类型 0量产、1试产、2返工、3校验
        /// </summary>
        public int WorkOrderType { get; set; }

        /// <summary>
        /// 车间编码
        /// </summary>
        public string WorkshopCode { get; set; }

        /// <summary>
        /// 资源编码
        /// </summary>
        public string ResourceCode { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 计划数量
        /// </summary>
        public decimal PlanQty { get; set; }

        /// <summary>
        /// 订单数量
        /// </summary>
        public decimal OrderQty { get; set; }

        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateTime PlanBeginDate { get; set; }

        /// <summary>
        /// 计划完成时间
        /// </summary>
        public DateTime PlanEndDate { get; set; }

        /// <summary>
        /// 建单人编码
        /// </summary>
        public string MakerCode { get; set; }

        /// <summary>
        /// 工单BOM列表
        /// </summary>
        public List<ErpWorkOrderBomData> BomList { get; } = new List<ErpWorkOrderBomData>();
    }

    /// <summary>
    /// 工单bom数据
    /// </summary>
    [Serializable]
    public class ErpWorkOrderBomData : ErpInfoData
    {
        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 需求数量
        /// </summary>
        public decimal RequireQty { get; set; }

        /// <summary>
        /// 单机定额
        /// </summary>
        public decimal SingleQty { get; set; }

        /// <summary>
        /// 是否反冲物料
        /// </summary>
        public bool IsRecoilItem { get; set; }

        /// <summary>
        /// 是否虚拟物料
        /// </summary>
        public bool IsVritualItem { get; set; }

        /// <summary>
        /// 是否按单标识
        /// </summary>
        public bool IsByBill { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}