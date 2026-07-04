using SIE.EMS.InventoryTasks;
using SIE.Web.Command;

namespace SIE.Web.EMS.InventoryTasks.Commands
{
    /// <summary>
    /// 下达
    /// </summary>
    [JsCommand("SIE.Web.EMS.InventoryTasks.Commands.ReleaseTaskCommand")]
    public class ReleaseTaskCommand : ViewCommand
    {
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="args">参数</param>
        /// <param name="scope">作用域</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            if (args == null || args.SelectedIds.Length == 0)
            {
                return false;
            }
            RT.Service.Resolve<InventoryTaskController>().ReleaseTask(args.SelectedIds[0]);
            return true;
        }
    }
}
