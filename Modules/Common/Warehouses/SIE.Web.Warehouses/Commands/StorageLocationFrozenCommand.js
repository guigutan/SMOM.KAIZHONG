SIE.defineCommand('SIE.Web.Warehouses.Commands.StorageLocationFrozenCommand', {
    meta: { text: "冻结/释放", group: "edit", iconCls: "icon-TableEdit icon-blue" },
    extend: 'SIE.cmd.Edit',
    canExecute: function (view) {
        var storageLocationlist = view.getSelection();
        if (storageLocationlist == null || storageLocationlist.length == 0) {
            var storageLocation = view.getCurrent();
            if (storageLocation == null || view.getData().isDirty() || storageLocation.data.AreaIsFrozen || storageLocation.data.WarehouseIsFrozen) {
                return false;
            }
        } else {
            if (view.getData().isDirty())
                return false;
            for (var i = 0; i < storageLocationlist.length; i++) {
                if (storageLocationlist[i].data.AreaIsFrozen || storageLocationlist[i].data.WarehouseIsFrozen)
                    return false;
            }
        }
        return true;
    },
    execute: function (view, source) {
        var storageLocation = view.getCurrent();
        if (storageLocation == null) {
            return false;
        }
        if (storageLocation.data.IsFrozen == false) {
            SIE.Msg.askQuestion(Ext.String.format('冻结库位后，该库位下的物料将不能进行出入库操作，正在操作的任务在解冻前不能保存，是否继续？'.t()), function () {
                view.execute({
                    data: view.getCurrent().data,
                    withIds: true,
                    selectIds: view.getSelectionIds(),
                    success: function (res) { //回调
                        view.reloadData();
                    }
                });
            });
        } else {
            view.execute({
                data: view.getCurrent().data,
                withIds: true,
                selectIds: view.getSelectionIds(),
                success: function (res) { //回调
                    view.reloadData();
                }
            });
        }
    }
});