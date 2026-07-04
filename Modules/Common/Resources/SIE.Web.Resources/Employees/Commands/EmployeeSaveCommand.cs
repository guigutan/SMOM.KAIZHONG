using SIE.Domain;
using SIE.Resources.Employees;
using SIE.Web.Command;

namespace SIE.Web.Resources.Employees.Commands
{
    /// <summary>
    /// 员工保存命令
    /// </summary>
    [JsCommand("SIE.Web.Resources.Employees.Commands.EmployeeSaveCommand")]
    public class EmployeeSaveCommand : FormSaveCommand
    {
        /// <summary>
        /// 保存员工
        /// </summary>
        /// <param name="entity"></param>
        protected override void DoSave(Entity entity)
        {
            Employee data = entity as Employee;
            RT.Service.Resolve<EmployeeController>().SaveEditedEmployee(data);
        }
    }
}
