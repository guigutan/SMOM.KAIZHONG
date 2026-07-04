SIE.defineCommand('SIE.Web.Inventory.Task.Commands.DeleteOperatorCommand', {
    extend: 'SIE.cmd.Delete',
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },

    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length == 0) {
            return false;
        }

        var task = view.getParent().getCurrent();
        if (task != null && (task.data.State == SIE.Inventory.Task.TaskState.Create.value || task.data.State == SIE.Inventory.Task.TaskState.Release.value
            || task.data.State == SIE.Inventory.Task.TaskState.Appoint.value))
            return true;
        return false;
    },
    execute: function (view, source) {
        var sel = view.getSelection();
        SIE.Msg.askQuestion(Ext.String.format('你确定删除选择的{0}条数据吗？确认后直接删除！'.t(), sel.length), function () {
            view.execute({
                data: view.getSelectionIds(),
                success: function (res) {
                    view.removeSelection();
                    view.getSelection().length -= sel.length;
                    view.setCurrent(null, true);
                    view.reloadData();
                },
            });
        });
    }
});