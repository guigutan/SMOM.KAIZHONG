using SIE.Security;
using SIE.Web.Command;

namespace SIE.Web.MES.TaskManagement.Dispatchs.Commands
{
    /// <summary>
    /// 个人任务详情
    /// </summary>
    [JsCommand("SIE.Web.MES.TaskManagement.Dispatchs.Commands.ShowTaskDetailCommand")]
    [AllowAnonymous]
    public class ShowTaskDetailCommand : SaveCommand
    {
    }
}
