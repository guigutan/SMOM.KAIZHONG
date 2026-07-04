SIE.defineCommand('SIE.Web.LES.StockOrders.Commands.EditStockOrderCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", iconCls: "icon-EditEntity icon-blue" },
    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length !== 1) {
            return false;
        }

        let cur = view.getCurrent();

        return cur && cur.getStockState() == SIE.LES.StockOrder.StockState.Created.value;
    },
});