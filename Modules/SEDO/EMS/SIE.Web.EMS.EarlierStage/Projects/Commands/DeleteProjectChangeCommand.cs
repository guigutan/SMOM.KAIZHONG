using SIE.EMS.EarlierStage.Projects;
using SIE.Web.Command;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.EMS.EarlierStage.Projects.Commands
{
    /// <summary>
    /// 删除项目变更
    /// </summary>
    [JsCommand("SIE.Web.EMS.EarlierStage.Projects.Commands.DeleteProjectChangeCommand")]
    public class DeleteProjectChangeCommand : DeleteCommand
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
            var ids = new List<double>();
            list.DeletedList.ForEach(p =>
            {
                var model = p as ProjectChange;
                if (model != null)
                    ids.Add(model.Id);
            });
            RT.Service.Resolve<ProjectChangeController>().DeleteCheckProjectChange(ids);
            return true;
        }
    }
}
