using SIE.MES.TaskManagement.Dispatchs;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.TaskManagement.Reports.Commands
{
    /// <summary>
    /// 报工暂停命令
    /// </summary>
    public class ReportTaskStopCommand : ViewCommand
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
            string errMsg = string.Empty;
            var selectedIds = args.SelectedIds.ToList();
            errMsg = RT.Service.Resolve<DispatchController>().SetPauseTasks(selectedIds);
            if (errMsg.Length == 0)
                return "暂停成功";
            else
                return errMsg;
        }
    }
}
