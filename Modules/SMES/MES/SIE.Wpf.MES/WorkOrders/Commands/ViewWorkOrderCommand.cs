using SIE.MES.WorkOrders;
using SIE.Wpf.Command;
using System;

namespace SIE.Wpf.MES.WorkOrders.Commands
{
    /// <summary>
    /// 查看工单
    /// </summary>
    [Command(ImageName = "PageSearch", Label = "查看工单", GroupType = CommandGroupType.View)]
    public class ViewWorkOrderCommand : ListEditCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            string key = CRT.Workbench.CreateKey(WorkOrderViewConfig.ReadonlyView, typeof(WorkOrder), view.Current as WorkOrder);

            CRT.Workbench.ShowView(key, v =>
            {
                v.Title = "查看工单".L10N();
                var template = new WorkOrderDetailTemplate(WorkOrderViewConfig.ReadonlyView);
                var ui = template.CreateUI();
                ui.MainView.Data = view.Current as WorkOrder;
                return ui;
            });
        }
    }
}
