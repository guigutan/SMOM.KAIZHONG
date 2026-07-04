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
    public class WarehouseSaveCommand : ListSaveCommand
    {
        /// <summary>
        /// 库区保存命令
        /// </summary>
        /// <param name="view"></param>
        protected override void OnSaving(ListLogicalView view)
        {
            List<Warehouse> selectList = view.SelectedEntities.OfType<Warehouse>().Where(p => p.PersistenceStatus == PersistenceStatus.New).ToList();
            RT.Service.Resolve<WarehouseController>().WarehouseValidateSegment(selectList);

            base.OnSaving(view);
        }
    }
}
