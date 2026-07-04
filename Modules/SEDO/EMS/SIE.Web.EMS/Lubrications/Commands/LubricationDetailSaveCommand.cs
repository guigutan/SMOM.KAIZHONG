using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Lubrications;
using SIE.Web.Command;
using System;

namespace SIE.Web.EMS.Lubrications.Commands
{
    /// <summary>
    /// 明细页添加润滑记录保存
    /// </summary>
    internal class LubricationDetailSaveCommand : FormSaveCommand
    {
        protected override void DoSave(Entity entity)
        {
            if (entity == null)
            {
                throw new ValidationException("没有数据可以保存。".L10N());
            }
            if (!(entity is Lubrication))
            {
                throw new ValidationException("该数据不是校验记录数据格式。".L10N());
            }
            var lub = entity as Lubrication;
            RT.Service.Resolve<LubricationController>().LubricationDetailSave(lub);
        }
    }
}
