using SIE.Domain;
using SIE.Warehouses;
using SIE.Wpf.Command;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Wpf.Warehouses.Command
{
    /// <summary>
    /// 库区保存命令
    /// </summary>
    [Command(ImageName = "SaveEntity", Label = "保存", ToolTip = "保存", Gestures = "Ctrl+S", GroupType = 10)]
    public class StorageAreaSaveCommand : ListSaveCommand
    {
        /// <summary>
        /// 库区保存命令
        /// </summary>
        /// <param name="view"></param>
        protected override void OnSaving(ListLogicalView view)
        {
            List<StorageArea> selectList = view.SelectedEntities.OfType<StorageArea>().Where(p => p.PersistenceStatus == PersistenceStatus.New).ToList();
            RT.Service.Resolve<WarehouseController>().StorageAreaValidateSegment(selectList);

            base.OnSaving(view);
        }
    }
}
