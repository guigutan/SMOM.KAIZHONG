SIE.defineCommand('SIE.Web.Inventory.Task.Commands.ReleaseTaskManagementCommand', {
    meta: { text: "释放", group: "edit", iconCls: "icon-TableEdit icon-blue" },
    extend: 'SIE.cmd.Edit',
    canExecute: function (view) {
        if (view.getSelection() != null || view.getSelection().length != 0) {
            var taskList = view.getSelection();
            if (view.getData().isDirty()) {
                return false;
            }

            var taskList = view.getSelection();
            if (taskList.any(function(p){ return p.getState() != SIE.Inventory.Task.TaskState.Create.value && p.getState() != SIE.Inventory.Task.TaskState.Appoint.value &&
                p.getState() != SIE.Inventory.Task.TaskState.Executing.value && p.getState() != SIE.Inventory.Task.TaskState.Frozen.value &&
                p.getState() != SIE.Inventory.Task.TaskState.Abnormal.value;
            }))
                return false;
        }

        return true;
    },
    execute: function (view, source) {
        view.execute({
            data: view.getCurrent().data,
            withIds: true,
            selectIds: view.getSelectionIds(),
            success: function (res) { //回调
                view.reloadData();
            }
        });
    }
});