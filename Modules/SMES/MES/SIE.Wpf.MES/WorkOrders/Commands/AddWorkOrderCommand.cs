using SIE.Common;
using SIE.Domain;
using SIE.MES.WorkOrders;
using SIE.Wpf.Command;
using SIE.Wpf.Items.ViewModels;
using System;
using System.Linq;

namespace SIE.Wpf.MES.WorkOrders.Commands
{
    /// <summary>
    /// 表格添加工单命令
    /// </summary>
    [Command(ImageName = "AddEntity", Label = "添加", Hierarchy = "工单生成", GroupType = CommandGroupType.Edit)]
    public class AddWorkOrderCommand : ListAddCommand
    {
        /// <summary>
        /// 重写视图显示方法
        /// </summary>
        /// <param name="entity">Entity</param>
        protected override void ShowView(Entity entity)
        {
            string key = CRT.Workbench.CreateKey(ViewConfig.DetailsView, typeof(WorkOrder), entity);

            CRT.Workbench.ShowView(key, v =>
            {
                v.Title = Meta.Label + View.Meta.Label;
                var template = new WorkOrderDetailTemplate(ViewConfig.DetailsView);
                var ui = template.CreateUI();
                ui.MainView.Data = entity;
                return ui;
            });
        }

        /// <summary>
        /// 重写实体创建后方法
        /// </summary>
        /// <param name="entity">Entity</param>
        protected override void OnItemCreated(Entity entity)
        {
            try
            {
                var workOrder = entity as WorkOrder;
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
        }

        /// <summary>
        /// 设置工单属性值变更
        /// </summary>
        /// <param name="workOrder">WorkOrder</param>
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

    /// <summary>
    /// 表单添加命令
    /// </summary>
    [Command(ImageName = "AddEntity", Label = "添加", GroupType = CommandGroupType.Edit)]
    public class WorkOrderFormAddCommand : FormAddCommand
    {
        /// <summary>
        /// 重写实体创建后方法
        /// </summary>
        /// <param name="entity">Entity</param>
        protected override void OnNewItemCreated(Entity entity)
        {
            try
            {
                var workOrder = entity as WorkOrder;
                workOrder.Source = SourceType.Internal;
                workOrder.State = Core.WorkOrders.WorkOrderState.Release;
                workOrder.KitType = null;
                workOrder.Type = SIE.Core.WorkOrders.WorkOrderType.Mass;
                workOrder.MakerId = RT.IdentityId;
                workOrder.MakeDate = RF.Find<WorkOrder>().GetDbTime();
                workOrder.No = RT.Service.Resolve<WorkOrderController>().GetWorkOrderNo();
                workOrder.Template = new Core.Items.LabelPrintTemplate();
                var workOrderPropertyChanged = RT.Service.Resolve<WorkOrderPropertyChanged>();
                workOrder.PropertyChanged += workOrderPropertyChanged.WorkOrderOnPropertyChanged;
                SetWoPropertyValuesChanged(workOrder);
            }
            catch (Exception exc)
            {
                exc.Alert();
            }

            base.OnNewItemCreated(entity);
        }

        /// <summary>
        /// 设置工单属性值变更
        /// </summary>
        /// <param name="workOrder">WorkOrder</param>
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
