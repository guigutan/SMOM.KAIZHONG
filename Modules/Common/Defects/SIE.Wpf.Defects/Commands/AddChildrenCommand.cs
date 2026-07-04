using SIE.Wpf.Command;
using System.Linq;

namespace SIE.Wpf.Defects.Commands
{
    /// <summary>
    /// 添加子缺陷责任分类命令
    /// </summary>
    [Command(ImageName = "FileTree", Label = "添加子", Location = MetaModel.View.CommandLocation.All, GroupType = 10)]
    class AddChildrenCommand : TreeAddChildCommand
    {
        /// <summary>
        /// 是否可执行
        /// </summary>
        /// <param name="view">视图</param>
        /// <returns>bool</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            return base.CanExecute(view) && View.SelectedEntities.Any();
        }
    }
}