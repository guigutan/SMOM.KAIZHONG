using SIE.Domain;
using SIE.Resources.Employees;
using SIE.Web.Command;
using System;
using System.Collections.Generic;

namespace SIE.Web.Resources.Employees.Commands
{
    /// <summary>
    /// 添加用户命令
    /// </summary>
    [JsCommand("SIE.Web.Resources.Employees.Commands.SelectResourceCommand")]
    public class SelectResourceCommand : ViewCommand
    {
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var meta = ClientEntities.Find(args.Type);
            var savedData = RF.Find(meta.EntityType).NewList();
            var employeeResourceList = args.Data.ToJsonObject<List<EmployeeResource>>();
            Check.NotNullOrEmpty(employeeResourceList, nameof(employeeResourceList));

            if (null == employeeResourceList || employeeResourceList.Count == 0)
                throw new ArgumentNullException(nameof(employeeResourceList));

            foreach (var item in employeeResourceList)
            {
                var employeeResource = new EmployeeResource();
                employeeResource.ResourceId = item.ResourceId;
                employeeResource.EmployeeId = item.EmployeeId;
                savedData.Add(employeeResource);
            }
            RF.Save(savedData);
            return true;
        }
    }
}
