using SIE.ObjectModel;

namespace SIE.Tech.Processs.Scripts
{
    /// <summary>
    /// 目标类型
    /// </summary>
    public enum TargetType
    {
        /// <summary>
        /// 工序参数
        /// </summary>
        [Label("工序参数")]
        ProcessParam = 5,
        /// <summary>
        /// 工艺路线工序参数
        /// </summary>
        [Label("工艺路线工序参数")]
        RoutingProcessParam = 10,
        /// <summary>
        /// 工单工序参数
        /// </summary>
        [Label("工单工序参数")]
        WoProcessParam = 15,
        /// <summary>
        /// 产品工艺路线参数
        /// </summary>
        [Label("产品工艺路线参数")]
        ProductRoutingParam = 20,
    }
}