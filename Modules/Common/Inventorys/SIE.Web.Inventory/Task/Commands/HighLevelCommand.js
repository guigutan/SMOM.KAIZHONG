SIE.defineCommand('SIE.Web.Inventory.Task.Commands.HighLevelCommand', {
    meta: { text: "高", group: "edit", hierarchy: "设置优先级", iconCls: "icon-TableEdit icon-blue" },
    extend: 'SIE.cmd.Edit',
    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length != 1)
            return false;
        if (view.getCurrent().isDirty()) {
            return false;
        }
        var task = view.getCurrent();
        if (task.data.State == SIE.Inventory.Task.TaskState.Create.value || task.data.State == SIE.Inventory.Task.TaskState.Release.value
            || task.data.State == SIE.Inventory.Task.TaskState.Appoint.value || task.data.State == SIE.Inventory.Task.TaskState.Frozen.value || task.data.State == SIE.Inventory.Task.TaskState.Abnormal.value)
            return true;
        return false;
    },
    execute: function (view, source) {
        SIE.Msg.wait("操作中......".t());
        view.execute({
            data: view.getCurrent().data,
            withIds: true,
            selectIds: view.getSelectionIds(),
            success: function (res) { //回调
                SIE.Msg.showInstantMessage("操作完成".t());
                view.reloadData();
            },
            error: function (res) {
                SIE.Msg.showMessage(res.Message);
            },
        });
    }
});