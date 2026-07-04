using SIE.Domain;
using SIE.EMS.EarlierStage.Projects;
using SIE.Web.Command;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.EMS.EarlierStage.Projects.Commands
{
    /// <summary>
    /// 删除项目
    /// </summary>
    [JsCommand("SIE.Web.EMS.EarlierStage.Projects.Commands.DeleteProjectCommand")]
    public class DeleteProjectCommand : DeleteCommand
    {
        /// <summary>
        /// 保存前动作
        /// </summary>
        /// <param name="data"></param>
        protected override void OnSaving(EntityList data)
        {
            base.OnSaving(data);
            var ids = new List<double>();
            data.DeletedList.ForEach(p =>
            {
                var model = p as Project;
                if (model != null)
                    ids.Add(model.Id);
            });
            RT.Service.Resolve<ProjectController>().DeleteCheckProjectState(ids);
        }
    }
}
