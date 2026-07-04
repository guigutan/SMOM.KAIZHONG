using SIE.ObjectModel;

namespace SIE.Resources.WipResources
{
    /// <summary>
    /// 资源类型
    /// </summary> 
    public enum ResourceType
    {
        /// <summary>
        /// 加工单元
        /// </summary>
        [Label("加工单元")]
        ProcessingCell = 1,

        /// <summary>
        /// 集群资源
        /// </summary>
        [Label("集群资源")]
        ClusterResource = 2,

        /// <summary>
        /// 生产线
        /// </summary>
        [Label("生产线")]
        ProductionLine = 4,

        /// <summary>
        /// 副资源
        /// </summary>
        [Label("副资源")]
        SecondaryResource = 8,

        /// <summary>
        /// 无限产能型
        /// </summary>
        [Label("无限产能型")]
        InfiniteCapacity = 16,
    }

    /// <summary>
    /// 资源甘特图类型
    /// </summary> 
    public enum ResourceGanttType
    {
        /// <summary>
        /// 加工单元
        /// </summary>
        [Label("加工单元")]
        ProcessingCell = 1,

        /// <summary>
        /// 生产线
        /// </summary>
        [Label("生产线")]
        ProductionLine = 4,

        /// <summary>
        /// 副资源
        /// </summary>
        [Label("副资源")]
        SecondaryResource = 8,

        /// <summary>
        /// 无限产能型
        /// </summary>
        [Label("无限产能型")]
        InfiniteCapacity = 16,
    }
}