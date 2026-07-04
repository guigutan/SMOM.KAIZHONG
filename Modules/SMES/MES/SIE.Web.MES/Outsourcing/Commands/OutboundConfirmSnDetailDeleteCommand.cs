using SIE.Domain;
using SIE.MES.Outsourcing;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.Outsourcing.Commands
{
    /// <summary>
    /// 删除
    /// </summary>
    public class OutboundConfirmSnDetailDeleteCommand : ViewCommand<List<double>>
    {

        protected override object Excute(List<double> args, string scope)
        {
            //删除
            RT.Service.Resolve<OutsourcingController>().DeleteOutboundConfirmSnDetail(args);
            return true;
        }
    }
}
