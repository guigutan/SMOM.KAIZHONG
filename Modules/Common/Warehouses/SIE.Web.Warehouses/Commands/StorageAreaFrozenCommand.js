SIE.defineCommand('SIE.Web.Warehouses.Commands.StorageAreaFrozenCommand', {
    meta: { text: "冻结/释放", group: "edit", iconCls: "icon-TableEdit icon-blue" },
    extend: 'SIE.cmd.Edit',
    canExecute: function (view) {
        var storagearea = view.getCurrent();
        if (storagearea == null) {
            return false;
        }
        if (view.getSelection() != null || view.getSelection().length != 0) {
            var storagearealist = view.getSelection();
            if (view.getData().isDirty())
                return false;
            for (var i = 0; i < storagearealist.length; i++) {
                if (storagearealist[i].data.IsFrozen != storagearea.data.IsFrozen || storagearea.IsWarehouseFrozen == false)
                    return false;
            }
        }
        else {
            if (storagearea == null || view.getData().isDirty()) {
                return false
            }
        }
        return true;
    },
    execute: function (view, source) {
        var storagearea = view.getCurrent();
        if (storagearea == null) {
            return false;
        }
        if (storagearea.data.IsFrozen == false) {
            SIE.Msg.askQuestion(Ext.String.format('冻结库区后，该库区下的物料将不能进行出入库操作，正在操作的任务在解冻前不能保存，是否继续？'.t()), function () {
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