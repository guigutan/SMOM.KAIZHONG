using SIE.EMS.Equipments.Boms;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.Web.EMS.Equipments.Boms.Commands
{
    /// <summary>
    /// 设备BOM明细降级命令
    /// </summary>
    public class DowngradeSparPartCommand : ViewCommand
    {
        /// <summary>
        /// 执行降级
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var parentDetailId = args.Data.ToJsonObject<double>();
            var detailId = args.SelectedIds.First();
            RT.Service.Resolve<EquipBomController>().DowngradeEquipBomDetail(detailId, parentDetailId);
            return true;
        }
    }
}
