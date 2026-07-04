SIE.defineCommand('SIE.Web.Resources.CalendarSchemes.Commands.CalendarWeekEditCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit" },

    canExecute: function (view) {
        this.selectedItems = view.getSelection();
        if (this.selectedItems.length > 1) {
            return false;
        }
        var entity = view.getCurrent();
        var parent = view.getParent().getCurrent();
        if (entity == null || parent == null || parent.data.ActiveDate > new Date()) {
            return false;
        }
        return true;
    }
});