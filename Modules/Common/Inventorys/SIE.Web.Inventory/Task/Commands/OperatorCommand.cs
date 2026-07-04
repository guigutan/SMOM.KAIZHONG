using SIE.Domain;
using SIE.Inventory.Task;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.Inventory.Task.Commands
{
    #region 添加命令
    /// <summary>
    /// 添加命令
    /// </summary>
    public class AddOperatorCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>bool</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var meta = ClientEntities.Find(args.Type);
            var savedData = RF.Find(meta.EntityType).NewList();
            var operatorList = args.Data.ToJsonObject<List<Operator>>();
            Check.NotNullOrEmpty(operatorList, nameof(operatorList));
            if (null == operatorList || operatorList.Count == 0)
            {
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(operatorList)));
            }
            foreach (var item in operatorList)
            {
                Operator operatorUser = new Operator();
                operatorUser.EmployeeId = item.Id;
                operatorUser.TaskManagementId = item.TaskManagementId;
                savedData.Add(operatorUser);
            }
            var task = RF.GetById<TaskManagement>(operatorList[0].TaskManagementId);
            if (task.ReleaseDate > DateTime.Now)
            {
                task.State = TaskState.Create;
            }
            else
            {
                task.State = TaskState.Appoint;
            }

            RF.Save(task);
            RF.Save(savedData);
            return true;
        }
    }
    #endregion

    #region 修改命令
    /// <summary>
    /// 修改命令
    /// </summary>
    public class EditOperatorCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>bool</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            return true;
        }
    }
    #endregion

    #region 删除命令
    /// <summary>
    /// 修改命令
    /// </summary>
    public class DeleteOperatorCommand : ViewCommand<double[]>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>bool</returns>
        protected override object Excute(double[] args, string scope)
        {
            List<double> empIdList = args.ToList();
            EntityList<Operator> employees = RT.Service.Resolve<TaskController>().GetOperatorList(empIdList);
            employees.ForEach(p => p.PersistenceStatus = PersistenceStatus.Deleted);
            var task = RF.GetById<TaskManagement>(employees[0].TaskManagementId);
            if (task.ReleaseDate > DateTime.Now)
            {
                task.State = TaskState.Create;
            }
            else
            {
                var operators = task.OperatorList.Where(p => !empIdList.Contains(p.Id)).ToList();
                if (operators.Count > 0)
                    task.State = TaskState.Appoint;
                else
                    task.State = TaskState.Release;
            }

            RF.Save(task);
            RF.Save(employees);
            return true;
        }
    }
    #endregion
}
