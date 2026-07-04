SIE.defineCommand('SIE.Web.EMS.SpareParts.Commands.ResetSparePartStoreDetailsCommand', {
    extend: 'SIE.cmd.FormSave',
    meta: { text: "清空", group: "edit", iconCls: "icon-Reload icon-blue" },
    canExecute: function (view) {
        return true;
    },
    execute: function (view, source) {
        var entity = view.getCurrent();

        entity.setSparePartId(null);
        entity.setSparePartId_Display("");
        entity.setSparePartName("");
        entity.setControlMethod("");
        entity.setIsReplacement("");
        entity.setQualityStatus("");
        entity.setNumber("");
        entity.setCanReturnQty("");
        entity.setUnitPrice("");
        entity.setStorageLocationId(null);
        entity.setStorageLocationId_Display("");
        entity.setPartOutDepotDetailId(null);
        entity.setPartOutDepotDetailId_Display("");
        entity.setIsCreateNewLabel(false);
        entity.setScanValue("");
        entity.setIsSelectSparePart(false);

        if (view.SparePartStoreComp)
            view.SparePartStoreComp.setValue("");

        if (!entity.data.IsExistDetail) {
            entity.setStorePartType(null);
            entity.setWarehouseId(null);
            entity.setWarehouseId_Display("");
            entity.setMessage("请先维护【拆机件/原件】/【仓库】！".t());
        }
    }
});