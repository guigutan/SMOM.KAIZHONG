using SIE.MES.TaskManagement.Dispatchs;
using SIE.Web.Command;
using System;
using System.Linq;

namespace SIE.Web.MES.TaskManagement.Dispatchs.Commands
{
    /// <summary>
    /// 撤销派工任务单
    /// </summary>
    [JsCommand("SIE.Web.MES.TaskManagement.Dispatchs.Commands.CancelDispatchTaskCommand")]
    public class CancelDispatchTaskCommand : ViewCommand
    {
        /// <summary>
        /// 执行撤销派工操作
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>撤销派工结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            string errMsg = string.Empty;
            var selectedIds = args.SelectedIds.ToList();
            errMsg = RT.Service.Resolve<DispatchController>().CancelDispatchTasks(selectedIds);
            if (errMsg.Length == 0)
                return "撤销派工成功".L10N();
            else
                return errMsg;
        }
    }
}
