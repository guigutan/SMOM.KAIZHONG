using SIE.Domain;
using SIE.Resources.Employees;

namespace SIE.Web.Inventory.Task
{
    /// <summary>
    /// 员工数据查询
    /// </summary>
    public class EmployeeDataQuery : Data.DataQueryer
    {
        /// <summary>
        /// 获取员工数据
        /// </summary>
        /// <returns>员工数据列表</returns>
        public EntityList<Employee> GetEmployees()
        {
            var employeeList = RT.Service.Resolve<EmployeeController>().GetGetEmployeeList();
            return employeeList;
        }
    }
}
