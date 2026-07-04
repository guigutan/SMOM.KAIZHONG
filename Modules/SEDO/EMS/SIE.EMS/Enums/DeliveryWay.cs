using SIE.ObjectModel;

namespace SIE.EMS.Enums
{
    /// <summary>
    /// 发货方式
    /// </summary>
    [Label("发货方式")]
    public enum DeliveryWay
    {
        /// <summary>
        /// 自提
        /// </summary>
        [Label("自提")]
        Myself = 0,
        /// <summary>
        /// 快递
        /// </summary>
        [Label("快递")]
        Courier = 1,
    }
}
