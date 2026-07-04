using SIE.Domain;
using SIE.EMS.ViceTransfers;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.ViceTransfers.Commands
{
    /// <summary>
    /// 保存副资产
    /// </summary>
    [JsCommand("SIE.Web.EMS.ViceTransfers.Commands.SaveTransferCommand")]
    public class SaveTransferCommand : FormSaveCommand
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        protected override void DoSave(Entity entity)
        {
            var model = entity as ViceTransfer;
            RT.Service.Resolve<ViceTransferController>().SaveViceTransfer(model);
        }
    }
}
