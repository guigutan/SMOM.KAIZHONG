using SIE.ObjectModel;

namespace SIE.MES.WorkOrders.Configs
{
    /// <summary>
    /// 工单工序BOM数据类型
    /// </summary>
    public enum WorkOrderBomSourceType
    {
        /// <summary>
        /// 工艺路线
        /// </summary>
        [Label("工艺路线")]
        RoutingProcessBom = 0,
        /// <summary>
        /// 产品工序BOM
        /// </summary>
        [Label("产品工序BOM")]
        ProductRoutingVersionBom = 1,
    }
}
