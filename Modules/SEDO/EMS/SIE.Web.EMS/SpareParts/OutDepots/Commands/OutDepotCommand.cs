using SIE.Domain;
using SIE.EMS.SpareParts.OutDepots;
using SIE.EMS.SpareParts.OutDepots.Controllers;
using SIE.EMS.SpareParts.OutDepots.Enums;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.SpareParts.OutDepots.Commands
{
    /// <summary>
    /// 出库
    /// </summary>
    [JsCommand("SIE.Web.EMS.SpareParts.OutDepots.Commands.OutDepotCommand")]
    public class OutDepotCommand : FormSaveCommand
    {
        /// <summary>
        /// 出库
        /// </summary>
        /// <param name="entity"></param>
        protected override void DoSave(Entity entity)
        {
            var outDepot = entity as OutDepot;
            RT.Service.Resolve<OutDepotController>().OutDepotComp(outDepot);
        }
    }
}
