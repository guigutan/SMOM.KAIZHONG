using System;

namespace SIE.EventMessages
{
    /// <summary>
    /// 包装完成后提供入库的相关信息
    /// </summary>
    [Serializable]
    public class PackingCompleteEvent
    {
        /// <summary>
        /// 入库的包装
        /// </summary>
        public double RootPackingRelationId { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>
        public double ProductId { get; set; }

        /// <summary>
        /// 资源ID
        /// </summary>
        public double ResourceId { get; set; }

        /// <summary>
        /// 班组ID 
        /// </summary>
        public double? WorkerGroupId { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public double UserId { get; set; }

        /// <summary>
        /// 目标仓库ID
        /// </summary>
        public double WarehouseId { get; set; }

        /// <summary>
        /// 工位ID
        /// </summary>
        public double StationId { get; set; }
    }
}
