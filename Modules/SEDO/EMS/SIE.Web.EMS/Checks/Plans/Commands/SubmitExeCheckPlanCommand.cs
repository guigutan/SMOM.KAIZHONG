using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Checks;
using SIE.EMS.Checks.Plans;
using SIE.EMS.Checks.Plans.ViewModels;
using SIE.Web.Command;
using System;
using System.Linq;

namespace SIE.Web.EMS.Checks.Plans.Commands
{
    /// <summary>
    /// 保存执行点检计划
    /// </summary>
    [JsCommand("SIE.Web.EMS.Checks.Plans.Commands.SubmitExeCheckPlanCommand")]
    public class SubmitExeCheckPlanCommand : FormSaveCommand
    {
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="entity">实体</param>
        protected override void DoSave(Entity entity)
        {
            if (entity == null)
                throw new ValidationException("没有数据可以提交。".L10N());
            var bill = entity as CheckPlan;
            if (bill == null)
                throw new ValidationException("该数据不是点检计划数据格式。".L10N());

            var rtn = RT.Service.Resolve<CheckPlanController>().SubmitCheckPlan(bill);

            base.DoSave(rtn);
        }
    }
}
