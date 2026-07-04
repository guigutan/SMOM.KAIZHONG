using DevExpress.XtraRichEdit.Import.Doc;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.Reports;
using SIE.Resources.Employees;
using SIE.Web.Command;

namespace SIE.Web.MES.TaskManagement.Reports.Commands
{
    /// <summary>
    /// 开工
    /// </summary>
    [JsCommand("SIE.Web.MES.TaskManagement.Reports.ReportTaskStartWorkCommand")]
    public class ReportTaskStartWorkCommand : ViewCommand
    {
        public const string FullName = "SIE.Web.MES.TaskManagement.Reports.ReportTaskStartWorkCommand";

        protected override object Excute(ViewArgs args, string scope)
        {
            var dispatchTask = args.Data.ToJsonObject<DispatchTask>();
            if (dispatchTask == null) return false;
            //生成派工明细
            var employee = RT.Service.Resolve<EmployeeController>().GetEmployeeById(RT.IdentityId);
            RT.Service.Resolve<ReportController>().StartWork(employee, dispatchTask);
            return true;
        }
    }
}

