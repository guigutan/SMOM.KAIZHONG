using SIE.Web.Command;

namespace SIE.Web.EMS.Checks.Plans.Commands
{
    /// <summary>
    /// 生成点检计划 命令
    /// </summary>
    [JsCommand("SIE.Web.EMS.Checks.Plans.Commands.GenerateCheckPlanCommand")]
    public class GenerateCheckPlanCommand : ViewCommand
    {
        /// <summary>
        /// 执行生成点检计划命令
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            return true;
        }
    }
}
