using SIE.Domain;
using SIE.Web.Command;

namespace SIE.Web.Equipments.EquipAccounts.Commands
{
    /// <summary>
    /// 设备缸槽保存
    /// </summary>
    [JsCommand("SIE.Web.Equipments.EquipAccounts.Commands.EquipAccountSlotSaveCommand")]
    public class EquipAccountSlotSaveCommand : SaveCommand
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        protected override void DoSave(EntityList data)
        {
            base.DoSave(data);
        }
    }
}
