using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.SpecialEquipment.RegularInspections;
using SIE.Web.Command;
using System;

namespace SIE.Web.EMS.SpecialEquipment.RegularInspections.Commands
{
    /// <summary>
    /// 审核驳回
    /// </summary>
    internal class AuditRejectCommand : FormSaveCommand
    {
        protected override void DoSave(Entity entity)
        {
            if (entity == null)
            {
                throw new ValidationException("没有数据可以保存。".L10N());
            }
            if (!(entity is RegularInspection))
            {
                throw new ValidationException("该数据不是校验记录数据格式。".L10N());
            }
            var record = entity as RegularInspection;
            if (!record.ApprovalInfo.IsNotEmpty())
            {
                throw new ValidationException("审核意见必填验证。".L10N());
            }
            RT.Service.Resolve<RegularInspectionController>().AuditRejectRecord(record);
        }
    }
}
