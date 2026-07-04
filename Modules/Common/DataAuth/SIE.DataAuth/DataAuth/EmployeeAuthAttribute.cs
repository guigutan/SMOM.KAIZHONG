using System;

namespace SIE.DataAuth
{
    /// <summary>
    /// 按员工授权的实体标记
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
#pragma warning disable S4060 // Non-abstract attributes should be sealed
    public class EmployeeAuthAttribute : Attribute
#pragma warning restore S4060 // Non-abstract attributes should be sealed
    {
        /// <summary>
        /// 员工id属性名称
        /// </summary>
        public string EmployeeIdProperty { get; set; }

        /// <summary>
        /// 控制业务权限的属性名称
        /// </summary>
        public string AuthIdProperty { get; set; }

        /// <summary>
        /// 创建<see cref="EmployeeAuthAttribute"/>
        /// </summary>
        /// <param name="employeeIdProperty"></param>
        /// <param name="authIdProperty"></param>
        public EmployeeAuthAttribute(string employeeIdProperty, string authIdProperty)
        {
            Check.NotNull(employeeIdProperty, nameof(employeeIdProperty));
            Check.NotNullOrEmpty(authIdProperty, nameof(authIdProperty));
            EmployeeIdProperty = employeeIdProperty;
            AuthIdProperty = authIdProperty;
        }
    }
}
