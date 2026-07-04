using SIE.MES.TaskManagement.HeatTreatments;
using SIE.Web.Command;
using System;
using System.Linq;

namespace SIE.Web.MES.TaskManagement.HeatTreatments.Commands
{
    /// <summary>
    /// 报工命令
    /// </summary>
    public class HeatTreatmentReportCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        protected override object Excute(ViewArgs args, string scope)
        {
            var ids = args.SelectedIds.ToList();
            // 报工确认
            RT.Service.Resolve<HeatTreatmentController>().HeatTreatmentReport(ids);
            return true;
        }
    }
}
