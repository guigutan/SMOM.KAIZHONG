SIE.defineCommand('SIE.Web.Resources.CalendarSchemes.Commands.CalendarSchemeDeleteCommand', {
    extend: 'SIE.cmd.Delete',
    meta: { text: "删除", group: "edit" },

    canExecute: function (view) {
        var result = this.callParent(arguments);
        if (result === false) {
            return false;
        }
        if (view.getSelection().length > 0) {
            var flag = true;
            Ext.each(view.getSelection(), function (item) {
                if (item.getIsDefault() === 1) {
                    flag = false;
                }
            });
            return flag;
        }
        return false;
    }
});