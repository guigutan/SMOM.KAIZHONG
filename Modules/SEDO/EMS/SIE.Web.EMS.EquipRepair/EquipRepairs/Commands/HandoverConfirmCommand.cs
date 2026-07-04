using SIE.EMS.EquipRepair.Controller;
using SIE.EMS.EquipRepair.EquipRepairs.ViewModels;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SIE.EMS.EquipRepair.EquipRepairs;
using SIE.Domain;

namespace SIE.Web.EMS.EquipRepair.EquipRepairs.Commands
{
    /// <summary>
    /// 交机确认命令
    /// </summary>
    public class HandoverConfirmCommand : FormSaveCommand
    {
        /// <summary>
        /// 交机确认命令执行
        /// </summary>
        /// <param name="args">视图参数</param>
        /// <param name="scope">命令参数</param>
        /// <returns>返回参数</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            if (args.Data.Contains("\"_model\":\"SIE.EMS.EquipRepair.EquipRepairs.EquipRepairBill\""))
            {
                var entity = GetDeserializeData(args, scope)[0] as EquipRepairBill;
                RF.Save(entity);
                return true;
            }
            else
            {
                var info = args.Data.ToJsonObject<HandoverConfirmInfo>();
                RT.Service.Resolve<RepairController>().HandoverConfirm(info.repairBill, info.detailList);
                return true;
            }
        }
    }
}
