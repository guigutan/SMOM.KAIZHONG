using SIE.Domain;
using SIE.Resources.Employees;

namespace SIE.xUnit.Resources.Employees
{
    /// <summary>
    /// 员工测试数据控制器
    /// </summary>
    public class EmployeeTestController : DomainController
    {
        /// <summary>
        /// 创建员工
        /// </summary>
        /// <param name="isSave"></param>
        /// <returns></returns>
        public virtual Employee CreateEmployee(bool isSave = true)
        {
            Employee entity = new Employee();
            entity.GenerateId();
            entity.Code = $"EmpCode{entity.Id}";
            entity.Name = $"EmpName{entity.Id}";
            if (isSave)
                RF.Save(entity);
            return entity;
        }
    }
}
