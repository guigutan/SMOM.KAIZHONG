using SIE.Wpf.Command;

namespace SIE.Wpf.MES.TaskManagement.Reports
{
    /// <summary>
    /// 重新开始 命令
    /// </summary>
    [Command(ImageName = "Refresh",
        Label = "重新开始",
        ToolTip = "重新开始",
        GroupType = CommandGroupType.None)]
    public class RestartReportCommand : DetailViewCommand
    {
        /// <summary>
        /// 是否可执行的逻辑
        /// </summary>
        /// <param name="view">视图对象</param>
        /// <returns>返回是否可执行</returns>
        public override bool CanExecute(DetailLogicalView view)
        {
            return base.CanExecute(view) && (view.Current as TaskReportViewModel) != null;
        }

        /// <summary>
        /// 执行具体的逻辑。
        /// </summary>
        /// <param name="view">视图对象</param>
        public override void Execute(DetailLogicalView view)
        {
            var vm = view.Current as TaskReportViewModel;
            vm.Reset(true);
            vm.FocuseBarcode();
        }
    }
}
