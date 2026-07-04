using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.MES.WorkOrders;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.WorkOrders.Commands
{
    /// <summary>
    /// 手动完工
    /// </summary>
    public class WorkOrderCompletionCommand : ViewCommand
    {
        protected override object Excute(ViewArgs args, string scope)
        {
            var entity = args.Data.ToJsonObject<SIE.MES.WorkOrders.WorkOrder>();
            entity.State = WorkOrderState.Finish;
            RF.Save(entity);
         
            return true;
        }
    }
}
