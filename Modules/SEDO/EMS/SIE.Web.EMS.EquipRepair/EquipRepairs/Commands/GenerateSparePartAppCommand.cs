using SIE.Domain.Validation;
using SIE.EMS.EquipRepair.Controller;
using SIE.EMS.EquipRepair.EquipRepairs;
using SIE.Web.Command;
using System;
using System.Collections.Generic;

namespace SIE.Web.EMS.EquipRepair.EquipRepairs.Commands
{
    /// <summary>
    /// 生成备件申请单 命令
    /// </summary>
    [JsCommand("SIE.Web.EMS.EquipRepair.EquipRepairs.Commands.GenerateSparePartAppCommand")]
    public class GenerateSparePartAppCommand : ViewCommand
    {
        /// <summary>
        /// 执行生成备件申请单命令
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var datas = args.Data.ToJsonObject<List<EquipRepairSparePartApl>>();
            if (datas.Count > 0)
            {
                RT.Service.Resolve<RepairController>().UIGenerateSparePartApp(datas);
            }
            else
            {
                throw new ValidationException("未添加申请单明细".L10N());
            }
            return true;
        }
    }
}
