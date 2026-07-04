using SIE.MES.Outsourcing;
using SIE.MES.WorkReportPlans;
using SIE.Web.Command;
using System.Linq;

namespace SIE.Web.MES.WorkReportPlans.Commands
{
    /// <summary>
    /// 设置默认值
    /// </summary>
    [JsCommand("SIE.Web.MES.WorkReportPlans.Commands.SetDefaultCommand")]
    public class SetDefaultCommand : ViewCommand
    {
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="args">参数</param>
        /// <param name="scope">作用域</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            if (args == null || args.SelectedIds.Length == 0)
            {
                return false;
            }
            
            RT.Service.Resolve<WorkReportPlanController>().SetDefaultCommand(args.SelectedIds.FirstOrDefault());

            return true;
        }
    }
}
