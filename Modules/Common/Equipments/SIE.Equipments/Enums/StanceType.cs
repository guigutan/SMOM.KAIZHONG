using SIE.ObjectModel;

namespace SIE.Equipments.Enums
{
    /// <summary>
    /// 站位类型
    /// </summary>
    public enum StanceType
    {
        /// <summary>
        /// 带状
        /// </summary>
        [Label("带状")]
        Banding = 5,

        /// <summary>
        /// Tray盘
        /// </summary>
        [Label("Tray盘")]
        Tray = 10,

        /// <summary>
        /// 手贴
        /// </summary>
        [Label("手贴")]
        HandStick = 15,

        /// <summary>
        /// 管装
        /// </summary>
        [Label("管装")]
        Tube = 20,
    }
}