SIE.defineCommand('SIE.Web.Warehouses.Commands.StorageAreaDisableCommand', {
    extend: 'SIE.cmd.Disable',
    meta: { text: "禁用", group: "business", iconCls: "icon-NetworkError icon-red" },

    canExecute: function (view) {
        var warehouselist = view.getSelection();
        if (warehouselist == null || warehouselist.length == 0) {
            return false;
        }

        var warehouse = view.getCurrent();
        if (warehouse == null) {
            return false;
        }

        for (var i = 0; i < warehouselist.length; i++) {
            if (warehouselist[i].data.State != warehouse.data.State) return false;
        }


        if (warehouse.data.State == 0 || view.getData().isDirty()) {
            return false;
        }
        return true;
    },
    execute: function (view, source) {
        SIE.Msg.askQuestion(Ext.String.format('确定禁用选中的资料?'.t()), function () {
            view.execute({
                data: view.getCurrent().data,
                withIds: true,
                selectIds: view.getSelectionIds(),
                success: function (res) { //回调
                    view.reloadData();
                }
            });
        });
    }
});