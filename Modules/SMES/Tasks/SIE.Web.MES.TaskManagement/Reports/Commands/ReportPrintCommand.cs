using SIE.Web.Command;
using System;

namespace SIE.Web.MES.TaskManagement.Reports.Commands
{
    /// <summary>
    /// 打印
    /// </summary>
    [JsCommand("SIE.Web.MES.TaskManagement.Reports.ReportPrintCommand")]
    public class ReportPrintCommand : ViewCommand
    {
        public const string FullName = "SIE.Web.MES.TaskManagement.Reports.ReportPrintCommand";

        protected override object Excute(ViewArgs args, string scope)
        {
            throw new NotImplementedException();
        }
    }
}
