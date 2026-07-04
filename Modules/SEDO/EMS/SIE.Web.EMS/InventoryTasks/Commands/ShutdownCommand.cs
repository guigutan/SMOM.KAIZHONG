using SIE.Domain.Validation;
using SIE.EMS.InventoryPlans;
using SIE.EMS.InventoryTasks;
using SIE.Equipments.WorkFlows;
using SIE.Web.Command;
using System;
using System.Collections.Generic;

namespace SIE.Web.EMS.InventoryTasks.Commands
{
    /// <summary>
    /// 关闭
    /// </summary>
    public class ShutdownCommand : ViewCommand
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
                throw new ValidationException("关闭信息异常".L10N());
            if (info.Remark.IsNullOrWhiteSpace())
                throw new ValidationException("关闭原因不能为空".L10N());
            List<double> selectedIds = new List<double>(args.SelectedIds);
            RT.Service.Resolve<InventoryTaskController>().Shutdown(selectedIds, info.Remark);
            return true;
        }


    }
}
