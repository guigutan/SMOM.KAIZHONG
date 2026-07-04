using SIE.EMS.Equipments.Boms;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.Equipments.Boms.Commands
{
    /// <summary>
    /// 设备BOM明细升级命令
    /// </summary>
    public class UpgradeSparePartCommand : ViewCommand
    {
        /// <summary>
        /// 执行升级
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var detailId = args.Data.ToJsonObject<double>();
            RT.Service.Resolve<EquipBomController>().UpgradeEquipBomDetail(detailId);
            return true;
        }
    }
}
