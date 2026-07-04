using SIE.Domain;
using SIE.Warehouses;
using SIE.Wpf.Command;
using System.Linq;

namespace SIE.Wpf.Warehouses.Command
{
    /// <summary>
    /// 库区删除命令
    /// </summary>
    [Command(ImageName = "DeleteEntity", Label = "删除", ToolTip = "删除", GroupType = CommandGroupType.Edit)]
    public class StorageAreaDeleteCommand : ListDeleteCommand
    {
        /// <summary>
        /// 子类重写此方法来返回是否可执行的逻辑
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <returns>是否可执行</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            if (!view.SelectedEntities.Any())
            {
                StorageArea storagearea = view.Current as StorageArea;
                return storagearea != null && (storagearea.State == State.Disable || storagearea.PersistenceStatus == PersistenceStatus.New);
            }
            else
            {
                return view.SelectedEntities.OfType<StorageArea>().Count(p => p.PersistenceStatus == PersistenceStatus.New || p.State == State.Disable) == view.SelectedEntities.Count;
            }
        }
    }
}
