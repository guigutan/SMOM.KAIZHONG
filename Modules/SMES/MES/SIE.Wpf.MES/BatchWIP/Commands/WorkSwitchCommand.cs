using SIE.Barcodes.WipBatchs;
using SIE.MES.BatchWIP;
using SIE.MES.WorkOrders;
using SIE.Wpf.Barcodes.WipBatchs;
using SIE.Wpf.Command;
using System;

namespace SIE.Wpf.MES.BatchWIP.Commands
{
    /// <summary>
    /// 工单选择命令
    /// </summary>
    [Command(Label = "工单选择", ImageName = "Magnify", GroupType = CommandGroupType.Edit)]
    public class WorkSwitchCommand : DetailViewCommand
    {
        /// <summary>
        /// 查询条件
        /// </summary>
        /// <param name="view">逻辑视图</param>
        /// <returns>条件结果</returns>
        public override bool CanExecute(DetailLogicalView view)
        {
            var model = View.Current as BatchDataCollectionViewModel;
            return base.CanExecute(view) && model != null;
        }

        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="view">逻辑视图</param>
        public override void Execute(DetailLogicalView view)
        {
            var model = View.Current as BatchDataCollectionViewModel;
            var workcell = model.GetWorkcell();
            var orderView = AutoUI.ViewFactory.CreateListView(typeof(BatchWorkOrder), BatchWorkOrderViewConfig.BatchWorkOrderView, view.ModuleKey);
            orderView.Data = RT.Service.Resolve<BatchManageController>().GetInputBatchs(workcell.ResourceId, workcell.ProcessId, workcell.StationId);
            CRT.Workbench.ShowDialog(Guid.NewGuid().ToString("N"), orderView.Control, v =>
            {
                v.Title = this.Label.L10N();
                v.Width = 900;
                v.Height = 450;

                v.Closing += (o, e) =>
                {
                    if (v.Result == 0)
                    {
                        var workorder = orderView.Current as WorkOrder;
                        if (workorder == null || workorder.Id == model.WorkOrderId)
                            return;

                        model.SwitchWorkOrder(workcell, workorder.Id);
                    }
                };
            });
        }
    }
}
