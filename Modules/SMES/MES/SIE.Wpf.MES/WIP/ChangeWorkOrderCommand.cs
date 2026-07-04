using SIE.Wpf.Command;
using System;

namespace SIE.Wpf.MES.WIP
{
    /// <summary>
    /// 切换在制工单命令
    /// </summary>
    [Command(ImageName = "Magnify",
        Label = "切换在制工单",
        ToolTip = "切换在制工单",
        GroupType = CommandGroupType.Edit)]
    public class ChangeWorkOrderCommand : DetailViewCommand
    {
        /// <summary>
        /// 是否可执行的逻辑
        /// </summary>
        /// <param name="view">视图对象</param>
        /// <returns>返回是否可执行</returns>
        public override bool CanExecute(DetailLogicalView view)
        {
            return base.CanExecute(view) && (view.Current as DataCollectionViewModel) != null;
        }

        /// <summary>
        /// 执行具体的逻辑。
        /// </summary>
        /// <param name="view">视图对象</param>
        public override void Execute(DetailLogicalView view)
        {
            var model = View.Current as DataCollectionViewModel;

            var workcell = model.GetWorkcell();

            var changeWorkOrderView = new ChangeWorkOrderViewModel(workcell);

            var template = new DetailsUITemplate(typeof(ChangeWorkOrderViewModel), ViewConfig.DetailsView, view.ModuleKey);

            var ui = template.CreateUI();
            ui.MainView.Data = changeWorkOrderView;

            CRT.Workbench.ShowDialog(ui, w =>
            {
                w.Title = "选择当前工位要切换到的工单".L10N();
                w.Height = 150;
                w.Width = 350;
                w.Closing += (s, e) =>
                {
                    if (w.Result == 0)
                    {
                        if (changeWorkOrderView.WorkOrder == null || changeWorkOrderView.WorkOrder.Id == model.WorkOrderId)
                        {
                            return;
                        }

                        model.ChangeWorkStationWorkOrder(workcell, changeWorkOrderView.WorkOrder.Id);
                    }
                };
            });
        }
    }
}