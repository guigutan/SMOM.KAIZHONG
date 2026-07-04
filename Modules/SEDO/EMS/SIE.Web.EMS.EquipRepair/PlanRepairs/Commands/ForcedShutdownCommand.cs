using SIE.Domain.Validation;
using SIE.EMS.EquipRepair.PlanRepairs;
using SIE.Equipments.WorkFlows;
using SIE.Fixtures;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.EquipRepair.PlanRepairs.Commands
{
    /// <summary>
    /// 强制关单
    /// </summary>
    public class ForcedShutdownCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var info = args.Data.ToJsonObject<ExamineInfo>();
            if (info == null)
                throw new ValidationException("关单信息异常".L10N());
            List<double> selectedIds = new List<double>(args.SelectedIds);
            RT.Service.Resolve<PlanRepairsController>().ForcedShutdown(selectedIds);
            return true;
        }

       
    }
}
