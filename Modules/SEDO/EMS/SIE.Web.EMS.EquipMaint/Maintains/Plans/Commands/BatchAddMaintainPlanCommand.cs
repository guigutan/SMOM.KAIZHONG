using SIE.EMS.Maintains.Controller;
using SIE.EMS.Maintains.Plans;
using SIE.EMS.Maintains.Plans.ViewModels;
using SIE.Web.Command;
using System.Collections.Generic;

namespace SIE.Web.EMS.EquipMaint.Maintains.Plans.Commands
{
    /// <summary>
    /// 批量添加保养计划
    /// </summary>
    [JsCommand(CommandName)]
    public class BatchAddMaintainPlanCommand : ViewCommand<ViewArgs>
    {
        /// <summary>
        /// 批量添加保养计划命令名字
        /// </summary>
        public const string CommandName = "SIE.Web.EMS.EquipMaint.Maintains.Plans.Commands.BatchAddMaintainPlanCommand";

        /// <summary>
        /// 批量添加保养计划命令执行
        /// </summary>
        /// <param name="args">视图参数</param>
        /// <param name="scope">命令参数</param>
        /// <returns>返回参数</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            //var mix = args.Data.ToJsonObject<MaintainPlanProject>();
            //RT.Service.Resolve<MaintainController>().BatchAddMaintainPlan(mix.AddMaintainPlan, mix.EquipAccountIds);
            var mix = args.Data.ToJsonObject<MaintainPlanDetail>();
            var lastString = RT.Service.Resolve<MaintainController>().AddMaintainPlans(mix.MaintainPlanList, mix.EquipAccountIds);
            return lastString;
        }

        /// <summary>
        /// 序列化的model
        /// </summary>
        public class MaintainPlanProject
        {
            /// <summary>
            /// 保养计划
            /// </summary>
            public MaintainPlan AddMaintainPlan { get; set; }

            /// <summary>
            /// 设备台账ID列表
            /// </summary>
            public List<double> EquipAccountIds { get; set; }
        }
    }
}
