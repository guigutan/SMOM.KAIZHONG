SIE.defineCommand('SIE.Web.LES.StockOrders.Commands.StockOrderMergeTimesEditCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", iconCls: "icon-EditEntity icon-blue" },
    canExecute: function (view) {
        var parent = view.getParent();
        if (parent) {
            var current = view.getParent().getCurrent();
            if (current && current.getState() == 0) {
                if (view.getCurrent())
                    return true;
            }
        }
        return false;
    },
});