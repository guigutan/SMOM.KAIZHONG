using System;
using System.Collections.Generic;

namespace SIE.EventMessages
{
    /// <summary>
    /// 库存调拨参数
    /// </summary>
    [Serializable]
    public class InvAllotParam
    {
        /// <summary>
        /// 订单类型
        /// </summary>
        public int OrderType { get; set; }

        /// <summary>
        /// 源仓库ID
        /// </summary>
        public double WarehouseId { get; set; }

        /// <summary>
        /// 目标仓库ID
        /// </summary>
        public double TargetWarehouseId { get; set; }

        /// <summary>
        /// 货主编码
        /// </summary>
        public string StorerCode { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProp { get; set; }

        /// <summary>
        /// 指定项目号
        /// </summary>
        public string ProjectNo { get; set; }

        /// <summary>
        /// 指定任务号
        /// </summary>
        public string TaskNo { get; set; }

        /// <summary>
        /// 指定批次号
        /// </summary>
        public string LotCode { get; set; }

        /// <summary>
        /// 指定生产批次
        /// </summary>
        public string ProductBatch { get; set; }

        /// <summary>
        /// 事务日期
        /// </summary>
        public DateTime? DeliveryDate { get; set; }

        /// <summary>
        /// 单号
        /// </summary>
        public string SaleOrderNo { get; set; }

        /// <summary>
        /// 相关单号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 是否同发货日期
        /// </summary>
        public bool IsSameDeliveryDate { get; set; }

        /// <summary>
        /// 是否同单号
        /// </summary>
        public bool IsSameNo { get; set; }

        /// <summary>
        /// 是否同相关单号
        /// </summary>
        public bool IsSameOrderNo { get; set; }

        /// <summary>
        /// 调拨来源需求明细参数
        /// </summary>
        public List<InvAllotReqDtlParam> InvAllotReqDtlParams { get; set; } = new List<InvAllotReqDtlParam>();
    }

    /// <summary>
    /// 调拨来源需求明细参数
    /// </summary>
    public class InvAllotReqDtlParam
    {
        /// <summary>
        /// 物料ID
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 需求数
        /// </summary>
        public decimal RequireQty { get; set; }

        /// <summary>
        /// 指定项目号
        /// </summary>
        public string ProjectNo { get; set; }

        /// <summary>
        /// 指定任务号
        /// </summary>
        public string TaskNo { get; set; }

        /// <summary>
        /// 指定批次号
        /// </summary>
        public string LotCode { get; set; }

        /// <summary>
        /// 指定生产批次
        /// </summary>
        public string ProductBatch { get; set; }

        /// <summary>
        /// 发运需求单号
        /// </summary>
        public string SoRequireNo { get; set; }

        /// <summary>
        /// 发运需求明细号
        /// </summary>
        public string SoRequireDtlNo { get; set; }

        /// <summary>
        /// 发货日期
        /// </summary>
        public DateTime? DeliveryDate { get; set; }

        /// <summary>
        /// 相关单号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProp { get; set; }

        /// <summary>
        /// 物料扩展属性名称
        /// </summary>
        public string ItemExtPropName { get; set; }

        /// <summary>
        /// 发货计划ID
        /// </summary>
        public double ShipPlanId { get; set; }
    }
}