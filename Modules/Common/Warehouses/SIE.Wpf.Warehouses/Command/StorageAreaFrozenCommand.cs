using SIE.Domain;
using SIE.Warehouses;
using SIE.Wpf.Command;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Wpf.Warehouses.Command
{
    /// <summary>
    /// 库区更改冻结状态
    /// </summary>
    [Command(ImageName = "TableEdit",
            Label = "冻结/释放",
            ToolTip = "冻结/释放",
            GroupType = CommandGroupType.Business)]
    public class StorageAreaFrozenCommand : ListViewCommand
    {
        /// <summary>
        /// 是否能执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <returns>返回true代表可以执行，否则返回false</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var storagearea = view.Current as StorageArea;
            if (view.SelectedEntities.Any())
            {
                var storagearealist = view.SelectedEntities.OfType<StorageArea>().ToList();
                foreach (var item in storagearealist)
                {
                    if (item.IsFrozen != storagearea.IsFrozen || storagearea.PersistenceStatus != PersistenceStatus.Unchanged) return false;
                }

                return true;
            }
            else
            {
                return storagearea != null && storagearea.PersistenceStatus == PersistenceStatus.Unchanged && !storagearea.Warehouse.IsFrozen;
            }
        }

        /// <summary>
        /// 执行具体的逻辑。
        /// </summary>
        /// <param name="view">此命令对应的视图对象。</param>
        public override void Execute(ListLogicalView view)
        {
            try
            {
                var storagearea = view.Current as StorageArea;
                if (!storagearea.IsFrozen)
                {
                    if (!CRT.MessageService.AskQuestion("冻结库区后，该库区下的物料将不能进行出入库操作，正在操作的任务在解冻前不能保存，是否继续？".L10N()))
                        return;
                }

                IEnumerable<StorageArea> selected = view.SelectedEntities.OfType<StorageArea>();
                List<double> idlist = selected.Select(p => p.Id).ToList(); //库区Id列表
                RT.Service.Resolve<WarehouseController>().FrozenStorageAreas(idlist);
            }
            catch (Exception exc)
            {
                CRT.MessageService.ShowError(exc.Message);
            }
        }
    }
}