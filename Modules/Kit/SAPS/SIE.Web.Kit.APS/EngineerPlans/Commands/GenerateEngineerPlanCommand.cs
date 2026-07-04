using SIE.Kit.APS.EngineerPlans;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Kit.APS.EngineerPlans.Commands
{
    /// <summary>
    /// 同步工程计划信息
    /// </summary>
    [JsCommand("SIE.Web.Kit.APS.EngineerPlans.Commands.GenerateEngineerPlanCommand")]
    public class GenerateEngineerPlanCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            return RT.Service.Resolve<EngineerPlanController>().GenerateEngineerPlan();
        }
    }
}
