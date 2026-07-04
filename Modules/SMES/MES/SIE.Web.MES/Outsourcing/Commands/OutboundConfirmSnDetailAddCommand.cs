using SIE.Domain;
using SIE.MES.Outsourcing;
using SIE.Resources.UserGroups;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.Outsourcing.Commands
{
    public class OutboundConfirmSnDetailAddCommand: ViewCommand
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var meta = ClientEntities.Find(args.Type);
            var savedData = RF.Find(meta.EntityType).NewList();
            var details = args.Data.ToJsonObject<List<OutboundConfirmSnDetail>>();
            Check.NotNullOrEmpty(details, nameof(details));

            if (null == details || details.Count == 0)
                throw new ArgumentNullException(nameof(details));

            RT.Service.Resolve<OutsourcingController>().SelectAddOutboundConfirmSnDetail(details);

            return true;
        }
    }
}
