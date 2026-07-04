using SIE.Domain;
using SIE.EMS.SpareParts.Applys;
using SIE.EMS.SpareParts.Applys.Controllers;
using SIE.EMS.SpareParts.OutDepots;
using SIE.EMS.SpareParts.OutDepots.Controllers;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.SpareParts.OutDepots.Commands
{ /// <summary>
  /// 保存
  /// </summary>
    [JsCommand("SIE.Web.EMS.SpareParts.OutDepots.Commands.SaveOutDepotCommand")]
    public class SaveOutDepotCommand : FormSaveCommand
    {
        protected override void DoSave(Entity entity)
        {
            var od = entity as OutDepot;

            //保存
            RT.Service.Resolve<OutDepotController>().SaveOutDepot(od);
        }
    }
}
