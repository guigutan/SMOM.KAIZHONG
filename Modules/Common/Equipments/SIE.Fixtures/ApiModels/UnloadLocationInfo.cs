using System;

namespace SIE.Fixtures.ApiModels
{
    /// <summary>
    /// 出库库位信息
    /// </summary>
    [Serializable]
    public class UnloadLocationInfo : ResultDataInfo
    {
        /// <summary>
        /// 库位Id
        /// </summary>
        public double? LocationId { get; set; }

        /// <summary>
        /// 库位
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 仓库Id
        /// </summary>
        public double? WarehouseId { get; set; }

        /// <summary>
        /// 仓库
        /// </summary>
        public string Warehouse { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Qty { get; set; }
    }
}
