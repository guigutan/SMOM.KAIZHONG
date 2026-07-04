using SIE.ObjectModel;

namespace SIE.EMS.MainenanceProjects
{
    /// <summary>
	/// 项目类型
	/// </summary>
	public enum ProjectType
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
        Maintain = 5,

        /// <summary>
        /// 润滑
        /// </summary>
        [Label("润滑")]
        Lubrication = 6,

        /// <summary>
        /// 设备定检
        /// </summary>
        [Label("设备定检")]
        PeriodicalInsp = 7,

        /// <summary>
        /// 计量校验
        /// </summary>
        [Label("计量校验")]
        Verify = 10,
        /// <summary>
        /// 计划维修
        /// </summary>
        [Label("计划维修")]
        PlanRepair = 11
    }
}
