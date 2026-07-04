using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.Design;
using Elsa.Services.Models;
using SIE.Common.WorkFlow.Models;
using SIE.WorkFlow.Core;
using SIE.WorkFlow.Models;

namespace SIE.DataTrace.Activities.Core
{
    /// <summary>
    /// 追溯节点基类
    /// </summary>
    public abstract class DataTraceActivityBase : PanelActivity
    {
        #region 属性
        #region 是否追溯节点
        /// <summary>
        /// 是否追溯节点
        /// </summary>
        [ActivityInput(Label = "是否追溯节点", DefaultValue = true, UIHint = ActivityInputUIHints.Checkbox, Order = -1)]
        public virtual bool isDataTrace { get { return true; } set { } }
        #endregion
        #endregion


        /// <summary>
        /// 数据同步是否已完成
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected virtual bool IsDataSyncFinished(ActivityExecutionContext context)
        {
            return true;
        }

        //
        // 摘要:
        //     执行
        //
        // 参数:
        //   context:
        protected override IActivityExecutionResult OnExecute(ActivityExecutionContext context)
        {
            ResetCurrentIngOrg(context);
            RejectArriveInput rejectArriveInput = context.Input as RejectArriveInput;
            if (rejectArriveInput != null)
            {
                RejectArrive(context, rejectArriveInput);
            }

            OnArriveActivityExecute(context);

            //数据已完成时，继续流转
            if (IsDataSyncFinished(context))
                return ReturnResult(context);
            else
                SaveFlowProcessRecordWithMessage(context, "数据填充中。", "", null);

            return Suspend();
        }

        /// <summary>
        /// 恢复
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override IActivityExecutionResult OnResume(ActivityExecutionContext context)
        {
            //数据已完成时，继续流转
            if (IsDataSyncFinished(context))
                return ReturnResult(context);
            return Suspend();
        }

        //
        // 摘要:
        //     返回节点结果
        //
        // 参数:
        //   context:
        protected override IActivityExecutionResult ReturnResult(ActivityExecutionContext context)
        {
            VariableBase sieVariable = context.GetSieVariable<VariableBase>();
            AppRuntime.InvOrg = sieVariable.InvOrgId;

            SaveFlowProcessRecordWithMessage(context, "数据填充完成。", "", null);
            //输出类型，通知下个节点（例如追溯审核时可以用到）
            var output = new DataTraceActivityInfo()
            {
                ActivityId = context.ActivityId,
                EntityType = entityTypeName
            };
            return Done(output);
        }

    }
}
