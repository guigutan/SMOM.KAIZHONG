using SIE.Domain;
using SIE.EMS.EquipRepair.PlanRepairs;
using SIE.Web.Command;

namespace SIE.Web.EMS.EquipRepair.PlanRepairs.Commands
{
    /// <summary>
    /// 保存
    /// </summary>
    [JsCommand("SIE.Web.EMS.EquipRepair.PlanRepairs.Commands.SavePlanRepairsCommand")]
    public class SavePlanRepairsCommand : FormSaveCommand
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        protected override void DoSave(Entity entity)
        {
            var model = entity as PlanRepair;
            RT.Service.Resolve<PlanRepairsController>().SavePlanRepair(model);
        }
    }
}
