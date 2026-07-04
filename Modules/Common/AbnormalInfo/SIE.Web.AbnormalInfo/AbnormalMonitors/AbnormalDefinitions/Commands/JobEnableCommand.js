SIE.defineCommand('SIE.Web.AbnormalInfo.AbnormalMonitors.AbnormalDefinitions.Commands.JobEnableCommand', {
    extend: 'SIE.cmd.Enable',
    meta: { text: "启用", group: "business", iconCls: "icon-Play icon-green", tooltip: "", },
    canExecute: function (listview) {
        this.selectedItems = listview.getSelection();
        if (this.selectedItems.length === 0) {
            return false;
        }
        for (i = 0, len = this.selectedItems.length; i < len; i++) {
            var item = this.selectedItems[i];
            if (SIE.Domain.State.Enable.value === item.getJobConfigState()) {
                return false;
            }
        }
        var p = listview.getCurrent();
        if (p === null) { return false; }
        if (p.isNew()) { return false; }
        return true;
    },

})