using SIE.Wpf.Command;
using System.Linq;

namespace SIE.Wpf.MES.PanelBindings.Commands
{
    /// <summary>
    /// 移除条码命令
    /// </summary>
    [Command(ImageName = "CalendarRemove", Label = "移除条码", ToolTip = "移除条码", GroupType = CommandGroupType.Edit)]
    public class RemoveSnCommand : ListViewCommand
    {
        /// <summary>
        /// 是否可执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <returns>是否可执行</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var models = view.SelectedEntities.OfType<SnViewModel>();
            return models.Count() == 1;
        }

        /// <summary>
        /// 命令执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            var vm = (view.Relations[0].View.Current as PanelBindingViewModel);
            var sn = view.Current as SnViewModel;
            vm.RemoveSn(sn.Sn);
            view.Data.Remove(sn);
        }
    }
}
