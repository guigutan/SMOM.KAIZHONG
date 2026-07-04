using SIE.Domain.Validation;
using SIE.Resources.Employees;
using SIE.Web.Command;
using System.Collections.Generic;
using System;

namespace SIE.Web.Resources.Employees.Commands
{
    /// <summary>
    /// 执行员工维护的工厂删除命令
    /// </summary>
    [JsCommand("SIE.Web.Resources.Employees.Commands.EmployeeEnterpriseDelCommand")]
    public class EmployeeEnterpriseDelCommand : ViewCommand
    {
        /// <summary>
        /// 命令执行
        /// </summary>
        /// <param name="Id">员工ID</param>
        /// <param name="scope">作用域</param>
        /// <returns></returns>

        protected override object Excute(ViewArgs args, string scope)
        {
            List<double> ids = args.Data.ToJsonObject<List<double>>();
            if (ids == null || ids.Count == 0)
                throw new ValidationException("请选择需要删除的数据".L10N());
            RT.Service.Resolve<EmployeeController>().DeleteEmployeeEnterprise(ids);
            return true;
        }
    }
}

