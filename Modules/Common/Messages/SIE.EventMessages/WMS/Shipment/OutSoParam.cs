using SIE.Core.Enums;
using System;
using System.Collections.Generic;

namespace SIE.EventMessages.Shipment
{
    /// <summary>
    /// 虚拟出库数据
    /// </summary>
    [Serializable]
    public class OutSoParam
    {
        /// <summary>
        /// 货主编码
        /// </summary>
        public string ShipperCode { get; set; }

        /// <summary>
        /// 货主名称
        /// </summary>
        public string ShipperName { get; set; }

        /// <summary>
        /// 收货仓库
        /// </summary>
        public double WarehouseId { get; set; }

        ///// <summary>
        ///// 项目号
        ///// </summary>
        //public string ProjectNo { get; set; }

        ///// <summary>
        ///// 任务号
        ///// </summary>
        //public string TaskNo { get; set; }

        /// <summary>
        /// 虚拟出库单明细参数
        /// </summary>
        public List<OutSoDtlParam> OutSoDtlParams { get; set; } = new List<OutSoDtlParam>();

        /// <summary>
        /// 单据小类
        /// </summary>
        public double TransactionId { get; set; }

        /// <summary>
        /// 订单类型
        /// </summary>
        public OrderType OrderType { get; set; }

        /// <summary>
        /// 供应商Id
        /// </summary>
        public double? SupplierId { get; set; }

        /// <summary>
        /// 客户Id
        /// </summary>
        public double? CustomerId { get; set; }

        /// <summary>
        /// 生产部门Id
        /// </summary>
        public double? EnterpriseId { get; set; }
    }

    /// <summary>
    /// 虚拟出库单明细
    /// </summary>
    public class OutSoDtlParam
    {
        /// <summary>
        /// 物料 
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProp { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtPropName { get; set; }

        /// <summary>
        /// 批次
        /// </summary>
        public string Lot { get; set; }

        /// <summary>
        /// 收货库位
        /// </summary>
        public double ReceiveStorageLocationId { get; set; }

        /// <summary>
        /// LPN
        /// </summary>
        public string Lpn { get; set; }

        /// <summary>
        /// 实到数量
        /// </summary>
        public decimal ActualQty { get; set; }

        /// <summary>
        /// ASN单号
        /// </summary>
        public string AsnNo { get; set; }

        /// <summary>
        /// 收货明细行号
        /// </summary>
        public string AsnDtlLineNo { get; set; }

        /// <summary>
        /// 项目号
        /// </summary>
        public string ProjectNo { get; set; }

        /// <summary>
        /// 任务号
        /// </summary>
        public string TaskNo { get; set; }

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
}
