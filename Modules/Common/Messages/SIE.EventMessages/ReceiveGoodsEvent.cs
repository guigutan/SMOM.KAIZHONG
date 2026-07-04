using SIE.ObjectModel;
using System.Collections.Generic;

namespace SIE.EventMessages
{
    /// <summary>
    /// WMS来料接收信息
    /// </summary>
    public class ReceiveGoodsEventList
    {
        public List<ReceiveGoodsEvent> ReceiveGoodsEvent = new List<ReceiveGoodsEvent>();
    }

    /// <summary>
    /// WMS来料接收信息
    /// </summary>
    public class ReceiveGoodsEvent
    {
        /// <summary>
        /// IQC报检记录ID
        /// </summary>
        public double InspectionId { get; set; }

        /// <summary>
        /// 采购单ID
        /// </summary>
        public double? PoId { get; set; }

        /// <summary>
        /// 采购单号
        /// </summary>
        public string PoNo { get; set; }

        /// <summary>
        /// ASN送货单ID
        /// </summary>
        public double AsnId { get; set; }

        /// <summary>
        /// ASN送货单号
        /// </summary>
        public string AsnNo { get; set; }

        /// <summary>
        /// ASN送货单行ID
        /// </summary>
        public double AsnDtlId { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 供应商
        /// </summary>
        public double SupplierId { get; set; }

        /// <summary>
        /// 生产批次号
        /// </summary>
        public string BatchNo { get; set; }

        /// <summary>
        /// 来料批次号
        /// </summary>
        public string Lot { get; set; }

        /// <summary>
        /// 急料标识
        /// </summary>
        public bool IsUrgent { get; set; }

        /// <summary>
        /// 来料类型
        /// </summary>
        public ReceiveGoodType ReceiveGoodType { get; set; }

        /// <summary>
        /// 仓库
        /// </summary>
        public double? WarehouseId { get; set; }
    }

    /// <summary>
    /// 来料类型
    /// </summary>
    public enum ReceiveGoodType
    {
        /// <summary>
        /// 正常
        /// </summary>
        [Label("正常")]
        Normal,
        /// <summary>
        /// 退料
        /// </summary>
        [Label("退料")]
        Return
    }
}
