SIE.defineCommand('SIE.Web.LES.StockOrders.Commands.StockOrderMergeIssuedEditCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", iconCls: "icon-EditEntity icon-blue" },
    canExecute: function (view) {
        var current = view.getCurrent();
        if (current) {
            if (current.getState() == 0)
                return true;
        }
        return false;
    },
    onEditting: function (entity) {
        if (entity) {
            entity.setState(0);
        }
    },
});