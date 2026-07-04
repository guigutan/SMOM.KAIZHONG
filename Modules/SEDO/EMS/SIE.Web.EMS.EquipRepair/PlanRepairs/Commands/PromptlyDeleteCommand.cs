using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.EquipRepair.PlanRepairs.Commands
{
    /// <summary>
    ///  带判断审核状态的立即删除
    /// </summary>
    [JsCommand("SIE.Web.EMS.EquipRepair.PlanRepairs.Commands.PromptlyDeleteCommand")]
    public class PromptlyDeleteCommand : DeleteCommand
    {

    }
}
