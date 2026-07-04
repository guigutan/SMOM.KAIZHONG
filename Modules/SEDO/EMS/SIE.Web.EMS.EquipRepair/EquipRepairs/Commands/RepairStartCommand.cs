using SIE.Core.Enums;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.DevicePurs;
using SIE.EMS.EquipRepair.Controller;
using SIE.EMS.EquipRepair.EquipRepairs;
using SIE.EMS.EquipRepair.EquipRepairs.Enums;
using SIE.EMS.EquipRepair.EquipRepairs.ViewModels;
using SIE.Equipments.EquipAccounts;
using SIE.Resources.Employees;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.Web.EMS.EquipRepair.EquipRepairs.Commands
{
    /// <summary>
    /// 维修开始
    /// </summary>
    [JsCommand("SIE.Web.EMS.EquipRepair.EquipRepairs.Commands.RepairStartCommand")]
    public class RepairStartCommand : ViewCommand
    {
        /// <summary>
        /// 维修开始
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>bool</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var repairStartViewModel = args.Data.ToJsonObject<RepairStartViewModel>();

            RT.Service.Resolve<RepairController>().StartRepair(repairStartViewModel);

            return true;
        }
    }
}
