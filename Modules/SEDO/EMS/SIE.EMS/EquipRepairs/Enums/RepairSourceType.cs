using SIE.ObjectModel;

namespace SIE.EMS.EquipRepairs.Enums
{
    /// <summary>
    /// 来源类型
    /// </summary>
    public enum RepairSourceType
    {
        /// <summary>
		/// 点检
		/// </summary>
		[Label("点检")]
        Check = 0,

        /// <summary>
        /// 保养
        /// </summary>
        [Label("保养")]
        Maintain = 1,

        /// <summary>
        /// 手工创建
        /// </summary>
        [Label("手工创建")]
        NewCreated = 2,

        /// <summary>
        /// 特种设备定检
        /// </summary>
        [Label("特种设备定检")]
        RegularInspection = 3,

        /// <summary>
        /// 润滑
        /// </summary>
        [Label("润滑")]
        Lubrication = 4,

        /// <summary>
        /// 计量设备定检
        /// </summary>
        [Label("计量设备定检")]
        Calibration = 5,

        /// <summary>
        /// 计划维修
        /// </summary>
        [Label("计划维修")]
        PlanRepair = 6,

        /// <summary>
        /// 状态维修
        /// </summary>
        [Label("状态维修")]
        AlarmDetail = 7
    }
}
