using SIE.Domain;
using SIE.EMS.Checks;
using SIE.EMS.Checks.Plans;
using SIE.EMS.EquipRepair.Controller;
using SIE.EMS.EquipRepair.EquipRepairs;
using SIE.Web.Command;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.EMS.EquipRepair.EquipRepairs.Commands
{
    /// <summary>
    /// 更换备件 命令
    /// </summary>
    [JsCommand("SIE.Web.EMS.EquipRepair.EquipRepairs.Commands.ExeChangeSparePartCommand")]
    public class ExeChangeSparePartCommand : ViewCommand
    {
        /// <summary>
        /// 执行更换备件命令
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var datas = args.Data.ToJsonObject<List<EquipRepairSparePartChg>>();
            if (datas.Count > 0)
            {
                RT.Service.Resolve<RepairController>().UIChangeEquipRepairSparePart(datas);
            }
            return true;
        }
    }
}
