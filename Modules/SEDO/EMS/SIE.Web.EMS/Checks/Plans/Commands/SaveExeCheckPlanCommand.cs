using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Checks.Plans;
using SIE.EMS.Enums;
using SIE.Web.Command;
using System;
using System.Linq;

namespace SIE.Web.EMS.Checks.Plans.Commands
{
    /// <summary>
    /// 保存执行点检计划
    /// </summary>
    [JsCommand("SIE.Web.EMS.Checks.Plans.Commands.SaveExeCheckPlanCommand")]
    public class SaveExeCheckPlanCommand : FormSaveCommand
    {
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="entity">实体</param>
        protected override void DoSave(Entity entity)
        {
            if (entity == null)
            {
                throw new ValidationException("没有数据可以提交。".L10N());
            }
            if (!(entity is CheckPlan))
                throw new ValidationException("该数据不是点检计划数据格式。".L10N());

            var bill = entity as CheckPlan;

            if (bill.CheckProjectList.Count == 0)
            {
                throw new ValidationException("不存在点检项目，不允许提交。".L10N());
            }

            if (bill.ExeState == CheckExeState.NotPerformed)
            {
                bill.ExeState = CheckExeState.Performing;
                bill.CheckDate = RF.Find<CheckPlan>().GetDbTime();
            }

            bill.CheckProjectList.ForEach(p =>
            {
                if (p.CheckResult == CheckMaintainResult.OK || p.CheckResult == CheckMaintainResult.NG || p.CheckResult == CheckMaintainResult.Unright || p.ActualValue.HasValue)
                {
                    p.ExeState = CheckExeState.Performing;
                }
            });

            bill.CheckPlanSparePartList.ForEach(p =>
            {
                //没有执行更换的备件项目，不保存申请单数据
                if (p.State == ChangeSparePartState.New) p.PartOutDepotDetailId = null;
            });

            base.DoSave(bill);
        }
    }
}
