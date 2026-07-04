using SIE.MES.TaskManagement.Dispatchs;
using SIE.Web.Command;
using System;
using System.Linq;

namespace SIE.Web.MES.TaskManagement.Dispatchs.Commands
{
    /// <summary>
    /// 设置普通任务单
    /// </summary>
    [JsCommand("SIE.Web.MES.TaskManagement.Dispatchs.Commands.SetNormalCommand")]
    public class SetNormalCommand : ViewCommand
    {
        /// <summary>
        /// 执行设置普通操作
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>设置普通结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            string errMsg = string.Empty;
            var selectedIds = args.SelectedIds.ToList();
            errMsg = RT.Service.Resolve<DispatchController>().SetNormalTasks(selectedIds);
            if (errMsg.Length == 0)
                return "设置普通成功".L10N();
            else
                return errMsg;
        }
    }
}
