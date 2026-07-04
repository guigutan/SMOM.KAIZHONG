using SIE.Web.Command;

namespace SIE.Web.EMS.EquipMaint.Maintains.Plans.Commands
{
    /// <summary>
    /// 生成保养计划 命令
    /// </summary>
    [JsCommand(CommandName)]
    public class GenerateMaintainPlanCommand : ViewCommand
    {
        /// <summary>
        ///生成保养计划命令名字
        /// </summary>
        public const string CommandName = "SIE.Web.EMS.EquipMaint.Maintains.Plans.Commands.GenerateMaintainPlanCommand";

        /// <summary>
        /// 生成保养计划命令执行
        /// </summary>
        /// <param name="args">视图参数</param>
        /// <param name="scope">命令参数</param>
        /// <returns>返回参数</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            return true;
        }
    }
}
