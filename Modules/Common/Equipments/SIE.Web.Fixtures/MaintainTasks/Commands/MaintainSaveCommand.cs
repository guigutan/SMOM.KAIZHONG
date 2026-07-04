using SIE.Domain;
using SIE.Fixtures.MaintainTasks;
using SIE.Web.Command;

namespace SIE.Web.Fixtures.MaintainTasks.Commands
{
    /// <summary>
    /// 保养保存命令
    /// </summary>
    [JsCommand("SIE.Web.Fixtures.MaintainTasks.Commands.MaintainSaveCommand")]
    public class MaintainSaveCommand : FormSaveCommand
    {
        /// <summary>
        /// 进行保存
        /// </summary>
        /// <param name="entity">保养任务</param>
        protected override void DoSave(Entity entity)
        {
            var task = entity as MaintainTask;
            RT.Service.Resolve<MaintainTaskController>().SaveMaintainTask(task);
        }
    }
}
