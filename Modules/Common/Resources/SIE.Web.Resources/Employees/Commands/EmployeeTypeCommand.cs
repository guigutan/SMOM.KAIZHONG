using SIE.Domain;
using SIE.Resources.Employees;
using SIE.Web.Command;

namespace SIE.Web.Resources.Employees.Commands
{
    /// <summary>
    /// 设为组长
    /// </summary>
    [JsCommand("SIE.Web.Resources.Employees.Commands.ChargehandCommand")]
    public class ChargehandCommand : ChangeEmployeeBaseCommand
    {
        /// <summary>
        /// 设为组长
        /// </summary>
        /// <param name="employee">员工</param>
        public override void DoSave(Employee employee)
        {
            employee.EmployeeType = EmployeeType.Chargehand;
            RF.Save(employee);
        }
    }

    /// <summary>
    /// 设为班长
    /// </summary>
    [JsCommand("SIE.Web.Resources.Employees.Commands.MonitorCommand")]
    public class MonitorCommand : ChangeEmployeeBaseCommand
    {
        /// <summary>
        /// 设为班长
        /// </summary>
        /// <param name="employee">员工</param>
        public override void DoSave(Employee employee)
        {
            employee.EmployeeType = EmployeeType.Monitor;
            RF.Save(employee);
        }
    }

    /// <summary>
    /// 设为班组长
    /// </summary>
    [JsCommand("SIE.Web.Resources.Employees.Commands.ForemanCommand")]
    public class ForemanCommand : ChangeEmployeeBaseCommand
    {
        /// <summary>
        /// 设为班组长
        /// </summary>
        /// <param name="employee">员工</param>
        public override void DoSave(Employee employee)
        {
            employee.EmployeeType = EmployeeType.Foreman;
            RF.Save(employee);
        }
    }

    /// <summary>
    /// 清空员工类型
    /// </summary>
    [JsCommand("SIE.Web.Resources.Employees.Commands.ClearTypeCommand")]
    public class ClearTypeCommand : ChangeEmployeeBaseCommand
    {
        /// <summary>
        /// 清空员工类型
        /// </summary>
        /// <param name="employee">员工</param>
        public override void DoSave(Employee employee)
        {
            employee.EmployeeType = null;
            RF.Save(employee);
        }
    }
}
