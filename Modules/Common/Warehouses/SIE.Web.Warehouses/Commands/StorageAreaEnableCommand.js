SIE.defineCommand('SIE.Web.Warehouses.Commands.StorageAreaEnableCommand', {
    extend: 'SIE.cmd.Enable',
    meta: { text: "启用", group: "business", iconCls: "icon-NetworkNormal icon-green" },

    canExecute: function (view) {
        var storagearealist = view.getSelection();
        if (storagearealist == null || storagearealist.length == 0) {
            return false;
        }

        var storagearea = view.getCurrent();
        if (storagearea == null) {
            return false;
        }

        for (var i = 0; i < storagearealist.length; i++) {
            if (storagearealist[i].data.State != storagearea.data.State) return false;
        }

        if (storagearea.data.State == 1 || view.getData().isDirty()) {
            return false;
        }
        return true;
    },
    execute: function (view, source) {
        SIE.Msg.askQuestion(Ext.String.format('确定启用选中的资料?'.t()), function () {
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