using SIE.EMS.Maintains.Controller;
using SIE.EMS.Maintains.Plans.ViewModels;
using SIE.Web.Command;

namespace SIE.Web.EMS.EquipMaint.Maintains.Plans.Commands
{
    /// <summary>
    /// 添加保养计划
    /// </summary>
    [JsCommand(CommandName)]
    public class AddMaintainPlanCommand : ViewCommand<ViewArgs>
    {
        /// <summary>
        /// 添加保养计划命令名字
        /// </summary>
        public const string CommandName = "SIE.Web.EMS.EquipMaint.Maintains.Plans.Commands.AddMaintainPlanCommand";

        /// <summary>
        /// 添加保养计划命令执行
        /// </summary>
        /// <param name="args">视图参数</param>
        /// <param name="scope">命令参数</param>
        /// <returns>返回参数</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            //var mix = args.Data.ToJsonObject<ExecuteDataResult>();
            //RT.Service.Resolve<MaintainController>().AddMaintainPlanCommand(mix.MaintainPlanList, mix.ProjectDetailList);
            var mix = args.Data.ToJsonObject<MaintainPlanDetail>();
            var lastString = RT.Service.Resolve<MaintainController>().AddMaintainPlans(mix.MaintainPlanList, mix.EquipAccountIds);
            return lastString;
        }
    }
}
