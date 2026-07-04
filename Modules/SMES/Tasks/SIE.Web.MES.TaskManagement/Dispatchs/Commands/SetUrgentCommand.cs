using SIE.MES.TaskManagement.Dispatchs;
using SIE.Web.Command;
using System;
using System.Linq;

namespace SIE.Web.MES.TaskManagement.Dispatchs.Commands
{
    /// <summary>
    /// 设置紧急任务单
    /// </summary>
    [JsCommand("SIE.Web.MES.TaskManagement.Dispatchs.Commands.SetUrgentCommand")]
    public class SetUrgentCommand : ViewCommand
    {
        /// <summary>
        /// 执行设置紧急操作
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>设置紧急结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            string errMsg = string.Empty;
            var selectedIds = args.SelectedIds.ToList();
            errMsg = RT.Service.Resolve<DispatchController>().SetUrgentTasks(selectedIds);
            if (errMsg.Length == 0)
                return "设置紧急成功".L10N();
            else
                return errMsg;
        }
    }
}
