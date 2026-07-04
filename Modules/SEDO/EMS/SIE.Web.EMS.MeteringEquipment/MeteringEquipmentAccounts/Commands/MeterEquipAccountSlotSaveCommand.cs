using SIE.Domain;
using SIE.Web.Command;

namespace SIE.Web.EMS.MeteringEquipment.MeteringEquipmentAccounts.Commands
{
    /// <summary>
    /// 设备缸槽保存
    /// </summary>
    [JsCommand("SIE.Web.EMS.MeteringEquipment.MeteringEquipmentAccounts.Commands.MeterEquipAccountSlotSaveCommand")]
    public class MeterEquipAccountSlotSaveCommand : SaveCommand
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
