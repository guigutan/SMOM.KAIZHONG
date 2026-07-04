using Elsa.Attributes;
using Elsa.Services.Models;
using SIE.RedCardManagment.WorkFlow.Common;
using SIE.WorkFlow.Core;
using SIE.WorkFlow.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.RedCardManagment.WorkFlow.Activities.RedCardApplyFinish
{
    /// <summary>
    /// 红牌申请-结束节点
    /// </summary>
    [SieWorkFlowActivity(
        Category = RedCardWorkFlowText.RedCardWorkFlowName,
        Description = RedCardWorkFlowText.EndNode,
        DisplayName = RedCardWorkFlowText.End,
        IconType = SIE.WorkFlow.Core.Enums.ActivityIconType.FormFinish,
        Outcomes = null
   )]
    public class RedCardApplyFinishActivity : BackgroundActivity
    {

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

        /// <summary>
        /// 节点具体业务 
        /// </summary>
        /// <param name="context"></param>
        protected override void OnActivityExecute(ActivityExecutionContext context)
        {
            RT.Service.Resolve<RedCardApplyBillWorkflowController>().FinishApply(context);
        }

        /// <summary>
        /// 任务处理记录 
        /// </summary>
        /// <param name="context"></param>
        protected override void SaveFlowProcessRecord(ActivityExecutionContext context)
        {
            SaveFlowProcessRecordWithMessage(context, "自动处理".L10N(), "", null);
        }
    }
}
