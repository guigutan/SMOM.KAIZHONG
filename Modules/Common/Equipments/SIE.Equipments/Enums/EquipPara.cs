using SIE.ObjectModel;

namespace SIE.Equipments.Enums
{
    /// <summary>
    /// 设备物联设备参数
    /// </summary>
    public enum EquipPara
    {
        /// <summary>
        /// 手动取值
        /// </summary>
        [Label("手动取值")]
        ManualValue,

        /// <summary>
        /// 自动取值
        /// </summary>
        [Label("自动取值")]
        AutomaticValue,
    }
}
