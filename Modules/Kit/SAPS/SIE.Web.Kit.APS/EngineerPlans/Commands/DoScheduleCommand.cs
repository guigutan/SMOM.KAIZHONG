using SIE.Kit.APS.EngineerPlans;
using SIE.Web.Command;
using System;

namespace SIE.Web.Kit.APS.EngineerPlans.Commands
{
    /// <summary>
    /// 排计划命令
    /// </summary>
    public class DoScheduleCommand : ViewCommand
    {
        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="args">视图参数</param>
        /// <param name="scope">SIE.MetaModel.View.Block.EntityType</param>
        /// <returns>object</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var ctl = RT.Service.Resolve<EngineerPlanController>();
            ctl.DoSchedule();
            return 1;
        }
    }
}
