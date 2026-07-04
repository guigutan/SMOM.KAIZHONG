using SIE.Web.Command;
using System;

namespace SIE.Web.MES.TaskManagement.Reports.Commands
{
    /// <summary>
    /// 报工
    /// </summary>
    [JsCommand("SIE.Web.MES.TaskManagement.Reports.ReportCommand")]
    public class ReportCommand : ViewCommand
    {
        public const string FullName = "SIE.Web.MES.TaskManagement.Reports.ReportCommand";

        protected override object Excute(ViewArgs args, string scope)
        {
            throw new NotImplementedException();
        }
    }
}
