using SIE.Domain;
using SIE.MES.Storages;
using SIE.MetaModel.View;
using SIE.Wpf.Command;

namespace SIE.Wpf.MES.Storages.Commands
{
    /// <summary>
    /// 货位物料的删除命令
    /// </summary>
    [Command(ImageName = "DeleteEntity",
    Label = "删除",
    ToolTip = "删除数据",
    Gestures = "Delete",
    Location = CommandLocation.All,
    GroupType = CommandGroupType.Edit)]
    public class DeleteItemStorageCommand : ListDeleteCommand
    {
        /// <summary>
        /// 是否能执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <returns>返回true代表可以执行，否则返回false</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            return base.CanExecute(view) && view.Current != null && (view.Current as ItemStorage).Qty == 0;
        }

        /// <summary>
        ///  货位物料的删除命令执行方法
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            base.Execute(view);
            if (view.Parent.Current.PersistenceStatus == PersistenceStatus.Unchanged)
                view.Parent.Current.PersistenceStatus = PersistenceStatus.Modified;

            var storageArea = view.Parent.Parent.Current as StorageArea;
            if (storageArea.PersistenceStatus == PersistenceStatus.Unchanged)
                storageArea.PersistenceStatus = PersistenceStatus.Modified;
        }
    }
}