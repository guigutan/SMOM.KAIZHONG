using SIE.MES.TaskManagement.Dispatchs;
using SIE.Web.Command;
using System.Linq;

namespace SIE.Web.MES.TaskManagement.Dispatchs.Commands
{
    /// <summary>
    /// 强制关闭任务单
    /// </summary>
    [JsCommand("SIE.Web.MES.TaskManagement.Dispatchs.Commands.CloseTaskCommand")]
    public class CloseTaskCommand : ViewCommand
    {
        /// <summary>
        /// 执行强制关闭操作
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>强制关闭结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            string errMsg = string.Empty;
            var selectedIds = args.SelectedIds.ToList();
            errMsg = RT.Service.Resolve<DispatchController>().SetCloseTasks(selectedIds);
            if (errMsg.Length == 0)
                return "强制关闭成功";
            else
                return errMsg;
        }
    }
}
