using SIE.MetaModel.View;
using SIE.Packages.Boxs;
using SIE.Wpf.Command;

namespace SIE.Wpf.Packages.Boxs
{
    /// <summary>
    /// 周转箱删除命令
    /// 周转箱类型为报废或者闲置可以删除
    /// </summary>
    [Command(ImageName = "DeleteEntity", Label = "删除", ToolTip = "删除数据", Gestures = "Delete", Location = CommandLocation.All, GroupType = 10)]
    public class TurnoverBoxDeleteCommand : ListDeleteCommand
    {
        /// <summary>
        /// 判断删除命令能否执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <returns>能执行返回true，不能返回false</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var turnoverBox = view.Current as TurnoverBox;
            return turnoverBox != null && (turnoverBox.State == BoxState.Scrap || turnoverBox.State == BoxState.Unused);
        }
    }
}
