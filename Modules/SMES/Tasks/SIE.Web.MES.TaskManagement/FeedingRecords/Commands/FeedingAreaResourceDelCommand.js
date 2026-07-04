SIE.defineCommand('SIE.Web.MES.TaskManagement.FeedingRecords.Commands.FeedingAreaResourceDelCommand', {
    extend: 'SIE.cmd.Delete',
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },
    canExecute: function (view) {
        var selections = view.getSelection();
        if (selections.length == 0) {//选中项
            return false;
        }
        return true;
    },
    execute: function (view, source) {
        var selectionIds = view.getSelectionIds();
        view.execute({
            data: selectionIds,
            success: function (res) {
                store.commitChanges();
                view.removeSelection();
                view.setCurrent(null, true);
            },
            error: function (res) {
                store.rejectChanges();
            }
        });
    }
});