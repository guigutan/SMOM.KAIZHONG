using SIE.Domain;
using SIE.EMS.InventoryTasks;
using SIE.Web.Command;

namespace SIE.Web.EMS.InventoryTasks.Commands
{
    /// <summary>
    /// 保存盘点任务
    /// </summary>
    [JsCommand("SIE.Web.EMS.InventoryTasks.Commands.SaveInventoryTaskCommand")]
    public class SaveInventoryTaskCommand : SaveCommand
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
            EntityList<InventoryTask> taskList = list as EntityList<InventoryTask>;
            RT.Service.Resolve<InventoryTaskController>().SaveInventoryTaskList(taskList);
            return list;
        }
    }
}
