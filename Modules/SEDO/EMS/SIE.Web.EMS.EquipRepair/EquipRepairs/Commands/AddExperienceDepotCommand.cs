using SIE.EMS.EquipRepair.Controller;
using SIE.EMS.EquipRepair.EquipRepairs;
using SIE.Web.Command;

namespace SIE.Web.EMS.EquipRepair.EquipRepairs.Commands
{
    /// <summary>
    /// 添加经验库
    /// </summary>
    [JsCommand("SIE.Web.EMS.EquipRepair.EquipRepairs.Commands.AddExperienceDepotCommand")]
    public class AddExperienceDepotCommand : ViewCommand
    {
        /// <summary>
        /// 维修暂停
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>bool</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var repairBill = args.Data.ToJsonObject<EquipRepairBill>();
            RT.Service.Resolve<RepairController>().AddExperienceDepotByRepair(repairBill, args.SelectedIds[0] == 0);
            return true;
        }

    }
}
