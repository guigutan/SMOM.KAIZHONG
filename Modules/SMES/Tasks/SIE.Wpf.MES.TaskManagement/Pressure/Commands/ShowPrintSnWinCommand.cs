using SIE.Wpf.Command;
using SIE.Wpf.MES.WIP.Pressure;

namespace SIE.Wpf.MES.TaskManagement.Pressure.Commands
{
    /// <summary>
    /// SN生成并打印 命令
    /// </summary>
    [Command(ImageName = "PrintData", Label = "生成并打印", ToolTip = "生成并打印", GroupType = CommandGroupType.Edit)]
    public class ShowPrintSnWinCommand : DetailViewCommand
    {
        /// <summary>
        /// 是否可执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <returns>bool</returns>
        public override bool CanExecute(DetailLogicalView view)
        {
            return view.Current != null && (view.Current as PressureViewModel)?.WorkOrder != null;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        public override void Execute(DetailLogicalView view)
        {
            var vm = view.Current as PressureSnPrintViewModel;
            vm?.ShowPrintSnWin();
        }
    }

}
