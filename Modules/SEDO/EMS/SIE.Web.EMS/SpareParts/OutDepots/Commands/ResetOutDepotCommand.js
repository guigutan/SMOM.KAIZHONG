SIE.defineCommand('SIE.Web.EMS.SpareParts.OutDepots.Commands.ResetOutDepotCommand', {
    extend: 'SIE.cmd.FormSave',
    meta: { text: "清空", group: "edit", iconCls: "icon-Reload icon-blue" },
    canExecute: function (view) {
        return true;
    },
    execute: function (view, source) {
        var entity = view.getCurrent();

        entity.setIsBarcode(true);
        entity.setScanValue("");
        entity.setGetDepartmentId(null);
        entity.setGetDepartmentId_Display("");
        entity.setSparePartId(null);
        entity.setSparePartId_Display("");
        entity.setSparePartName("");
        entity.setControlMethod("");
        entity.setStorageLocationId(null);
        entity.setStorageLocationId_Display("");
        entity.setStorageLocationNum("");
        entity.setAdviceStorageLocation("");

        if (view.outDepotComp)
            view.outDepotComp.setValue("");

        if (!entity.data.IsExistDetail)
        {
            entity.setWarehouseId(null);
            entity.setWarehouseId_Display("");
            entity.setQualityStatus(null);
            entity.setMessage("请先维护【出库仓库】/【质量状态】！".t());
        }
    }
});