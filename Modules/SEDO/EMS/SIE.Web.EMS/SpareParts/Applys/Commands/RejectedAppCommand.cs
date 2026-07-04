using SIE.Domain;
using SIE.EMS.SpareParts.Applys;
using SIE.EMS.SpareParts.Applys.Controllers;
using SIE.EMS.SpareParts.Applys.Enums;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.SpareParts.Applys.Commands
{
    /// <summary>
    /// 驳回
    /// </summary>
    [JsCommand("SIE.Web.EMS.SpareParts.Applys.Commands.RejectedAppCommand")]
    public class RejectedAppCommand : FormSaveCommand
    {
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="entity"></param>
        protected override void DoSave(Entity entity)
        {
            var spa = entity as SparePartApp;
            
            //保存
            RT.Service.Resolve<SparePartAppController>().ChageAuditSatate(spa, AuditState.Returned);
        }
    }
}
