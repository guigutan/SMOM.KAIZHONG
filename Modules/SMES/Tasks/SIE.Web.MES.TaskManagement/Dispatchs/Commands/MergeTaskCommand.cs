using SIE.MES.TaskManagement.Dispatchs;
using SIE.Web.Command;
using System;
using System.Linq;

namespace SIE.Web.MES.TaskManagement.Dispatchs.Commands
{
    /// <summary>
    /// 合并派工任务单
    /// </summary>
    [JsCommand("SIE.Web.MES.TaskManagement.Dispatchs.Commands.MergeTaskCommand")]
    public class MergeTaskCommand : ViewCommand
    {
        /// <summary>
        /// 执行合并操作
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>合并结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            string errMsg = string.Empty;
            var selectedIds = args.SelectedIds.ToList();
            errMsg = RT.Service.Resolve<DispatchController>().MergeDispatchTasks(selectedIds);
            if (errMsg.Length == 0)
                return "合并成功".L10N();
            else
                return errMsg;
        }
    }
}
