using SIE.Wpf.Command;
using System.Linq;

namespace SIE.Wpf.MES.PanelBindings.Commands
{
    /// <summary>
    /// 绑定命令
    /// </summary>
    [Command(ImageName = "Link", Label = "绑定", ToolTip = "绑定", GroupType = CommandGroupType.Edit)]
    public class ManualBindingCommand : ListViewCommand
    {
        /// <summary>
        /// 是否可执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <returns>是否可执行</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var models = view.SelectedEntities.OfType<PanelViewModel>();
            return models.Count() == 1 && !models.First().IsBindComplete && models.First().BindingQty > 0;
        }

        /// <summary>
        /// 命令执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            var vm = (view.Relations[0].View.Current as PanelBindingViewModel);
            vm.PanelBinding();
        }
    }
}
