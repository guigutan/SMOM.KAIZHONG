using Newtonsoft.Json;
using SIE.Core.Common.Service;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.WorkFlow.PurchaseRequisition.WorkFlowCategoryConfig;
using SIE.Equipments.Enums;
using SIE.WorkFlow.Base.Common.Models;
using SIE.WorkFlow.Base.CustomForms;
using SIE.WorkFlow.Base.FlowDefinitions;
using System;
using System.Collections.Generic;

namespace SIE.EMS.WorkFlow.PurchaseRequisition
{
    /// <summary>
    /// 资产采购申请工作流控制器
    /// </summary>
    public class PurchaseRequisitionWorkflowService : DomainService
    {
        /// <summary>
        /// 工作流定义控制器
        /// </summary>
        private readonly FlowDefinitionController _flowDefinitionController;

        /// <summary>
        /// 通用表单控制器
        /// </summary>
        private readonly FlowCustomFormController _flowCustomFormController;

        /// <summary>
        /// 构造函数
        /// </summary>
        public PurchaseRequisitionWorkflowService(FlowDefinitionController flowDefinitionController,
             FlowCustomFormController flowCustomFormController)
        {
            _flowDefinitionController = flowDefinitionController;            
            _flowCustomFormController = flowCustomFormController;
        }

        /// <summary>
        /// 匹配资产采购申请工作流定义
        /// </summary>
        /// <param name="purchaseType">采购类型</param>
        /// <param name="purchaseObjectType">采购对象类型</param>        
        /// <returns>工作流定义</returns>
        private FlowDefinition GetWorkflowDefinition(PurchaseType? purchaseType, PurchaseObjectType? purchaseObjectType)
        {
            FlowDefinition flowDefinition;

            //1.获取资产采购申请的流程定义。
            var deflist = _flowDefinitionController.GetWorkflowDefinitionsByCategory(
                PurchaseRequisitionWorkFlowCategory.CategoryName, State.Enable);

            Dictionary<double, PurchaseRequisitionWorkFlowCategoryConfig> list
                = new Dictionary<double, PurchaseRequisitionWorkFlowCategoryConfig>();

            foreach (var def in deflist)
            {
                if (def.CategoryConfig.IsNullOrEmpty())
                {
                    continue;
                }

                var config = JsonConvert.DeserializeObject<PurchaseRequisitionWorkFlowCategoryConfig>(def.CategoryConfig);

                list.Add(def.Id, config);
            }

            //2.【采购类型】、【采购对象类型】匹配
            flowDefinition = GetWorkflowDefinition(deflist, list, purchaseType, purchaseObjectType);

            if (flowDefinition != null)
            {
                return flowDefinition;
            }

            //3.【空采购对象类型】、【采购类型】匹配
            flowDefinition = GetWorkflowDefinition(deflist, list, purchaseType);
            if (flowDefinition != null)
            {
                return flowDefinition;
            }

            //5.【空采购类型】、【空采购对象类型】匹配
            flowDefinition = GetWorkflowDefinition(deflist, list);

            if (flowDefinition == null)
            {
                throw new ValidationException("流程定义匹配失败，无法发起流程".L10N());
            }

            return flowDefinition;
        }

        /// <summary>
        /// 按【采购类型】、【采购对象】匹配
        /// </summary>
        /// <param name="flowDefinitions">工作流定义列表</param>
        /// <param name="flowConfigDictionary">工作流配置字典</param>
        /// <param name="purchaseType">采购类型（项目采购、非项目采购）</param>
        /// <param name="purchaseObjectType">采购对象类型（设备、备件...)</param> 
        /// <returns></returns>
        private FlowDefinition GetWorkflowDefinition(EntityList<FlowDefinition> flowDefinitions,
            Dictionary<double, PurchaseRequisitionWorkFlowCategoryConfig> flowConfigDictionary, PurchaseType? purchaseType,
            PurchaseObjectType? purchaseObjectType)
        {
            FlowDefinition flowDefinition = null;
            foreach (var def in flowConfigDictionary)
            {
                var config = def.Value;
                if (config.PurchaseType == purchaseType && config.PurchaseObjectType == purchaseObjectType)
                {
                    flowDefinition = (FlowDefinition)flowDefinitions.Find(def.Key);
                    break;
                }
            }

            return flowDefinition;
        }

        /// <summary>
        /// 按【采购类型】、【空采购对象】匹配
        /// </summary>
        /// <param name="flowDefinitions">工作流定义列表</param>
        /// <param name="flowConfigDictionary">工作流配置字典</param>
        /// <param name="purchaseType">采购类型（项目采购、非项目采购）</param>        
        /// <returns></returns>
        private FlowDefinition GetWorkflowDefinition(EntityList<FlowDefinition> flowDefinitions,
            Dictionary<double, PurchaseRequisitionWorkFlowCategoryConfig> flowConfigDictionary, PurchaseType? purchaseType)
        {
            FlowDefinition flowDefinition = null;
            foreach (var def in flowConfigDictionary)
            {
                var config = def.Value;
                if (config.PurchaseType == purchaseType && !config.PurchaseObjectType.HasValue)
                {
                    flowDefinition = (FlowDefinition)flowDefinitions.Find(def.Key);
                    break;
                }
            }

            return flowDefinition;
        }

        /// <summary>
        /// 按【空采购类型】、【空采购对象】匹配
        /// </summary>
        /// <param name="flowDefinitions">工作流定义列表</param>
        /// <param name="flowConfigDictionary">工作流配置字典</param>        
        /// <returns></returns>
        private FlowDefinition GetWorkflowDefinition(EntityList<FlowDefinition> flowDefinitions,
            Dictionary<double, PurchaseRequisitionWorkFlowCategoryConfig> flowConfigDictionary)
        {
            FlowDefinition flowDefinition = null;
            foreach (var def in flowConfigDictionary)
            {
                var config = def.Value;
                if (!config.PurchaseType.HasValue && !config.PurchaseObjectType.HasValue)
                {
                    flowDefinition = (FlowDefinition)flowDefinitions.Find(def.Key);
                    break;
                }
            }

            return flowDefinition;
        }

        /// <summary>
        /// 创建通用表单工作流
        /// </summary>
        /// <param name="workFlowForm"></param>
        private double CreateCustomFormWorkFlowProxy(WorkFlowForm workFlowForm)
        {
            CustomForm form = new CustomForm();

            form.FlowDefinitionId = workFlowForm.FlowDefinitionId;
            form.StarterId = workFlowForm.StarterId;
            form.FieldList = new List<FormField>();

            foreach (var field in workFlowForm.FieldList)
            {
                FormField formField = new FormField()
                {
                    Name = field.Name,
                    Value = field.Value,
                };

                form.FieldList.Add(formField);
            }

            form.CustomLists = new List<CustomList>();
            foreach (var workFlowCustomList in workFlowForm.WorkFlowCustomLists)
            {
                CustomList customList = new CustomList()
                {
                    ListName = workFlowCustomList.ListName,
                };

                customList.CutomListFields = new List<CutomListField>();
                foreach (var cutomListField in workFlowCustomList.CutomListFields)
                {
                    customList.CutomListFields.Add(new CutomListField()
                    {
                        FieldName = cutomListField.FieldName,
                        FieldLabel = cutomListField.FieldLabel,
                        FieldWidth = cutomListField.FieldWidth,
                    });
                }

                customList.CutomListValueList = new List<List<CutomListValue>>();
                foreach (var cutomListValues in workFlowCustomList.CutomListValueList)
                {
                    List<CutomListValue> cutomListValueRows = new List<CutomListValue>();
                    foreach (var cutomListValue in cutomListValues)
                    {
                        cutomListValueRows.Add(new CutomListValue()
                        {
                            FieldName = cutomListValue.FieldName,
                            FieldValue = cutomListValue.FieldValue,
                        });
                    }

                    customList.CutomListValueList.Add(cutomListValueRows);
                }

                form.CustomLists.Add(customList);
            }

            form.Attachments = new List<AttachmentInfoModel>(); 
            foreach (var workFlowAttachmentInfoModel in workFlowForm.Attachments)
            {
                AttachmentInfoModel attachment = new AttachmentInfoModel()
                {
                    FileName = workFlowAttachmentInfoModel.FileName,
                    FilePath = workFlowAttachmentInfoModel.FilePath,
                    FileExtesion = workFlowAttachmentInfoModel.FileExtesion,
                    FileSize = workFlowAttachmentInfoModel.FileSize,
                };

                form.Attachments.Add(attachment);
            }

            return _flowCustomFormController.CreateCustomFormWorkFlow(form);
        }

        /// <summary>
        /// 创建资产采购申请的工作流实例
        /// </summary>
        /// <param name="workFlowForm">自定表单</param>
        /// <param name="purchaseType">采购类型</param>
        /// <param name="purchaseObjectType">采购对象类型</param>
        public virtual double CreatePurchaseRequisitionWorkFlow(WorkFlowForm workFlowForm, PurchaseType? purchaseType,
            PurchaseObjectType? purchaseObjectType)
        {
            var flowDefinition = GetWorkflowDefinition(purchaseType, purchaseObjectType);

            if (flowDefinition == null)
            {
                throw new EntityNotFoundException("未找到【工作流分类】是【资产采购申请】的工作流定义".L10N());
            }

            workFlowForm.FlowDefinitionId = flowDefinition.Id;

            return CreateCustomFormWorkFlowProxy(workFlowForm);
        }
    }
}
