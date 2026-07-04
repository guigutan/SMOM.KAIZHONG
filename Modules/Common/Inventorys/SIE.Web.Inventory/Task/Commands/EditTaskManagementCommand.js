SIE.defineCommand('SIE.Web.Inventory.Task.Commands.EditTaskManagementCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", iconCls: "icon-EditEntity icon-blue" },

    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length != 1) {
            return false;
        }
        var task = view.getCurrent();
        if (task != null && (task.data.State == SIE.Inventory.Task.TaskState.Create.value || task.data.State == SIE.Inventory.Task.TaskState.Release.value
                || task.data.State == SIE.Inventory.Task.TaskState.Appoint.value || task.data.State == SIE.Inventory.Task.TaskState.Frozen.value || task.data.State == SIE.Inventory.Task.TaskState.Abnormal.value))
            return true;
        return false;
    }
});