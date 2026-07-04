using SIE.ObjectModel;

namespace SIE.ShipPlan
{
    /// <summary>
    /// 发货来源类型
    /// </summary>
    public enum DeliverySourceType
    {
        /// <summary>
        /// 自建
        /// </summary>
        [Label("自建")]
        SelfBuild,

        /// <summary>
        /// 外部
        /// </summary>
        [Label("外部")]
        External,

        /// <summary>
        /// 外部
        /// </summary>
        [Label("ERP数据")]
        Erp,
    }
}
