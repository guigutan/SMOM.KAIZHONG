using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.MeteringEquipment.Calibrations;
using SIE.Web.Command;
using System;

namespace SIE.Web.EMS.MeteringEquipment.Calibrations.Commands
{
    /// <summary>
    /// 审核保存
    /// </summary>
    public class AuditSubmitCommand : FormSaveCommand
    {
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="entity"></param>
        protected override void DoSave(Entity entity)
        {
            if (entity == null)
            {
                throw new ValidationException("没有数据可以提交。".L10N());
            }
            if (!(entity is Calibration))
            {
                throw new ValidationException("该数据不是校验记录数据格式。".L10N());
            }
            var record = entity as Calibration;
            RT.Service.Resolve<CalibrationController>().AuditSumbitRecord(record);
        }
    }
}
