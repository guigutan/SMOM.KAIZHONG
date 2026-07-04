using System;

namespace SIE.Fixtures.ApiModels
{
    /// <summary>
    /// 工治具归还信息
    /// </summary>
    [Serializable]
    public class ReturnInfo
    {
        /// <summary>
        /// ID编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 归还数量
        /// </summary>
        public int Qty { get; set; }

        /// <summary>
        /// 产线ID
        /// </summary>
        public double ResourceId { get; set; }

        /// <summary>
        /// 工单ID
        /// </summary>
        public double WorkOrderId { get; set; }
        /// <summary>
        /// 仓库
        /// </summary>

        public double? WarehouseId { get; set; }
    }
}
