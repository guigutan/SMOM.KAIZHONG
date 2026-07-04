using SIE.DIST;
using SIE.Wpf.Command;

namespace SIE.Wpf.DIST
{
    /// <summary>
    /// 修改命令
    /// </summary>
    [Command(ImageName = "EditEntity", Label = "修改", ToolTip = "修改数据", Gestures = "Ctrl+Shift+E", GroupType = 10)]
    public class GoodsIssueEditCommand : ListEditCommand
    {
        /// <summary>
        /// 能否执行修改
        /// </summary>
        /// <param name="view">当前列表逻辑视图</param>
        /// <returns>True:可以执行 False:不可以执行</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var goodsIssue = view.Current as GoodsIssue;
            return goodsIssue != null && goodsIssue.DistributionQty <= 0;
        }
    }
}