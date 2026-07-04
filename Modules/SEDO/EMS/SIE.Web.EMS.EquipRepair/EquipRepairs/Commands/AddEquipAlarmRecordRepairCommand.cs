using SIE.Domain.Validation;
using SIE.EMS.EquipRepair.Controller;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.EMS.EquipRepair.EquipRepairs.Commands
{
    /// <summary>
    /// 生成维修工单
    /// </summary>
    public class AddEquipAlarmRecordRepairCommand : ViewCommand
    {
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="args">参数</param>
        /// <param name="scope">作用域</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            if (args == null)
            {
                return false;
            }

            List<double> selectedIds = new List<double>(args.SelectedIds);
            if (!selectedIds.Any())
            {
                throw new ValidationException("请先选择数据".L10N());
            }

            RT.Service.Resolve<RepairController>().AddEquipAlarmRecordRepair(selectedIds);
            return true;
        }
    }
}
