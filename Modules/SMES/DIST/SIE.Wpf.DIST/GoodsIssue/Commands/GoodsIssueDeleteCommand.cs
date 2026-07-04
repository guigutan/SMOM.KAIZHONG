using SIE.DIST;
using SIE.Wpf.Command;

namespace SIE.Wpf.DIST
{
    /// <summary>
    /// 删除发料单命令
    /// </summary>
    [Command(ImageName = "DeleteEntity", Label = "删除", ToolTip = "删除数据", Gestures = "Delete", GroupType = 10)]
    public class GoodsIssueDeleteCommand : ListDeleteCommand
    {
        /// <summary>
        /// 判断删除发料命令能否执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <returns>能执行返回true，不能执行返回false</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var goodsIssue = view.Current as GoodsIssue;
            return goodsIssue != null && goodsIssue.DistributionQty <= 0;
        }
    }
}