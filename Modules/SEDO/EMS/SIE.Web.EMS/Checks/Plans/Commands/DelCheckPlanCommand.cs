using SIE.Web.Command;

namespace SIE.Web.EMS.Checks.Plans.Commands
{
    /// <summary>
    /// 删除点检明细 命令
    /// </summary>
    [JsCommand("SIE.Web.EMS.Checks.Plans.Commands.DelCheckPlanCommand")]
    public class DelCheckPlanCommand : ViewCommand
    {
        /// <summary>
        /// 执行删除点检明细操作
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
