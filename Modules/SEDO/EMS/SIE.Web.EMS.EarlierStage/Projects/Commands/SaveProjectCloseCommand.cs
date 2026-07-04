using SIE.EMS.EarlierStage.Projects;
using SIE.Web.Command;

namespace SIE.Web.EMS.EarlierStage.Projects.Commands
{
    /// <summary>
    /// 保存项目结项
    /// </summary>
    [JsCommand("SIE.Web.EMS.EarlierStage.Projects.Commands.SaveProjectCloseCommand")]
    public class SaveProjectCloseCommand : FormSaveCommand
    {
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="args">参数</param>
        /// <param name="scope">作用域</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var list = GetDeserializeData(args, scope);
            var entity = list.Count > 0 ? list[0] : null;
            if (entity != null)
            {
                var projectClose = entity as ProjectClose;
                RT.Service.Resolve<ProjectChangeController>().SaveProjectClose(projectClose);
            }
            return entity;
        }
    }
}
