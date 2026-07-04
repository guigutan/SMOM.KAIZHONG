using SIE.ProductIntfc.FirstInsps;
using SIE.ProductIntfc.InspLogs;
using SIE.Wpf.Command;
using System;

namespace SIE.Wpf.ProductIntfc.FirstInsps.Commands
{
    /// <summary>
    /// 报检规则命令
    /// </summary>
    [Command(Label = "报检规则", ImageName = "ListGear", ToolTip = "报检规则", Gestures = "Ctrl+Shift+G", GroupType = 10)]
    public class FirstInspRuleCommand : ListViewCommand
    {

        /// <summary>
        /// 报检规则命令执行方法
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            var template = new ListUITemplate(typeof(FirstInspRule), ViewConfig.ListView);
            var ui = template.CreateUI();
            ui.MainView.Data = RT.Service.Resolve<InspLogController>().GetFirstInspRuleList();
            CRT.Workbench.ShowDialog(Guid.NewGuid().ToString(), ui.Control, w =>
            {
                w.Title = "报检规则".L10N();
                w.MinHeight = 350;
                w.MinWidth = 550;
                w.Height = 350;
                w.Width = 550;
                w.Commands.Clear();
            });
        }


    }

    /// <summary>
    /// 报检规则命令
    /// </summary>
    [Command(Label = "修改", ImageName = "EditEntity", ToolTip = "修改", GroupType = CommandGroupType.Edit)]
    public class FirstInspListCommand : ListEditCommand
    {
        /// <summary>
        /// 不能修改参数=工单的
        /// </summary>
        /// <param name="view">视图</param>
        /// <returns></returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var item = view.Current as FirstInspRule;
            return item.Parameter != FirstInspParam.WorkOrder;
        }
    }
}
