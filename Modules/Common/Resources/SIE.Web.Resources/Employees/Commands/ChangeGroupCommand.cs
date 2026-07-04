using SIE.Domain;
using SIE.Resources.Employees;
using SIE.Web.Command;
using System;
using System.Collections.Generic;

namespace SIE.Web.Resources.Employees.Commands
{
    /// <summary>
    /// 员工通用保存命令
    /// </summary>
    /// <seealso cref="SIE.Web.Command.ViewCommand" />
    public class ChangeEmployeeBaseCommand : ViewCommand
    {
        /// <summary>
        /// 重写此方法实现命令逻辑
        /// </summary>
        /// <param name="args">视图参数</param>
        /// <param name="scope"><see cref="!: SIE.MetaModel.View.Block.EntityType.FullName" /></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">employee</exception>
        protected override object Excute(ViewArgs args, string scope)
        {
            var employeeList = args.Data.ToJsonObject<List<Employee>>();
            if (employeeList == null || employeeList.Count == 0)
                throw new ArgumentNullException(nameof(employeeList));
            foreach (var employee in employeeList)
            {
                DoSave(employee);
            }
            return true;
        }

        /// <summary>
        /// 具体的保存逻辑，子类可重写
        /// </summary>
        /// <param name="employee">员工.</param>
        public virtual void DoSave(Employee employee)
        {
            RF.Save(employee);
        }
    }

    /// <summary>
    /// 转班组
    /// </summary>
    [JsCommand("SIE.Web.Resources.Employees.Commands.ChangeGroupCommand")]
    public class ChangeGroupCommand : ChangeEmployeeBaseCommand
    {
    }
}
