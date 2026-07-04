using System;

namespace SIE.EventMessages.WMS.Inventory
{

    /// <summary>
    /// 获取库存参数
    /// </summary>
    [Serializable]
    public class LotLpnOnhandParams
    {
        /// <summary>
        /// 物料
        /// </summary>
        public double? ItemId { get; set; } 

        /// <summary>
        /// 仓库
        /// </summary>
        public double? WarehouseId { get; set; }

        /// <summary>
        /// 库位
        /// </summary>
        public double? LocationId { get; set; }         
    }

    /// <summary>
    /// 库存数据
    /// </summary>
    [Serializable]
    public class LotLpnOnhandDataInfo
    {
        /// <summary>
        /// 物料
        /// </summary>
        public double? ItemId { get; set; }

        /// <summary>
        /// 仓库
        /// </summary>
        public double? WarehouseId { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProp { get; set; }

        /// <summary>
        /// 可用量
        /// </summary>
        public decimal AvailableQty { get; set; }
    }
}
