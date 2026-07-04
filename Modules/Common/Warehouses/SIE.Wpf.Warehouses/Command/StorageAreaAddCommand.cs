using SIE.Domain;
using SIE.Warehouses;
using SIE.Wpf.Command;

namespace SIE.Wpf.Warehouses.Command
{
    /// <summary>
    /// 库区添加命令
    /// </summary>
    [Command(ImageName = "AddEntity", Label = "添加", ToolTip = "添加数据", Gestures = "Ctrl+Shift+N", GroupType = 10)]
    public class StorageAreaAddCommand : ListAddCommand
    {
        /// <summary>
        /// 重写创建实体后方法
        /// </summary>
        /// <param name="entity">Entity</param>
        protected override void OnItemCreated(Entity entity)
        {
            var warehousearea = entity as StorageArea;
            warehousearea.Code = RT.Service.Resolve<WarehouseController>().GetStorageAreaCode();
            base.OnItemCreated(entity);
        }
    }
}
