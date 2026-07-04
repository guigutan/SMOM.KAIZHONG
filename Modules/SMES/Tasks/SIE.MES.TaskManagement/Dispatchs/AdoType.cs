using SIE.ObjectModel;

namespace SIE.MES.TaskManagement.Dispatchs
{
    /// <summary>
    /// 对象类型
    /// </summary>
    public enum AdoType
    {
        /// <summary>
        /// 员工
        /// </summary>
        [Label("员工")]
        Employee,

        /// <summary>
        /// 班组
        /// </summary>
        [Label("班组")]
        WorkGroup,

        /// <summary>
		/// 员工组
		/// </summary>
		[Label("员工组")]
        EmployeeGroup,

        /// <summary>
        /// 工位
        /// </summary>
        [Label("工位")]
        Station
    }

    /// <summary>
	/// 组类
	/// </summary>
    public enum AdoGroup
    {
        /// <summary>
        /// 班组
        /// </summary>
        [Label("班组")]
        WorkGroup,

        /// <summary>
		/// 员工组
		/// </summary>
		[Label("员工组")]
        EmployeeGroup,

        /// <summary>
        /// 工位
        /// </summary>
        [Label("工位")]
        Station
    }
}