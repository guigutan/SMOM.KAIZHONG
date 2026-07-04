using SIE.Wpf.Command;

namespace SIE.Wpf.MES.WIP.Packings.Commands
{
    /// <summary>
    /// 包装重置命令
    /// </summary>
    [Command(ImageName = "Refresh", Label = "重新开始", ToolTip = "重新开始", GroupType = CommandGroupType.Edit)]
    public class PackingResetCommand : DetailViewCommand
    {
        /// <summary>
        /// 是否可执行
        /// </summary>
        /// <param name="view">明细逻辑视图</param>
        /// <returns>bool</returns>
        public override bool CanExecute(DetailLogicalView view)
        {
            return view.Current as PackingViewModel != null;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="view">明细逻辑视图</param>
        public override void Execute(DetailLogicalView view)
        {
            var vm = (view.Current as PackingViewModel);
            vm.Reset(ResetType.CollectRestart);
        }
    }

    /// <summary>
    /// 包装重置命令
    /// </summary>
    [Command(ImageName = "Refresh", Label = "重新开始", ToolTip = "重新开始", GroupType = CommandGroupType.Edit)]
    public class NewPackingResetCommand : DetailViewCommand
    {
        /// <summary>
        /// 是否可执行
        /// </summary>
        /// <param name="view">明细逻辑视图</param>
        /// <returns>bool</returns>
        public override bool CanExecute(DetailLogicalView view)
        {
            return view.Current as NewPackingViewModel != null;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="view">明细逻辑视图</param>
        public override void Execute(DetailLogicalView view)
        {
            var vm = (view.Current as NewPackingViewModel);
            vm.Reset(ResetType.CollectRestart);
        }
    }

    /// <summary>
    /// 直接包装重置命令
    /// </summary>
    [Command(ImageName = "Refresh", Label = "重新开始", ToolTip = "重新开始", GroupType = CommandGroupType.Edit)]
    public class DirectPackingResetCommand : DetailViewCommand
    {
        /// <summary>
        /// 是否可执行
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        public override bool CanExecute(DetailLogicalView view)
        {
            return view.Current as DirectPackingViewModel != null;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="view"></param>
        /// <exception cref="System.NotImplementedException"></exception>
        public override void Execute(DetailLogicalView view)
        {
            var vm = (view.Current as DirectPackingViewModel);
            vm.Reset(ResetType.CollectRestart);
        }
    }
}