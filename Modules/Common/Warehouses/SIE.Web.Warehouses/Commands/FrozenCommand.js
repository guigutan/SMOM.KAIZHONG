SIE.defineCommand('SIE.Web.Warehouses.Commands.FrozenCommand', {
    meta: { text: "冻结/释放", group: "edit", iconCls: "icon-TableEdit icon-blue" },
    extend: 'SIE.cmd.Edit',
    canExecute: function (view) {
        var warehouse = view.getCurrent();
        if (warehouse == null) {
            return false;
        }
        if (view.getSelection() != null || view.getSelection().length != 0) {
            var warehouselist = view.getSelection();
            if (view.getData().isDirty()) {
                return false;
            }
            for (var i = 0; i < warehouselist.length; i++) {
                if (warehouselist[i].data.IsFrozen != warehouse.data.IsFrozen)
                    return false;
            }
        }
        else {
            if (warehouse == null || view.getData().isDirty()) {
                return false
            }
        }
        return true;
    },
    execute: function (view, source) {
        var warehouse = view.getCurrent();
        if (warehouse == null) {
            return false;
        }
        if (warehouse.data.IsFrozen == false) {
            SIE.Msg.askQuestion(Ext.String.format('冻结仓库后，该仓库下的物料将不能进行出入库操作，正在操作的任务在解冻前不能保存，是否继续？'.t()), function () {
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