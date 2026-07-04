using SIE.Core.Enums;
using System;
using System.Collections.Generic;

namespace SIE.EventMessages
{
    /// <summary>
    /// 发货计划分析库存数据
    /// </summary>
    [Serializable]
    public class AnalysOnhandData
    {
        /// <summary>
        /// 仓库Id
        /// </summary>
        public double? WarehouseId { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public string LotCode { get; set; }

        /// <summary>
        /// 批次描述
        /// </summary>
        public string LotCodeDesc { get; set; }

        /// <summary>
        /// 可用数
        /// </summary>
        public decimal AvailableQty { get; set; }

        /// <summary>
        /// 物料Id 
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 数量来源（0-库存 1-调拨 2-采购 3-在制）
        /// </summary>
        public int QtyFrom { get; set; }

        /// <summary>
        /// 完工日期或交货日期
        /// </summary>
        public DateTime? FinishDate { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtPropName { get; set; }

        /// <summary>
        /// 标识
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Lpn
        /// </summary>
        public string Lpn { get; set; }

        /// <summary>
        /// 库区
        /// </summary>
        public double? AreaId { get; set; }

        /// <summary>
        /// 库位
        /// </summary>
        public double? LocationId { get; set; }

        /// <summary>
        /// 项目号
        /// </summary>
        public string ProjectNo { get; set; }

        /// <summary>
        /// 任务号
        /// </summary>
        public string TaskNo { get; set; }

        /// <summary>
        /// 货主
        /// </summary>
        public string StoreCode { get; set; }
    }

    /// <summary>
    /// ASN工单收货数据
    /// </summary>
    public class AsnWoData
    {
        /// <summary>
        /// 工单号
        /// </summary>
        public string Wo { get; set; }

        /// <summary>
        /// 收货数量
        /// </summary>
        public decimal ReceiptQty { get; set; }
    }

    /// <summary>
    /// ASN单数据
    /// </summary>
    public class AsnData
    {
        /// <summary>
        /// ASN单号
        /// </summary>
        public string AsnNo { get; set; }

        /// <summary>
        /// 供应商
        /// </summary>
        public string SupplierCode { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName { get; set; }

        /// <summary>
        /// 订单类型
        /// </summary>
        public OrderType OrderType { get; set; }

        /// <summary>
        /// 单据小类
        /// </summary>
        public string TransactionCode { get; set; }

        /// <summary>
        /// 收货仓库
        /// </summary>
        public string WarehouseCode { get; set; }

        /// <summary>
        /// 收货仓库名称
        /// </summary>
        public string WarehouseName { get; set; }

        /// <summary>
        /// 优先级
        /// </summary>
        public int PriorityType { get; set; }
    }

    /// <summary>
    /// 收货明细数据
    /// </summary>
    [Serializable]
    public class AsnDetailData
    {
        /// <summary>
        /// ASN单号
        /// </summary>
        public string AsnNo { get; set; }

        /// <summary>
        /// 收货明细行号
        /// </summary>
        public string LineNo { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 物料类型
        /// </summary>
        public int ItemType { get; set; }

        /// <summary>
        /// 规格型号
        /// </summary>
        public string ItemSpecificationModel { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string ItemUnitName { get; set; }

        /// <summary>
        /// 物料是否启用扩展属性
        /// </summary>
        public bool ItemEnableExtendProperty { get; set; }

        /// <summary>
        /// 收货数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 未建单可退货数
        /// </summary>
        public decimal UnCreateReturnQty { get; set; }

        /// <summary>
        /// 采购订单ID
        /// </summary>
        public double PurchaseOrderId { get; set; }

        /// <summary>
        /// 采购订单号
        /// </summary>
        public string PurchaseOrderNo { get; set; }

        /// <summary>
        /// 采购订单明细ID
        /// </summary>
        public double PurchaseOrderDetailId { get; set; }

        /// <summary>
        /// 采购订单明细行号
        /// </summary>
        public string PurchaseOrderDetailLineNo { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 创建ren
        /// </summary>
        public string CreateByName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public string UpdateByName { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdateDate { get; set; }


        /// <summary>
        /// 辅助单位Id
        /// </summary>
        public double SecondUnitId
        {
            get;
            set;
        }

        /// <summary>
        /// 分子
        /// </summary>
        public decimal Numerator
        {
            get;
            set;
        }

        /// <summary>
        /// 分母
        /// </summary>
        public decimal Denominator
        {
            get;
            set;
        }

        /// <summary>
        /// 转换率
        /// </summary>
        public decimal ConvertFigre
        {
            get;
            set;
        }

        /// <summary>
        /// 辅助单位名称
        /// </summary>
        public string SecondUnitName
        {
            get;
            set;
        }

        public int SecondPrecision { get; set; } = 3;

        public int MainPrecision { get; set; } = 3;

        public int SecondTrade { get; set; }

        public int MainTrade { get; set; }

    }


    /// <summary>
    /// 其他入库ASN明细参数数据
    /// </summary>
    [Serializable]
    public class OtherInAsnDtlParamData
    {
        /// <summary>
        /// 库存组织
        /// </summary>
        public int? InvOrgId { get; set; }

        /// <summary>
        /// 0 两步调拨  1跨组织调拨
        /// </summary>
        public int AlloteType { get; set; }

        /// <summary>
        /// 来源仓库
        /// </summary>
        public double? SourceWhId { get; set; }

        /// <summary>
        /// 收货仓库ID
        /// </summary>
        public double WarehouseId { get; set; }

        /// <summary>
        /// 发运单号
        /// </summary>
        public string SoNo { get; set; }

        /// <summary>
        /// 货主
        /// </summary>
        public string ShipperCode { get; set; }

        /// <summary>
        /// 明细行号
        /// </summary>
        public string SoDtlLineNo { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 物料库存属性
        /// </summary>
        public string ItemExtProp { get; set; }

        /// <summary>
        /// 物料扩展属性名称
        /// </summary>
        public string ItemExtPropName { get; set; }

        /// <summary>
        /// 库存状态
        /// </summary>
        public int OnhandState { get; set; }

        /// <summary>
        /// 批次ID
        /// </summary>
        public double LotId { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public string LotCode { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 没有转换前的数量
        /// </summary>
        public decimal NoChangeQty { get; set; }

        /// <summary>
        /// 项目号
        /// </summary>
        public string ProjectNo { get; set; }

        /// <summary>
        /// 任务号
        /// </summary>
        public string TaskNo { get; set; }

        /// <summary>
        /// 来源ERP库存组织名称
        /// </summary>
        public string FromErpOrgName { get; set; }

        /// <summary>
        /// 分配Id
        /// </summary>
        public string AssignId { get; set; }

        /// <summary>
        /// 库存组织Id
        /// </summary>
        public double LotLpnOnhandId { get; set; }

        /// <summary>
        /// 转换率
        /// </summary>
        public decimal? ConvertConfig { get; set; }

        /// <summary>
        /// 收货库位
        /// </summary>
        public double StorageLocationId { get; set; }

        /// <summary>
        /// 分配明细Id
        /// </summary>
        public double AssignDtlId { get; set; }

        /// <summary>
        /// 分配对应的序列号
        /// </summary>
        public List<string> Sns { get; set; } = new List<string>();

        /// <summary>
        /// 是否导入序列号
        /// </summary>
        public bool IsImportSn { get; set; }

        /// <summary>
        /// 来源库存组织名称
        /// </summary>
        public string SourceInvOrgName { get; set; }

        /// <summary>
        /// 在途仓库Id
        /// </summary>
        public double OnWayWhId { get; set; }

        /// <summary>
        /// 在途仓库Id
        /// </summary>
        public double OnWayLocId { get; set; }

        /// <summary>
        /// 来源物料Id
        /// </summary>
        public double FromItemId { get; set; }

        /// <summary>
        /// 来源扩展属性
        /// </summary>
        public string FromItemExtProp { get; set; }
    }

    [Serializable]
    public class OutRetAsnParam
    {

        /// <summary>
        /// 供应商
        /// </summary>
        public double SupplierId { get; set; }

        /// <summary>
        /// 收货仓库ID
        /// </summary>
        public double WarehouseId { get; set; }

        /// <summary>
        /// 虚仓
        /// </summary>
        public bool WhIsVirtual { get; set; }

        /// <summary>
        /// 收货库位
        /// </summary>
        public double StorageLocationId { get; set; }

        /// <summary>
        /// 发运单号
        /// </summary>
        public string SoNo { get; set; }

        /// <summary>
        /// 发运单行号
        /// </summary>
        public string SoDtlLineNo { get; set; }

        /// <summary>
        /// 货主
        /// </summary>
        public string ShipperCode { get; set; }

        /// <summary>
        /// 分配明细Id
        /// </summary>
        public double AssignDtlId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 采购订单BomId
        /// </summary>
        public double PoBomId { get; set; }

        /// <summary>
        /// 采购订单明细Id
        /// </summary>
        public double PoDtlId { get; set; }

        /// <summary>
        /// 采购订单Id
        /// </summary>
        public double PoId { get; set; }

        /// <summary>
        /// 物料
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 扩展属性
        /// </summary>
        public string ItemExtProp { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtPropName { get; set; }
    }

    /// <summary>
    ///  离线ASN明细数据
    /// </summary>
    [Serializable]
    public class OutLineAsnDtlDataSimple
    {
        /// <summary>
        /// 物料
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 单号
        /// </summary>
        public string AsnNo { get; set; }

        /// <summary>
        /// 批次
        /// </summary>
        public string Lot { get; set; }
    }

    /// <summary>
    /// 退料创建ASN数据
    /// </summary>
    [Serializable]
    public class ReturnMaterialData
    {
        /// <summary>
        /// 库存组织Id
        /// </summary>
        public double InvOrgId { get; set; }

        /// <summary>
        /// 退料单号
        /// </summary>
        public string BillNo { get; set; }

        /// <summary>
        /// 生产部门
        /// </summary>
        public string EnterpriseCode { get; set; }

        /// <summary>
        /// 生产部门ID(可不传)
        /// </summary>
        public double? EnterpriseId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 单据小类 0-工单退料/生产退料 1-车间退料
        /// </summary>
        public int ReType { get; set; }

        /// <summary>
        /// 行号
        /// </summary>
        public string LineNo { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string WoNo { get; set; }

        /// <summary>
        /// 按原批次退
        /// </summary>
        public bool IsReturnLot { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public string LotCode { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 扩展属性
        /// </summary>
        public string ItemExtProp { get; set; }

        /// <summary>
        /// 扩展属性
        /// </summary>
        public string ItemExtPropName { get; set; }

        /// <summary>
        /// 退料数（辅）
        /// </summary>
        public decimal SecondQty { get; set; }

        /// <summary>
        /// 辅助单位ID
        /// </summary>
        public double SecondUnitId { get; set; }

        /// <summary>
        /// 辅助单位名称
        /// </summary>
        public string SecondUnitName { get; set; }

        /// <summary>
        /// 主单位数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 主单位ID
        /// </summary>
        public double UnitId { get; set; }

        /// <summary>
        /// 库存状态 10-合格 20-不合格
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// 线边仓Id
        /// </summary>
        public double? WarehouseId { get; set; }

        /// <summary>
        /// 线边仓库位Id
        /// </summary>
        public double? LocId { get; set; }

        /// <summary>
        /// 接收仓库
        /// </summary>
        public double? ReceiveWarehouseId { get; set; }

        /// <summary>
        /// 项目号
        /// </summary>
        public string ProjectNo { get; set; }

        /// <summary>
        /// 来源标签号
        /// </summary>
        public string SourceLabel { get; set; }
    }

    /// <summary>
    /// 采购明细数据
    /// </summary>
    [Serializable]
    public class PoDtlData
    {
        /// <summary>
        /// 可退货数
        /// </summary>
        public decimal CanReturnQty { get; set; }

        /// <summary>
        /// 采购单号
        /// </summary>
        public string PoNo { get; set; }

        /// <summary>
        /// 采购明细行号
        /// </summary>
        public string PoLineNo { get; set; }

        /// <summary>
        /// 采购单Id
        /// </summary>
        public double PoId { get; set; }

        /// <summary>
        /// 采购明细Id
        /// </summary>
        public double PoDtlId { get; set; }

        /// <summary>
        /// ASN明细Id
        /// </summary>
        public double AsnDtlId { get; set; }

        /// <summary>
        /// 任务号
        /// </summary>
        public string TaskNo { get; set; }

        /// <summary>
        /// 项目号
        /// </summary>
        public string ProjectNo { get; set; }

        /// <summary>
        /// 货主
        /// </summary>
        public string StorerCode { get; set; }

        /// <summary>
        /// 是否不合格
        /// </summary>
        public bool IsFail { get; set; }

        /// <summary>
        /// 是否不合格筛选
        /// </summary>
        public bool IsPick { get; set; }
    }

    /// <summary>
    /// 明细数据
    /// </summary>
    [Serializable]
    public class AsnDtlData
    {
        /// <summary>
        /// ASN单号
        /// </summary>
        public string AsnNo { get; set; }

        /// <summary>
        /// 行号
        /// </summary>
        public string LineNo { get; set; }

        /// <summary>
        /// 收货数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 明细Id
        /// </summary>
        public double AsnDtlId { get; set; }

        /// <summary>
        /// 扩展属性
        /// </summary>
        public string ItemExtProp { get; set; }

        /// <summary>
        /// 扩展属性
        /// </summary>
        public string ItemExtPropName { get; set; }

        /// <summary>
        /// 启用送货明细
        /// </summary>
        public bool IsEnableDelivery { get; set; }

        /// <summary>
        /// 采购明细id
        /// </summary>
        public double? PoDtlId { get; set; }

        /// <summary>
        /// 合格数
        /// </summary>
        public decimal QualifiedQty { get; set; }

        /// <summary>
        /// 不合格数
        /// </summary>
        public decimal UnQualifiedQty { get; set; }

        /// <summary>
        /// 送货明细数据
        /// </summary>
        public List<DeliveryData> DeliveryDatas { get; set; } = new List<DeliveryData>();

    }

    [Serializable]
    public class DeliveryData
    {
        /// <summary>
        /// 送货明细Id
        /// </summary>
        public double AsnDeliveryId { get; set; }

        /// <summary>
        /// 采购明细Id
        /// </summary>
        public double? PoDtlId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 类型 1 合格数 0不合格数
        /// </summary>
        public int QtyType { get; set; }
    }



    /// <summary>
    /// 调拨创建ASN单返回数据
    /// </summary>
    [Serializable]
    public class AllotAsnData
    {
        /// <summary>
        /// 分配明细Id
        /// </summary>
        public double AssignDtlId { get; set; }

        /// <summary>
        /// 收货明细Id
        /// </summary>
        public double AsnDtlId { get; set; }

        /// <summary>
        /// 收货单号
        /// </summary>
        public string AsnNo { get; set; }

        /// <summary>
        /// ASN单Id
        /// </summary>
        public double AsnId { get; set; }
    }
}
