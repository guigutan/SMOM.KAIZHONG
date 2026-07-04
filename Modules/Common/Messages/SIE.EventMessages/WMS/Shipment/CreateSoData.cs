using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EventMessages.WMS.Shipment
{
    /// <summary>
    /// 创建发运单数据
    /// </summary>
    [Serializable]
    public class MesMoveCreateSoData
    {
        /// <summary>
        /// 备料单号
        /// </summary>
        public string BillNo { get; set; }

        /// <summary>
        /// 车间Id
        /// </summary>
        public double WorkShopId { get; set; }

        /// <summary>
        /// 标签数据
        /// </summary>
        public List<MesLabelData> MesLabelDatas { get; set; } = new List<MesLabelData>();

        /// <summary>
        /// 目标仓库Id
        /// </summary>
        public double? TargetWarhouseId { get; set; }

        /// <summary>
        /// 目标库位Id
        /// </summary>
        public double? TargetStorageLocationId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 是否更新库存(MES不用管这个字段)
        /// </summary>
        public bool IsUpdateOnhand { get; set; }
    }

    /// <summary>
    /// 标签数据
    /// </summary>
    [Serializable]
    public class MesLabelData
    {
        /// <summary>
        /// 行号
        /// </summary>
        public string LineNo { get; set; }

        /// <summary>
        /// 标签号
        /// </summary>
        public string LabelNo { get; set; }

        /// <summary>
        /// 挪料来源库位
        /// </summary>
        public double StorageLocationId { get; set; }

        /// <summary>
        /// 仓库Id
        /// </summary>
        public double WarehouseId { get; set; }

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string WarehouseCode { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public string LotCode { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProp { get; set; }

        /// <summary>
        /// 物料扩展属性名称
        /// </summary>
        public string ItemExtPropName { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 不合格数量
        /// </summary>
        public bool IsFail { get; set; }
    }

    /// <summary>
    /// 走账数据
    /// </summary>
    [Serializable]
    public class WMSTranData
    {
        /// <summary>
        /// 仓库Id
        /// </summary>
        public double WarehouseId { get; set; }

        /// <summary>
        /// 库位Id
        /// </summary>
        public double StorageLocationId { get; set; }
    }

    /// <summary>
    /// 委外耗料单发运单数据
    /// </summary>
    [Serializable]
    public class OutUseBillData
    {
        /// <summary>
        /// 发货仓库Id
        /// </summary>
        public double WarehouseId { get; set; }

        /// <summary>
        /// 指定库位ID
        /// </summary>
        public double AppointLocId { get; set; }

        /// <summary>
        /// 来源单号
        /// </summary>
        public string AsnNo { get; set; }

        /// <summary>
        /// 货主编码
        /// </summary>
        public string StorerCode { get; set; }

        /// <summary>
        /// 货主名称
        /// </summary>
        public string StorerName { get; set; }

        /// <summary>
        /// 处理方式
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 客户Id
        /// </summary>
        public double? CustomerId { get; set; }

        /// <summary>
        /// 关联ID 按送货明细-收货分配ID 不按送货明细-收货明细ID
        /// </summary>
        public double BillDtlId { get; set; }

        /// <summary>
        /// 供应商ID
        /// </summary>
        public double? SupplierId { get; set; }

        /// <summary>
        /// 发运单明细数据
        /// </summary>

        public List<OutUseBillDetailData> DetailDatas = new List<OutUseBillDetailData>();

    }

    /// <summary>
    /// 创建耗料单回写
    /// </summary>
    [Serializable]
    public class ReturnUseBillData
    {
        /// <summary>
        /// 明细ID或者收货分配明细
        /// </summary>
        public double BillDtlId { get; set; }

        /// <summary>
        /// ASN单号
        /// </summary>
        public string AsnNo { get; set; }

        /// <summary>
        /// 发运单号
        /// </summary>
        public string ShippmentNo { get; set; }
    }
    /// <summary>
    /// 耗料单明细数据
    /// </summary>
    [Serializable]
    public class OutUseBillDetailData
    {
        /// <summary>
        /// 物料ID
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
        /// 订货数
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 采购订单ID
        /// </summary>
        public double? PurOrderId { get; set; }

        /// <summary>
        /// 采购订单明细ID
        /// </summary>
        public double PurOrderDtlId { get; set; }

        /// <summary>
        /// 采购订单明细BomID
        /// </summary>
        public double PurOrderDtlBomId { get; set; }
    }
}
