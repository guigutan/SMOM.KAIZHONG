SIE.defineCommand('SIE.Web.Resources.Commands.DeleteCommand', {
    extend: 'SIE.cmd.Delete',
    meta: { text: "删除", group: "edit" },

    canExecute: function (view) {
        var result = this.callParent(arguments);
        if (result === false) {
            return false;
        }
        if (view.getCurrent() !== null) {
            return !view.getCurrent().hasChildNodes();
        }
        return false;
    }
});