using SIE.EMS.EarlierStage.Projects;
using SIE.Web.Command;
using System.Collections.Generic;

namespace SIE.Web.EMS.EarlierStage.Projects.Commands
{
    /// <summary>
    /// 撤回项目结项
    /// </summary>
    [JsCommand("SIE.Web.EMS.EarlierStage.Projects.Commands.CancelProjectCloseCommand")]
    public class CancelProjectCloseCommand : ViewCommand
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
            RT.Service.Resolve<ProjectChangeController>().CancelProjectClose(selectedIds);
            return true;
        }
    }
}
