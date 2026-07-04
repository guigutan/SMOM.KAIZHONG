using SIE.Common;
using SIE.Core.Items;
using SIE.Domain;
using SIE.MES.WorkOrders;
using SIE.Wpf.Command;

namespace SIE.Wpf.MES.WorkOrders.Commands
{
    /// <summary>
    /// 表格界面，工单复制命令
    /// </summary>
    [Command(ImageName = "CopyEntity", Label = "复制添加", Hierarchy = "工单生成", GroupType = CommandGroupType.Edit)]
    internal class WorkOrderListCopyCommand : ListCopyCommand
    {
        /// <summary>
        /// 显示
        /// </summary>
        /// <param name="entity">实体</param>
        protected override void ShowView(Entity entity)
        {
            var workOrderCopy = entity as WorkOrder;
            var workOrder = View.CreateNewItem() as WorkOrder;
            var workOrderPropertyChanged = RT.Service.Resolve<WorkOrderPropertyChanged>();
            workOrder.PropertyChanged += workOrderPropertyChanged.WorkOrderOnPropertyChanged;
            var labelTemplate = new Core.Items.LabelPrintTemplate();
            labelTemplate.GenerateId();
            workOrder.Template = labelTemplate;
            workOrder.Product = workOrderCopy.Product;
            workOrder.WorkShop = workOrderCopy.WorkShop;
            workOrder.Resource = workOrderCopy.Resource;

            workOrder.Source = SourceType.Internal;
            workOrder.State = Core.WorkOrders.WorkOrderState.Release;
            workOrder.KitType = null;
            workOrder.Type = workOrderCopy.Type;
            workOrder.MakerId = RT.IdentityId;
            workOrder.MakeDate = RF.Find<WorkOrder>().GetDbTime();
            workOrder.No = RT.Service.Resolve<WorkOrderController>().GetWorkOrderNo();

            workOrder.SaleOrderNo = workOrderCopy.SaleOrderNo;
            workOrder.CustomerOrderNo = workOrderCopy.CustomerOrderNo;
            workOrder.OrderQty = workOrderCopy.OrderQty;
            workOrder.PlanQty = workOrderCopy.PlanQty;

            string key = CRT.Workbench.CreateKey(ViewConfig.DetailsView, typeof(WorkOrder), workOrder);
            CRT.Workbench.ShowView(key, v =>
            {
                v.Title = Meta.Label + View.Meta.Label;
                var template = new WorkOrderDetailTemplate(ViewConfig.DetailsView);
                var ui = template.CreateUI();
                ui.MainView.Data = workOrder;
                return ui;
            });
        }
    }

    /// <summary>
    /// 表单界面，工单复制命令
    /// </summary>
    [Command(ImageName = "CopyEntity", Label = "复制添加", GroupType = CommandGroupType.Edit)]
    internal class WorkOrderFormCopyCommand : FormCopyCommand
    {
        /// <summary>
        /// 复制
        /// </summary>
        /// <param name="targetEntity">目标实体</param>
        /// <returns>赋值后的实体</returns>
        protected override Entity Copy(Entity targetEntity)
        {
            var workOrderCopy = base.Copy(targetEntity as WorkOrder) as WorkOrder;
            var workOrder = RF.Find(View.EntityType).New() as WorkOrder;
            workOrder.GenerateId();
            var workOrderPropertyChanged = RT.Service.Resolve<WorkOrderPropertyChanged>();
            workOrder.PropertyChanged += workOrderPropertyChanged.WorkOrderOnPropertyChanged;
            LabelPrintTemplate template = new Core.Items.LabelPrintTemplate();
            template.GenerateId();
            workOrder.Template = template;
            workOrder.Product = workOrderCopy.Product;
            workOrder.WorkShop = workOrderCopy.WorkShop;
            workOrder.Resource = workOrderCopy.Resource;

            workOrder.Source = SourceType.Internal;
            workOrder.State = Core.WorkOrders.WorkOrderState.Release;
            workOrder.KitType = null;
            workOrder.Type = workOrderCopy.Type;
            workOrder.MakerId = RT.IdentityId;
            workOrder.MakeDate = RF.Find<WorkOrder>().GetDbTime();
            workOrder.No = RT.Service.Resolve<WorkOrderController>().GetWorkOrderNo();

            workOrder.SaleOrderNo = workOrderCopy.SaleOrderNo;
            workOrder.CustomerOrderNo = workOrderCopy.CustomerOrderNo;
            workOrder.OrderQty = workOrderCopy.OrderQty;
            workOrder.PlanQty = workOrderCopy.PlanQty;
            return workOrder;
        }
    }
}
