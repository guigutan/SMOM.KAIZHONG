using SIE.Wpf.Command;
using System;

namespace SIE.Wpf.DIST
{
    /// <summary>
    /// 提交命令
    /// </summary>
    [Command(ImageName = "Check", Label = "提交", ToolTip = "提交", GroupType = CommandGroupType.View)]
    public class SubmitCommand : DetailViewCommand
    {
        /// <summary>
        /// 判断命令能否执行
        /// </summary>
        /// <param name="view">明细逻辑视图</param>
        /// <returns>能执行返回true，不能执行返回false</returns>
        public override bool CanExecute(DetailLogicalView view)
        {
            var model = view.Current as GoodsIssueViewModel;
            if (model == null || !model.TurnoverBox.IsNotEmpty())
                return false;
            return true;
        }

        /// <summary>
        /// 提交命令执行方法
        /// </summary>
        /// <param name="view">明细逻辑视图</param>
        public override void Execute(DetailLogicalView view)
        {
            var model = view.Current as GoodsIssueViewModel;
            model.Submit();
        }
    }
}