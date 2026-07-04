SIE.defineCommand('SIE.Web.EMS.SpareParts.Commands.SparePartStoreCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "入库", group: "edit", iconCls: "icon-TextEdit icon-blue" },
    canExecute: function (view) {
        if (view.getSelection() == null) {
            return false;
        }
        return view.getSelection().length == 1
            && (view.getSelection()[0].data.InboundStatus == 10 || view.getSelection()[0].data.InboundStatus == 20)
            && view.getSelection()[0].data.InboundType != 90;
    },
    execute: function (view, source) {

        var me = this;
        var editEntity = me.getEditEntity();

        CRT.Workbench.addPage({
            entityType: me.view.model,
            recordId: editEntity.data.Id,
            title: '入库-备件入库'.t(),
            viewGroup: "StoreDetailsViewGroup",
            isDetail: true,
            ignoreQuery: true,
            token: view.token,
            params: {
                Id: editEntity.data.Id,
                StoreCode: editEntity.data.StoreCode,
                InboundType: editEntity.data.InboundType,
                WarehouseId: editEntity.data.WarehouseId,
                WarehouseId_Display: editEntity.data.WarehouseId_Display,
                QualityStatus: editEntity.data.QualityStatus,
                CreateDate: editEntity.data.CreateDate,
            }
        });
    }
});