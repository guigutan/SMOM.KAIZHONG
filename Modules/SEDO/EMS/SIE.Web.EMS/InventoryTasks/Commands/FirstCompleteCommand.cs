using SIE.EMS.InventoryTasks;
using SIE.Web.Command;
using System.Collections.Generic;

namespace SIE.Web.EMS.InventoryTasks.Commands
{
    /// <summary>
    /// 初盘完成
    /// </summary>
    [JsCommand("SIE.Web.EMS.InventoryTasks.Commands.FirstCompleteCommand")]
    public class FirstCompleteCommand : ViewCommand
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
            RT.Service.Resolve<InventoryTaskController>().FirstComplete(selectedIds);
            return true;
        }
    }
}
