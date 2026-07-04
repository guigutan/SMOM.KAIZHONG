using Elsa;
using Elsa.Models;
using Elsa.Persistence;
using Elsa.Services;
using SIE.Common.Employees;
using SIE.Common.WorkFlow.Models;
using SIE.DataTrace.DataSync.Core;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.WorkFlow.Base.FlowDefinitions;
using SIE.WorkFlow.Base.FlowInstances;
using SIE.WorkFlow.WorkFlowInstances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.DataTrace.Activities
{
    /// <summary>
    /// 数据追溯工作流控制器
    /// </summary>
    public class DataTraceWorkFlowController : DomainController
    {
        private readonly System.IServiceProvider _serviceProvider;
        public  DataTraceWorkFlowController(System.IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        /// <summary>
        /// 触发追溯节点
        /// </summary>
        /// <param name="model"></param>
        public virtual void TriggerDataTraceActivity(DataSyncParam model)
        {
            var wfInstanceId = RF.GetById<FlowInstance>(model.FlowInstanceId)?.WorkflowInstanceId;
            TriggerActivity(model.ActivityId,  wfInstanceId);
        }

        /// <summary>
        /// 触发流程
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="wfInstanceId"></param>
        /// <exception cref="ValidationException"></exception>
        private  void TriggerActivity(string activityId, string? wfInstanceId)
        {
            TriggerModel input = new TriggerModel()
            {
                ActivityId = activityId,
                TriggerPeople = RT.Service.Resolve<EmployeeController>().GetEmployeeNameById(RT.IdentityId),
                WorkflowInstanceId = wfInstanceId
            };
            var result = RT.Service.Resolve<FlowInstanceController>().ExecutePendingWorkflowAsync(wfInstanceId, activityId, input);
            if (result.Executed && result.WorkflowInstance.Fault != null)
            {
                throw new ValidationException(result.WorkflowInstance.Fault.Message);
            }
        }

        /// <summary>
        /// 根据实体类型去触发流程相关节点
        /// </summary>
        /// <param name="flowInstanceId"></param>
        /// <param name="traceMainDataId"></param>
        /// <param name="entityType"></param>
        public virtual void TriggerDataTraceActivityByEntity(double flowInstanceId,double traceMainDataId,string entityType)
        {
            var instanc = RF.GetById<FlowInstance>(flowInstanceId);

            //1.获取流程实例
            var workflowInstanceStore = (IWorkflowInstanceStore)_serviceProvider.GetService(typeof(IWorkflowInstanceStore));
            var workflowInstance = workflowInstanceStore.FindByIdAsync(instanc.WorkflowInstanceId, default).Result;

            //2.0获取所有的工作流实例中的所有节点
            var flowDefinition = RF.GetById<FlowDefinition>(instanc.FlowDefinitionId);
          
            if (workflowInstance == null)
                throw new ValidationException("流程定义不存在".L10N());

            //3.0遍历节点，那些节点是追溯节点，并且节点处于挂起状态
            var activityDatas = workflowInstance.ActivityData;
            List<string> targetActivityIds = new List<string>();
            foreach (var blockActivity in workflowInstance.BlockingActivities)
            {
                if (activityDatas.ContainsKey(blockActivity.ActivityId))
                {
                    var activityData = workflowInstance.ActivityData[blockActivity.ActivityId];
                    if (activityData.ContainsKey("isDataTrace") && (bool)activityData["isDataTrace"] 
                        && (activityData["entityTypeName"] != null && activityData["entityTypeName"].ToString() == entityType))
                    {
                        targetActivityIds.Add(blockActivity.ActivityId);
                    }
                }
            }

            //触发节点流程。（正常只会匹配到一个节点，但也不排除匹配到有多个节点的可能）
            if(targetActivityIds.IsNotEmpty())
            {
                targetActivityIds.ForEach((actId) =>
                {
                    TriggerActivity(actId, instanc.WorkflowInstanceId);
                });
            }
        }
    }
}
