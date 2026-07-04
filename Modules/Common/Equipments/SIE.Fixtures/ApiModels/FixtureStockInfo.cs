using System;

namespace SIE.Fixtures.ApiModels
{
    /// <summary>
    /// 推荐位置信息
    /// </summary>
    [Serializable]
    public class FixtureStockInfo
    {
        /// <summary>
        /// ID编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 仓库Id
        /// </summary>
        public double WarehouseId { get; set; }

        /// <summary>
        /// 仓库
        /// </summary>
        public string Warehouse { get; set; }

        /// <summary>
        /// 库位Id
        /// </summary>
        public double LocationId { get; set; }

        /// <summary>
        /// 库位
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 库存数
        /// </summary>
        public int Qty { get; set; }
    }
}