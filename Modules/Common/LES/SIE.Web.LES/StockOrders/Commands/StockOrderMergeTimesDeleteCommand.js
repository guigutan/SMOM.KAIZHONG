SIE.defineCommand('SIE.Web.LES.StockOrders.Commands.StockOrderMergeTimesDeleteCommand', {
    extend: 'SIE.cmd.Delete',
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },
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
})