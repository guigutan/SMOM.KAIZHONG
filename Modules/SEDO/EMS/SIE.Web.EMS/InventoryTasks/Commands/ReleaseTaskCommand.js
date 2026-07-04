SIE.defineCommand('SIE.Web.EMS.InventoryTasks.Commands.ReleaseTaskCommand', {
    meta: { text: "下达", group: "edit", iconCls: "icon-ArrowWithCircleDown icon-green" },
    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length !== 1) {
            return false;
        }
        var p = view.getCurrent();
        if (p == null) return false;
        if (p.data.InventoryTaskStatus !== 10) return false;
        return true;
    },
    execute: function (view, source) {
        var selectModels = view.getSelection();
        var selectIds = view.getSelectionIds(selectModels);
        view.execute({
            withIds: true,
            selectIds: selectIds,
            success: function (res) {
                SIE.Msg.showMessage("下达成功!".t());
                view.reloadData();
            }
        });
    }
});