using SIE.Domain;
using SIE.EMS.Maintains.Controller;
using SIE.EMS.Maintains.Plans;
using SIE.Web.Command;

namespace SIE.Web.EMS.EquipMaint.Maintains.Plans.Commands
{
    /// <summary>
    /// 修改保养计划
    /// </summary>
    [JsCommand(CommandName)]
    public class EditEquipMaintainPlanCommand : ViewCommand<ViewArgs>
    {
        /// <summary>
        /// 修改保养计划命令名字
        /// </summary>
        public const string CommandName = "SIE.Web.EMS.EquipMaint.Maintains.Plans.Commands.EditEquipMaintainPlanCommand";

        /// <summary>
        /// 修改保养计划命令执行
        /// </summary>
        /// <param name="args">视图参数</param>
        /// <param name="scope">命令参数</param>
        /// <returns>返回参数</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var editPlans = args.Data.ToJsonObject<EntityList<MaintainPlan>>();
            RT.Service.Resolve<MaintainController>().EditMaintainPlans(editPlans);
            return true;
        }
    }
}
