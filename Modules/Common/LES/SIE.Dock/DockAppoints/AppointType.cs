using SIE.ObjectModel;

namespace SIE.Dock.DockAppoints
{
    /// <summary>
    /// 预约类型
    /// </summary>
    public enum AppointType
    {
        /// <summary>
        /// 送货
        /// </summary>
        [Label("送货")]
        Delivery,

        /// <summary>
        /// 提货
        /// </summary>
        [Label("提货")]
        PickUp,
    }
}