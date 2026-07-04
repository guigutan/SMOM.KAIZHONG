using Org.BouncyCastle.Asn1.Crmf;
using SIE.Core.Enums;
using System;
using System.Collections.Generic;

namespace SIE.EventMessages.Shipment
{
    /// <summary>
    /// 发运订单参数
    /// </summary>
    [Serializable]
    public class SoParam
    {
        /// <summary>
        /// 订单类型
        /// </summary>
        public int OrderType { get; set; }

        /// <summary>
        /// 仓库ID
        /// </summary>
        public double WarehouseId { get; set; }

        /// <summary>
        /// 货主编码
        /// </summary>
        public string StorerCode { get; set; }

        /// <summary>
        /// 客户
        /// </summary>
        public double? CustomerId { get; set; }

        /// <summary>
        /// 供应商
        /// </summary>
        public double? SupplierId { get; set; }

        /// <summary>
        /// 生产部门
        /// </summary>
        public double? EnterpriseId { get; set; }

        /// <summary>
        /// 发货日期
        /// </summary>
        public DateTime? DeliveryDate { get; set; }

        /// <summary>
        /// 计划单号
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
        /// 发运订单需求明细参数
        /// </summary>
        public List<SoRequireDtlParam> SoRequireDtlParams { get; set; } = new List<SoRequireDtlParam>();

        /// <summary>
        /// 订单明细行号
        /// </summary>
        public string PlanLineNo { get; set; }

        /// <summary>
        /// 调拨模式
        /// </summary>
        public int? AllotMode { get; set; }

        /// <summary>
        /// 目标仓库
        /// </summary>
        public double? TargetWhId { get; set; }

        /// <summary>
        /// 是否来源备料计划
        /// </summary>
        public bool IsStockPlan { get; set; }
    }

    /// <summary>
    /// 发运订单需求明细参数
    /// </summary>
    public class SoRequireDtlParam
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
        /// 相关单行号
        /// </summary>
        public string OrderLineNo { get; set; }

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

        /// <summary>
        /// ERP单据ID
        /// </summary>
        public double? ErpOrderId { get; set; }

        /// <summary>
        /// 库存组织名称
        /// </summary>
        public string ErpOrganizationName { get; set; }

        /// <summary>
        /// 业务实体名称
        /// </summary>
        public string ErpOrgName { get; set; }

        /// <summary>
        ///  Erp明细行Id或主键值
        /// </summary>
        public string ErpDetailId { get; set; }

        /// <summary>
        /// ERP工单号
        /// </summary>
        public string ErpWoNo { get; set; }

        /// <summary>
        /// 预计最早发货时间
        /// </summary>
        public string ScheduleShipDate { get; set; }
    }

    /// <summary>
    /// 退货发运单数据
    /// </summary>
    public class ReturnSoParam
    {
        /// <summary>
        /// 订单类型
        /// </summary>
        public OrderType OrderType { get; set; }
        /// <summary>
        /// 单据小类编码
        /// </summary>
        public string TransactionCode { get; set; }

        /// <summary>
        /// 仓库ID
        /// </summary>
        public double WarehouseId { get; set; }

        /// <summary>
        /// 货主编码
        /// </summary>
        public string StorerCode { get; set; }

        /// <summary>
        /// 客户
        /// </summary>
        public double? CustomerId { get; set; }

        /// <summary>
        /// 供应商
        /// </summary>
        public double? SupplierId { get; set; }

        /// <summary>
        /// 退货原因
        /// </summary>
        public double? ReasonId { get; set; }
      
        /// <summary>
        /// 退货明细数据
        /// </summary>
        public List<ReturnSoDtlData> ReturnSoDtlDatas { get; set; } = new List<ReturnSoDtlData>();
    }

    /// <summary>
    /// 退货发运需求数据
    /// </summary>
    public class ReturnSoReqDtlData
    {
        /// <summary>
        /// 送货明细ID
        /// </summary>
        public double AsnDeliveryDetailId { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 采购订单明细ID
        /// </summary>
        public double? PoDtlId { get; set; }

        /// <summary>
        /// 采购订单ID
        /// </summary>
        public double? PoId { get; set; }

        /// <summary>
        /// 退货数
        /// </summary>
        public decimal ReturnQty { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProp { get; set; }

        /// <summary>
        /// 物料扩展属性名称
        /// </summary>
        public string ItemExtPropName { get; set; }

        /// <summary>
        /// 收货明细ID
        /// </summary>
        public double AsnDtlId { get; set; }
         
        /// <summary>
        /// 辅助单位Id
        /// </summary>
        public double SecondUnitId
        {
            get; set;
        }
        
        /// <summary>
        /// 分子
        /// </summary>
        public decimal Numerator
        {
            get; set;
        }
            
        /// <summary>
        /// 分母
        /// </summary>
        public decimal Denominator
        {
            get; set;
        }
       
        /// <summary>
        /// 转换率
        /// </summary>
        public decimal ConvertFigre
        {
            get; set;
        }      
    }

    /// <summary>
    /// 退货发运明细数据
    /// </summary>
    public class ReturnSoDtlData
    {
        /// <summary>
        /// 收货明细ID
        /// </summary>
        public double AsnDtlId { get; set; }

        /// <summary>
        /// 行号
        /// </summary>
        public string LineNo { get; set; }

        /// <summary>
        /// 相关单号-ASN单号
        /// </summary>
        public string AsnNo { get; set; }

        /// <summary>
        /// 送货明细ID
        /// </summary>
        public double? AsnDeliveryDetailId { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 采购单明细ID
        /// </summary>
        public double? PoDtlId { get; set; }

        /// <summary>
        /// 采购单ID
        /// </summary>
        public double? PoId { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public string LotCode { get; set; }

        /// <summary>
        /// 货主
        /// </summary>
        public string ProjectNo { get; set; }

        /// <summary>
        /// 任务号
        /// </summary>
        public string TaskNo { get; set; }

        /// <summary>
        /// 退货数
        /// </summary>
        public decimal ReturnQty { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProp { get; set; }

        /// <summary>
        /// 物料扩展属性名称
        /// </summary>
        public string ItemExtPropName { get; set; }

        /// <summary>
        /// 包装规则ID
        /// </summary>
        public double ItemPackageRuleId { get; set; }

        /// <summary>
        /// 包装规则明细ID
        /// </summary>
        public double ItemPackageRuleDetailId { get; set; }

        /// <summary>
        /// 辅助单位Id
        /// </summary>
        public double SecondUnitId
        {
            get; set;
        }

        /// <summary>
        /// 分子
        /// </summary>
        public decimal Numerator
        {
            get; set;
        }

        /// <summary>
        /// 分母
        /// </summary>
        public decimal Denominator
        {
            get; set;
        }

        /// <summary>
        /// 转换率
        /// </summary>
        public decimal ConvertFigre
        {
            get; set;
        }

        /// <summary>
        /// 来源主键值，ERP主键值
        /// </summary>
        public string SourceKey { get; set; }

        /// <summary>
        /// ERP采购单号
        /// </summary>
        public string ErpPoNo { get; set; }

        /// <summary>
        /// ERP库存组织名称
        /// </summary>
        public string ErpOrganizationName { get; set; }

        /// <summary>
        /// 码盘LPN列表
        /// </summary>
        public List<ReturnLpnData> Lpns { get; set; } = new List<ReturnLpnData>();
    }

    /// <summary>
    /// 码盘LPN列表
    /// </summary>
    public class ReturnLpnData
    {
        /// <summary>
        /// LPN
        /// </summary>
        public string Lpn { get; set; }

        /// <summary>
        /// 库位ID
        /// </summary>
        public double LocId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal RecQty { get; set; }

        /// <summary>
        /// 退货序列号
        /// </summary>
        public List<ReturnSn> ReturnSn { get; set; } = new List<ReturnSn>();
    }

    [Serializable]
    public class ReturnSn
    {
        /// <summary>
        /// 序列号
        /// </summary>
        public string Sn { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }
    }
}
