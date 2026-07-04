using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.MeteringEquipment.Calibrations;
using SIE.Web.Command;
using System;

namespace SIE.Web.EMS.MeteringEquipment.Calibrations.Commands
{
    /// <summary>
    /// 保存录入检验报告
    /// </summary>
    public class CalibrationDetailSaveCommand : FormSaveCommand
    {
        /// <summary>
        /// 保存验证
        /// </summary>
        /// <param name="entity"></param>
        protected override void DoSave(Entity entity)
        {
            if (entity == null)
            {
                throw new ValidationException("没有数据可以保存。".L10N());
            }
            if (!(entity is Calibration))
            {
                throw new ValidationException("该数据不是校验记录数据格式。".L10N());
            }
            var record = entity as Calibration;
            RT.Service.Resolve<CalibrationController>().SaveCalibrationInput(record);
        }
    }
}
