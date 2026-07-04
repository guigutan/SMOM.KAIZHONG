using SIE.MES.TaskManagement.SchedulingInfs;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.TaskManagement.SchedulingInfs.Commands
{
    /// <summary>
    /// 排程校验
    /// </summary>
    public class SchedulingInfValidCommand : ViewCommand<List<double>>
    {
        protected override object Excute(List<double> args, string scope)
        {
            RT.Service.Resolve<SchedulingInfController>().ValidSchedulingInf(args);
            return true;
        }
    }
}
