using SIE.Wpf.Command;

namespace SIE.Wpf.Items
{
    /// <summary>
    /// 全不选
    /// </summary>
    [Command(ImageName = "Delete", Label = "全不选")]
    public class UnSelectedAllCommand : ListViewCommand
    {
        /// <summary>
        /// 判断是否可执行
        /// </summary>
        /// <param name="view">List逻辑视图</param>
        /// <returns>bool</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            return base.CanExecute(view);
        }

        /// <summary>
        /// 命令执行代码块
        /// </summary>
        /// <param name="view">List逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            view.Data.Clear();
        }
    }
}
