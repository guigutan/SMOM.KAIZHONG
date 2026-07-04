using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.MeteringEquipment.MeteringEquipmentAccounts;
using SIE.Web.Command;
using System;

namespace SIE.Web.EMS.MeteringEquipment.MeteringEquipmentAccounts.Commands
{
    /// <summary>
    /// 保存检验规程
    /// </summary>
    public class EquipAccountCalibrationSaveCommand : SaveCommand
    {

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="data"></param>
        protected override void DoSave(EntityList data)
        {
            if (data == null)
            {
                throw new ValidationException("没有数据可以保存。".L10N());
            }

           var EquipAccountCalibrationList = data as EntityList<EquipAccountCalibration>;
            foreach (var newLubPro in EquipAccountCalibrationList)
            {
                if (!newLubPro.PrevInspectionDate.HasValue)
                {
                    throw new ValidationException("校验名称【{0}】上次检验日期不能为空".L10nFormat(newLubPro.InspectionRule.Name));
                }
                if (!newLubPro.NextInspectionDate.HasValue)
                {
                    throw new ValidationException("校验名称【{0}】下次检验日期不能为空".L10nFormat(newLubPro.InspectionRule.Name));
                }
            }

            RT.Service.Resolve<MeteringEquipmentAccountController>().SaveEquipAccountCalibration(EquipAccountCalibrationList);

        }
    }
}
