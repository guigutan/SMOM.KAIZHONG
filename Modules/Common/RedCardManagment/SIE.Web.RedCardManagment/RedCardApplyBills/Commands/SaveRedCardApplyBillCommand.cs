using SIE.Domain;
using SIE.Domain.Validation;
using SIE.RedCardManagment.RedCardApplyBills;
using SIE.RedCardManagment.RedCardApplyBills.Service;
using SIE.Web.Command;
using System;

namespace SIE.Web.RedCardManagment.RedCardApplyBills.Commands
{
    class SaveRedCardApplyBillCommand : FormSaveCommand
    {
        protected override void DoSave(Entity entity)
        {
            if (entity == null)
            {
                throw new ValidationException("没有数据可以保存。".L10N());
            }
            if (!(entity is RedCardApplyBill))
                throw new ValidationException("该数据不是红牌申请单数据格式。".L10N());

            var bill = entity as RedCardApplyBill;

            RT.Service.Resolve<RedCardApplyBillService>().SaveBill(bill);
        }
    }
}
