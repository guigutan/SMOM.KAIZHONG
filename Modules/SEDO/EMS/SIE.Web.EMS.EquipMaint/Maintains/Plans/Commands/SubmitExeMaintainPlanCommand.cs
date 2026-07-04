using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Maintains.Plans;
using SIE.Web.Command;
using System;
using SIE.EMS.Maintains.Controller;

namespace SIE.Web.EMS.EquipMaint.Maintains.Plans.Commands
{
    /// <summary>
    /// 提交执行保养执行
    /// </summary>
    [JsCommand("SIE.Web.EMS.EquipMaint.Maintains.Plans.Commands.SubmitExeMaintainPlanCommand")]
    public class SubmitExeMaintainPlanCommand : FormSaveCommand
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

            var bill = entity as MaintainPlan;

            if (bill == null)
            {
                throw new ValidationException("该数据不是保养执行数据格式。".L10N());
            }

            RT.Service.Resolve<MaintainController>().SubmitMaintainPlan(bill);
        }
    }
}
