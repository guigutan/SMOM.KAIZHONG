using SIE.Domain;
using SIE.EMS.InventoryPlans;
using SIE.Web.Command;
using System.Linq;

namespace SIE.Web.EMS.InventoryPlans.Commands
{
    /// <summary>
    /// 保存盘点人
    /// </summary>
    [JsCommand("SIE.Web.EMS.InventoryPlans.Commands.SaveInventoryCounterCommand")]
    public class SaveInventoryCounterCommand : SaveCommand
    {
        /// <summary>
        /// 保存后动作
        /// </summary>
        /// <param name="data">数据</param>
        protected override void OnSaved(EntityList data)
        {
            base.OnSaved(data);
            var counterList = data as EntityList<InventoryCounter>;
            if (counterList != null && counterList.Any())
            {
                RT.Service.Resolve<InventoryPlanController>().UpdateInventoryCounter(counterList.FirstOrDefault().InventoryPlan);
            }
        }
    }
}
