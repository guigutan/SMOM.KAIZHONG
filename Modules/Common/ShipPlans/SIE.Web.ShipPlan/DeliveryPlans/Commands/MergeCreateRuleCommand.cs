using SIE.ShipPlan;
using SIE.Web.Command;

namespace SIE.Web.ShipPlan.Commands
{
    /// <summary>
    /// 合并创单规则命令
    /// </summary>
    public class InitMergeCreateRuleCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>object</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            RT.Service.Resolve<DeliveryPlanController>().InitMergeCreateRule();
            return true;
        }
    }
}