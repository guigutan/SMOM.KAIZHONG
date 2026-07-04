using SIE.Wpf.Command;

namespace SIE.Wpf.DIST
{
    /// <summary>
    /// 重新开始命令
    /// </summary>
    [Command(ImageName = "Refresh", Label = "重新开始", GroupType = CommandGroupType.View)]
    public class RestartCommand : DetailViewCommand
    {
        /// <summary>
        /// 重新开始命令执行方法
        /// </summary>
        /// <param name="view">明细逻辑视图</param>
        public override void Execute(DetailLogicalView view)
        {
            var model = view.Current as GoodsIssueViewModel;
            model.Restart();
        }
    }
}
