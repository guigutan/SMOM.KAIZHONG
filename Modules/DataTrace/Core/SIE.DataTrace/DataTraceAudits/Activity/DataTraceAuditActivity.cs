using Elsa.ActivityResults;
using Elsa.Services.Models;
using SIE.DataTrace.Activities.Core;
using SIE.WorkFlow.Activities.Audit;
using SIE.WorkFlow.Base.Common.Activities.Audit;
using SIE.WorkFlow.Core;
using SIE.WorkFlow.Core.Attributes;
using SIE.WorkFlow.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.DataTrace.DataTraceAudits.Activity
{
    /// <summary>
    /// 追溯节点
    /// </summary>
    [SieWorkFlowActivity(
       Category = "工单追溯",
       Description = "追溯审核节点",
       DisplayName = "追溯审核",
       IconType = ActivityIconType.Audit,
       Outcomes = new[] { OutcomeNames.Agree, OutcomeNames.Reject }
   )]
    public class DataTraceAuditActivity : Audit
    {
        /// <summary>
        /// 到达节点时处理
        /// </summary>
        /// <param name="context"></param>
        public override void OnArriveActivityExecute(ActivityExecutionContext context)
        {
            if (context.Input is DataTraceActivityInfo traceInput)
            {
                //把数据追溯节点的信息写入到上下文中
                context.SetVariable($"TraceInput_{context.ActivityId}", traceInput);
            }
        }

        /// <summary>
        /// 返回节点结果
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override IActivityExecutionResult ReturnResult(ActivityExecutionContext context)
        {
            var result = base.ReturnResult(context);
            if (result is OutcomeResult outcomeResult)
            {
                if (outcomeResult.Outcomes.FirstOrDefault() == OutcomeNames.Agree)
                {
                    var variable = context.GetSieVariable<DataTraceVariable>();
                    var traceMainDataId = variable.TraceMainDataId;
                    var traceInput = context.GetVariable($"TraceInput_{context.ActivityId}") as DataTraceActivityInfo;
                    if (traceInput == null)
                        return result;
                    //同意时，添加保存签名信息
                    if (context.Input is AuditModel input && traceMainDataId != null)
                    {
                            RT.Service.Resolve<DataTraceAuditController>().SaveDataTraceSignature(traceMainDataId.Value, variable.FlowInstanceId, traceInput.ActivityId, traceInput.EntityType, input.FlowTaskId);

                        return Outcome(OutcomeNames.Agree, traceInput); //继续把追溯节点信息输出，用于多个追溯审核节点的场景
                    }
                }
            }

            //默认返回基类结果
            return result;
        }
    }
}
