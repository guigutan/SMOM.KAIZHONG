using SIE.Tech.Processs;
using System;

namespace SIE.MES.BatchWIP
{
    /// <summary>
    /// 批次采集明细查询
    /// </summary>
    [Serializable]
    public class CollectDetailQuery
    {
        /// <summary>
        /// 操作人
        /// </summary>
        public double OperateById { get; set; }

        /// <summary>
        /// 资源ID
        /// </summary>
        public double ResourceId { get; set; }

        /// <summary>
        /// 工序ID
        /// </summary>
        public double ProcessId { get; set; }

        /// <summary>
        /// 工位ID
        /// </summary>
        public double StationId { get; set; }

        /// <summary>
        /// 工单ID
        /// </summary>
        public double WorkOrderId { get; set; }

        /// <summary>
        /// 出入类型
        /// </summary>
        public PlugType PlugType { get; set; }
    }
}