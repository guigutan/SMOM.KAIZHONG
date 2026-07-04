using SIE.Domain;
using SIE.Inventory.Strategy;
using SIE.Web.Command;
using System.Collections.Generic;
using System;
using System.Linq;

namespace SIE.Web.Inventory.Strategy.Commands
{
    /// <summary>
    /// 添加命令
    /// </summary>
    [JsCommand("SIE.Web.Inventory.Strategy.Commands.AddTaskAllotRuleCommand")]
    public class AddTaskAllotRuleCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>bool</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var data = args.Data.ToJsonObject<TaskAllotRule>();
            var code = RT.Service.Resolve<RuleController>().GetTaskAllotRuleCode();
            data.Code = code;
            data.State = State.Enable;
            data.Priority = 5;
            return data;
        }
    }

    /// <summary>
    /// 删除命令
    /// </summary>
    [JsCommand("SIE.Web.Inventory.Strategy.Commands.DeleteTaskAllotRuleCommand")]
    public class DeleteTaskAllotRuleCommand : DeleteCommand
    {
    }

    /// <summary>
    /// 选择员工命令
    /// </summary>
    public class SelectTaskAllotRuleEmployeeCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>object</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var meta = ClientEntities.Find(args.Type);
            var savedData = RF.Find(meta.EntityType).NewList();
            var employeeList = args.Data.ToJsonObject<List<TaskAllotRuleEmployee>>();
            Check.NotNullOrEmpty(employeeList, nameof(employeeList));
            if (employeeList == null || employeeList.Count == 0)
            {
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(employeeList)));
            }
            foreach (var item in employeeList)
            {
                var employee = new TaskAllotRuleEmployee();
                employee.EmployeeId = item.EmployeeId;
                employee.TaskAllotRuleId = item.TaskAllotRuleId;
                savedData.Add(employee);
            }
            RF.Save(savedData);
            return true;
        }
    }

    /// <summary>
    /// 删除命令
    /// </summary>
    public class DeleteTaskAllotRuleEmployeeCommand : ViewCommand<double[]>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>object</returns>
        protected override object Excute(double[] args, string scope)
        {
            RT.Service.Resolve<RuleController>().DeleteTaskAllotRuleEmployees(args.ToList());
            return true;
        }
    }
}
