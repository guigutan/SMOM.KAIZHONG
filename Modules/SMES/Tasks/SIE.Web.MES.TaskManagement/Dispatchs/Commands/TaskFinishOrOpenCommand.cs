using SIE.MES.TaskManagement.Dispatchs;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.TaskManagement.Dispatchs.Commands
{
    /// <summary>
    /// 完工/执行按钮
    /// </summary>
    public class TaskFinishOrOpenCommand : ViewCommand<List<double>>
    {
        protected override object Excute(List<double> args, string scope)
        {
            RT.Service.Resolve<DispatchController>().FinishOrOpenTasks(args);
            return true;
        }
    }
}
