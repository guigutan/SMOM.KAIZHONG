using SIE.Domain;
using SIE.Warehouses;
using SIE.Wpf.Command;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Wpf.Warehouses.Command
{
    /// <summary>
    /// 库区启用命令
    /// </summary>
    [Command(ImageName = "Play", Label = "启用", ToolTip = "启用", GroupType = CommandGroupType.Business)]
    public class StorageAreaEnableCommand : ListViewCommand
    {
        /// <summary>
        /// 命令是否可执行
        /// </summary>
        /// <param name="view">列表视图</param>
        /// <returns>true/false</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            StorageArea storagearea = null;
            if (view != null && view.Current != null)
            {
                storagearea = view.Current as StorageArea;
                var storagesrealist = view.SelectedEntities.OfType<StorageArea>().ToList();
                foreach (var item in storagesrealist)
                {
                    if (item.State != storagearea.State) return false;
                }

                return storagearea != null && storagearea.State != State.Enable && storagearea.PersistenceStatus == PersistenceStatus.Unchanged;
            }
            return false;
        }

        /// <summary>
        /// 命令是否可执行
        /// </summary>
        /// <param name="view">列表视图</param>
        public override void Execute(ListLogicalView view)
        {
            if (view != null && view.Current != null)
            {
                if (CRT.MessageService.AskQuestion("确定启用选中的资料?".L10N()))
                {
                    IEnumerable<StorageArea> selected = view.SelectedEntities.OfType<StorageArea>();
                    List<double> idlist = selected.Select(p => p.Id).ToList(); //库区Id列表
                    RT.Service.Resolve<WarehouseController>().EnabelStorageAreas(idlist);
                }
            }
        }
    }

    /// <summary>
    /// 库区禁用命令
    /// </summary>
    [Command(ImageName = "Cancel", Label = "停用", ToolTip = "停用", GroupType = CommandGroupType.Business)]
    public class StorageAreaDisableCommand : ListViewCommand
    {
        /// <summary>
        /// 命令是否可执行
        /// </summary>
        /// <param name="view">列表视图</param>
        /// <returns>true/false</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            StorageArea storagearea = null;
            if (view != null && view.Current != null)
            {
                storagearea = view.Current as StorageArea;
                var storagearealist = view.SelectedEntities.OfType<StorageArea>().ToList();
                foreach (var item in storagearealist)
                {
                    if (item.State != storagearea.State) return false;
                }

                return storagearea != null && storagearea.State != State.Disable && storagearea.PersistenceStatus == PersistenceStatus.Unchanged;
            }
            return false; 
        }

        /// <summary>
        /// 命令是否可执行
        /// </summary>
        /// <param name="view">列表视图</param>
        public override void Execute(ListLogicalView view)
        {
            if (view != null && view.Current != null)
            {
                if (CRT.MessageService.AskQuestion("确定禁用选中的资料?".L10N()))
                {
                    IEnumerable<StorageArea> selected = view.SelectedEntities.OfType<StorageArea>();
                    List<double> idlist = selected.Select(p => p.Id).ToList(); //库区Id列表  

                    RT.Service.Resolve<WarehouseController>().DisableStorageAreas(idlist);
                }
            }
        }
    }
}