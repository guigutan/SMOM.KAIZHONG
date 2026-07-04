using SIE.MES.TaskManagement.Dispatchs;
using SIE.Wpf.Command;

namespace SIE.Wpf.MES.TaskManagement.Reports.Commands
{
    /// <summary>
    /// 开工 命令
    /// </summary>
    [Command(ImageName = "Play",
        Label = "开工",
        ToolTip = "开工",
        GroupType = CommandGroupType.Edit)]
    public class StartTaskReportCommand : DetailViewCommand
    {
        /// <summary>
        /// 是否可执行的逻辑
        /// </summary>
        /// <param name="view">视图对象</param>
        /// <returns>返回是否可执行</returns>
        public override bool CanExecute(DetailLogicalView view)
        {
            var vm = view.Current as TaskReportViewModel;
            return base.CanExecute(view) && vm.DispatchTask != null && (vm.DispatchTask.TaskStatus == DispatchTaskStatus.Dispatched
                || vm.DispatchTask.TaskStatus == DispatchTaskStatus.Dispatching ||
                vm.DispatchTask.TaskStatus == DispatchTaskStatus.ToDispatch);
        }

        /// <summary>
        /// 执行具体的逻辑。
        /// </summary>
        /// <param name="view">视图对象</param>
        public override void Execute(DetailLogicalView view)
        {
            var vm = view.Current as TaskReportViewModel;
            vm.StartWork();
        }
    }
}
