using System;

namespace SIE.LES.StockOrders.Models
{
    /// <summary>
    /// 物料需求数据
    /// </summary>
    [Serializable]
    public class ItemRequireData
    {
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
        /// 本次需求数量
        /// </summary>
        public decimal RequireQty { get; set; }

        /// <summary>
        /// 工单总需求数
        /// </summary>
        public decimal WoTotalQty { get; set; }

        /// <summary>
        /// 工单剩余数量
        /// </summary>
        public decimal WoSurplusQty { get; set; }

        /// <summary>
        /// 需求时间
        /// </summary>
        public DateTime RequireDate { get; set; }

        /// <summary>
        /// 仓库ID
        /// </summary>
        public double? WarehouseId { get; set; }

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string WarehouseCode { get; set; }

        /// <summary>
        /// 库位ID
        /// </summary>
        public double? LocId { get; set; }

        /// <summary>
        /// 库位编码
        /// </summary>
        public string LocCode { get; set; }

        /// <summary>
        /// 是否启用手动接收
        /// </summary>
        public bool IsEnabelManualRec { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProp { get; set; }

        /// <summary>
        /// 物料扩展属性名称
        /// </summary>
        public string ItemExtPropName { get; set; }

        /// <summary>
        /// 是否允许编辑物料扩展属性
        /// </summary>
        public bool IsAllowEdit { get; set; }
    }
}
