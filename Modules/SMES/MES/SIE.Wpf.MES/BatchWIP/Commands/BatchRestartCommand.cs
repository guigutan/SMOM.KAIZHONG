using SIE.Wpf.Command;
using SIE.Wpf.MES.WIP;

namespace SIE.Wpf.MES.BatchWIP.Commands
{
    /// <summary>
    /// 采集重新开始命令
    /// </summary>
    [Command(ImageName = "Refresh", Label = "重新开始", ToolTip = "重新开始", GroupType = CommandGroupType.Edit)]
    public class BatchRestartCommand : DetailViewCommand
    {
        /// <summary>
        /// 是否可执行的逻辑
        /// </summary>
        /// <param name="view">视图对象</param>
        /// <returns>返回是否可执行</returns>
        public override bool CanExecute(DetailLogicalView view)
        {
            return base.CanExecute(view) && (view.Current as BatchDataCollectionViewModel) != null;
        }

        /// <summary>
        /// 执行具体的逻辑。
        /// </summary>
        /// <param name="view">视图对象</param>
        public override void Execute(DetailLogicalView view)
        {
            var vm = view.Current as BatchDataCollectionViewModel;
            vm.Reset(resetType: ResetType.CollectRestart);
            vm.FocuseBarcode();
        }
    }
}
