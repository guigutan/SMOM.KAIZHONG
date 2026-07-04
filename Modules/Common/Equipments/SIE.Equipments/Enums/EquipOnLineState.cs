using SIE.ObjectModel;

namespace SIE.Equipments.Enums
{
    /// <summary>
    /// 设备在线状态
    /// </summary>
    public enum EquipOnLineState
    {
        /// <summary>
        /// 离线
        /// </summary>
        [Label("离线")]
        OffLine = 0,

        /// <summary>
        /// 在线
        /// </summary>
        [Label("在线")]
        OnLine = 1,
    }
}
