using SIE.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.DataTrace.Activities.MessageQueue
{
    /// <summary>
    /// 数据追溯流程触发-发送消息控制器
    /// </summary>
    public class DataTraceMqController : DomainController
    {
        /// <summary>
        /// 发送数据追溯流程触发消息-拉取方式-指定节点ID
        /// </summary>
        public virtual void SendDataTraceWorkFlowQueue(double flowInstanceId, string activityId, double traceMainDataId)
        {
            if (RT.InvOrg == null)
                throw new ValidationException("库存组织不能为空。".L10N());
            var taskInfo = new DataTraceWorkFlowQueueInfo()
            {
                FlowInstanceId = flowInstanceId,
                InvOrg = RT.InvOrg.Value,
                ActivityId = activityId,
                TraceMainDataId = traceMainDataId
            };
            RT.MQueueEventBus.PublishAsync<DataTraceWorkFlowQueueInfo>(taskInfo);
        }

        /// <summary>
        /// 发送数据追溯流程触发消息-推送方式-指定实体类型
        /// </summary>
        public virtual void SendDataTraceWorkFlowQueueByEntityType(double flowInstanceId, string entityType, double traceMainDataId)
        {
            if (RT.InvOrg == null)
                throw new ValidationException("库存组织不能为空。".L10N());
            var taskInfo = new DataTraceWorkFlowQueueEntityInfo()
            {
                FlowInstanceId = flowInstanceId,
                InvOrg = RT.InvOrg.Value,
                EntityType = entityType,
                TraceMainDataId = traceMainDataId
            };
            RT.MQueueEventBus.PublishAsync<DataTraceWorkFlowQueueEntityInfo>(taskInfo);
        }
    }
}
