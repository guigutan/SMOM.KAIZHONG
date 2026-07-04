using Elsa;
using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.Services.Models;
using SIE.RedCardManagment.WorkFlow.Common;
using SIE.WorkFlow.Core;
using SIE.WorkFlow.Core.Attributes;
using SIE.WorkFlow.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.RedCardManagment.WorkFlow.Activities.RedCardApplyBill
{
    /// <summary>
    /// 红牌申请-单据信息节点
    /// </summary>
    [SieWorkFlowActivity(
       Category = RedCardWorkFlowText.RedCardWorkFlowName,
       Description = RedCardWorkFlowText.StartNode,
       DisplayName = RedCardWorkFlowText.ApplyInfo,
       IconType = SIE.WorkFlow.Core.Enums.ActivityIconType.Form,
       Outcomes = new[] { OutcomeNames.Done }
   )]
    public class RedCardApplyBillActivity : PanelAutoPassActivity
    {
        #region 属性

        #region 实体类型
        /// <summary>
        /// 实体类型
        /// </summary>
        [ActivityInput(Label = "实体类型", Order = -1)]
        public override string entityTypeName { get; set; } = typeof(SIE.RedCardManagment.RedCardApplyBills.RedCardApplyBill).FullName;
        #endregion

        #region 节点必选
        /// <summary>
        /// 节点必选
        /// </summary>
        [ActivityInput(Label = "是否必选", DefaultValue = "是", UIHint = "dropdown", Options = new string[]
   {
            "是"
   }, Hint = "勾选必选，在此分类中定义的流程，必须有该节点.", Order = 10f)]
        public override string isRequired
        {
            get;
            set;
        }
        #endregion
        #endregion

        /// <summary>
        /// 流程到达
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override void OnActivityExecute(ActivityExecutionContext context)
        {
            if (context.Input is RedCardApplyBillActivityInput input)
            {
                base.SetVariable(context, input.Variable);
                SaveFlowProcessRecordWithMessage(context, "发起".L10N(), input.Variable.Starter, null);
            }
        }

        /// <summary>
        /// 返回节点结果
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override IActivityExecutionResult ReturnResult(ActivityExecutionContext context)
        {
            //驳回到当前节点时，挂起，等待任务重新发起
            if (context.Input is RejectArriveInput)
                return Suspend();
            else
                return Done();
        }

        /// <summary>
        /// 被驳回到该节点时，回写驳回标识和驳回原因
        /// </summary>
        /// <param name="context"></param>
        /// <param name="input"></param>
        protected override void OnRejectArrive(ActivityExecutionContext context, RejectArriveInput input)
        {

            RT.Service.Resolve<RedCardApplyBillWorkflowController>().RejectArriveRedCardApplyBillActivity(context, input);
            SaveFlowProcessRecordWithMessage(context, "被驳回，待重新发起流程".L10N(), null, null);

        }
    }
}
