using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Maintains.ApiModels;
using SIE.EMS.Maintains.Controller;
using SIE.EMS.Maintains.Plans;
using SIE.Web.Command;
using System;
using System.Collections.Generic;

namespace SIE.Web.EMS.EquipMaint.Maintains.Confirmations.Commands
{
    /// <summary>
    /// 保存：保养确认
    /// </summary>
    [JsCommand("SIE.Web.EMS.EquipMaint.Maintains.Confirmations.Commands.SubmitMaintainConfirmationCommand")]
    public class SubmitMaintainConfirmationCommand : FormSaveCommand
    {
        
    }
}
