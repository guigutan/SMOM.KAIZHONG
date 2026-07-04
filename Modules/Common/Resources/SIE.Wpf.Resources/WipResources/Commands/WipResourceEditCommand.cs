using SIE.Wpf.Command;

namespace SIE.Wpf.Resources.WipResources.Commands
{
    /// <summary>
    /// 生产资源修改命令
    /// </summary>
    [Command(ImageName = "EditEntity", Label = "修改", GroupType = CommandGroupType.Edit)]
    public class WipResourceEditCommand : ListEditCommand
    {
        /// <summary>
        /// 片断修改命令能否执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <returns>能执行返回true，不能执行返回false</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            return base.CanExecute(view);
        }
    }
}
