SIE.defineCommand('SIE.Web.Resources.CalendarSchemes.Commands.CalendarSchemeWeekEditCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit" },

    canExecute: function (view) {
        this.selectedItems = view.getSelection();
        if (this.selectedItems.length > 1) {
            return false;
        }
        var entity = view.getCurrent();
        if (entity == null || entity.data.ActiveDate > new Date()) {
            return false;
        }
        return true;
    }
});