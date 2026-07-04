using SIE.Wpf.Command;

namespace SIE.Wpf.MES.WIP.PackRecombine.Commands
{
    /// <summary>
    /// 包装拆合重置命令
    /// </summary>
    [Command(ImageName = "Refresh", Label = "重新开始", ToolTip = "重新开始", GroupType = CommandGroupType.Edit)]
    public class PackRecombineReset : DetailViewCommand
    {
        /// <summary>
        /// 是否可执行
        /// </summary>
        /// <param name="view">明细逻辑视图</param>
        /// <returns>bool</returns>
        public override bool CanExecute(DetailLogicalView view)
        {
            return view.Current as PackRecombineBaseViewModel != null;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="view">明细逻辑视图</param>
        public override void Execute(DetailLogicalView view)
        {
            var vm = (view.Current as PackRecombineBaseViewModel);
            vm?.Reset();
        }
    }
}