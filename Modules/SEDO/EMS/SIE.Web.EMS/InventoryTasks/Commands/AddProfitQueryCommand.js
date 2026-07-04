SIE.defineCommand('SIE.Web.EMS.InventoryTasks.Commands.AddProfitQueryCommand', {
    meta: { text: "查询", group: "edit", iconCls: "icon-Search icon-blue" },
    canExecute: function (view) {
        var p = view.getCurrent();
        if (p == null) return false;
        if (p.data.EquipmentCode.length == 0) return false;
        return true;
    },
    execute: function (view, source) {
        var entity = view.getCurrent();
        SIE.invokeDataQuery({
            method: 'AddProfitQuery',
            params: [entity.data.EquipmentCode],
            action: 'queryer',
            type: 'SIE.Web.EMS.InventoryTasks.InventoryTaskDataQueryer',
            token: view.token,
            success: function (res) {
                if (res.Result == null) {
                    //获取不到设备台账，界面为B状态，管理部门带出盘点任务的管理部门，其他字段为空
                    entity.setAddProfitUIState(20);
                    entity.setManageDeptName(entity.data.TaskManageDeptName);
                    entity.setEquipmentName(null);
                    entity.setAccountUseState(null);
                    entity.setAccountState(null);
                    entity.setTypeCategory(null);
                    entity.setEquipTypeId(null);
                    entity.setEquipModelId(null);
                    entity.setEquipModelName(null);
                    entity.setSpecifications(null);
                    entity.setUseLevel(null);
                    entity.setUseDeptId(null);
                    entity.setUserId(null);
                    entity.setRealWorkShopId(null);
                    entity.setRealResourceId(null);
                    entity.setRealWarehouseId(null);
                    entity.setStorageLocationId(null);
                    entity.setRealLocation(null);
                    entity.setPhotoFilePath(null);
                } else {
                    //能够获取到时，界面为A状态，下面的字段带出设备台账的字段值
                    var info = res.Result.data.items[0].data;
                    entity.setAddProfitUIState(10);
                    entity.setManageDeptName(info.ManageDepartmentName);
                    entity.setEquipmentCode(info.Code);
                    entity.setEquipmentName(info.Name);
                    entity.setAccountUseState(info.UseState);
                    entity.setAccountState(info.State);
                    entity.setTypeCategory(info.TypeCategory);
                    entity.setEquipTypeId_Display(info.EquipTypeCode);
                    entity.setEquipTypeId(info.EquipTypeViewId);
                    entity.setEquipModelId_Display(info.EquipModelId_Display);
                    entity.setEquipModelId(info.EquipModelId);
                    entity.setEquipModelName(info.ModelName);
                    entity.setSpecifications(info.Specifications);
                    entity.setUseLevel(info.UseLevel);
                    entity.setUseDeptId_Display(info.UseDepartmentId_Display);
                    entity.setUseDeptId(info.UseDepartmentId);
                    entity.setUserId_Display(info.UserId_Display);
                    entity.setUserId(info.UserId);
                    entity.setRealWorkShopId_Display(info.WorkShopId_Display);
                    entity.setRealWorkShopId(info.WorkShopId);
                    entity.setRealResourceId_Display(info.ResourceId_Display);
                    entity.setRealResourceId(info.ResourceId);
                    entity.setRealWarehouseId_Display(info.WarehouseId_Display);
                    entity.setRealWarehouseId(info.WarehouseId);
                    entity.setStorageLocationId_Display(info.StorageLocationId_Display);
                    entity.setStorageLocationId(info.StorageLocationId);
                    entity.setRealLocation(info.InstallationLocation);
                    entity.setPhotoFilePath(null);
                }
            }
        });
    }
});