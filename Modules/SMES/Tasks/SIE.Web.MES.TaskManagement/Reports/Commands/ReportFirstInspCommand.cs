using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.Reports;
using SIE.Web.Command;
using System;

namespace SIE.Web.MES.TaskManagement.Reports.Commands
{
    /// <summary>
    /// 任务单首件报检
    /// </summary>
    public class ReportFirstInspCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var dispatchTask = args.Data.ToJsonObject<DispatchTask>();
            if (dispatchTask == null)
                return "当前报检的任务单不存在".L10N();
            RT.Service.Resolve<ReportController>().ReportFirstInsp(dispatchTask);
            return true;
        }
    }
}
