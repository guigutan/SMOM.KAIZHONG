using SIE.Web.Command;

namespace SIE.Web.EMS.Checks.Commands
{
    /// <summary>
    /// 删除保养明细 命令
    /// </summary>
    [JsCommand(CommandName)]
    public class DelMaintainPlanCommand : ViewCommand
    {
        /// <summary>
        /// 删除保养明细命令名称
        /// </summary>
        public const string CommandName = "SIE.Web.EMS.Checks.Commands.DelMaintainPlanCommand";

        /// <summary>
        /// 执行删除操作
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
