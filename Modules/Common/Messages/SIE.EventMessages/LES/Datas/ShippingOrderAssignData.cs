using System;
using System.Collections.Generic;

namespace SIE.EventMessages.LES
{
    /// <summary>
    /// 发运单数据
    /// </summary>
    [Serializable]
    public class ShippingOrderData
    {
        /// <summary>
        /// 发运单号
        /// </summary>
        public string BillNo { get; set; }
        /// <summary>
        /// 备料单号
        /// </summary>
        public string SourceNo { get; set; }
        /// <summary>
        /// 单据小类
        /// </summary>
        public string TransactionCode { get; set; }
        /// <summary>
        /// 单据小类名称
        /// </summary>
        public string TransactionName { get; set; }

        /// <summary>
        /// 交货日期
        /// </summary>
        public DateTime? DeliveryDate { get; set; }

        /// <summary>
        /// 生产部门
        /// </summary>
        public double? EnterpriseId { get; set; }
        /// <summary>
        /// 生产部门
        /// </summary>
        public string EnterpriseCode { get; set; }
        /// <summary>
        /// 生产部门名称
        /// </summary>
        public string EnterpriseName { get; set; }

        /// <summary>
        /// 发货仓库Id
        /// </summary>
        public double ShippingWarehouseId { get; set; }
        /// <summary>
        /// 发货仓库编码
        /// </summary>
        public string ShippingWarehouseCode { get; set; }
        /// <summary>
        /// 发货仓库名称
        /// </summary>
        public string ShippingWarehouseName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateByName { get; set; }
        /// <summary>
        /// 总数
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// 发运单明细列表
        /// </summary>
        public List<ShippingOrderDetailData> DetailLists { get; set; } = new List<ShippingOrderDetailData>();

    }
    /// <summary>
    /// 发运单明细
    /// </summary>
    [Serializable]
    public class ShippingOrderDetailData
    {
        /// <summary>
        /// 发运单号
        /// </summary>
        public string BillNo { get; set; }

        /// <summary>
        /// 发运单行号
        /// </summary>
        public string SoLineNo { get; set; }
        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 物料扩展属性名称
        /// </summary>
        public string ItemExtProp { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtPropName { get; set; }
        /// <summary>
        /// 项目号
        /// </summary>
        public string ProjectNo { get; set; }

        /// <summary>
        /// 物料单位
        /// </summary>
        public string ItemUnitName { get; set; }


        /// <summary>
        /// 拣货数
        /// </summary>
        public decimal PickQty { get; set; }
        /// <summary>
        /// 拣货数（辅）
        /// </summary>
        public decimal SecondPickQty { get; set; }

        /// <summary>
        /// 辅助单位Id
        /// </summary>
        public double SecondUnitId { get; set; }

        /// <summary>
        /// 辅助单位 （辅）
        /// </summary>
        public string SecondUnitName { get; set; }

        /// <summary>
        /// 对应序列号
        /// </summary>
        public List<ShippingOrderSnData> SnList { get; set; } = new List<ShippingOrderSnData>();
    }
    /// <summary>
    /// 发运单分配明细
    /// </summary>
    [Serializable]
    public class ShippingOrderAssignData
    {
        /// <summary>
        /// 发运单号
        /// </summary>
        public string BillNo { get; set; }

        /// <summary>
        /// 来源单号
        /// </summary>
        public string SourceNo { get; set; }

        /// <summary>
        /// 分配Id
        /// </summary>
        public double SoAssignId { get; set; }

        /// <summary>
        /// 分配Id
        /// </summary>
        public string AssignId { get; set; }

        /// <summary>
        /// 相关单号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 相关单行号
        /// </summary>
        public string OrderLineNo { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 物料扩展属性名称
        /// </summary>
        public string ItemExtProp { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtPropName { get; set; }

        /// <summary>
        /// 物料单位
        /// </summary>
        public string ItemUnitName { get; set; }

        /// <summary>
        /// 是否序列号管理
        /// </summary>
        public bool IsSerialNumber { get; set; }

        /// <summary>
        /// 拣货数
        /// </summary>
        public decimal PickQty { get; set; }
        /// <summary>
        /// 拣货数（辅）
        /// </summary>
        public decimal SecondPickQty { get; set; }

        /// <summary>
        /// 辅助单位Id
        /// </summary>
        public double SecondUnitId { get; set; }

        /// <summary>
        /// 辅助单位 （辅）
        /// </summary>
        public string SecondUnitName { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public string LotCode { get; set; }

        /// <summary>
        /// 是否合并拣货
        /// </summary>
        public bool IsMerge { get; set; }

        /// <summary>
        /// 目前LPN
        /// </summary>
        public string TargetLpn { get; set; }

        /// <summary>
        /// 发运单行号
        /// </summary>
        public string SoLineNo { get; set; }

        /// <summary>
        /// 对应序列号
        /// </summary>
        public List<ShippingOrderSnData> SnList { get; set; } = new List<ShippingOrderSnData>();
    }
    /// <summary>
    /// 发运单SN
    /// </summary>
    [Serializable]
    public class ShippingOrderSnData
    {

        /// <summary>
        /// SN
        /// </summary>
        public string SN { get; set; }

        /// <summary>
        /// 发运单号
        /// </summary>
        public string BillNo { get; set; }
        /// <summary>
        /// 来源单号
        /// </summary>
        public string SourceNo { get; set; }

        /// <summary>
        /// 分配Id
        /// </summary>
        public string AssignId { get; set; }


        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }
        /// <summary>
        /// 拣货数（辅）
        /// </summary>
        public decimal SecondPickQty { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public string LotCode { get; set; }
        /// <summary>
        /// 是否序列号管理
        /// </summary>
        public bool IsSerialNumber { get; set; }

        /// <summary>
        /// 是否合并拣货
        /// </summary>
        public bool IsMerge { get; set; }

        /// <summary>
        /// 项目号
        /// </summary>
        public string ProjectNo { get; set; }


    }

    /// <summary>
    /// 获取发运单
    /// </summary>
    [Serializable]
    public class QueryShipDataParam
    {
        /// <summary>
        /// 库存组织Id
        /// </summary>
        public int InvOrgId { get; set; }

        /// <summary>
        /// 发运单号、备料单号
        /// </summary>
        public string SoNo { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string WoNo { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }
        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 车间
        /// </summary>
        public string WorkShop { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Resource { get; set; }
        /// <summary>
        /// 发货仓库
        /// </summary>
        public string ShippingWarehouse { get; set; }
        /// <summary>
        /// 创建时间/需求时间
        /// </summary>
        public DateTime? BeginTime { get; set; }
        /// <summary>
        /// 创建时间/需求时间
        /// </summary>
        public DateTime? EndTime { get; set; }


        /// <summary>
        /// PageNumber
        /// </summary>
        public int PageNumber { get; set; }
        /// <summary>
        /// PageSize
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 关键字
        /// </summary>
        public string Keyword { get; set; }


    }
    /// <summary>
    /// 获取发运单明细
    /// </summary>
    [Serializable]
    public class QueryShipDetailDataParam : QueryShipDetailDataParambase
    {

        /// <summary>
        /// 生产部门Id
        /// </summary>
        public double EnterpriseId { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string WoNo { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }
        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 标签条码
        /// </summary>
        public string Barcode { get; set; }
    }

    /// <summary>
    /// 获取发运单明细
    /// </summary>
    [Serializable]
    public class QueryShipDetailDataParambase
    {
        /// <summary>
        /// 库存组织Id
        /// </summary>
        public int InvOrgId { get; set; }

        /// <summary>
        /// 发运单号、备料单号
        /// </summary>
        public string SoNo { get; set; }
    }

    /// <summary>
    /// 发运单序列号明细
    /// </summary>
    [Serializable]
    public class ShippingOrderAssignSnData : ShippingOrderAssignData
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string Sn { get; set; }

    }
}
