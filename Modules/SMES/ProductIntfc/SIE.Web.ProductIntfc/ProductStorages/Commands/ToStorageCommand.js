SIE.defineCommand('SIE.Web.ProductIntfc.ProductStorages.Commands.ToStorageCommand', {
    meta: { text: "入库", group: "edit", iconCls: "icon-WarehouseImport icon-blue" },
    executeIntervalMode: SIE.cmd.IntervalMode.Debounce.value,
    canExecute: function (view) {
        this.selectItems = view.getSelectedEntities();
        return this.selectItems.length > 0;
    },
    execute: function (view, source) {
        var me = view;
        var selectModels = view.getSelection();
        var selectIds = view.getSelectionIds(selectModels);
        SIE.Msg.wait("正在入库......".t());
        view.execute({
            withIds: true,
            selectIds: selectIds,
            success: function (res) {
                var errMsg = res.Result;
                if (errMsg == '入库成功') {
                    view.getParent().reloadData();
                    view.getParent().findChild("SIE.ProductIntfc.ProductStorages.InStorageBill").getParent().reloadData();
                }

                SIE.Msg.showMessage(errMsg);
                //SIE.Msg.close();
            }
        });
    }
});