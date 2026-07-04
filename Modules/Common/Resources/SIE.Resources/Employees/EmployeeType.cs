using SIE.ObjectModel;

namespace SIE.Resources.Employees
{
    /// <summary>
    /// 员工类型
    /// </summary>
    public enum EmployeeType
    {
        /// <summary>
        /// 组长
        /// </summary>
        [Label("组长")]
        Chargehand,

        /// <summary>
        /// 班长
        /// </summary>
        [Label("班长")]
        Monitor,

        /// <summary>
        /// 班组长
        /// </summary>
        [Label("班组长")]
        Foreman,
    }
}