using SIE.Core.ApiModels;
using SIE.Domain;
using SIE.EMS.Checks;
using SIE.EMS.Checks.Plans;
using SIE.EMS.Checks.Plans.ViewModels;
using SIE.EMS.Equipments;
using SIE.EMS.Equipments.Accounts;
using SIE.Equipments.EquipAccounts;
using SIE.Web.Command;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.EMS.Checks.Plans.Commands
{
    /// <summary>
    /// 批量添加点检计划
    /// </summary>
    [JsCommand("SIE.Web.EMS.Checks.Plans.Commands.BatchAddCheckPlanCommand")]
    public class BatchAddCheckPlanCommand : ViewCommand<ViewArgs>
    {
        /// <summary>
        /// 执行批量添加点检计划
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var checkPlanPrj = args.Data.ToJsonObject<CheckPlanProject>();
            List<BaseDataInfo> equipList = RT.Service.Resolve<EquipAccountController>().GetEquipAccountBaseInfos(checkPlanPrj.EquipAccountsIds);

            RT.Service.Resolve<CheckPlanController>().BatchAddCheckPlan(checkPlanPrj.AddCheckPlan, equipList, CheckSourceType.NewCreated);
            return new AddCheckPlanResultInfo() { ErrMsg = "" };
        }
    }
}
