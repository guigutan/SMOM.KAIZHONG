using SIE.MES.TaskManagement.Dispatchs;
using SIE.Web.Command;
using System;
using System.Linq;

namespace SIE.Web.MES.TaskManagement.Dispatchs.Commands
{
    /// <summary>
    /// 恢复任务单
    /// </summary>
    [JsCommand("SIE.Web.MES.TaskManagement.Dispatchs.Commands.ResumeTaskCommand")]
    public class ResumeTaskCommand : ViewCommand
    {
        /// <summary>
        /// 执行恢复操作
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>恢复结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            string errMsg = string.Empty;
            var selectedIds = args.SelectedIds.ToList();
            errMsg = RT.Service.Resolve<DispatchController>().SetResumeTasks(selectedIds);
            if (errMsg.Length == 0)
                return "恢复成功".L10N();
            else
                return errMsg;
        }
    }
}
