using SIE.Common;
using SIE.Items;
using SIE.Wpf.Command;

namespace SIE.Wpf.Items
{
    /// <summary>
    /// 删除物料命令
    /// </summary>
    [Command(ImageName = "DeleteEntity", Label = "删除", GroupType = CommandGroupType.Edit)]
    public class ItemDeleteCommand : ListDeleteCommand
    {
        /// <summary>
        /// 是否执行命令
        /// </summary>
        /// <param name="view">List逻辑视图</param>
        /// <returns>bool</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var item = view.Current as Item;
            return item != null && item.SourceType != SourceType.External && view.SelectedEntities.Count > 0;
        }
    }
}
