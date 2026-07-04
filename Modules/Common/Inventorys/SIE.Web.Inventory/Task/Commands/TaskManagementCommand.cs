using SIE.Common.Prints;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Inventory.Task;
using SIE.Warehouses;
using SIE.Web.Command;
using SIE.Web.Common.Prints;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.Inventory.Task.Commands
{
    #region 修改命令
    /// <summary>
    /// 修改命令
    /// </summary>
    public class EditTaskManagementCommand : ViewCommand
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

    #region 保存命令
    /// <summary>
    /// 保存命令
    /// </summary>
    public class SaveTaskManagementCommand : SaveCommand
    {
        /// <summary>
        /// 保存前事件
        /// </summary>
        /// <param name="data">实体列表</param>
        protected override void OnSaving(EntityList data)
        {
            foreach (TaskManagement task in data)
            {
                if (task.ReleaseDate > DateTime.Now)
                {
                    task.State = TaskState.Create;
                }
                else
                {
                    var operatorList = RT.Service.Resolve<TaskController>().GetOperators(task.Id);
                    if (operatorList.Count > 0 && task.State == TaskState.Release)
                        task.State = TaskState.Appoint;
                    else
                        task.State = TaskState.Release;
                }

                RT.Service.Resolve<TaskController>().ValidateTaskManagement(task);
            }
            base.OnSaving(data);
        }
    }
    #endregion

    #region 释放命令

    /// <summary>
    /// 释放命令
    /// </summary>
    public class ReleaseTaskManagementCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>bool</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            List<double> idlist = args.SelectedIds.ToList();
            RT.Service.Resolve<TaskController>().ReleaseTasks(idlist.Distinct().ToList());
            return true;
        }
    }

    #endregion

    #region 冻结命令

    /// <summary>
    /// 冻结命令
    /// </summary>
    public class FrozenTaskManagementCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>bool</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            List<double> idlist = args.SelectedIds.ToList();
            RT.Service.Resolve<TaskController>().FrozenTasks(idlist.Distinct().ToList());
            return true;
        }
    }

    #endregion

    #region 优先级命令

    /// <summary>
    /// 加急命令
    /// </summary>
    public class UrgentLevelCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>bool</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var task = args.Data.ToJsonObject<TaskManagement>();
            int maxCount = RT.Service.Resolve<TaskController>().GetTaskUrgentMaxCount();

            if (maxCount > 0)
            {
                var urgentTaskCount = RT.Service.Resolve<TaskController>().GetTaskManagement(task.FromWarehouseId.Value).Count(p => p.Level == TaskLevel.Urgent);

                if (urgentTaskCount >= maxCount)
                    throw new ValidationException("加急任务数:[{0}]不能超过最大加急数:[{1}]".L10nFormat(urgentTaskCount, maxCount));
            }

            RT.Service.Resolve<TaskController>().UpdateTaskLevel(task.Id, TaskLevel.Urgent);

            return true;
        }
    }

    /// <summary>
    /// 设置高等级命令
    /// </summary>
    public class HighLevelCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>bool</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var task = args.Data.ToJsonObject<TaskManagement>();
            RT.Service.Resolve<TaskController>().UpdateTaskLevel(task.Id, TaskLevel.High);

            return true;
        }
    }

    /// <summary>
    /// 设置中等级命令
    /// </summary>
    public class MiddleLevelCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>bool</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var task = args.Data.ToJsonObject<TaskManagement>();
            RT.Service.Resolve<TaskController>().UpdateTaskLevel(task.Id, TaskLevel.Middle);

            return true;
        }
    }

    /// <summary>
    /// 设置低等级命令
    /// </summary>
    public class LowLevelCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>bool</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var task = args.Data.ToJsonObject<TaskManagement>();
            RT.Service.Resolve<TaskController>().UpdateTaskLevel(task.Id, TaskLevel.Low);

            return true;
        }
    }

    #endregion

    #region 查看命令
    /// <summary>
    /// 查看任务管理单
    /// </summary>
    public class ViewTaskManagementCommand : ViewCommand
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

    #region 打印命令
    /// <summary>
    /// 打印单据
    /// </summary>
    public class PrintTaskManagementCommand : ViewCommand<PrintDatas>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">打印数据</param>
        /// <param name="scope">scope</param>
        /// <returns>object</returns>
        protected override object Excute(PrintDatas args, string scope)
        {
            var billTemplateId = args.BillTemplateId;
            List<double> taskManagementIdList = args.BillIdList.ToList();
            var taskManagements = RT.Service.Resolve<TaskController>().GetTaskManagements(taskManagementIdList);
            if (taskManagements.Count <= 0)
            {
                throw new ValidationException("未选择打印数据".L10N());
            }

            // 1.获取打印模板
            PrintTemplate template = RF.GetById<PrintTemplate>(billTemplateId) ?? throw new ValidationException("打印模板为空或已禁用".L10N());

            //2.根据类型获取报表处理对像
            var report = ReportFactory.Current.GetReportByExtension(template.Type);

            //4.创建实体打印对像 如果清楚实体打印对像自己NEW 一个出来也行
            var printable = new TaskManagementPrintable();
            var printData = new PrintDataCommon();
            //5.调用打印处理函数返回打印模板BASE64字符串到前台，用于传输到打印预览页面
            printData.Url = report.PrintProcess(printable, template.Id, template.Content, () =>
            {
                List<TaskManagement> printData = new List<TaskManagement>();
                printData.AddRange(taskManagements);

                return printData;
            });
            printData.Type = template.Type;
            return printData;
        }
    }
    #endregion
}
