using SIE.Domain;
using SIE.EMS.Equipments;
using SIE.EMS.EquipRepair.Controller;
using SIE.EMS.EquipRepair.EquipRepairs;
using SIE.EMS.EquipRepair.EquipRepairs.ViewModels;
using SIE.EMS.EquipRepairs.Enums;
using SIE.Web.Command;

namespace SIE.Web.EMS.EquipMaint.Maintains.Plans.Commands
{
    /// <summary>
    /// 保养执行报修
    /// </summary>
    [JsCommand(CommandName)]
    public class AddEquipRepairCommand : ViewCommand
    {
        /// <summary>
        /// 添加保养计划命令名字
        /// </summary>
        public const string CommandName = "SIE.Web.EMS.EquipMaint.Maintains.Plans.Commands.AddEquipRepairCommand";

        /// <summary>
        /// 命令执行
        /// </summary>
        /// <param name="args">视图参数</param>
        /// <param name="scope">命令参数</param>
        /// <returns>返回参数</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            return true;
        }
    }
}
