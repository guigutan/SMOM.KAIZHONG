using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.ViceTransfers;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.ViceTransfers.Commands
{

    /// <summary>
    /// 保存副资产执行
    /// </summary>
    [JsCommand("SIE.Web.EMS.ViceTransfers.Commands.ExecutSaveComamns")]
    public class ExecutSaveComamns : ViewCommand<ViewArgs>
    {
        protected override object Excute(ViewArgs args, string scope)
        {
            return null;
        }
    }
}
