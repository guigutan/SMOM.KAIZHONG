using SIE.Domain;
using SIE.EMS.SpecialEquipment.RegularInspections;
using SIE.Web.Command;
using System;
using SIE.Domain.Validation;

namespace SIE.Web.EMS.SpecialEquipment.RegularInspections.Commands
{
    /// <summary>
    /// 保存命令
    /// </summary>
    internal class InputSaveCommand : FormSaveCommand
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
            RT.Service.Resolve<RegularInspectionController>().SaveRegularInspection(record);
        }
    }
}
