using SIE.ObjectModel;

namespace SIE.Equipments.Enums
{
    /// <summary>
    /// 设备运行状态
    /// </summary>
    public enum EquipRunningState
    {
        /// <summary>
        /// 未知
        /// </summary>
        [Label("未知")]
        Unknown = 0,

        /// <summary>
        /// 运行
        /// </summary>
        [Label("运行")]
        Running = 1,

        /// <summary>
        /// 停机
        /// </summary>
        [Label("停机")]
        Halted = 2,

        /// <summary>
        /// 故障
        /// </summary>
        [Label("故障")]
        Breakdown = 3,

        /// <summary>
        /// 关机
        /// </summary>
        [Label("关机")]
        Shutdowned = 4,
    }
}
