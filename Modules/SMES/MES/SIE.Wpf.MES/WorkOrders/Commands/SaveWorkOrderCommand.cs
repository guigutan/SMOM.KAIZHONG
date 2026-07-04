using SIE.Common;
using SIE.Common.Configs;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.WorkOrders;
using SIE.MES.WorkOrders.Configs;
using SIE.Wpf.Command;
using SIE.Wpf.Items.ViewModels;
using System;
using System.Linq;

namespace SIE.Wpf.MES.WorkOrders.Commands
{
    /// <summary>
    /// 工单保存命令
    /// </summary>
    [Command(ImageName = "SaveEntity", Label = "保存", GroupType = CommandGroupType.Edit)]
    public class SaveWorkOrderCommand : FormSaveCommand
    {
        /// <summary>
        /// 命令是否可执行
        /// </summary>
        /// <param name="view">逻辑视图</param>
        /// <returns>bool</returns>
        public override bool CanExecute(DetailLogicalView view)
        {
            return true;
        }

        /// <summary>
        /// 工单保存
        /// </summary>
        /// <param name="entity">工单</param>
        protected override void DoSave(Entity entity)
        {
            var ctl = RT.Service.Resolve<WorkOrderController>();
            var workOrder = entity as WorkOrder;
            ValidateTemplateSetting(workOrder);
            WorkOrder newWorkOrder = null;
            var criteria = new WorkOrderCriteria();
            criteria.WorkOrderNo = workOrder.No;
            criteria.PlanBeginDate.DateRangeType = ObjectModel.DateRangeType.All;
            criteria.PlanEndDate.DateRangeType = ObjectModel.DateRangeType.All;
            var currTemplate = workOrder.GetProperty(WOLabelPrintDetailProperty.LabelPrintTemProperty);
            var currAttacWoWipBatch = workOrder.GetProperty(WoWipBatchExt.AttacWoWipBatchProperty);

            if (workOrder.PersistenceStatus == PersistenceStatus.New)
            {
                SetNewWorkOrderValues(workOrder);
                SetNewWorkOrderBomValues(workOrder);
                SetNewWorkOrderProcessBomValues(workOrder);
                workOrder.Layout = null;
                var wo = ctl.SaveWorkOrder(workOrder, workOrder.Template, WorkOrderLogType.Create, "MES新增");
                wo.MarkSaved();  // 保存工单成功后修改工单状态为Unchanged，防止多次点击保存时违反唯一性约束
            }
            else
            {
                SetModifierWorkOrderValues(workOrder);
                SetModifierWorkOrderBomValues(workOrder);
                SetModifierWorkOrderProcessBomValues(workOrder);
                ctl.UpdateWorkOrder(workOrder, currTemplate);
            }

            newWorkOrder = ctl.GetWorkOrders(criteria).FirstOrDefault();
            if (newWorkOrder != null)
            {
                var template = RF.GetById<Core.Items.LabelPrintTemplate>(currTemplate.Id);
                currTemplate.Clone(template, CloneOptions.ReadDbRow());
                newWorkOrder.SetProperty(WOLabelPrintDetailProperty.LabelPrintTemProperty, currTemplate);
                var batch = RT.Service.Resolve<Core.WorkOrders.WorkOrderController>().GetWipBatch(workOrder.Id);
                currAttacWoWipBatch.Clone(batch, CloneOptions.ReadDbRow());
                newWorkOrder.SetProperty(WoWipBatchExt.AttacWoWipBatchProperty, currAttacWoWipBatch);
                workOrder.Clone(newWorkOrder, CloneOptions.DeepClone());
            }
        }

        /// <summary>
        /// 验证打印设置
        /// </summary>
        /// <param name="workOrder">工单</param>
        private void ValidateTemplateSetting(WorkOrder workOrder)
        {
            if (workOrder.PackageRuleDetailList.Where(p => p.IsInStockLabel).ToList().Count > 1)
                throw new ValidationException("包装规则入库标签只能选择一个".L10N());
            var config = ConfigService.GetConfig(new PrintTemplateConfig(), typeof(WorkOrder));
            if (config.IsNeed != true) return;
            if (workOrder.Template.NumberRule == null)
                throw new ValidationException("条码规则不能为空".L10N());
            if (workOrder.Template.LabelTemplate == null)
                throw new ValidationException("标签模板不能为空".L10N());
            if (workOrder.Template.PackingTemplate == null)
                throw new ValidationException("包装模板不能为空".L10N());
        }

        /// <summary>
        /// 新增时，设置工单属性值
        /// </summary>
        /// <param name="workOrder">工单</param>
        private void SetNewWorkOrderValues(WorkOrder workOrder)
        {
            var workOrderValues = new EntityList<PropertyValueViewModel>();
            var vm = workOrder.LocalContext.GetPropertyOrDefault<EntityList<PropertyValueViewModel>>("WorkOrderValueList");
            if (vm != null)
            {
                if (vm.Any(p => p.DefinitionId == 0))
                    throw new ValidationException("属性值的物料属性不能为空。");
                ////工单属性值
                workOrderValues.AddRange(vm);
            }

            if (workOrderValues != null)
            {
                workOrder.PropertyValueList.Clear(); //清空工单属性值列表
                foreach (var workorderValue in workOrderValues)  //工单属性值
                {
                    foreach (var value in workorderValue.Values)
                    {
                        WorkOrderPropertyValue item = new WorkOrderPropertyValue()
                        {
                            Definition = workorderValue.Definition,
                            Value = value,
                            WorkOrderId = workOrder.Id
                        };
                        item.GenerateId();
                        workOrder.PropertyValueList.Add(item);
                    }
                }
            }
        }

        /// <summary>
        /// 新增时，设置工单BOM属性值
        /// </summary>
        /// <param name="workOrder">工单</param>
        private void SetNewWorkOrderBomValues(WorkOrder workOrder)
        {
            foreach (var bom in workOrder.BomList)
            {
                var workorderBomValues = new EntityList<PropertyValueViewModel>();
                var vm = bom.LocalContext.GetPropertyOrDefault<EntityList<PropertyValueViewModel>>("WorkOrderBomValueList");
                if (vm != null)
                {
                    workorderBomValues.AddRange(vm);
                    bom.PropertyValueList.Clear();  //清空工单BOM属性值
                    foreach (var workorderBomValue in workorderBomValues)  //工单BOM属性值
                    {
                        foreach (var value in workorderBomValue.Values)
                        {
                            WorkOrderBomPropertyValue item = new WorkOrderBomPropertyValue()
                            {
                                Definition = workorderBomValue.Definition,
                                Value = value,
                                BomId = bom.Id
                            };
                            item.GenerateId();
                            bom.PropertyValueList.Add(item);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 新增时，设置工序BOM属性值
        /// </summary>
        /// <param name="workOrder">工单</param>
        private void SetNewWorkOrderProcessBomValues(WorkOrder workOrder)
        {
            foreach (var processBom in workOrder.ProcessBomList)
            {
                var workorderProcessBomValues = new EntityList<PropertyValueViewModel>();
                var vm = processBom.LocalContext.GetPropertyOrDefault<EntityList<PropertyValueViewModel>>("WorkOrderProcessBomValueList");
                if (vm != null)
                    workorderProcessBomValues.AddRange(vm);
                processBom.PropertyValueList.Clear();   //清空工单工序bom属性值
                foreach (var workorderProcessBomValue in workorderProcessBomValues)  //工单工序bom属性值
                {
                    foreach (var value in workorderProcessBomValue.Values)
                    {
                        WorkOrderProcessBomPropertyValue item = new WorkOrderProcessBomPropertyValue()
                        {
                            Definition = workorderProcessBomValue.Definition,
                            Value = value,
                            ProcessBomId = processBom.Id
                        };
                        item.GenerateId();
                        processBom.PropertyValueList.Add(item);
                    }
                }
            }
        }

        /// <summary>
        /// 修改时，设置工单属性值
        /// </summary>
        /// <param name="workOrder">工单</param>
        private void SetModifierWorkOrderValues(WorkOrder workOrder)
        {
            var workorderValues = workOrder.LocalContext.GetPropertyOrDefault<EntityList<PropertyValueViewModel>>("PropertyValueViewModel");
            EntityList<WorkOrderPropertyValue> targetValue = new EntityList<WorkOrderPropertyValue>();  //编辑后的属性值
            if (workorderValues != null)
            {
                if (workorderValues.Any(p => p.DefinitionId == 0))
                    throw new ValidationException("属性值的物料属性不能为空。");
                ModelsToValues(workorderValues, targetValue);  //类型转换
            }
            EntityList<WorkOrderPropertyValue> soureValue = new EntityList<WorkOrderPropertyValue>();
            soureValue.AddRange(workOrder.PropertyValueList);
            foreach (var propertyValue in soureValue)  //修改前工单属性值
            {
                //匹配属性值是否存在，存在则修改，不存在则删除
                var result = targetValue.Where(p => p.DefinitionId == propertyValue.DefinitionId && p.Value == propertyValue.Value).FirstOrDefault();
                if (result == null)
                {
                    ////属性不存在，删除
                    workOrder.PropertyValueList.Remove(propertyValue);
                }
                else
                {
                    ////移除没有变化的属性值，剩下的为新增属性值
                    targetValue.Remove(result);
                }
            }

            workOrder.PropertyValueList.AddRange(targetValue);
        }

        /// <summary>
        /// 将ViewModel转换成PropertyValue
        /// </summary>
        /// <param name="workorderValues">PropertyValueViewModel列表</param>
        /// <param name="targetValue">PropertyValue列表</param>
        /// <returns>WorkOrderPropertyValue列表</returns>
        private EntityList<WorkOrderPropertyValue> ModelsToValues(EntityList<PropertyValueViewModel> workorderValues, EntityList<WorkOrderPropertyValue> targetValue)
        {
            foreach (var workorderValue in workorderValues)
            {
                foreach (var value in workorderValue.Values)
                {
                    WorkOrderPropertyValue propertyValue = new WorkOrderPropertyValue()
                    {
                        Definition = workorderValue.Definition,
                        Value = value,
                        WorkOrderId = workorderValue.ParentId
                    };
                    targetValue.Add(propertyValue);
                }
            }

            return targetValue;
        }

        /// <summary>
        /// 修改时，设置工单BOM属性值
        /// </summary>
        /// <param name="workOrder">工单</param>
        private void SetModifierWorkOrderBomValues(WorkOrder workOrder)
        {
            foreach (var bom in workOrder.BomList)
            {
                var bomValues = bom.LocalContext.GetPropertyOrDefault<EntityList<PropertyValueViewModel>>("WorkOrderBomValueList");
                EntityList<WorkOrderBomPropertyValue> targetValue = new EntityList<WorkOrderBomPropertyValue>();  //编辑后的BOM属性值
                if (bomValues != null)
                    ModelsToBomValues(bomValues, targetValue);  //类型转换
                EntityList<WorkOrderBomPropertyValue> soureValue = new EntityList<WorkOrderBomPropertyValue>();
                soureValue.AddRange(bom.PropertyValueList);
                foreach (var propertyValue in soureValue)  //修改前工单BOM属性值
                {
                    //匹配属性值是否存在，存在则修改，不存在则删除
                    var result = targetValue.Where(p => p.DefinitionId == propertyValue.DefinitionId && p.Value == propertyValue.Value).FirstOrDefault();
                    if (result == null)
                    {
                        ////属性不存在，删除
                        bom.PropertyValueList.Remove(propertyValue);
                    }
                    else
                    {
                        ////移除没有变化的属性值，剩下的为新增属性值
                        targetValue.Remove(result);
                    }
                }

                bom.PropertyValueList.AddRange(targetValue);
            }
        }

        /// <summary>
        /// 将ViewModel转换成BOMValue
        /// </summary>
        /// <param name="bomValues">PropertyValueViewModel列表</param>
        /// <param name="targetValue">BomPropertyValue列表</param>
        /// <returns>WorkOrderBomPropertyValue列表</returns>
        private EntityList<WorkOrderBomPropertyValue> ModelsToBomValues(EntityList<PropertyValueViewModel> bomValues, EntityList<WorkOrderBomPropertyValue> targetValue)
        {
            foreach (var workorderValue in bomValues)
            {
                foreach (var value in workorderValue.Values)
                {
                    WorkOrderBomPropertyValue propertyValue = new WorkOrderBomPropertyValue()
                    {
                        Definition = workorderValue.Definition,
                        Value = value,
                        BomId = workorderValue.ParentId
                    };
                    targetValue.Add(propertyValue);
                }
            }

            return targetValue;
        }

        /// <summary>
        /// 修改时，设置工序BOM属性值
        /// </summary>
        /// <param name="workOrder">工单</param>
        private void SetModifierWorkOrderProcessBomValues(WorkOrder workOrder)
        {
            foreach (var processBom in workOrder.ProcessBomList)
            {
                var processBomValues = processBom.LocalContext.GetPropertyOrDefault<EntityList<PropertyValueViewModel>>("WorkOrderProcessBomValueList");
                EntityList<WorkOrderProcessBomPropertyValue> targetValue = new EntityList<WorkOrderProcessBomPropertyValue>();  //编辑后的BOM属性值
                if (processBomValues != null)
                    ModelsToProcessBomValues(processBomValues, targetValue);  //类型转换
                EntityList<WorkOrderProcessBomPropertyValue> soureValue = new EntityList<WorkOrderProcessBomPropertyValue>();
                soureValue.AddRange(processBom.PropertyValueList);
                foreach (var propertyValue in soureValue)  //修改前工单BOM属性值
                {
                    //匹配属性值是否存在，存在则修改，不存在则删除
                    var result = targetValue.Where(p => p.DefinitionId == propertyValue.DefinitionId && p.Value == propertyValue.Value).FirstOrDefault();
                    if (result == null)
                    {
                        ////属性不存在，删除
                        processBom.PropertyValueList.Remove(propertyValue);
                    }
                    else
                    {
                        ////移除没有变化的属性值，剩下的为新增属性值
                        targetValue.Remove(result);
                    }
                }

                processBom.ProcessId = processBom.RoutingProcess?.ProcessId;
                processBom.PropertyValueList.AddRange(targetValue);
            }
        }

        /// <summary>
        /// 将ViewModel转换成ProcessValue
        /// </summary>
        /// <param name="processBomValues">PropertyValueViewModel列表</param>
        /// <param name="targetValue">processBomValue列表</param>
        /// <returns>WorkOrderrProcessBomPropertyValue列表</returns>
        private EntityList<WorkOrderProcessBomPropertyValue> ModelsToProcessBomValues(EntityList<PropertyValueViewModel> processBomValues, EntityList<WorkOrderProcessBomPropertyValue> targetValue)
        {
            foreach (var processBomValue in processBomValues)
            {
                foreach (var value in processBomValue.Values)
                {
                    WorkOrderProcessBomPropertyValue propertyValue = new WorkOrderProcessBomPropertyValue()
                    {
                        Definition = processBomValue.Definition,
                        Value = value,
                        ProcessBomId = processBomValue.ParentId
                    };
                    targetValue.Add(propertyValue);
                }
            }

            return targetValue;
        }
    }

    /// <summary>
    /// 工单保存并新增命令
    /// </summary>
    [Command(Label = "保存添加", ImageName = "SaveIncrease",
        ToolTip = "保存后添加当前数据", GroupType = CommandGroupType.Edit)]
    public class WorkOrderFormSaveThenAddCommand : SaveWorkOrderCommand
    {
        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="view">明细视图</param>
        public override void Execute(DetailLogicalView view)
        {
            base.Execute(view);
            CRT.Workbench.GetViewContent(view.ViewId).Title = "添加".L10N() + view.Meta.Label;
            View.IsReadOnly = MetaModel.ReadOnlyStatus.None;
            var workOrder = CreatWorkOrder();
            View.Current = workOrder;
        }

        /// <summary>
        /// 创建工单
        /// </summary>
        /// <returns>工单</returns>
        protected WorkOrder CreatWorkOrder()
        {
            var workOrder = new WorkOrder();
            try
            {
                workOrder.GenerateId();
                workOrder.Source = SourceType.Internal;
                workOrder.State = Core.WorkOrders.WorkOrderState.Release;
                workOrder.KitType = null;
                workOrder.Type = SIE.Core.WorkOrders.WorkOrderType.Mass;
                workOrder.MakerId = RT.IdentityId;
                workOrder.MakeDate = RF.Find<WorkOrder>().GetDbTime();
                var template = new Core.Items.LabelPrintTemplate();
                template.GenerateId();
                workOrder.Template = template;
                var workOrderPropertyChanged = RT.Service.Resolve<WorkOrderPropertyChanged>();
                workOrder.PropertyChanged += workOrderPropertyChanged.WorkOrderOnPropertyChanged;
                SetWoPropertyValuesChanged(workOrder);
                workOrder.No = RT.Service.Resolve<WorkOrderController>().GetWorkOrderNo();
            }
            catch (Exception exc)
            {
                exc.Alert();
            }
            return workOrder;
        }

        /// <summary>
        /// 设置工单属性值变更
        /// </summary>
        /// <param name="workOrder">工单</param>
        private void SetWoPropertyValuesChanged(WorkOrder workOrder)
        {
            var workOrderValueList = new EntityList<PropertyValueViewModel>();
            workOrder.LocalContext.SetExtendedProperty("WorkOrderValueList", workOrderValueList);
            workOrder.PropertyValueList.CollectionChanged += (s, e) =>
            {
                var propertyValues = s as EntityList<WorkOrderPropertyValue>;
                var result = propertyValues.GroupBy(p => p.DefinitionId).Select(f => new PropertyValueViewModel { DefinitionId = f.Key, Values = f.Select(p => p.Value).ToList(), Type = f.Select(p => p.WorkOrder).FirstOrDefault().GetType(), ParentId = f.Select(p => p.WorkOrderId).FirstOrDefault() });
                workOrderValueList.Clear();
                workOrderValueList.AddRange(result);
                foreach (var value in workOrderValueList)
                    value.ItemId = workOrder.ProductId;
                workOrder.LocalContext.SetExtendedProperty("WorkOrderValueList", workOrderValueList);
            };
        }
    }
}