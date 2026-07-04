using System;

namespace SIE.EventMessages.SupplyOnway
{
    /// <summary>
    /// 采购单数据
    /// </summary>
    [Serializable]
    public class PoInfo
    {
        /// <summary>
        /// 采购单号
        /// </summary>
        public string PoNo { get; set; }

        /// <summary>
        /// 采购单明细行号
        /// </summary>
        public string PoDtlLineNo { get; set; }

        /// <summary>
        /// 未收数
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 交货日期
        /// </summary>
        public DateTime DeliveryDate { get; set; }

        /// <summary>
        /// 采购单来源主键
        /// </summary>
        public string PoSourceKey { get; set; }

        /// <summary>
        /// 采购单明细来源主键
        /// </summary>
        public string PoDtlSourceKey { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 供应商ID
        /// </summary>
        public double SupplierId { get; set; }

        /// <summary>
        /// 仓库ID
        /// </summary>
        public double? WarehouseId { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProp { get; set; }

        /// <summary>
        /// 物料扩展属性名称
        /// </summary>
        public string ItemExtPropName { get; set; }
    }
}
