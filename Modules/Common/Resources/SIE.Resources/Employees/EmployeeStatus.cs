using SIE.ObjectModel;

namespace SIE.Resources.Employees
{
    /// <summary>
    /// 员工状态
    /// </summary>
    public enum EmployeeStatus
    {
        /// <summary>
        /// 在职
        /// </summary>
        [Label("在职")]
        Job,

        /// <summary>
        /// 离职
        /// </summary>
        [Label("离职")]
        UnJob,
    }
}