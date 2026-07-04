SIE.defineCommand('SIE.Web.Inventory.Task.Commands.FrozenTaskManagementCommand', {
    meta: { text: "冻结", group: "edit", iconCls: "icon-TableEdit icon-blue" },
    extend: 'SIE.cmd.Edit',
    canExecute: function (view) {
        if (view.getSelection() != null || view.getSelection().length != 0) {
            var taskList = view.getSelection();
            if (view.getData().isDirty()) {
                return false;
            }
            var list = [];
            for (var i = 0; i < taskList.length; i++) {
                if (taskList[i].data.State != SIE.Inventory.Task.TaskState.Create.value && taskList[i].data.State != SIE.Inventory.Task.TaskState.Appoint.value
                && taskList[i].data.State != SIE.Inventory.Task.TaskState.Release.value) {
                    return false;
                }
                if (list.indexOf(taskList[i].data.State) == -1) {
                    list.push(taskList[i].data.State);
                }
            }
            if (list.length > 1)
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