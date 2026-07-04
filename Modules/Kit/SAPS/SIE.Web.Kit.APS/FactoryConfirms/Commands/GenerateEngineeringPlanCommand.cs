using SIE.Domain;
using SIE.Kit.APS.FactoryConfirms;
using SIE.Web.Command;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.Kit.APS.FactoryConfirms.Commands
{
    /// <summary>
    /// 生成工程计划命令
    /// </summary>
    public class GenerateEngineeringPlanCommand : ViewCommand
    {
        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            List<double> orderDetailIds = args.SelectedIds.ToList();
            var msg = RT.Service.Resolve<FactoryConfirmsController>().GenerateEngineeringPlan(orderDetailIds);
            return msg;
        }
    }
}