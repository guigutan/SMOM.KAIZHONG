SIE.defineCommand('SIE.Web.Resources.CalendarSchemes.Commands.CalendarSchemeWeekDeleteCommand', {
    extend: 'SIE.cmd.Delete',
    meta: { text: "删除", group: "edit" },
    
    canExecute: function (view) {
        this.selectedItems = view.getSelection();
        if (this.selectedItems.length > 1) {
            return false;
        }
        var entity = view.getCurrent();
        if (entity == null || entity.data.ActiveDate <= new Date() || (view.getData().data.items.length == 1 && view.getParent().getCurrent().data.IsEnable == 1)) {
            return false;
        }
        return true;
    }
});