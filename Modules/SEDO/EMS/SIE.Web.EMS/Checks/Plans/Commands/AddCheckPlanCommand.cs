using DocumentFormat.OpenXml.EMMA;
using SIE.Domain;
using SIE.EMS.Checks;
using SIE.EMS.Checks.Plans;
using SIE.EMS.Checks.Plans.ViewModels;
using SIE.EMS.Equipments.Accounts;
using SIE.Equipments.EquipAccounts;
using SIE.Web.Command;

namespace SIE.Web.EMS.Checks.Plans.Commands
{
    /// <summary>
    /// 添加点检计划
    /// </summary>
    [JsCommand("SIE.Web.EMS.Checks.Plans.Commands.AddCheckPlanCommand")]
    public class AddCheckPlanCommand : ViewCommand<ViewArgs>
    {
        /// <summary>
        /// 添加点检计划
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var checkPlanPrj = args.Data.ToJsonObject<CheckPlanProject>();
            RT.Service.Resolve<CheckPlanController>().AddCheckPlan(checkPlanPrj.AddCheckPlan, CheckSourceType.NewCreated);
            return new AddCheckPlanResultInfo() { ErrMsg = "" };
        }

    }
}
