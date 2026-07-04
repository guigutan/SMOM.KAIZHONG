using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.Web.Command;
using System;
using System.Collections.Generic;

namespace SIE.Web.MES.TaskManagement.ProcessTaskLists.Commands
{
    /// <summary>
    /// 删除命令
    /// </summary>
    [JsCommand(CommandName)]
    public class TaskDeleteCommand : ViewCommand
    {
        /// <summary>
        /// 删除命令
        /// </summary>
        public const string CommandName = "SIE.Web.MES.TaskManagement.ProcessTaskLists.Commands.TaskDeleteCommand";

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            if (args == null || null == args.SelectedIds || args.SelectedIds.Length == 0)
            {
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(args.SelectedIds)));
            }
            RT.Service.Resolve<DispatchController>().DeleteDispatch(args.SelectedIds);
            return true;
        }
    }
}
