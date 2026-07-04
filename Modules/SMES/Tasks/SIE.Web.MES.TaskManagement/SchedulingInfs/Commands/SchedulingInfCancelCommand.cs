using SIE.MES.TaskManagement.SchedulingInfs;
using SIE.MES.TaskManagement.SchedulingInfs.ViewModels;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.TaskManagement.SchedulingInfs.Commands
{
    /// <summary>
    /// 作废按钮
    /// </summary>
    public class SchedulingInfCancelCommand : ViewCommand<SchedulingInfCancelViewModel>
    {
        protected override object Excute(SchedulingInfCancelViewModel viewModel, string scope)
        {
            ///作废
            RT.Service.Resolve<SchedulingInfController>().SchedulingInfCancel(viewModel);
            return true;
        }
    }
}
