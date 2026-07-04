using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.WorkOrders;
using SIE.Wpf.Command;
using SIE.Wpf.MES.WorkOrders.Routings;
using System;
using System.Linq;

namespace SIE.Wpf.MES.WorkOrders.Commands
{
    /// <summary>
    /// 工单工艺路线 修改
    /// </summary>
    [Command(ImageName = "Edit", Label = "修改工艺路线", GroupType = CommandGroupType.Edit)]
    public class UpdateRoutingLayoutCommand : ListEditCommand
    {
        /// <summary>
        /// 是否可执行
        /// 非关闭或者完工状态的工单暂停后可以修改
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <returns>bool</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            if (view.Current == null && view.SelectedEntities.Count() != 1)
                return false;
            var workOrder = view.Current as WorkOrder;
            return workOrder != null && workOrder.State != Core.WorkOrders.WorkOrderState.Close && workOrder.State != Core.WorkOrders.WorkOrderState.Finish && workOrder.IsPause == YesNo.Yes;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            var workOrder = view.Current as WorkOrder;
            ValidateWorkOrder(workOrder);
            var design = new WorkOrderRoutingEdit();
            design.InitializeLayout(workOrder.Layout?.Layout);
            var result = CRT.Workbench.ShowDialog(Guid.NewGuid().ToString(), design, w =>
            {
                w.Title = "修改 工单工艺路线".L10N();
                w.Closing += (o, e) =>
                {
                    try
                    {
                        var xml = design.container.Model.Serialize();
                        if (w.Result == 0)
                        {
                            design.container.Model.ValidateSave();
                        }
                        else
                        {
                            if (xml != workOrder.Layout.Layout)
                            {
                                e.Cancel = !CRT.MessageService.AskQuestion("直接退出将不会保存数据，是否退出？".L10N());
                            }
                        }
                    }
                    catch (Exception exc)
                    {
                        exc.Alert();
                        e.Cancel = true;
                    }
                };

                w.Closed += (o, e) =>
                {
                    if (w.Result == 0)
                    {
                        RT.Service.Resolve<WorkOrderController>().UpdateLayout(workOrder.Id, design.container.Model.Serialize());
                        view.QueryView?.TryExecuteQuery();
                    }
                };
            });
        }

        /// <summary>
        /// 验证工单
        /// </summary>
        /// <param name="workOrder">工单</param>
        private void ValidateWorkOrder(WorkOrder workOrder)
        {
            if (workOrder.IsPause == YesNo.No)
                throw new ValidationException("工单必须要暂停状态才可以修改工艺路线".L10N());
            if (workOrder.State == Core.WorkOrders.WorkOrderState.Close || workOrder.State == Core.WorkOrders.WorkOrderState.Finish)
                throw new ValidationException("不允许修改，工单已{0}".L10nFormat(workOrder.State.ToLabel()));
        }
    }
}