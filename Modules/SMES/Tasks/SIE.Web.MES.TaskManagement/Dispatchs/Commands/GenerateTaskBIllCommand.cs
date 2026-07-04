using SIE.Domain.Validation;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.Dispatchs.ViewModels;
using SIE.Web.Command;
using System;

namespace SIE.Web.MES.TaskManagement.Dispatchs.Commands
{
    /// <summary>
    /// 生成任务单
    /// </summary>
    [JsCommand("SIE.Web.MES.TaskManagement.Dispatchs.GenerateTaskBIllCommand")]
    public class GenerateTaskBIllCommand : ViewCommand<ViewArgs>
    {
        public const string FullName = "SIE.Web.MES.TaskManagement.Dispatchs.GenerateTaskBIllCommand";

        /// <summary>
        /// 生成任务单命令
        /// </summary>
        /// <param name="args">数据</param>
        /// <param name="scope">scope</param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var billInfo = args.Data.ToJsonObject<TaskBillInfo>();
            var isAccordConfig = billInfo.IsAccordConfig;
            var workOrder = billInfo.WorkOrder;
            if (workOrder == null) return "工单有误".L10N();
            if (RT.Service.Resolve<DispatchController>().IsExistsDispatchTask(workOrder.Id))
                return "工单已经有任务单".L10N();
            if (workOrder.State != SIE.Core.WorkOrders.WorkOrderState.Release || workOrder.IsPause == YesNo.Yes)
                throw new ValidationException("发放状态工单才可生成任务单".L10N());
            var taskConfig = RT.Service.Resolve<DispatchController>().GetDispatchTaskConfig();
            if (!taskConfig.IsGenerate) throw new ValidationException("工单没有配置生成任务单".L10N());
            RT.Service.Resolve<DispatchController>().GenerateWorkOrderDispatchTasks(workOrder.Id, isAccordConfig, taskConfig);
            return true;
        }
    }
}
