using SIE.Domain;
using SIE.Tech.Processs;
using SIE.Web.Command;
using System;
using System.Collections.Generic;

namespace SIE.Web.Tech.Processs.Commands
{
    /// <summary>
    /// 选择工序命令
    /// </summary>
    [JsCommand("SIE.Web.Tech.Processs.Commands.SelectProcessCommand")]
    public class SelectProcessCommand : ViewCommand
    {
        /// <summary>
        /// 选择工序命令执行方法
        /// </summary>
        /// <param name="args">视图参数</param>
        /// <param name="scope">作用域</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var processEmployeeList = args.Data.ToJsonObject<List<ProcessEmployee>>();
            Check.NotNullOrEmpty(processEmployeeList, nameof(processEmployeeList));
            if (processEmployeeList == null || processEmployeeList.Count == 0)
            {
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(processEmployeeList)));
            }

            foreach (var item in processEmployeeList)
            {
                var processEmployee = new ProcessEmployee();
                processEmployee.EmployeeId = item.EmployeeId;
                processEmployee.ProcessId = item.ProcessId;
                RF.Save(processEmployee);
            }

            return true;
        }
    }
}