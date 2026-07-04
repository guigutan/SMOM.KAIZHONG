using SIE.AbnormalInfo.AbnormalMonitors;
using SIE.AbnormalInfo.Common;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Web.Command;
using System;

namespace SIE.Web.AbnormalInfo.AbnormalMonitors.Commands
{
    class SubmitProcessCommand : FormSaveCommand
    {
        protected override void DoSave(Entity entity)
        {
            if (entity == null)
            {
                throw new ValidationException("没有数据可以提交。".L10N());
            }
            if (!(entity is AbnormalMonitorTask))
                throw new ValidationException("该数据不是异常任务数据格式。".L10N());

            var bill = entity as AbnormalMonitorTask;

            RT.Service.Resolve<AbnormalMonitorTaskService>().ProcessSubmit(bill);
        }
    }
}
