using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Lubrications;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.Lubrications.Commands
{
   internal class LubricationDetailSubmitCommand : FormSaveCommand
    {

        protected override void DoSave(Entity entity)
        {
            if (entity == null)
            {
                throw new ValidationException("没有数据可以提交。".L10N());
            }
            if (!(entity is Lubrication))
            {
                throw new ValidationException("该数据不是校验记录数据格式。".L10N());
            }
            var record = entity as Lubrication;
            RT.Service.Resolve<LubricationController>().LubricationDetailSumbit(record);
        }
    }
}
