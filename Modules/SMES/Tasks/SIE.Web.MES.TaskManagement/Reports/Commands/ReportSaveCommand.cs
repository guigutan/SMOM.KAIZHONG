using SIE.Web.Command;
using System;

namespace SIE.Web.MES.TaskManagement.Reports.Commands
{
    /// <summary>
    /// 保存报工记录
    /// </summary>
    [JsCommand("SIE.Web.MES.TaskManagement.Reports.ReportSaveCommand")]
    public class ReportSaveCommand : ViewCommand
    {
        public const string FullName = "SIE.Web.MES.TaskManagement.Reports.ReportSaveCommand";

        protected override object Excute(ViewArgs args, string scope)
        {
            throw new NotImplementedException();
        }
    }
}
