using SIE.Equipments.Enums;
using SIE.EMS.EarlierStage.Projects;
using SIE.Web.Command;
using System.Collections.Generic;

namespace SIE.Web.EMS.EarlierStage.Projects.Commands
{
    /// <summary>
    /// 撤回项目
    /// </summary>
    [JsCommand("SIE.Web.EMS.EarlierStage.Projects.Commands.CancelProjectCommand")]
    public class CancelProjectCommand : ViewCommand
    {
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="args">参数</param>
        /// <param name="scope">作用域</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            List<double> selectedIds = new List<double>(args.SelectedIds);
            RT.Service.Resolve<ProjectController>().CancelProject(selectedIds, ApprovalStatus.Draft, ApprovalResult.Retract);
            return true;
        }
    }
}
