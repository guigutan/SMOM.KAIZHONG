using SIE.EMS.EarlierStage.Projects;
using SIE.Web.Command;

namespace SIE.Web.EMS.EarlierStage.Projects.Commands
{
    /// <summary>
    /// 计划开始
    /// </summary>
    [JsCommand("SIE.Web.EMS.EarlierStage.Projects.Commands.StartWorkItemCommand")]
    public class StartWorkItemCommand : ViewCommand
    {
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="args">参数</param>
        /// <param name="scope">作用域</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var id = args.Data.ToJsonObject<double>();
            RT.Service.Resolve<ProjectController>().StartWorkItem(id);
            return true;
        }
    }
}
