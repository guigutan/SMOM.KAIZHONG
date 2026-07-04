SIE.defineCommand('SIE.Web.LES.StockOrders.Commands.StockOrderMergeTimesAddCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit" },
    canExecute: function (view) {
        var parent = view.getParent();
        if (parent) {
            var current = view.getParent().getCurrent();
            if (current && current.getState() == 0) {
                return true;
            }
        }
        return false;
    },
});