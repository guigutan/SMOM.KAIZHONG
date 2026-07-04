using AngleSharp.Dom;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SIE.Common.Configs;
using SIE.Core.Items;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.WorkOrders;
using SIE.MES.WorkOrders.Configs;
using SIE.MetaModel;
using SIE.Web.Command;
using SIE.Web.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SIE.Web.MES.WorkOrders.Commands
{
    /// <summary>
    /// 保存工单命令
    /// </summary>
    [JsCommand("SIE.Web.MES.WorkOrders.SaveWorkOrderCommand")]
    public class SaveWorkOrderCommand : FormSaveCommand
    {
        ///// <summary>
        ///// 工单保存事件
        ///// </summary>
        ///// <param name="entity">实体</param>
        //protected override void DoSave(Entity entity)
        //{
        //    var ctl = RT.Service.Resolve<WorkOrderController>();
        //    var workOrder = entity as WorkOrder;
        //    ValidateTemplateSetting(workOrder);
        //    var currTemplate = workOrder.GetProperty(WOLabelPrintDetailProperty.LabelPrintTemProperty);


        //    WorkOrderPropertyChanged proChanged = new WorkOrderPropertyChanged();
        //    if (workOrder.PersistenceStatus == PersistenceStatus.New)
        //    {
        //        if (workOrder.Template != null)
        //        {
        //            workOrder.Template.PersistenceStatus = PersistenceStatus.New;
        //        }

        //        proChanged.GenerateRoutingProcesss(workOrder, true, workOrder.RoutingProcessList);
        //        RemoveProcessBom(workOrder);
        //        ctl.SaveWorkOrder(workOrder, workOrder.Template, WorkOrderLogType.Create, "MES新增");
        //    }
        //    else
        //    {
        //        if (workOrder.RoutingProcessList.Any(p => p.PersistenceStatus == PersistenceStatus.New))
        //        {
        //            workOrder.Layout = null;
        //            proChanged.GenerateRoutingProcesss(workOrder, true, workOrder.RoutingProcessList);
        //        }
        //        ValidateRoutingProcess(workOrder);
        //        ctl.UpdateWorkOrder(workOrder, currTemplate, true);
        //    }
        //    workOrder.MarkSaved();  // 保存工单成功后修改工单状态为Unchanged，防止多次点击保存时违反唯一性约束    
        //    base.OnSaving(workOrder);
        //}

        /// <summary>
        /// 验证功能工艺路线工序信息
        /// </summary>
        /// <param name="workOrder">工单</param>
        private void ValidateRoutingProcess(WorkOrder workOrder)
        {
            //工艺路线工序判断（网络异常情况下可能会导致数据错乱）
            foreach (var processList in workOrder.RoutingProcessList
                .Where(x => string.IsNullOrEmpty(x.GroupId) || x.IsGroup == true))
            {
                if (!processList.ParameterList.Any())
                    throw new ValidationException("数据异常，请重新打开工单！".L10N());
            }
        }

        /// <summary>
        /// 验证打印设置
        /// </summary>
        /// <param name="workOrder">工单</param>
        private void ValidateTemplateSetting(WorkOrder workOrder)
        {
            if (workOrder.PackageRuleDetailList.Where(p => p.IsInStockLabel).ToList().Count > 1)
            {
                throw new ValidationException("包装规则入库标签只能选择一个".L10N());
            }

            var config = ConfigService.GetConfig(new PrintTemplateConfig(), typeof(WorkOrder));
            if (!config.IsNeed)
            {
                return;
            }

            if (workOrder.Template.NumberRule == null)
            {
                throw new ValidationException("条码规则不能为空".L10N());
            }

            if (workOrder.Template.LabelTemplate == null)
            {
                throw new ValidationException("标签模板不能为空".L10N());
            }

            if (workOrder.Template.PackingTemplate == null)
            {
                throw new ValidationException("外标签打印模板不能为空".L10N());
            }
        }

        /// <summary>
        /// 移除缓存的工序BOM
        /// </summary>
        /// <param name="workOrder">工单</param>
        private void RemoveProcessBom(WorkOrder workOrder)
        {
            var processBomList = workOrder.ProcessBomList;
            var rProcessIds = processBomList.Where(p => p.RoutingProcessId > 0).Select(p => p.RoutingProcessId.Value).ToList();
            var existRoutingIds = RT.Service.Resolve<WorkOrderController>().GetRoutingProcess(rProcessIds);
            var existRouting = processBomList.Where(p => p.RoutingProcessId > 0 && existRoutingIds.Contains(p.RoutingProcessId.Value)).ToList();
            existRouting.ForEach(p =>
            {
                workOrder.ProcessBomList.Remove(p);
            });
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="args">参数</param>
        /// <param name="scope">作用域</param>
        /// <returns>命令执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            const string pattern1 = ",\"SIE.Web.Items.ViewModels.PropertyValueViewModel(.*?)}]}";

            string resultContent = Regex.Replace(args.Data, pattern1, "");
            args.Data = resultContent;
            EntityList deserializeData = GetDeserializeData(args, scope);
            Domain.Entity entity = ((deserializeData.Count > 0) ? deserializeData[0] : null);
            if (entity != null)
            {
                base.DataList = deserializeData.GetRepository().NewList();
                base.DataList.Add(entity);
            }

            var ctl = RT.Service.Resolve<WorkOrderController>();
            var workOrder = entity as WorkOrder;
            SetTemplate(args, workOrder);

            ValidateTemplateSetting(workOrder);
            var currTemplate = workOrder.GetProperty(WOLabelPrintDetailProperty.LabelPrintTemProperty);

            base.OnSaving(workOrder);
            WorkOrderPropertyChanged proChanged = new WorkOrderPropertyChanged();
            if (workOrder.PersistenceStatus == PersistenceStatus.New)
            {
                if (workOrder.Template != null)
                {
                    workOrder.Template.PersistenceStatus = PersistenceStatus.New;
                }

                proChanged.GenerateRoutingProcesss(workOrder, true, workOrder.RoutingProcessList);
                RemoveProcessBom(workOrder);
                ctl.SaveWorkOrder(workOrder, workOrder.Template, WorkOrderLogType.Create, "MES新增".L10N());
            }
            else
            {
                if (workOrder.RoutingProcessList.Any(p => p.PersistenceStatus == PersistenceStatus.New))
                {
                    workOrder.Layout = null;
                    proChanged.GenerateRoutingProcesss(workOrder, true, workOrder.RoutingProcessList);
                }
                ValidateRoutingProcess(workOrder);
                ctl.UpdateWorkOrder(workOrder, currTemplate, true);
            }
            workOrder.MarkSaved();  // 保存工单成功后修改工单状态为Unchanged，防止多次点击保存时违反唯一性约束   
            return workOrder;
        }
        /// <summary>
        /// 设置模板
        /// </summary>
        /// <param name="args"></param>
        /// <param name="workOrder"></param>
        private void SetTemplate(ViewArgs args, WorkOrder workOrder)
        {
            const string pattern2 = ",\"SIE.Core.Items.LabelPrintTemplate(.*?)}]}";
            var match = Regex.Match(args.Data, pattern2);
            if (match.Success)
            {

                string resultContent2 = match.Groups[0].Value;
                resultContent2 = resultContent2.TrimStart(',').Replace("\"SIE.Core.Items.LabelPrintTemplate\":{\"u\":", "").TrimEnd('}');
                var labelPrintTemplateList = JsonConvert.DeserializeObject<List<LabelPrintTemplateModel>>(resultContent2);

                LabelPrintTemplateModel labelPrintTemplateModel = labelPrintTemplateList.Any() ? labelPrintTemplateList[0] : null;
                if (labelPrintTemplateModel != null && workOrder != null)
                {
                    if (workOrder.TemplateId.HasValue)
                    {
                        workOrder.Template.LabelTemplateId = labelPrintTemplateModel.LabelTemplateId;
                        workOrder.Template.NumberRuleId = labelPrintTemplateModel.NumberRuleId;
                        workOrder.Template.PackingTemplateId = labelPrintTemplateModel.PackingTemplateId;
                    }
                    else
                    {
                        
                        var template= new LabelPrintTemplate()
                        {
                            LabelTemplateId = labelPrintTemplateModel.LabelTemplateId,
                            NumberRuleId = labelPrintTemplateModel.NumberRuleId,
                            PackingTemplateId = labelPrintTemplateModel.PackingTemplateId,
                            PersistenceStatus = PersistenceStatus.New
                        };
                        template.GenerateId();
                        workOrder.Template = template;
                    }
                }

            }
        }
    }

    /// <summary>
    /// 保存并新增工单命令
    /// </summary>
    [JsCommand("SIE.Web.MES.WorkOrders.SaveAndAddWorkOrderCommand")]
    public class SaveAndAddWorkOrderCommand : SaveWorkOrderCommand
    {
    }

    /// <summary>
    /// 新增保存工单并关闭工单添加命令
    /// </summary>
    [JsCommand("SIE.Web.MES.WorkOrders.SaveAndClosedWorkOrderCommand")]
    public class SaveAndClosedWorkOrderCommand : SaveWorkOrderCommand
    {
    }

    /// <summary>
    /// 增加该类目的LabelPrintTemplate被写了属性变更导致反序列化失败
    /// </summary>
    [Serializable]
    public class LabelPrintTemplateModel
    {
        public double Id { get; set; }
        public double? LabelTemplateId { get; set; }
        public double? NumberRuleId { get; set; }
        public double? PackingTemplateId { get; set; }
    }
}