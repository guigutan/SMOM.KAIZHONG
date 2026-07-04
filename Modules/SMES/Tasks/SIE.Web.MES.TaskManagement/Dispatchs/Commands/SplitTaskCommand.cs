using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.Dispatchs.ViewModels;
using SIE.Web.Command;
using System;

namespace SIE.Web.MES.TaskManagement.Dispatchs.Commands
{
    /// <summary>
    /// 拆分任务单
    /// </summary>
    [JsCommand("SIE.Web.MES.TaskManagement.Dispatchs.Commands.SplitTaskCommand")]
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
            errMsg = RT.Service.Resolve<DispatchController>().SplitDispatchTask(data);
            if (errMsg.Length == 0)
                return "拆分成功".L10N();
            else
                return errMsg;
        }
    }
}
