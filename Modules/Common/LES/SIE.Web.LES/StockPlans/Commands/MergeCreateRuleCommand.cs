using SIE.ShipPlan;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.LES.StockPlans.Commands
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
