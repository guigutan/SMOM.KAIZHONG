using SIE.Domain;
using SIE.EMS.Equipments;
using SIE.Equipments.EquipAccounts;
using SIE.Web.Command;
using SIE.Web.Equipments.EquipAccounts.Commands;

namespace SIE.Web.EMS.Equipments.Accounts.Commands
{
    /// <summary>
    /// 设备台账编辑保存
    /// </summary>
    [JsCommand("SIE.Web.EMS.Equipments.Accounts.Commands.EquipmentsEditSaveAccountCommand")]
    public class EquipmentsEditSaveAccountCommand: EditSaveAccountCommand
    {
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="entity"></param>
        protected override void DoSave(Entity entity)
        {
            //新旧值
            var newEntity = entity as EquipAccount;
            
            ValidateEquipmentAccount(newEntity);

            //实体添加履历列表
            newEntity.ResumeList.AddRange(base.GenerateEquipAccountResumes(newEntity));

            RT.Service.Resolve<EquipController>().SaveEditEquipAccount(newEntity);
        }
    }
}
