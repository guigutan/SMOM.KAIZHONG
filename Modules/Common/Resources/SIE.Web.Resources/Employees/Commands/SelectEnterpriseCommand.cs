using SIE.Resources.Employees;
using SIE.Web.Command;
using System.Collections.Generic;

namespace SIE.Web.Resources.Employees.Commands
{
    /// <summary>
    /// 添加用户命令
    /// </summary>
    [JsCommand("SIE.Web.Resources.Employees.Commands.SelectEnterpriseCommand")]
    public class SelectEnterpriseCommand : ViewCommand
    {
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var employeeEnterpriseList = args.Data.ToJsonObject<List<EmployeeEnterprise>>();
            RT.Service.Resolve<EmployeeController>().SaveEmployeeEnterprise(employeeEnterpriseList);
            return true;
        }
    }
}