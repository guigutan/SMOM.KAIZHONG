SIE.defineCommand('SIE.Web.EMS.SpareParts.OutDepotHandovers.Commands.ResetScanHandoverDetailCommand', {
    extend: 'SIE.cmd.FormSave',
    meta: { text: "清空", group: "edit", iconCls: "icon-Reload icon-blue" },
    execute: function (view, source) {
        var entity = view.getCurrent();

        entity.setOutDepotHandoverBillId(null);
        entity.setOutDepotHandoverBillId_Display(null);
        entity.setOutDepotNo(null);
        entity.setIsSelectSparePart(false);
        entity.setSparePartId(null);
        entity.setSparePartId_Display(null);
        entity.setSparePartName(null);
        entity.setControlMethod(null);
        entity.setBarcode(null);
        entity.setQty(null);
        entity.setReceiveQty(null);
        entity.setMessage("请先维护【交接单号】！".t());

        var childStore = view.findChild('SIE.EMS.SpareParts.OutDepotHandovers.OutDepotHandoverDetail').getData();
        childStore.removeAll();
    }
});