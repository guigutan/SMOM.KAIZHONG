using SIE.Domain;
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
    /// 打开上料命令
    /// </summary>
    public class OpenFeedingCloseCommand : ViewCommand<List<double>>
    {
        protected override object Excute(List<double> args, string scope)
        {
            RT.Service.Resolve<DispatchController>().OpenFeedingClose(args);
            return true;
        }
    }
}
