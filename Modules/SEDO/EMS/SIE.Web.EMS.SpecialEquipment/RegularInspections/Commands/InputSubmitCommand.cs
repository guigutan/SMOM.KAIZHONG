using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.SpecialEquipment.RegularInspections;
using SIE.Web.Command;
using System;

namespace SIE.Web.EMS.SpecialEquipment.RegularInspections.Commands
{
    /// <summary>
    /// 特种设备定检提交
    /// </summary>
    internal class InputSubmitCommand : FormSaveCommand
    {

        protected override void DoSave(Entity entity)
        {
            if (entity == null)
            {
                throw new ValidationException("没有数据可以提交。".L10N());
            }
            if (!(entity is RegularInspection))
            {
                throw new ValidationException("该数据不是校验记录数据格式。".L10N());
            }
            var record = entity as RegularInspection;
            RT.Service.Resolve<RegularInspectionController>().SumbitRegularInspection(record);
        }
    }
}
