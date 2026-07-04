using System;
using System.Collections.Generic;

namespace SIE.EventMessages.WMS.Traces
{
    /// <summary>
    ///  库存追溯-库存信息
    /// </summary>
    [Serializable]
    public class WmsItemOnhandInfo
    {
        /// <summary>
        /// 总条数
        /// </summary>
        public int TotalCount { get; set; } = 0;

        /// <summary>
        /// 数据
        /// </summary>
        public List<WmsItemOnhandData> Data { get; set; }= new List<WmsItemOnhandData>();
    }


    /// <summary>
    /// 库存追溯-库存详细信息
    /// </summary>
    [Serializable]
    public class WmsItemOnhandData
    {
        /// <summary>
        /// 库存Id
        /// </summary>s
        public double? OnhandId { get; set; }

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string WarehouseCode { get; set; }

        /// <summary>
        /// 库位编码
        /// </summary>
        public string StorageLocationCode { get; set; }

        /// <summary>
        /// 库存状态
        /// </summary>
        public string OnhandState { get; set; }

        /// <summary>
        /// 现有量
        /// </summary>
        public decimal? Qty { get; set; }

        /// <summary>
        /// Asn单号
        /// </summary>
        public string AsnNo { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName { get; set; }

        /// <summary>
        /// 生产批次
        /// </summary>
        public string ProductBatch { get; set; }

        /// <summary>
        /// 生产日期
        /// </summary>
        public DateTime? ProductionDate { get; set; }

        /// <summary>
        /// 收货日期
        /// </summary>
        public DateTime? CollectDate { get; set; }

    }
}
