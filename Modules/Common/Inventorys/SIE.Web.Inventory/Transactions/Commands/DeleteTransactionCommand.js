SIE.defineCommand('SIE.Web.Inventory.Transactions.Commands.DeleteTransactionCommand', {
    extend: 'SIE.cmd.Delete',
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },

    canExecute: function (view) {
        var reason = view.getCurrent();
        if (reason == null || view.getSelection() == null || view.getSelection().length == 0 || (reason != null && reason.data.State != SIE.Domain.State.Disable.value))
            return false;

        var sel = view.getSelection();
        for (i = 0; i < sel.length; i++) {
            var item = sel[i].data;
            if (item.State != SIE.Domain.State.Disable.value || item.IsEdit == true) {
                return false;
            }
        }
        return true;
    }
});