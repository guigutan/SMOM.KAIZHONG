using SIE.MES.TaskManagement.Dispatchs;
using SIE.Security;
using SIE.Web.Command;

namespace SIE.Web.MES.TaskManagement.Dispatchs.Commands
{
    /// <summary>
    /// 保存派工任务可执行对象
    /// </summary>
    [JsCommand("SIE.Web.MES.TaskManagement.Dispatchs.Commands.TaskPerformerCommand")]
    [AllowAnonymous]
    public class TaskPerformerCommand : SaveCommand
    {
        /// <summary>
        /// 派工任务可执行对象命令执行方法
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>保存结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            string errMsg = string.Empty;
            var adoInfo = args.Data.ToJsonObject<AdoInfo>();
            errMsg = RT.Service.Resolve<DispatchController>().SaveTaskPerformer(adoInfo);
            if (errMsg.Length == 0)
                return "保存成功";
            else
                return errMsg;
        }
    }
}
