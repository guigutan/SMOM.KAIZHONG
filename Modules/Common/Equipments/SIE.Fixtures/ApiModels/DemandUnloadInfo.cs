using System;

namespace SIE.Fixtures.ApiModels
{
    /// <summary>
    /// 工治具需求出库信息
    /// </summary>
    [Serializable]
    public class DemandUnloadInfo
    {
        /// <summary>
        /// 需求单号
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// ID编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 仓库Id
        /// </summary>
        public double WarehouseId { get; set; }

        /// <summary>
        /// 库位Id
        /// </summary>
        public double LocationId { get; set; }

        /// <summary>
        /// 载具
        /// </summary>
        public string ToolCode { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Qty { get; set; }
    }
}