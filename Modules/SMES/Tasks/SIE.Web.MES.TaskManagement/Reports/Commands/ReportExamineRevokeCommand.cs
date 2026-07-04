using SIE.MES.TaskManagement.Reports;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.TaskManagement.Reports.Commands
{
    /// <summary>
    /// 报工记录审核撤回命令
    /// </summary>
    public class ReportExamineRevokeCommand : ViewCommand
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
            // 报工撤回
            RT.Service.Resolve<ReportController>().TaskExamineRevoke(ids);
            return true;
        }
    }
}
