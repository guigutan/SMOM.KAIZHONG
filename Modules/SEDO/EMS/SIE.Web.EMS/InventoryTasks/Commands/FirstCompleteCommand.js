SIE.defineCommand('SIE.Web.EMS.InventoryTasks.Commands.FirstCompleteCommand', {
    meta: { text: "初盘完成", group: "edit", iconCls: "icon-Check icon-blue" },
    canExecute: function (view) {
        var selectModels = view.getSelection();
        if (selectModels.length == 0) return false;
        var res = true;
        SIE.each(selectModels, function (model) {
            if (model.data.InventoryTaskStatus !== 20) {
                res = false;
                return false;
            }
        });
        return res;
    },
    execute: function (view, source) {
        var selectModels = view.getSelection();
        var selectIds = view.getSelectionIds(selectModels);
        view.execute({
            withIds: true,
            selectIds: selectIds,
            success: function (res) {
                SIE.Msg.showMessage("初盘完成!".t());
                view.reloadData();
            }
        });
    }
});