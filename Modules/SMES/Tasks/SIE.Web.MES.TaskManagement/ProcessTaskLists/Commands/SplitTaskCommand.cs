using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.ProcessTaskLists;
using SIE.Web.Command;
using System;

namespace SIE.Web.MES.TaskManagement.ProcessTaskLists.Commands
{
    /// <summary>
    /// 拆分任务单
    /// </summary>
    [JsCommand("SIE.Web.MES.TaskManagement.ProcessTaskLists.Commands.SplitTaskCommand")]
    public class SplitTaskCommand : ViewCommand
    {
        /// <summary>
        /// 执行拆分操作
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>拆分结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            string errMsg = string.Empty;
            var data = args.Data.ToJsonObject<SplitTaskViewModel>();
            RT.Service.Resolve<ProcessTaskListController>().SplitDispatchTask(data);
            return "生成成功".L10N();
        }
    }
}
