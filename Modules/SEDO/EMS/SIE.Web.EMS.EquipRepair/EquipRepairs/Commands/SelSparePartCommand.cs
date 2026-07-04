using SIE.Domain;
using SIE.EMS.EquipRepair.Controller;
using SIE.EMS.EquipRepair.EquipRepairs;
using SIE.EMS.EquipRepairs.Enums;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.EMS.EquipRepair.EquipRepairs.Commands
{
    /// <summary>
    /// 选择备件
    /// </summary>
    [JsCommand("SIE.Web.EMS.EquipRepair.EquipRepairs.Commands.SelSparePartCommand")]
    public class SelSparePartCommand : ViewCommand
    {
        /// <summary>
        /// 执行选择
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var sparePartList = args.Data.ToJsonObject<List<EquipRepairSparePartChg>>();
            Check.NotNullOrEmpty(sparePartList, nameof(sparePartList));
            if (sparePartList == null || sparePartList.Count == 0)
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(sparePartList)));

            EntityList<EquipRepairSparePartChg> equipRepairSpareParts = new EntityList<EquipRepairSparePartChg>();
            sparePartList.ForEach(p =>
            {
                p.PersistenceStatus = PersistenceStatus.New;
                p.ChangeQty = 1;
                equipRepairSpareParts.Add(p);
            });
            RF.Save(equipRepairSpareParts);
            if (sparePartList.FirstOrDefault()?.EquipRepairBill?.RepairState == EquipRepairState.WaitRepair)
            {
                RT.Service.Resolve<RepairController>().ChangeRepairState(sparePartList.FirstOrDefault().EquipRepairBillId, EquipRepairState.Repairing);
            }
            return true;
        }
    }
}
