using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Equipments.EquipmentCards;
using SIE.Web.Command;
using System;

namespace SIE.Web.Equipments.EquipmentCards.Commands
{
    /// <summary>
    /// 新增保存
    /// </summary>
    public class SaveEquipCardCommand : FormSaveCommand
    {
        /// <summary>
        /// 保存设备立卡
        /// </summary>
        /// <param name="entity"></param>
        protected override void DoSave(Entity entity)
        {
            if (entity == null)
            {
                throw new ValidationException("没有数据可以保存。".L10N());
            }
            if (!(entity is EquipmentCard))
            {
                throw new ValidationException("该数据不是校验记录数据格式。".L10N());
            }
            var card = entity as EquipmentCard;
            RT.Service.Resolve<EquipmentCardController>().SaveEquipmentCard(card);
        }
    }
}
