using SIE.Domain;
using SIE.EMS.EquipRepair.Controller;
using SIE.EMS.EquipRepair.EquipRepairs;
using SIE.EMS.EquipRepair.EquipRepairs.Enums;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.EquipRepair.EquipRepairs.Commands
{
    /// <summary>
    /// 维修继续
    /// </summary>
    [JsCommand("SIE.Web.EMS.EquipRepair.EquipRepairs.Commands.ContinueCommand")]
    public class ContinueCommand : FormSaveCommand
    {
        /// <summary>
        /// 维修继续
        /// </summary>
        /// <param name="entity"></param>
        protected override void DoSave(Entity entity)
        {
            var equipRepairBill = entity as EquipRepairBill;

            RT.Service.Resolve<RepairController>().Continue(equipRepairBill);
        }
    }
}
