using System;

namespace SIE.Fixtures.Models
{
    /// <summary>
    /// 仓库库位信息
    /// </summary>
    [Serializable]
    public class WareLocationInfo
    {
        /// <summary>
        /// 仓库Id
        /// </summary>
        public double WarehouseId { get; set; }

        /// <summary>
        /// 仓库
        /// </summary>
        public string Warehouse { get; set; }

        /// <summary>
        /// 库位ID
        /// </summary>
        public double LocationId { get; set; }

        /// <summary>
        /// 库位
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 在库数
        /// </summary>
        public decimal Qty { get; set; }
    }
}
