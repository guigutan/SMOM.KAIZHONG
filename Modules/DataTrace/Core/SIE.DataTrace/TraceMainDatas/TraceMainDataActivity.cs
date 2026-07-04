using Elsa.Attributes;
using Elsa.Services.Models;
using SIE.DataTrace.Activities.Core;
using SIE.WorkFlow.Core;
using SIE.WorkFlow.Core.Attributes;
using SIE.WorkFlow.Core.Enums;
using SIE.WorkFlow.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.DataTrace.TraceMainDatas
{
    /// <summary>
    /// PDCA-单据信息节点
    /// </summary>
    [SieWorkFlowActivity(
        Category = "工单追溯",
        Description = "追溯主节点",
        DisplayName = "追溯主节点",
        IconType = ActivityIconType.Form
   )]
    public class TraceMainDataActivity : PanelAutoPassActivity
    {
        #region 属性
        #region 实体类型
        /// <summary>
        /// 实体类型
        /// </summary>
        [ActivityInput(Label = "实体类型", Order = -1)]
        public override string entityTypeName { get; set; } = typeof(TraceMainData).FullName;
        #endregion

        #endregion

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override void OnActivityExecute(ActivityExecutionContext context)
        {
            if (context.Input is DataTraceVariable input)
            {
                base.SetVariable(context, input);
               
                SaveFlowProcessRecordWithMessage(context, "发起", input.Starter, null);
            }
            else if (context.Input is RejectArriveInput)
            {
                SaveFlowProcessRecordWithMessage(context, "自动重新发起", "", null);
            }
        }
    }
}
