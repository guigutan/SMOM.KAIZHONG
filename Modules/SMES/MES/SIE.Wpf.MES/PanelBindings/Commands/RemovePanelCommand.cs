using SIE.Wpf.Command;
using System.Linq;

namespace SIE.Wpf.MES.PanelBindings.Commands
{
    /// <summary>
    /// 移除拼板码命令
    /// </summary>
    [Command(ImageName = "CalendarRemove", Label = "移除条码", ToolTip = "移除条码", GroupType = CommandGroupType.Edit)]
    public class RemovePanelCommand : ListViewCommand
    {
        /// <summary>
        /// 是否可执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <returns>是否可执行</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var models = view.SelectedEntities.OfType<PanelViewModel>();
            return models.Count() == 1;
        }

        /// <summary>
        /// 命令执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            var vm = (view.Relations[0].View.Current as PanelBindingViewModel);
            var panel = view.Current as PanelViewModel;

            vm.RemovePanel(panel);

            if (panel.BindingDate != null)
            {
                view.Data.Remove(panel);
            }
        }
    }
}
