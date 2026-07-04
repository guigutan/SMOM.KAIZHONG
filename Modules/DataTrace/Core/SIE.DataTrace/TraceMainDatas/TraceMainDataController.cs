using SIE.Api;
using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Common.NumberRules;
using SIE.Core.Common;
using SIE.DataTrace.Activities.Core;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Resources;
using SIE.WorkFlow.Base.Common.Interfaces;
using SIE.WorkFlow.Base.FlowDefinitions;
using SIE.WorkFlow.Base.FlowInstances;
using SIE.WorkFlow.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.DataTrace.TraceMainDatas
{
    /// <summary>
    /// 追溯主数据控制器
    /// </summary>
    public partial class TraceMainDataController : DomainController
    {
        /// <summary>
        /// 工作流
        /// </summary>
        private ISieWorkflow _workflow { get { return RT.Service.Resolve<SieWorkflowController>(); } }

        /// <summary>
        /// 追溯流程
        /// </summary>
        public const string DataTraceWorkFlowSource = "追溯流程";

        /// <summary>
        /// 启动追溯流程
        /// </summary>
        /// <param name="definitionId">流程定义</param>
        /// <param name="contextDic">上下文</param>
        /// <param name="workflowType">追溯流程类型</param>
        /// <param name="correlationId">关联ID</param>
        /// <param name="flowDesc">流程描述</param>
        /// <param name="flowSource">流程来源</param>
        /// <returns></returns>
        public virtual TraceMainData StartDataTraceWorlFlow(double definitionId, string contextDic, string workflowType, string correlationId, string flowDesc = "", string flowSource = "")
        {
            //1.创建工作流实例
            var flowDef = RF.GetById<FlowDefinition>(definitionId);
            if (flowDef == null)
                throw new ValidationException("工作流定义不存在".L10N());

            var traceData = CreateNewMainTraceData(contextDic);

            var starterId = RT.IdentityId;

            var startInstance = _workflow.FindStartableWorkflowAsync(flowDef.DefinitionId, starterId, definitionId, flowDesc, flowSource, string.Empty, string.Empty, canCancel: false);
            var dataTraceVar = new DataTraceVariable();
            dataTraceVar.StarterId = starterId;
            dataTraceVar.Starter = RF.GetById<Employee>(starterId)?.Name;
            dataTraceVar.InvOrgId = RT.InvOrg;
            dataTraceVar.FlowInstanceId = startInstance.FlowInstanceId;
            dataTraceVar.TraceMainDataId = traceData.Id;

            traceData.FlowInstanceId = startInstance.FlowInstanceId;
            traceData.WorkFlowType = workflowType;
            traceData.CorrelationId = correlationId;
            RF.Save(traceData);

            //2.开始执行
            _workflow.TriggerWorkflowAsync(startInstance.InstanceGuid, startInstance.ActivityId, dataTraceVar);

            return traceData;
        }

        /// <summary>
        /// 生成追溯主数据
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual TraceMainData CreateNewMainTraceData(string context)
        {
            var no = GetTraceMainDataNo();
            var data = new TraceMainData()
            {
                No = no,
                Context = context
            };
            data.GenerateId();

            return data;
        }

        /// <summary>
        /// 查询追溯主数据
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList GetTraceMainDatas(TraceMainDataCriteria criteria)
        {
            var q = Query<TraceMainData>();
            q.WhereIf(criteria.No.IsNotEmpty(), p => p.No.Contains(criteria.No));
            q.WhereIf(criteria.FlowInstanceId.HasValue, p => p.FlowInstanceId == criteria.FlowInstanceId);
            q.WhereIf(criteria.WorkFlowType.IsNotEmpty(), p => p.WorkFlowType == p.WorkFlowType);
            q.WhereIf(criteria.Context.IsNotEmpty(), p => p.Context.Contains(criteria.Context));
            if (criteria.OrderInfoList.IsNotEmpty())
                q.OrderBy(criteria.OrderInfoList);
            return q.ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 生成追溯主数据单号
        /// </summary>
        /// <returns>来料检验单号</returns>
        public virtual string GetTraceMainDataNo()
        {
            var config = ConfigService.GetConfig<NoConfigValue>(new NoConfig(), typeof(TraceMainData));
            if (config == null || config.BacodeRule == null)
                throw new ValidationException("未找到追溯主数据单号生成规则,请检查规则配置".L10N());
            return RT.Service.Resolve<NumberRuleController>().GenerateSegment(config.BacodeRule.Id, 1).FirstOrDefault();
        }

        /// <summary>
        /// 获取追溯上下文
        /// </summary>
        /// <param name="mainId"></param>
        /// <returns></returns>
        public virtual Dictionary<string, object> GetTraceMainDataContext(double mainId)
        {
            var context = RF.GetById<TraceMainData>(mainId)?.Context;
            try
            {
                if (context.IsNotEmpty())
                    return context.ToJsonObjectCore<Dictionary<string, object>>();
            }
            catch (Exception ex)
            {
                RT.Logger.Error("解析追溯流程上下文异常".L10N(), ex);
            }
            return new Dictionary<string, object>();
        }

        /// <summary>
        /// 获取追溯上下文
        /// </summary>
        /// <param name="mainId"></param>
        /// <returns></returns>
        public virtual string GetTraceMainDataContextString(double mainId)
        {
            var mainData = RF.GetById<TraceMainData>(mainId);
            if (mainData == null)
                return null;
            else
                return mainData.Context;
        }

        /// <summary>
        /// 查询追溯流程的流程进度
        /// </summary>
        /// <param name="pageinfo"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList<FlowInstance> GetDataTraceFlowInstaces(PagingInfo pageinfo, string keyword)
        {
            var q = Query<FlowInstance>();
            q.Where(p => p.Source == TraceMainDataController.DataTraceWorkFlowSource);
            q.WhereIf(keyword.IsNotEmpty(), p => p.Code.Contains(keyword));
            return q.ToList(pageinfo, new EagerLoadOptions().LoadWithViewProperty());
        }
    }
}
