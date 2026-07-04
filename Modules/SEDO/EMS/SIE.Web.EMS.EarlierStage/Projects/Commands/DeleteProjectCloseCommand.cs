using SIE.Domain;
using SIE.EMS.EarlierStage.Projects;
using SIE.Web.Command;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.EMS.EarlierStage.Projects.Commands
{
    /// <summary>
    /// 删除项目结项
    /// </summary>
    [JsCommand("SIE.Web.EMS.EarlierStage.Projects.Commands.DeleteProjectCloseCommand")]
    public class DeleteProjectCloseCommand : DeleteCommand
    {
        /// <summary>
        /// 保存前动作
        /// </summary>
        /// <param name="data"></param>
        protected override void OnSaving(EntityList data)
        {
            base.OnSaving(data);
            var ids = new List<double>();
            if (data != null)
            {
                data.DeletedList.ForEach(p =>
                {
                    var model = p as ProjectClose;
                    if (model != null)
                    {
                        ids.Add(model.Id);
                    }
                });
            }
            RT.Service.Resolve<ProjectChangeController>().DeleteCheckProjectClose(ids);
        }
    }
}
