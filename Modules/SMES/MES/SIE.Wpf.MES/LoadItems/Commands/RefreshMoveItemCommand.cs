using SIE.Wpf.Command;
using System;

namespace SIE.Wpf.MES.LoadItems.Commands
{
    /// <summary>
    /// 刷新挪料列表
    /// </summary>
    [Command(ImageName = "Refresh", Label = "刷新", ToolTip = "刷新列表", GroupType = 3)]
    public class RefreshMoveItemCommand : ListViewCommand
    {
        /// <summary>
        /// 是否可执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <returns>bool</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var mainView = view.Relations.Find("mainView");
            return mainView?.Current is ILoadableItem;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            var m = view.Relations.Find("mainView").Current.CastTo<ILoadableItem>();
            m.RefreshMoveItem();
        }
    }
}
