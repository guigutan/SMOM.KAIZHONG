SIE.defineCommand('SIE.Web.Warehouses.Commands.StorageLocationDisableCommand', {
    extend: 'SIE.cmd.Disable',
    meta: { text: "禁用", group: "business", iconCls: "icon-NetworkError icon-red" },

    canExecute: function (view) {
        var storageLocationlist = view.getSelection();
        if (storageLocationlist == null || storageLocationlist.length == 0) {
            var storageLocation = view.getCurrent();
            if (storageLocation == null || view.getData().isDirty() || storageLocation.data.State != 1 || storageLocation.data.AreaState != 1) {
                return false;
            }
        } else {
            //if (view.Control.View.AllowEditing == true) {
            //    return false;
            //}
            if (view.getData().isDirty())
                return false;
            for (var i = 0; i < storageLocationlist.length; i++) {
                if (storageLocationlist[i].data.AreaState == 0 || storageLocationlist[i].data.State == 0)
                    return false;
            }
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