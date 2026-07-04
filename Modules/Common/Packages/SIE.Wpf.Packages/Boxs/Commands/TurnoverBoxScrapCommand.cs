using SIE.Packages.Boxs;
using SIE.Wpf.Command;

namespace SIE.Wpf.Packages.Boxs
{
    /// <summary>
    /// 报废周转箱命令 
    /// </summary>
    [Command(Label = "报废", ToolTip = "报废周转箱", GroupType = 30)]
    public class TurnoverBoxScrapCommand : ListViewCommand
    {
        /// <summary>
        /// 判断报废命令能否执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <returns>能执行返回true，不能执行返回false</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var current = view.Current as TurnoverBox;
            return current != null && current.State != BoxState.Scrap && current.State != BoxState.Inuse;
        }

        /// <summary>
        /// 报废命令执行方法
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            var box = view.Current as TurnoverBox;
            if (box != null)
            {
                box.State = BoxState.Scrap;

                ////RF.Save(box);
            }
        }
    }
}
