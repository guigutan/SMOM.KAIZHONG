using NPOI.POIFS.Storage;
using SIE.MES.WorkOrders;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.WorkOrders.Commands
{
    public class GeneraDispatchTaskCommand : ViewCommand<List<double>>
    {
        protected override object Excute(List<double> args, string scope)
        {
            RT.Service.Resolve<WorkOrderController>().GeneraDispatchTask(args);
            return true;
        }
    }
}
