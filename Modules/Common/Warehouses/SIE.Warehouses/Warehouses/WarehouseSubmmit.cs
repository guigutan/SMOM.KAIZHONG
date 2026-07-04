using SIE.Domain;
using SIE.Domain.Validation;
using System;

namespace SIE.Warehouses.Common
{
    /// <summary>
    /// 仓库保存后需要保存仓库基本资料
    /// </summary>
    [System.ComponentModel.DisplayName("仓库保存后需要保存仓库基本资料")]
    [System.ComponentModel.Description("仓库保存后需要保存仓库基本资料")]
    public class WarehouseSubmmit : OnSubmitted<Warehouse>
    {
        /// <summary>
        /// 保存仓库后执行
        /// </summary>
        /// <param name="entity">仓库实体</param>
        /// <param name="e">实体提交参数</param>
        protected override void Invoke(Warehouse entity, EntitySubmittedEventArgs e)
        {
            if (e.Action == SubmitAction.Insert || e.Action == SubmitAction.Update)
            {
                WarehouseInfo warehouseInfo = entity.GetProperty(WarehouseInfoDetailProperty.WarehouseInfoProperty);
                if (warehouseInfo == null)
                {
                    warehouseInfo = RT.Service.Resolve<WarehouseController>().GetWarehouseInfoDetail(entity.Id);
                    if (warehouseInfo != null)
                    {
                        return;
                    }

                    warehouseInfo = new WarehouseInfo() { Warehouse = entity, WarehouseId = entity.Id };
                }
                if (warehouseInfo.PersistenceStatus != PersistenceStatus.Unchanged)
                {
                    warehouseInfo.WarehouseId = entity.Id;
                    RF.Save(warehouseInfo);
                    warehouseInfo.MarkSaved();
                }
            }

            if (e.Action == SubmitAction.Insert)
            {
                try
                {
                    StorageArea storagearea = new StorageArea();
                    storagearea.Code = Warehouse.STAGE;
                    storagearea.Name = Warehouse.STAGE;
                    storagearea.LibraryType = LibraryType.Fictitious;
                    storagearea.WarehouseId = entity.Id;
                    RF.Save(storagearea);
                    storagearea.MarkSaved();

                    StorageArea storageareapackto = new StorageArea();
                    storageareapackto.Code = Warehouse.PICKTO;
                    storageareapackto.Name = Warehouse.PICKTO;
                    storageareapackto.LibraryType = LibraryType.Fictitious;
                    storageareapackto.WarehouseId = entity.Id;
                    RF.Save(storageareapackto);
                    storageareapackto.MarkSaved();

                    StorageLocation location = new StorageLocation();
                    location.Code = Warehouse.STAGE;
                    location.Name = Warehouse.STAGE;
                    location.LibraryType = LibraryType.Fictitious;
                    location.AreaId = storagearea.Id;
                    location.WarehouseId = entity.Id;
                    RF.Save(location);
                    location.MarkSaved();

                    StorageLocation locationpackto = new StorageLocation();
                    locationpackto.Code = Warehouse.PICKTO;
                    locationpackto.Name = Warehouse.PICKTO;
                    locationpackto.LibraryType = LibraryType.Fictitious;
                    locationpackto.AreaId = storageareapackto.Id;
                    locationpackto.WarehouseId = entity.Id;
                    RF.Save(locationpackto);
                    locationpackto.MarkSaved();
                    RT.Service.Resolve<WarehouseController>().InsertWarehouseAdminUser(entity.Id);
                }
                catch (Exception ex)
                {
                    throw new ValidationException(AppRuntime.Location.ConnectDataDirectly ? ex.Message : ex.InnerException.Message);
                }
            }
        }
    }
}
