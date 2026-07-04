using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.Dispatchs.ViewModels;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.TaskManagement.Dispatchs.Commands
{
    /// <summary>
    /// 排程退回
    /// </summary>
    public class SchedulingInfReturnCommand : ViewCommand<SchedulingInfReturnValidViewModel>
    {
        protected override object Excute(SchedulingInfReturnValidViewModel args, string scope)
        {
            var taskIds = args.TaskId.Split(',').Select(p => Convert.ToDouble(p)).ToList();
            RT.Service.Resolve<DispatchController>().SchedulingInfReturn(taskIds, args.ReturnReason);
            return true;
        }
    }
}
