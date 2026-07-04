using SIE.EMS.InventoryTasks;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.InventoryTasks.Commands
{
    /// <summary>
    /// 一键盘点
    /// </summary>
    [JsCommand("SIE.Web.EMS.InventoryTasks.Commands.OneKeyPassCommand")]
    public class OneKeyPassCommand : ViewCommand
    {
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="args">参数</param>
        /// <param name="scope">作用域</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            if (args == null)
            {
                return false;
            }
            List<double> selectedIds = new List<double>(args.SelectedIds);
            RT.Service.Resolve<InventoryTaskController>().OneKeyPass(selectedIds[0]);
            return true;
        }
    }
}
