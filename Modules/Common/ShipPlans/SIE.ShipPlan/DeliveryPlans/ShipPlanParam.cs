using System;
using System.Collections.Generic;

namespace SIE.ShipPlan
{
    /// <summary>
    /// 发货计划参数
    /// </summary>
    [Serializable]
    public class ShipPlanParam
    {
        /// <summary>
        /// 订单类型(70-销售出库；80-工单发料；81-委外工单发料；90-其他出库；100-供应商退货；120-直接调拨；121-两步调拨)
        /// </summary>
        public int OrderType { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public List<ItemExtPropData> ItemExtendProps { get; set; } = new List<ItemExtPropData>();

        /// <summary>
        /// 需求数
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 计划单号
        /// </summary>
        public string PlanOrderNo { get; set; }

        /// <summary>
        /// 计划明细行号
        /// </summary>
        public int PlanDtlLineNo { get; set; }

        /// <summary>
        /// 发货时间
        /// </summary>
        public DateTime? DeliveryDate { get; set; }

        /// <summary>
        /// 生产部门
        /// </summary>
        public string EnterpriseCode { get; set; }

        /// <summary>
        /// 客户
        /// </summary>
        public string CustomerCode { get; set; }

        /// <summary>
        /// 供应商
        /// </summary>
        public string SupplierCode { get; set; }

        /// <summary>
        /// 目标仓库(直接调拨和两步调拨时必填)
        /// </summary>
        public string TargetWarehouseCode { get; set; }

        /// <summary>
        /// 发货仓库
        /// </summary>
        public string WarehouseCode { get; set; }

        /// <summary>
        /// 相关单号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 相关订单行号
        /// </summary>
        public string OrderLineNo { get; set; }

        /// <summary>
        /// 货主编码
        /// </summary>
        public string StorerCode { get; set; }

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
        /// 生产资源ID
        /// </summary>
        public double? ResourceId { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProp { get; set; }

        /// <summary>
        /// 物料扩展属性名称
        /// </summary>
        public string ItemExtPropName { get; set; }
        /// <summary>
        /// 是否合并下发
        /// </summary>
        public bool IsMergeIssued { get; set; }
    }

    /// <summary>
    /// 物料扩展属性
    /// </summary>
    public class ItemExtPropData
    {
        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 物料扩展属性名称
        /// </summary>
        public string ItemExtPropName { get; set; }

        /// <summary>
        /// 物料扩展属性值
        /// </summary>
        public string ItemExtPropValue { get; set; }
    }
}
