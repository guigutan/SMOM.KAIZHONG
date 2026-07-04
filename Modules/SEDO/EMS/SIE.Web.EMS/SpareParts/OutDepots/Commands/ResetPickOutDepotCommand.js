SIE.defineCommand('SIE.Web.EMS.SpareParts.OutDepots.Commands.ResetPickOutDepotCommand', {
    extend: 'SIE.cmd.FormSave',
    meta: { text: "清空", group: "edit", iconCls: "icon-Reload icon-blue" },
    canExecute: function (view) {
        return true;
    },
    execute: function (view, source) {
        var entity = view.getCurrent();

        entity.setIsBarcode(true);
        entity.setScanValue("");
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

        entity.setMessage("请扫描序列号/批次号/备件编码！".t());
    }
});