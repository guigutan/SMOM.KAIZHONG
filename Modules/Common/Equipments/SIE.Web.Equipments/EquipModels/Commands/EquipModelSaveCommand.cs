using SIE.Core.Enums;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Equipments.EquipModels;
using SIE.Web.Command;
using System;

namespace SIE.Web.Equipments.EquipModels.Commands
{
    /// <summary>
    /// 设备型号表单保存
    /// </summary>
    public class EquipModelSaveCommand : FormSaveCommand
    {
        /// <summary>
        /// 保存前验证
        /// </summary>
        /// <param name="entity">实体</param>
        protected override void OnValidation(Entity entity)
        {
            base.OnValidation(entity);
            var equipModel = entity as EquipModel;
            var equipModelList = RT.Service.Resolve<EquipModelController>()
                .GetEquipModelByCode(equipModel.Id, equipModel.Code);

            if (equipModelList.Count > 0)
                throw new ValidationException("已经存在[型号编码]是{0}的[设备型号维护]".L10nFormat(equipModel.Code));

            if (equipModel.IndustryCategory == IndustryCategory.GeneralEquipment
                || equipModel.IndustryCategory == IndustryCategory.LogisticsEquipment)
            {
                equipModel.RailType = null;
                equipModel.VirtualDevice = YesNo.No;
                equipModel.FeederBinding = YesNo.No;
                equipModel.FeederLocFailSafe = State.Disable;
                equipModel.FeederBarcodeFailSafe = State.Disable;
                equipModel.IsDisabled = YesNo.No;
                equipModel.AgingType = null;
                equipModel.ProductionType = null;

                //equipModel.AverageBeat = 0;
                //equipModel.StandardCapacity = 0;
                //equipModel.CapacityUnit = null;
            }
            else if (equipModel.IndustryCategory == IndustryCategory.ElecInitIndustry)
            {
                if (equipModel.LocationList.Count <= 0)
                {
                    throw new ValidationException("行业属性为电子行业时，位置列表不能为空".L10N());
                }
                //equipModel.AverageBeat = 0;
                //equipModel.StandardCapacity = 0;
                //equipModel.CapacityUnit = null;
            }
            else if (equipModel.IndustryCategory == IndustryCategory.PcbInitIndustry)
            {
                equipModel.RailType = null;
                equipModel.VirtualDevice = YesNo.No;
                equipModel.FeederBinding = YesNo.No;
                equipModel.FeederLocFailSafe = State.Disable;
                equipModel.FeederBarcodeFailSafe = State.Disable;
                equipModel.IsDisabled = YesNo.No;
                equipModel.AgingType = null;
                equipModel.ProductionType = null;
            }
        }
    }
}
