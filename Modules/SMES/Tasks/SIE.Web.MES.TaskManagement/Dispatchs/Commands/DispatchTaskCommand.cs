using SIE.MES.TaskManagement.Dispatchs;
using SIE.Web.Command;
using System;
using System.Linq;

namespace SIE.Web.MES.TaskManagement.Dispatchs.Commands
{
    /// <summary>
    /// 派工任务单
    /// </summary>
    [JsCommand("SIE.Web.MES.TaskManagement.Dispatchs.Commands.DispatchTaskCommand")]
    public class DispatchTaskCommand : ViewCommand
    {
        /// <summary>
        /// 执行取消合并操作
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>取消合并结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            string errMsg = string.Empty;
            var selectedIds = args.SelectedIds.ToList();
            errMsg = RT.Service.Resolve<DispatchController>().DispatchTasks(selectedIds);
            if (errMsg.Length == 0)
                return "派工成功".L10N();
            else
                return errMsg;
        }
    }
}
