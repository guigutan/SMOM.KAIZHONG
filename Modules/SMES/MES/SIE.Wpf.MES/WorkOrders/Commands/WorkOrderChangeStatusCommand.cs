using SIE.Domain;
using SIE.MES.WorkOrders;
using SIE.Wpf.Command;
using SIE.Wpf.MES.WorkOrders.ViewModels;
using System;
using System.Linq;

namespace SIE.Wpf.MES.WorkOrders.Commands
{
    /// <summary>
    /// 更改工单状态命令
    /// </summary>
    public abstract class WorkOrderChangeStatusCommand : ListAddCommand
    {
        /// <summary>
        /// 工单帮助类
        /// </summary>
        protected WorkOrderHelper Helper { get; set; } = RT.Service.Resolve<WorkOrderHelper>();

        /// <summary>
        /// 是否可执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <returns>bool</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            return view.Current != null && view.Current is WorkOrder && view.SelectedEntities.Count == 1;
        }

        /// <summary>
        /// 创建编辑实体
        /// </summary>
        /// <returns>创建后的实体</returns>
        protected override Entity CreateNewItem()
        {
            var workOrder = View.Current as WorkOrder;
            return new WorkOrderChangeStatus(workOrder);
        }

        /// <summary>
        /// 显示状态变更信息输入框
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>int</returns>
        protected int ShowDetailsView(Entity entity)
        {
            var ui = new DetailsUITemplate<WorkOrderChangeStatus>().CreateUI();
            ui.MainView.Data = entity;
            ui.MainView.Data.MarkSaved();
            return CRT.Workbench.ShowDialog(ui, v =>
            {
                v.Title = this.Label + "工单".L10N();
                v.Width = 500;
                v.Height = 350;
                v.Closing += (o, e) =>
                {
                    if (v.Result != 0 && ui.MainView.Data.IsDirty)
                    {
                        if (CRT.MessageService.AskQuestion("直接退出将不会保存数据，是否退出？".L10N()) == false)
                        {
                            e.Cancel = true;
                        }
                    }
                    else if (v.Result == 0)
                    {
                        if ((entity as WorkOrderChangeStatus).Reason.IsNullOrEmpty())
                        {
                            CRT.MessageService.ShowError("改变工单状态的原因不能为空".L10N());
                            e.Cancel = true;
                        }
                    }
                };
            });
        }
    }

    /// <summary>
    /// 恢复工单
    /// </summary>
    [Command(ImageName = "RefreshCounterclockwiseDown", Hierarchy = "状态", Label = "恢复", GroupType = CommandGroupType.Edit)]
    public class WorkOrderResumeCommand : WorkOrderChangeStatusCommand
    {
        /// <summary>
        /// 是否可执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <returns>bool</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var workOrder = view.Current as WorkOrder;
            if (workOrder == null)
                return base.CanExecute(view);
            return base.CanExecute(view) && Helper.CanResume(workOrder);
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            var m = this.GetEditEntity() as WorkOrderChangeStatus;
            if (ShowDetailsView(m) == 0)
            {
                RT.Service.Resolve<WorkOrderController>().Resume(m.WorkOrder.Id, m.Reason);
                view.QueryView.TryExecuteQuery();
            }
        }
    }

    /// <summary>
    /// 暂停工单
    /// </summary>
    [Command(ImageName = "Pause", Hierarchy = "状态", Label = "暂停", GroupType = CommandGroupType.Edit)]
    public class WorkOrderPauseCommand : WorkOrderChangeStatusCommand
    {
        /// <summary>
        /// 是否可执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <returns>bool</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var workOrder = view.Current as WorkOrder;
            if (workOrder == null)
                return base.CanExecute(view);
            return base.CanExecute(view) && Helper.CanPause(workOrder);
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            var m = this.GetEditEntity() as WorkOrderChangeStatus;
            if (ShowDetailsView(m) == 0)
            {
                RT.Service.Resolve<WorkOrderController>().Pause(m.WorkOrder.Id, m.Reason);
                view.QueryView.TryExecuteQuery();
            }
        }
    }

    /// <summary>
    /// 关闭工单
    /// </summary>
    [Command(ImageName = "CloseCircle", Hierarchy = "状态", Label = "强制关闭", GroupType = CommandGroupType.Edit)]
    public class WorkOrderCloseCommand : WorkOrderChangeStatusCommand
    {
        /// <summary>
        /// 是否可执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <returns>bool</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var workOrder = view.Current as WorkOrder;
            if (workOrder == null)
                return base.CanExecute(view);
            return base.CanExecute(view) && Helper.CanClose(workOrder);
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            var m = this.GetEditEntity() as WorkOrderChangeStatus;
            if (ShowDetailsView(m) == 0)
            {
                RT.Service.Resolve<WorkOrderController>().Close(m.WorkOrder.Id, m.Reason);
                view.QueryView.TryExecuteQuery();
            }
        }
    }
}