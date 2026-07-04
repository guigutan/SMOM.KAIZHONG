using Elsa;
using Elsa.Models;
using Elsa.Persistence;
using Elsa.Services;
using Elsa.Services.Models;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SIE.Common;
using SIE.Common.Prints;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.WorkFlow.Base.FlowDefinitions;
using SIE.WorkFlow.Base.FlowInstances;
using SIE.WorkFlow.WorkFlowInstances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Entity = SIE.Domain.Entity;

namespace SIE.DataTrace.TraceMainDatas
{
    /// <summary>
    /// 
    /// </summary>
    public partial class TraceMainDataController : DomainController
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly FlowProcessRecordController _flowProcessRecordController;



        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        public TraceMainDataController(IServiceProvider serviceProvider, FlowProcessRecordController flowProcessRecordController)
        {
            _serviceProvider = serviceProvider;
            _flowProcessRecordController = flowProcessRecordController;
        }
        #region MyRegion
        /// <summary>
        /// 获取电子批流程追溯节点
        /// </summary>
        /// <param name="docId"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public virtual List<TraceDataPrintDataViewmodel> GetBillPrintTemplates(double docId, bool isSave = false)
        {
            var docment = RF.GetById<TraceMainData>(docId);
            var instanc = RF.GetById<FlowInstance>(docment.FlowInstanceId);
            //1.获取流程实例
            //workflowInstanceId->WorkflowInstance->definitionId,version->WorkflowDefinition
            var workflowInstanceStore = (IWorkflowInstanceStore)_serviceProvider.GetService(typeof(IWorkflowInstanceStore));
            var workflowInstance = workflowInstanceStore.FindByIdAsync(instanc.WorkflowInstanceId).Result;

            //2.0获取所有的工作流实例中的所有节点
            var flowDefinition = RF.GetById<FlowDefinition>(instanc.FlowDefinitionId);
            if (workflowInstance == null)
                throw new ValidationException("流程定义不存在".L10N());
            //3.0遍历节点，那些节点是追溯节点，并且节点已经执行
            var workflowBlueprint = _serviceProvider.GetService<IWorkflowRegistry>().GetAsync(flowDefinition.DefinitionId, null, VersionOptions.SpecificVersion(instanc.Version)).Result;
            var processRecords = _flowProcessRecordController.GetFlowProcessRecordsByInstanceId(instanc.Id);
            var activityDatas = workflowInstance.ActivityData;
            var dics=new Dictionary<string, TraceWorkflowPanelInfo>();
            //List<TraceWorkflowPanelInfo> workflowPanelInfoList = new List<TraceWorkflowPanelInfo>();
            foreach (var activityRecd in processRecords)
            {
                if (activityDatas.ContainsKey(activityRecd.ActivityId))
                {
                    var activityData = workflowInstance.ActivityData[activityRecd.ActivityId];
                    if (activityData.ContainsKey("isDataTrace") && (bool)activityData["isDataTrace"] && (activityData["entityTypeName"] != null))
                    {
                        var node = GetWorkflowPanelInfo(activityData, activityRecd, workflowBlueprint);
                        dics[activityRecd.ActivityId] = node;
                        //workflowPanelInfoList.Add(node);
                    }
                }
            }
            //获取所有打印模板
            var printTypes = dics.Values.Select(c => c.PrintEntityType).ToList();
            var printTemps = GetPrintTemplates(printTypes);
            var result = new List<TraceDataPrintDataViewmodel>();
            var variablesJson = JsonConvert.SerializeObject(workflowInstance.Variables.Data);
            dics.Values.ToList().ForEach(node =>
            {
                var model = new TraceDataPrintDataViewmodel();
                model.Template = printTemps.FirstOrDefault(c => c.EntityType == node.PrintEntityType);
                if (model.Template == null)
                {
                    throw new ValidationException($"追溯节点{node.Title},类型[{node.AssemblyName}],未设置打印模板!".L10N());
                }
                if (node.IsListView)
                {
                    model.ListData = GetActivityEntityList(variablesJson, node.ActivityType);
                }
                else
                {
                    var entity = GetActivityEntity(variablesJson, node.FlowTaskId, node.ActivityType, node.EntityTypeName);
                    model.ListData = new EntityList<Entity>();
                    model.ListData.Add(entity);
                }
                result.Add(model);
            });
            //if (isSave)
            //    RT.MQueueEventBus.PublishAsync<TraceFileViewmodel>(new TraceFileViewmodel { TraceMainId = docId, InvOrg = RT.InvOrg });
            return result;
        }
        /// <summary>
        /// 获取打印模板
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public virtual EntityList<PrintTemplate> GetPrintTemplates(List<string> types)
        {
            return Query<PrintTemplate>().Where(c => types.Contains(c.EntityType) && c.State==State.Enable).ToList().GroupBy(c => c.EntityType).Select(p => p.First()).AsEntityList();
        }
        /// <summary>
        /// 获取节点信息
        /// </summary>
        /// <param name="activityData"></param>
        /// <param name="processRecord"></param>
        /// <param name="workflowBlueprint"></param>
        /// <returns></returns>
        public virtual TraceWorkflowPanelInfo GetWorkflowPanelInfo(IDictionary<string, object?> activityData, FlowProcessRecord processRecord, IWorkflowBlueprint? workflowBlueprint)
        {
            var activityBlueprint = workflowBlueprint?.GetActivity(processRecord.ActivityId);
            TraceWorkflowPanelInfo workflowPanelInfo = new TraceWorkflowPanelInfo();
            workflowPanelInfo.ActivityId = processRecord.ActivityId;
            workflowPanelInfo.ActivityType = activityBlueprint?.Type;

            var fullTypeName = activityData["entityTypeName"]?.ToString();
            //if (fullTypeName == "SIE.QMS.WorkOrders.WorkOrder")
            //{
            //    fullTypeName = "SIE.QMS.WorkOrders.WorkOrder,SIE.QMS";
            //}
            workflowPanelInfo.EntityTypeName = fullTypeName;
            workflowPanelInfo.CreateTime = processRecord.CreateDate;
            workflowPanelInfo.FlowTaskId = processRecord.FlowTaskId;
            workflowPanelInfo.IsListView = Convert.ToBoolean(activityData["isListView"]);
            workflowPanelInfo.ViewGroup = activityData["readonlyViewGroup"]?.ToString();
            activityData.TryGetValue("assemblyName", out object assemblyName);
            workflowPanelInfo.AssemblyName = assemblyName?.ToString();
            workflowPanelInfo.PrintEntityType = GetPrintEntityType(workflowPanelInfo.AssemblyName);
            workflowPanelInfo.Title = activityBlueprint?.DisplayName != null ? activityBlueprint.DisplayName : activityBlueprint?.Name;
            return workflowPanelInfo;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        private string? GetPrintEntityType(string? assemblyName)
        {
            if (assemblyName == null) return null;
            Type type = Type.GetType(assemblyName);
            var billPrintableAttribute = type?.GetCustomAttribute<BillPrintableAttribute>();
            if (billPrintableAttribute == null) return null;
            var billPrintable = Activator.CreateInstance(billPrintableAttribute.PrintableType) as IBillPrintable;
            return billPrintable?.GetType().GetQualifiedName();
        }

        /// <summary>
        /// 获取节点-表单数据
        /// </summary>
        /// <param name="variablesJson"></param>
        /// <param name="flowTaskId"></param>
        /// <param name="activityType"></param>
        /// <param name="entityTypeName"></param>
        /// <returns></returns>
        private Entity GetActivityEntity(string variablesJson, double? flowTaskId, string activityType, string entityTypeName)
        {
            return AppRuntime.Service.Resolve<WorkFlowTaskController>().GetActivityEntity(variablesJson, flowTaskId, activityType, entityTypeName);
        }

        /// <summary>
        /// summary>
        /// 获取节点列表数据
        /// </summary>
        /// <param name="variablesJson"></param>
        /// <param name="activityType"></param>
        /// <returns></returns>
        private EntityList GetActivityEntityList(string variablesJson, string activityType)
        {
            return AppRuntime.Service.Resolve<WorkFlowTaskController>().GetActivityEntityList(variablesJson, activityType);
        }

        #region 追溯节点-归档

        /// <summary>
        /// /归档
        /// </summary>
        /// <param name="mainTraceId"></param>
        /// <returns></returns>
        public virtual void ArchivalFiles(double mainTraceId)
        {
            var docment = RF.GetById<TraceMainData>(mainTraceId);
            RT.MQueueEventBus.PublishAsync<TraceFileViewmodel>(new TraceFileViewmodel { TraceMainId = mainTraceId,TraceMainNo= docment.No, InvOrg = RT.InvOrg });
        }

        public virtual List<TraceDataPrintDataViewmodel> GetBillPrintTemplates(double docId, int? invOrg)
        {
            if (RT.InvOrg == null) RT.InvOrg = invOrg;
            var temps = GetBillPrintTemplates(docId);
            return temps;
        }
        #endregion

        #endregion
    }
}
